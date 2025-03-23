// This file defines the CustomerManager class, which handles customer-related operations.

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using FASCloset.Models;
using FASCloset.Data;

namespace FASCloset.Services
{
    public static class CustomerManager
    {
        private const string CustomerIdParameter = "@CustomerID";

        public static void AddCustomer(Customer customer)
        {
            DatabaseConnection.ExecuteDbOperation(connection => 
            {
                string query = "INSERT INTO Customers (Name, Email, Phone, Address) VALUES (@Name, @Email, @Phone, @Address)";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", customer.Name);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@Phone", customer.Phone);
                    command.Parameters.AddWithValue("@Address", customer.Address);
                    command.ExecuteNonQuery();
                }
                return true;
            });
        }

        public static void UpdateCustomer(Customer customer)
        {
            DatabaseConnection.ExecuteDbOperation(connection => 
            {
                string query = "UPDATE Customers SET Name = @Name, Email = @Email, Phone = @Phone, Address = @Address WHERE CustomerID = @CustomerID";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", customer.Name);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@Phone", customer.Phone);
                    command.Parameters.AddWithValue("@Address", customer.Address);
                    command.Parameters.AddWithValue(CustomerIdParameter, customer.CustomerID);
                    command.ExecuteNonQuery();
                }
                return true;
            });
        }

        public static void DeleteCustomer(int customerId)
        {
            DatabaseConnection.ExecuteDbOperation(connection => 
            {
                string query = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue(CustomerIdParameter, customerId);
                    command.ExecuteNonQuery();
                }
                return true;
            });
        }

        public static List<Customer> GetCustomers()
        {
            var customers = new List<Customer>();
            DatabaseConnection.ExecuteDbOperation(connection => 
            {
                string query = "SELECT CustomerID, Name, Email, Phone, Address FROM Customers";
                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                CustomerID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Phone = reader.GetString(3),
                                Address = reader.GetString(4)
                            });
                        }
                    }
                }
                return true;
            });
            return customers;
        }

        public static Customer? GetCustomerById(int customerId)
        {
            Customer? customer = null;
            DatabaseConnection.ExecuteDbOperation(connection => 
            {
                string query = "SELECT CustomerID, Name, Email, Phone, Address FROM Customers WHERE CustomerID = @CustomerID";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue(CustomerIdParameter, customerId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customer = new Customer
                            {
                                CustomerID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Phone = reader.GetString(3),
                                Address = reader.GetString(4)
                            };
                        }
                    }
                }
                return true;
            });
            return customer;
        }

        public static int GetLoyaltyPointsByCustomerId(int customerId)
        {
            int loyaltyPoints = 0;
            DatabaseConnection.ExecuteDbOperation(connection => 
            {
                string query = "SELECT LoyaltyPoints FROM Customers WHERE CustomerID = @CustomerID";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue(CustomerIdParameter, customerId);
                    loyaltyPoints = Convert.ToInt32(command.ExecuteScalar());
                }
                return true;
            });
            return loyaltyPoints;
        }
    }
}
