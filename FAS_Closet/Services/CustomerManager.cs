using System;
using System.Collections.Generic;
using System.Data;
using FASCloset.Data;
using FASCloset.Models;

namespace FASCloset.Services
{
    /// <summary>
    /// Manages customer-related operations including CRUD and search functions
    /// </summary>
    public static class CustomerManager
    {
        /// <summary>
        /// Gets all customers from the database
        /// </summary>
        /// <returns>List of all customers</returns>
        public static List<Customer> GetAllCustomers()
        {
            string query = "SELECT * FROM Customer ORDER BY Name";
            return DataAccessHelper.ExecuteReader(query, MapCustomerFromReader);
        }
        
        /// <summary>
        /// Gets all customers from the database (alias for GetAllCustomers)
        /// </summary>
        /// <returns>List of all customers</returns>
        public static List<Customer> GetCustomers()
        {
            return GetAllCustomers();
        }
        
        /// <summary>
        /// Gets a customer by ID
        /// </summary>
        /// <param name="customerId">ID of the customer to retrieve</param>
        /// <returns>Customer object or null if not found</returns>
        public static Customer? GetCustomerById(int customerId)
        {
            string query = "SELECT * FROM Customer WHERE CustomerID = @CustomerID";
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerID", customerId }
            };
            
            return DataAccessHelper.ExecuteReaderSingle(query, MapCustomerFromReader, parameters);
        }
        
        /// <summary>
        /// Searches for customers based on name, email, or phone
        /// </summary>
        /// <param name="searchTerm">Term to search for</param>
        /// <returns>List of matching customers</returns>
        public static List<Customer> SearchCustomers(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetAllCustomers();
                
            string query = @"
                SELECT * FROM Customer 
                WHERE Name LIKE @SearchTerm 
                OR Email LIKE @SearchTerm 
                OR Phone LIKE @SearchTerm 
                OR Address LIKE @SearchTerm
                ORDER BY Name";
                
            var parameters = new Dictionary<string, object>
            {
                { "@SearchTerm", $"%{searchTerm}%" }
            };
            
            return DataAccessHelper.ExecuteReader(query, MapCustomerFromReader, parameters);
        }
        
