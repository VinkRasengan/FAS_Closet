using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FASCloset.Config;
using FASCloset.Models;
using FASCloset.Services;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace FASCloset.Forms
{
    public partial class UcNotificationSettings : UserControl
    {
        // Constants for reused strings
        private const string ErrorTitle = "Error";
        private const string SuccessTitle = "Success";
        
        // Class fields for UI controls
        private CheckBox chkEnableEmailNotifications;
        private CheckBox chkEnableSmsNotifications;
        private TextBox txtEmailFrom;
        private TextBox txtEmailTo;
        private TextBox txtSmtpServer;
        private TextBox txtSmtpPort;
        private CheckBox chkSmtpSsl;
        private TextBox txtSmtpUsername;
        private TextBox txtSmtpPassword;
        private TextBox txtSmsRecipient;
        private DataGridView dgvNotificationLogs;
        
        public UcNotificationSettings()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void InitializeComponent()
        {
            // Initialize tab control for multiple settings pages
            var tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            
            // General tab
            TabPage generalTab = new TabPage("General");
            
            // Email tab
            TabPage emailTab = new TabPage("Email");
            
            // SMS tab
            TabPage smsTab = new TabPage("SMS");
            
            // Notification Log tab
            TabPage logTab = new TabPage("Notification Log");
            
            // Add tabs to tab control
            tabControl.TabPages.AddRange(new TabPage[] { generalTab, emailTab, smsTab, logTab });
            
            // Add controls to General tab
            this.chkEnableEmailNotifications = new CheckBox
            {
                Text = "Enable Email Notifications",
                Location = new Point(20, 20),
                Width = 200,
                Name = "chkEnableEmailNotifications"
            };
            
            this.chkEnableSmsNotifications = new CheckBox
            {
                Text = "Enable SMS Notifications",
                Location = new Point(20, 50),
                Width = 200,
                Name = "chkEnableSmsNotifications"
            };
            
            Button btnTestNotification = new Button
            {
                Text = "Send Test Notification",
                Location = new Point(20, 100),
                Width = 200,
                Height = 30,
                Name = "btnTestNotification"
            };
            btnTestNotification.Click += BtnTestNotification_Click;
            
            generalTab.Controls.Add(this.chkEnableEmailNotifications);
            generalTab.Controls.Add(this.chkEnableSmsNotifications);
            generalTab.Controls.Add(btnTestNotification);
            
            // Add controls to Email tab
            Label lblEmailFrom = new Label
            {
                Text = "Notification Email (From):",
                Location = new Point(20, 20),
                Width = 150,
                TextAlign = ContentAlignment.MiddleRight
            };
            
            this.txtEmailFrom = new TextBox
            {
                Location = new Point(180, 20),
                Width = 300,
                Name = "txtEmailFrom"
            };
            
            Label lblEmailTo = new Label
            {
                Text = "Recipient Email:",
                Location = new Point(20, 55),
                Width = 150,
                TextAlign = ContentAlignment.MiddleRight
            };
            
            this.txtEmailTo = new TextBox
            {
                Location = new Point(180, 55),
                Width = 300,
                Name = "txtEmailTo"
            };
            
            Label lblSmtpServer = new Label
            {
                Text = "SMTP Server:",
                Location = new Point(20, 90),
                Width = 150,
                TextAlign = ContentAlignment.MiddleRight
            };
            
            this.txtSmtpServer = new TextBox
            {
                Location = new Point(180, 90),
                Width = 300,
                Name = "txtSmtpServer"
            };
            
            Label lblSmtpPort = new Label
            {
                Text = "SMTP Port:",
                Location = new Point(20, 125),
                Width = 150,
                TextAlign = ContentAlignment.MiddleRight
            };
            
            this.txtSmtpPort = new TextBox
            {
                Location = new Point(180, 125),
                Width = 100,
                Name = "txtSmtpPort"
            };
            
            this.chkSmtpSsl = new CheckBox
            {
                Text = "Use SSL",
                Location = new Point(180, 160),
                Width = 100,
                Name = "chkSmtpSsl"
            };
            
            Label lblSmtpUsername = new Label
            {
                Text = "SMTP Username:",
                Location = new Point(20, 195),
                Width = 150,
                TextAlign = ContentAlignment.MiddleRight
            };
            
            this.txtSmtpUsername = new TextBox
            {
                Location = new Point(180, 195),
                Width = 300,
                Name = "txtSmtpUsername"
            };
            
            Label lblSmtpPassword = new Label
            {
                Text = "SMTP Password:",
                Location = new Point(20, 230),
                Width = 150,
                TextAlign = ContentAlignment.MiddleRight
            };
            
            this.txtSmtpPassword = new TextBox
            {
                Location = new Point(180, 230),
                Width = 300,
                UseSystemPasswordChar = true,
                Name = "txtSmtpPassword"
            };
            
            Button btnSaveEmailSettings = new Button
            {
                Text = "Save Email Settings",
                Location = new Point(180, 270),
                Width = 200,
                Height = 30,
                Name = "btnSaveEmailSettings"
            };
            btnSaveEmailSettings.Click += BtnSaveEmailSettings_Click;
            
            Button btnTestEmail = new Button
            {
                Text = "Test Email Settings",
                Location = new Point(400, 270),
                Width = 150,
                Height = 30,
                Name = "btnTestEmail"
            };
            btnTestEmail.Click += BtnTestEmail_Click;
            
            emailTab.Controls.Add(lblEmailFrom);
            emailTab.Controls.Add(this.txtEmailFrom);
            emailTab.Controls.Add(lblEmailTo);
            emailTab.Controls.Add(this.txtEmailTo);
            emailTab.Controls.Add(lblSmtpServer);
            emailTab.Controls.Add(this.txtSmtpServer);
            emailTab.Controls.Add(lblSmtpPort);
            emailTab.Controls.Add(this.txtSmtpPort);
            emailTab.Controls.Add(this.chkSmtpSsl);
            emailTab.Controls.Add(lblSmtpUsername);
            emailTab.Controls.Add(this.txtSmtpUsername);
            emailTab.Controls.Add(lblSmtpPassword);
            emailTab.Controls.Add(this.txtSmtpPassword);
            emailTab.Controls.Add(btnSaveEmailSettings);
            emailTab.Controls.Add(btnTestEmail);
            
            // Add controls to SMS tab
            Label lblSmsRecipient = new Label
            {
                Text = "Recipient Phone Number:",
                Location = new Point(20, 20),
                Width = 150,
                TextAlign = ContentAlignment.MiddleRight
            };
            
            this.txtSmsRecipient = new TextBox
            {
                Location = new Point(180, 20),
                Width = 200,
                Name = "txtSmsRecipient"
            };
            
            Label lblSmsInfo = new Label
            {
                Text = "SMS service integration requires an account with an SMS provider.",
                Location = new Point(20, 60),
                Width = 400,
                Font = new Font("Segoe UI", 9, FontStyle.Italic)
            };
            
            Button btnSaveSmsSettings = new Button
            {
                Text = "Save SMS Settings",
                Location = new Point(180, 100),
                Width = 200,
                Height = 30,
                Name = "btnSaveSmsSettings"
            };
            btnSaveSmsSettings.Click += BtnSaveSmsSettings_Click;
            
            smsTab.Controls.Add(lblSmsRecipient);
            smsTab.Controls.Add(this.txtSmsRecipient);
            smsTab.Controls.Add(lblSmsInfo);
            smsTab.Controls.Add(btnSaveSmsSettings);
            
            // Add controls to Log tab
            this.dgvNotificationLogs = new DataGridView
            {
                Dock = DockStyle.Fill,
                Name = "dgvNotificationLogs",
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            
            Button btnRefreshLogs = new Button
            {
                Text = "Refresh Logs",
                Dock = DockStyle.Bottom,
                Height = 30,
                Name = "btnRefreshLogs"
            };
            btnRefreshLogs.Click += BtnRefreshLogs_Click;
            
            logTab.Controls.Add(this.dgvNotificationLogs);
            logTab.Controls.Add(btnRefreshLogs);
            
            // Add tab control to the user control
            this.Controls.Add(tabControl);
        }

        private void LoadSettings()
        {
            // Load general settings
            chkEnableEmailNotifications.Checked = AppSettings.EmailNotificationsEnabled;
            chkEnableSmsNotifications.Checked = AppSettings.SmsNotificationsEnabled;
            
            // Load email settings
            txtEmailFrom.Text = AppSettings.NotificationEmailAddress;
            txtEmailTo.Text = AppSettings.NotificationRecipientEmail;
            txtSmtpServer.Text = AppSettings.SmtpServer;
            txtSmtpPort.Text = AppSettings.SmtpPort.ToString();
            chkSmtpSsl.Checked = AppSettings.SmtpUseSsl;
            txtSmtpUsername.Text = AppSettings.SmtpUsername;
            txtSmtpPassword.Text = AppSettings.SmtpPassword;
            
            // Load SMS settings
            txtSmsRecipient.Text = AppSettings.SmsRecipientNumber;
            
            // Load notification logs
            LoadNotificationLogs();
        }
        
        private void LoadNotificationLogs()
        {
            var logs = NotificationManager.GetNotificationLogs();
            dgvNotificationLogs.DataSource = logs;
            
            // Format columns for better readability
            if (dgvNotificationLogs.Columns.Count > 0)
            {
                dgvNotificationLogs.Columns["Message"].Visible = false;
            }
        }
        
        private void BtnSaveEmailSettings_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the configuration file
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                
                // Update settings
                config.AppSettings.Settings["EmailNotificationsEnabled"].Value = chkEnableEmailNotifications.Checked.ToString();
                config.AppSettings.Settings["NotificationEmailAddress"].Value = txtEmailFrom.Text;
                config.AppSettings.Settings["NotificationRecipientEmail"].Value = txtEmailTo.Text;
                config.AppSettings.Settings["SmtpServer"].Value = txtSmtpServer.Text;
                config.AppSettings.Settings["SmtpPort"].Value = txtSmtpPort.Text;
                config.AppSettings.Settings["SmtpUseSsl"].Value = chkSmtpSsl.Checked.ToString();
                config.AppSettings.Settings["SmtpUsername"].Value = txtSmtpUsername.Text;
                config.AppSettings.Settings["SmtpPassword"].Value = txtSmtpPassword.Text;
                
                // Save the changes
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                
                MessageBox.Show("Email settings saved successfully.", SuccessTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save email settings: {ex.Message}", ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnSaveSmsSettings_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the configuration file
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                
                // Update settings
                config.AppSettings.Settings["SmsNotificationsEnabled"].Value = chkEnableSmsNotifications.Checked.ToString();
                config.AppSettings.Settings["SmsRecipientNumber"].Value = txtSmsRecipient.Text;
                
                // Save the changes
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                
                MessageBox.Show("SMS settings saved successfully.", SuccessTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save SMS settings: {ex.Message}", ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnTestEmail_Click(object sender, EventArgs e)
        {
            try
            {
                // Send a test email using current form settings
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(txtSmtpServer.Text)
                {
                    Port = int.Parse(txtSmtpPort.Text),
                    EnableSsl = chkSmtpSsl.Checked,
                    Credentials = new System.Net.NetworkCredential(txtSmtpUsername.Text, txtSmtpPassword.Text)
                };
                
                var subject = "Test Email from FAS Closet";
                
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(
                    txtEmailFrom.Text,
                    txtEmailTo.Text,
                    subject,
                    "This is a test email from FAS Closet inventory notification system."
                );
                
                client.Send(message);
                MessageBox.Show("Test email sent successfully!", SuccessTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send test email: {ex.Message}", ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnTestNotification_Click(object sender, EventArgs e)
        {
            try
            {
                // Create test products with low stock for the demonstration
                List<Product> testProducts = new List<Product>
                {
                    new Product
                    {
                        ProductID = 1,
                        ProductName = "Test Product 1",
                        Stock = 2,
                        CategoryName = "Test Category",
                        IsLowStock = true
                    },
                    new Product
                    {
                        ProductID = 2,
                        ProductName = "Test Product 2",
                        Stock = 0,
                        CategoryName = "Test Category",
                        IsLowStock = true
                    }
                };

                // Send notification with test data
                string subject = "Test Low Stock Notification";
                NotificationManager.CheckAndSendLowStockNotifications(); // Use this instead of SendLowStockNotification
                
                MessageBox.Show("Test notification sent. Please check the notification log.", 
                    "Test Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                // Refresh logs
                LoadNotificationLogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending test notification: {ex.Message}", 
                    "Notification Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnRefreshLogs_Click(object sender, EventArgs e)
        {
            LoadNotificationLogs();
        }
    }
}
