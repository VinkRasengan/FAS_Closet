using System;
using FASCloset.Services;

namespace FASCloset.Models
{
    /// <summary>
    /// Represents a log entry for a sent notification
    /// </summary>
    public class NotificationLog
    {
        /// <summary>
        /// The unique identifier for the notification log
        /// </summary>
        public int LogID { get; set; }
        
        /// <summary>
        /// The type of notification
        /// </summary>
        public NotificationType Type { get; set; }
        
        /// <summary>
        /// The subject of the notification
        /// </summary>
        public string Subject { get; set; } = string.Empty;
        
        /// <summary>
        /// The message content of the notification
        /// </summary>
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// When the notification was sent/logged
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Gets a short preview of the message (first 50 characters)
        /// </summary>
        public string MessagePreview
        {
            get
            {
                if (Message.Length <= 50)
                    return Message;
                    
                return Message.Substring(0, 47) + "...";
            }
        }
        
        /// <summary>
        /// Gets the formatted timestamp as a string
        /// </summary>
        public string FormattedTimestamp => Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
    }
    
    /// <summary>
    /// Represents notification settings for a specific product
    /// </summary>
    public class ProductNotificationSettings
    {
        public int ProductID { get; set; }
        public bool EnableNotifications { get; set; } = true;
        public int CustomThreshold { get; set; } = 0; // 0 means use the default threshold from inventory
    }
}