        /// <summary>
        /// Adds a new customer to the database
        /// </summary>
        /// <param name="customer">Customer object to add</param>
        /// <returns>ID of the newly added customer, or -1 on failure</returns>
        public static int AddCustomer(Customer customer)
        {
            try
            {
                // Check if email or phone already exists
                if (IsEmailTaken(customer.Email))
                    throw new Exception("Email already exists in the system");
                    
                if (IsPhoneTaken(customer.Phone))
                    throw new Exception("Phone number already exists in the system");
                
                string query = @"
                    INSERT INTO Customer (Name, Email, Phone, Address, LoyaltyPoints)
                    VALUES (@Name, @Email, @Phone, @Address, @LoyaltyPoints);
                    SELECT last_insert_rowid();";
                    
                var parameters = new Dictionary<string, object>
                {
                    { "@Name", customer.Name },
                    { "@Email", customer.Email },
                    { "@Phone", customer.Phone },
                    { "@Address", customer.Address },
                    { "@LoyaltyPoints", customer.LoyaltyPoints }
                };
                
                return DataAccessHelper.ExecuteScalar<int>(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding customer: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// Updates an existing customer in the database
        /// </summary>
        /// <param name="customer">Updated customer information</param>
        /// <returns>True if update was successful</returns>
        public static bool UpdateCustomer(Customer customer)
        {
            try
            {
                // Check if email is taken by another customer
                if (IsEmailTakenByAnother(customer.Email, customer.Id))
                    throw new Exception("Email already exists for another customer");
                    
                // Check if phone is taken by another customer
                if (IsPhoneTakenByAnother(customer.Phone, customer.Id))
                    throw new Exception("Phone number already exists for another customer");
                
                string query = @"
                    UPDATE Customer 
                    SET Name = @Name,
                        Email = @Email,
                        Phone = @Phone,
                        Address = @Address,
                        LoyaltyPoints = @LoyaltyPoints
                    WHERE CustomerID = @CustomerID";
                    
                var parameters = new Dictionary<string, object>
                {
                    { "@CustomerID", customer.Id },
                    { "@Name", customer.Name },
                    { "@Email", customer.Email },
                    { "@Phone", customer.Phone },
                    { "@Address", customer.Address },
                    { "@LoyaltyPoints", customer.LoyaltyPoints }
                };
                
                int rowsAffected = DataAccessHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating customer: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// Deletes a customer from the database
        /// </summary>
        /// <param name="customerId">ID of the customer to delete</param>
        /// <returns>True if deletion was successful</returns>
        public static bool DeleteCustomer(int customerId)
        {
            try
            {
                string query = "DELETE FROM Customer WHERE CustomerID = @CustomerID";
                var parameters = new Dictionary<string, object>
                {
                    { "@CustomerID", customerId }
                };
                
                int rowsAffected = DataAccessHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting customer: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Updates loyalty points for a customer
        /// </summary>
        /// <param name="customerId">ID of the customer</param>
        /// <param name="points">Points to add (or subtract if negative)</param>
        /// <returns>True if update was successful</returns>
        public static bool UpdateLoyaltyPoints(int customerId, int points)
        {
            try
            {
                string query = @"
                    UPDATE Customer 
                    SET LoyaltyPoints = LoyaltyPoints + @Points
                    WHERE CustomerID = @CustomerID";
                    
                var parameters = new Dictionary<string, object>
                {
                    { "@CustomerID", customerId },
                    { "@Points", points }
                };
                
                int rowsAffected = DataAccessHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating loyalty points: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Gets the loyalty points for a specific customer
        /// </summary>
        /// <param name="customerId">ID of the customer</param>
        /// <returns>Number of loyalty points, or 0 if customer not found</returns>
        public static int GetLoyaltyPointsByCustomerId(int customerId)
        {
            string query = "SELECT LoyaltyPoints FROM Customer WHERE CustomerID = @CustomerID";
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerID", customerId }
            };
            
            try
            {
                return DataAccessHelper.ExecuteScalar<int>(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting loyalty points: {ex.Message}");
                return 0;
            }
        }
        
        /// <summary>
        /// Gets the top customers by purchase total
        /// </summary>
        /// <param name="count">Number of customers to retrieve</param>
        /// <returns>List of top customers</returns>
        public static List<Customer> GetTopCustomers(int count = 5)
        {
            string query = @"
                SELECT c.*, SUM(o.TotalAmount) as TotalPurchases
                FROM Customer c
                INNER JOIN Orders o ON c.CustomerID = o.CustomerID
                GROUP BY c.CustomerID
                ORDER BY TotalPurchases DESC
                LIMIT @Count";
                
            var parameters = new Dictionary<string, object>
            {
                { "@Count", count }
            };
            
            return DataAccessHelper.ExecuteReader(query, reader =>
            {
                var customer = MapCustomerFromReader(reader);
                customer.TotalPurchases = reader.GetDecimal(reader.GetOrdinal("TotalPurchases"));
                return customer;
            }, parameters);
        }
        
        /// <summary>
        /// Checks if an email address is already in use
        /// </summary>
        /// <param name="email">Email address to check</param>
        /// <returns>True if the email is already taken</returns>
        public static bool IsEmailTaken(string email)
        {
            string query = "SELECT COUNT(*) FROM Customer WHERE Email = @Email";
            var parameters = new Dictionary<string, object>
            {
                { "@Email", email }
            };
            
            int count = DataAccessHelper.ExecuteScalar<int>(query, parameters);
            return count > 0;
        }
        
        /// <summary>
        /// Checks if a phone number is already in use
        /// </summary>
        /// <param name="phone">Phone number to check</param>
        /// <returns>True if the phone number is already taken</returns>
        public static bool IsPhoneTaken(string phone)
        {
            string query = "SELECT COUNT(*) FROM Customer WHERE Phone = @Phone";
            var parameters = new Dictionary<string, object>
            {
                { "@Phone", phone }
            };
            
            int count = DataAccessHelper.ExecuteScalar<int>(query, parameters);
            return count > 0;
        }
        
        /// <summary>
        /// Checks if an email is taken by a customer other than the specified one
        /// </summary>
        /// <param name="email">Email to check</param>
        /// <param name="customerId">ID of the customer to exclude from check</param>
        /// <returns>True if the email is taken by another customer</returns>
        private static bool IsEmailTakenByAnother(string email, int customerId)
        {
            string query = "SELECT COUNT(*) FROM Customer WHERE Email = @Email AND CustomerID != @CustomerID";
            var parameters = new Dictionary<string, object>
            {
                { "@Email", email },
                { "@CustomerID", customerId }
            };
            
            int count = DataAccessHelper.ExecuteScalar<int>(query, parameters);
            return count > 0;
        }
        
        /// <summary>
        /// Checks if a phone number is taken by a customer other than the specified one
        /// </summary>
        /// <param name="phone">Phone number to check</param>
        /// <param name="customerId">ID of the customer to exclude from check</param>
        /// <returns>True if the phone number is taken by another customer</returns>
        private static bool IsPhoneTakenByAnother(string phone, int customerId)
        {
            string query = "SELECT COUNT(*) FROM Customer WHERE Phone = @Phone AND CustomerID != @CustomerID";
            var parameters = new Dictionary<string, object>
            {
                { "@Phone", phone },
                { "@CustomerID", customerId }
            };
            
            int count = DataAccessHelper.ExecuteScalar<int>(query, parameters);
            return count > 0;
        }
        
        /// <summary>
        /// Maps a database record to a Customer object
        /// </summary>
        /// <param name="reader">Data reader containing customer data</param>
        /// <returns>Populated Customer object</returns>
        private static Customer MapCustomerFromReader(IDataReader reader)
        {
            return new Customer
            {
                Id = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                Address = reader.GetString(reader.GetOrdinal("Address")),
                LoyaltyPoints = reader.GetInt32(reader.GetOrdinal("LoyaltyPoints"))
            };
        }
    }
}
