// This file defines the CustomerManager class, which handles customer-related operations.

using System;
using System.Collections.Generic;
using System.Data;
using FASCloset.Data;
using FASCloset.Models;
using Microsoft.Data.Sqlite;

namespace FASCloset.Services
{
    public class CustomerManager
    {
        public static List<Customer> GetCustomers()
        {
            string query = "SELECT * FROM Customer ORDER BY Name";
            
            return DataAccessHelper.ExecuteReader(query, reader => new Customer
            {
                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                Name = reader["Name"].ToString() ?? string.Empty,
                Email = reader["Email"].ToString() ?? string.Empty,
                Phone = reader["Phone"].ToString() ?? string.Empty,
                Address = reader["Address"].ToString() ?? string.Empty
            });
        }
        
        public static Customer GetCustomerById(int customerId)
        {
            string query = "SELECT * FROM Customer WHERE CustomerID = @CustomerId";
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerId", customerId }
            };
            
            return DataAccessHelper.ExecuteReaderSingle(query, reader => new Customer
            {
                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                Name = reader["Name"].ToString() ?? string.Empty,
                Email = reader["Email"].ToString() ?? string.Empty,
                Phone = reader["Phone"].ToString() ?? string.Empty,
                Address = reader["Address"].ToString() ?? string.Empty
            }, parameters);
        }
        
        public static void AddCustomer(Customer customer)
        {
            string query = "INSERT INTO Customer (Name, Email, Phone, Address, LoyaltyPoints) VALUES (@Name, @Email, @Phone, @Address, 0)";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Name", customer.Name },
                { "@Email", customer.Email },
                { "@Phone", customer.Phone },
                { "@Address", customer.Address }
            };
            
            DataAccessHelper.ExecuteNonQuery(query, parameters);
            
            // Get the ID of the just-added customer
            customer.CustomerID = DataAccessHelper.ExecuteScalar<int>("SELECT last_insert_rowid()");
        }
        
        public static void UpdateCustomer(Customer customer)
        {
            string query = @"
                UPDATE Customer 
                SET Name = @Name, 
                    Email = @Email, 
                    Phone = @Phone, 
                    Address = @Address
                WHERE CustomerID = @CustomerID";
                
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerID", customer.CustomerID },
                { "@Name", customer.Name },
                { "@Email", customer.Email },
                { "@Phone", customer.Phone },
                { "@Address", customer.Address }
            };
            
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }
        
        public static void DeleteCustomer(int customerId)
        {
            string query = "DELETE FROM Customer WHERE CustomerID = @CustomerID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerID", customerId }
            };
            
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }
        
        public static int GetLoyaltyPointsByCustomerId(int customerId)
        {
            string query = "SELECT LoyaltyPoints FROM Customer WHERE CustomerID = @CustomerID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerID", customerId }
            };
            
            return DataAccessHelper.ExecuteScalar<int>(query, parameters);
        }
        
        public static void UpdateLoyaltyPoints(int customerId, int points)
        {
            string query = @"UPDATE Customer SET LoyaltyPoints = @Points WHERE CustomerID = @CustomerID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerID", customerId },
                { "@Points", points }
            };
            
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }
    }
}
