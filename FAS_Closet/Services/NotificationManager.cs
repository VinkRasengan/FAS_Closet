using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using FASCloset.Config;
using FASCloset.Models;
using FASCloset.Data;
using Microsoft.Data.Sqlite;
using System.Linq;

namespace FASCloset.Services
{
    /// <summary>
    /// Manages notifications including email alerts and SMS messages
    /// </summary>
    public static class NotificationManager
    {
        /// <summary>
        /// Checks for low stock items and sends notifications if needed
        /// </summary>
        public static void CheckAndSendLowStockNotifications()
        {
            try
            {
                // Don't proceed if notifications are disabled
                if (!AppSettings.EmailNotificationsEnabled && !AppSettings.SmsNotificationsEnabled)
                    return;
                
                // Get items with low stock
                var lowStockItems = InventoryManager.GetLowStockProducts();
                
                // Don't proceed if there are no low stock items
                if (lowStockItems.Count == 0)
                    return;
                
                // Build notification message
                string subject = $"Low Stock Alert - {lowStockItems.Count} products";
                string message = BuildLowStockMessage(lowStockItems);
                
                // Send notification
                if (AppSettings.EmailNotificationsEnabled)
                {
                    SendEmailNotification(subject, message);
                }
                
                if (AppSettings.SmsNotificationsEnabled)
                {
                    SendSmsNotification(subject, GetSmsMessage(lowStockItems));
                }
                
                // Log notification
                LogNotification(NotificationType.LowStock, subject, message);
            }
            catch (Exception ex)
            {
                // Log error and continue - don't throw from here as it may crash application startup
                Console.WriteLine($"Error in CheckAndSendLowStockNotifications: {ex.Message}");
                LogNotification(NotificationType.Error, "Error sending low stock notification", ex.Message);
            }
        }

        /// <summary>
        /// Displays a popup for low stock items
        /// </summary>
        /// <param name="lowStockItems">List of products with low stock</param>
        public static void ShowLowStockPopup(List<Product> lowStockItems)
        {
            if (lowStockItems == null || lowStockItems.Count == 0)
                return;

            StringBuilder sb = new StringBuilder("⚠ Low stock warning:\n\n");
            foreach (var product in lowStockItems.Take(5))
            {
                sb.AppendLine($"- {product.ProductName} (Stock: {product.Stock})");
            }

            if (lowStockItems.Count > 5)
                sb.AppendLine($"\n...and {lowStockItems.Count - 5} more.");

            MessageBox.Show(sb.ToString(), "Low Stock Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        
        /// <summary>
        /// Creates a nicely formatted message for low stock items
        /// </summary>
        /// <param name="lowStockItems">List of products with low stock</param>
        /// <returns>Formatted message string</returns>
        private static string BuildLowStockMessage(List<Product> lowStockItems)
        {
            var sb = new StringBuilder("The following products are low on stock and may need reordering:\n\n");
            
            foreach (var product in lowStockItems)
            {
                sb.AppendLine($"• {product.ProductName}");
                sb.AppendLine($"  Current stock: {product.Stock}");
                sb.AppendLine($"  Category: {product.CategoryName}");
                sb.AppendLine($"  ID: {product.ProductID}\n");
            }
            
            sb.AppendLine("\nPlease take appropriate action to restock these items.");
            return sb.ToString();
        }
        
        /// <summary>
        /// Creates a shorter SMS-friendly message for low stock items
        /// </summary>
        /// <param name="lowStockItems">List of products with low stock</param>
        /// <returns>Formatted SMS message string</returns>
        private static string GetSmsMessage(List<Product> lowStockItems)
        {
            var criticalItems = lowStockItems.Where(p => p.Stock == 0).ToList();
            var lowItems = lowStockItems.Where(p => p.Stock > 0).ToList();
            
            string message = $"FAS Closet Alert: ";
            
            if (criticalItems.Count > 0)
                message += $"{criticalItems.Count} out of stock, ";
                
            message += $"{lowItems.Count} low stock. Check inventory.";
            
            return message;
        }
        
        /// <summary>
        /// Send an email notification
        /// </summary>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body</param>
        public static void SendEmailNotification(string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(AppSettings.SmtpServer, AppSettings.SmtpPort)
                {
                    EnableSsl = AppSettings.SmtpUseSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(AppSettings.SmtpUsername, AppSettings.SmtpPassword)
                };
                
                using var message = new MailMessage
                {
                    From = new MailAddress(AppSettings.NotificationEmailAddress),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };
                
                message.To.Add(AppSettings.NotificationRecipientEmail);
                
                client.Send(message);
            }
            catch (Exception ex)
            {
                LogNotification(NotificationType.Error, "Email notification error", ex.Message);
                throw;
            }
        }
        
        /// <summary>
        /// Send an SMS notification
        /// </summary>
        /// <param name="subject">SMS subject</param>
        /// <param name="message">SMS content</param>
        public static void SendSmsNotification(string subject, string message)
        {
            try
            {
                // Placeholder for actual SMS implementation
                // This would typically use an SMS gateway API
                Console.WriteLine($"SMS: {subject} - {message}");
                
                // Log the notification
                LogNotification(NotificationType.SMS, subject, "SMS notification sent");
            }
            catch (Exception ex)
            {
                LogNotification(NotificationType.Error, "SMS notification error", ex.Message);
                throw;
            }
        }
        
        /// <summary>
        /// Get all notification logs
        /// </summary>
        /// <returns>List of notification log entries</returns>
        public static List<NotificationLog> GetNotificationLogs()
        {
            // Make sure we have a valid NotificationLog table
            EnsureNotificationLogTableExists();
            
            string query = @"
                SELECT * FROM NotificationLog
                ORDER BY Timestamp DESC";
                
            return DataAccessHelper.ExecuteReader(query, reader => new NotificationLog
            {
                LogID = reader.GetInt32(reader.GetOrdinal("LogID")),
                Type = Enum.Parse<NotificationType>(reader.GetString(reader.GetOrdinal("Type"))),
                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                Message = reader.GetString(reader.GetOrdinal("Message")),
                Timestamp = reader.GetDateTime(reader.GetOrdinal("Timestamp"))
            });
        }
        
        /// <summary>
        /// Log a notification to the database
        /// </summary>
        /// <param name="type">Type of notification</param>
        /// <param name="subject">Notification subject</param>
        /// <param name="message">Notification message</param>
        public static void LogNotification(NotificationType type, string subject, string message)
        {
            try
            {
                // Make sure we have a valid NotificationLog table
                EnsureNotificationLogTableExists();
                
                string query = @"
                    INSERT INTO NotificationLog (Type, Subject, Message, Timestamp)
                    VALUES (@Type, @Subject, @Message, @Timestamp)";
                    
                var parameters = new Dictionary<string, object>
                {
                    { "@Type", type.ToString() },
                    { "@Subject", subject },
                    { "@Message", message },
                    { "@Timestamp", DateTime.Now }
                };
                
                DataAccessHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                // Just write to console as we can't recursively log errors
                Console.WriteLine($"Error logging notification: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Ensure the NotificationLog table exists
        /// </summary>
        private static void EnsureNotificationLogTableExists()
        {
            DatabaseConnection.ExecuteDbOperation(connection =>
            {
                string checkTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='NotificationLog';";
                using (var command = new SqliteCommand(checkTableQuery, connection))
                {
                    if (command.ExecuteScalar() == null)
                    {
                        using (var createCommand = new SqliteCommand(@"
                            CREATE TABLE NotificationLog (
                                LogID INTEGER PRIMARY KEY AUTOINCREMENT,
                                Type TEXT NOT NULL,
                                Subject TEXT NOT NULL,
                                Message TEXT NOT NULL,
                                Timestamp DATETIME NOT NULL
                            );", connection))
                        {
                            createCommand.ExecuteNonQuery();
                        }
                    }
                }
            });
        }
    }
    
    /// <summary>
    /// Types of notifications
    /// </summary>
    public enum NotificationType
    {
        LowStock,
        Order,
        System,
        Error,
        Email,
        SMS
    }
}
