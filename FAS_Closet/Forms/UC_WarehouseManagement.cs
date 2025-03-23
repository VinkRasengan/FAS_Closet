using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class UcWarehouseManagement : UserControl
    {
        private readonly BindingSource warehouseBindingSource = new BindingSource();
        private readonly User currentUser;
        
        // Form controls
        private DataGridView dataGridViewWarehouses;
        private Panel editPanel;
        private TextBox txtWarehouseName;
        private TextBox txtWarehouseAddress;
        private TextBox txtWarehouseDescription;
        private ComboBox cmbManager;
        private CheckBox chkIsActive;
        private Button btnSave;
        private Button btnCancel;
        
        // Current editing state
        private int? editingWarehouseId = null;
        
        public UcWarehouseManagement(User user)
        {
            InitializeComponent();
            
            currentUser = user;
            
            // Set up data binding
            warehouseBindingSource.DataSource = typeof(List<Warehouse>);
            dataGridViewWarehouses.DataSource = warehouseBindingSource;
            
            // Load data
            LoadWarehouses();
            LoadManagers();
        }
        
        private void InitializeComponent()
        {
            this.dataGridViewWarehouses = new DataGridView();
            this.editPanel = new Panel();
            this.txtWarehouseName = new TextBox();
            this.txtWarehouseAddress = new TextBox();
            this.txtWarehouseDescription = new TextBox();
            this.cmbManager = new ComboBox();
            this.chkIsActive = new CheckBox();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            
            // Main layout
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Size = new System.Drawing.Size(800, 600);
            this.Name = "UcWarehouseManagement";
            this.Padding = new System.Windows.Forms.Padding(10);
            
            // DataGridView setup
            this.dataGridViewWarehouses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWarehouses.Dock = DockStyle.Top;
            this.dataGridViewWarehouses.Location = new Point(10, 10);
            this.dataGridViewWarehouses.Name = "dataGridViewWarehouses";
            this.dataGridViewWarehouses.Size = new Size(780, 300);
            this.dataGridViewWarehouses.TabIndex = 0;
            this.dataGridViewWarehouses.AllowUserToAddRows = false;
            this.dataGridViewWarehouses.ReadOnly = true;
            this.dataGridViewWarehouses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewWarehouses.MultiSelect = false;
            this.dataGridViewWarehouses.AutoGenerateColumns = false;
            
            // Add columns
            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.DataPropertyName = "WarehouseID";
            colId.HeaderText = "ID";
            colId.Width = 40;
            
            DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
            colName.DataPropertyName = "Name";
            colName.HeaderText = "Name";
            colName.Width = 150;
            
            DataGridViewTextBoxColumn colAddress = new DataGridViewTextBoxColumn();
            colAddress.DataPropertyName = "Address";
            colAddress.HeaderText = "Address";
            colAddress.Width = 200;
            
            DataGridViewTextBoxColumn colManager = new DataGridViewTextBoxColumn();
            colManager.DataPropertyName = "ManagerName";
            colManager.HeaderText = "Manager";
            colManager.Width = 120;
            
            DataGridViewCheckBoxColumn colActive = new DataGridViewCheckBoxColumn();
            colActive.DataPropertyName = "IsActive";
            colActive.HeaderText = "Active";
            colActive.Width = 60;
            
            DataGridViewTextBoxColumn colDesc = new DataGridViewTextBoxColumn();
            colDesc.DataPropertyName = "Description";
            colDesc.HeaderText = "Description";
            colDesc.Width = 200;
            
            this.dataGridViewWarehouses.Columns.AddRange(new DataGridViewColumn[] {
                colId, colName, colAddress, colManager, colActive, colDesc
            });
            
            // Edit panel setup
            this.editPanel.BorderStyle = BorderStyle.FixedSingle;
            this.editPanel.Location = new Point(10, 320);
            this.editPanel.Name = "editPanel";
            this.editPanel.Size = new Size(780, 240);
            this.editPanel.TabIndex = 1;
            this.editPanel.Padding = new Padding(10);
            
            // Labels
            Label lblName = new Label();
            lblName.AutoSize = true;
            lblName.Location = new Point(20, 20);
            lblName.Text = "Warehouse Name:";
            
            Label lblAddress = new Label();
            lblAddress.AutoSize = true;
            lblAddress.Location = new Point(20, 50);
            lblAddress.Text = "Address:";
            
            Label lblManager = new Label();
            lblManager.AutoSize = true;
            lblManager.Location = new Point(20, 80);
            lblManager.Text = "Manager:";
            
            Label lblDescription = new Label();
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(20, 110);
            lblDescription.Text = "Description:";
            
            // Text inputs
            this.txtWarehouseName.Location = new Point(140, 20);
            this.txtWarehouseName.Name = "txtWarehouseName";
            this.txtWarehouseName.Size = new Size(250, 23);
            
            this.txtWarehouseAddress.Location = new Point(140, 50);
            this.txtWarehouseAddress.Name = "txtWarehouseAddress";
            this.txtWarehouseAddress.Size = new Size(350, 23);
            
            this.cmbManager.Location = new Point(140, 80);
            this.cmbManager.Name = "cmbManager";
            this.cmbManager.Size = new Size(200, 23);
            this.cmbManager.DropDownStyle = ComboBoxStyle.DropDownList;
            
            this.txtWarehouseDescription.Location = new Point(140, 110);
            this.txtWarehouseDescription.Name = "txtWarehouseDescription";
            this.txtWarehouseDescription.Size = new Size(350, 23);
            this.txtWarehouseDescription.Multiline = true;
            
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Location = new Point(140, 140);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Text = "Active";
            this.chkIsActive.Checked = true;
            
            // Buttons
            this.btnSave.Location = new Point(140, 180);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(100, 30);
            this.btnSave.Text = "Save";
            this.btnSave.Click += BtnSave_Click;
            
            this.btnCancel.Location = new Point(250, 180);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(100, 30);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += BtnCancel_Click;
            
            // Add controls to panel
            this.editPanel.Controls.Add(lblName);
            this.editPanel.Controls.Add(lblAddress);
            this.editPanel.Controls.Add(lblManager);
            this.editPanel.Controls.Add(lblDescription);
            this.editPanel.Controls.Add(this.txtWarehouseName);
            this.editPanel.Controls.Add(this.txtWarehouseAddress);
            this.editPanel.Controls.Add(this.cmbManager);
            this.editPanel.Controls.Add(this.txtWarehouseDescription);
            this.editPanel.Controls.Add(this.chkIsActive);
            this.editPanel.Controls.Add(this.btnSave);
            this.editPanel.Controls.Add(this.btnCancel);
            
            // Action buttons below the grid
            FlowLayoutPanel actionPanel = new FlowLayoutPanel();
            actionPanel.Location = new Point(10, 570);
            actionPanel.Name = "actionPanel";
            actionPanel.Size = new Size(780, 40);
            actionPanel.Padding = new Padding(0);
            
            Button btnAdd = new Button();
            btnAdd.Text = "Add Warehouse";
            btnAdd.Size = new Size(120, 30);
            btnAdd.Click += BtnAdd_Click;
            
            Button btnEdit = new Button();
            btnEdit.Text = "Edit Warehouse";
            btnEdit.Size = new Size(120, 30);
            btnEdit.Click += BtnEdit_Click;
            
            Button btnDelete = new Button();
            btnDelete.Text = "Deactivate";
            btnDelete.Size = new Size(120, 30);
            btnDelete.Click += BtnDelete_Click;
            
            Button btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Size = new Size(80, 30);
            btnRefresh.Click += BtnRefresh_Click;
            
            actionPanel.Controls.Add(btnAdd);
            actionPanel.Controls.Add(btnEdit);
            actionPanel.Controls.Add(btnDelete);
            actionPanel.Controls.Add(btnRefresh);
            
            // Add to user control
            this.Controls.Add(this.dataGridViewWarehouses);
            this.Controls.Add(actionPanel);
            this.Controls.Add(this.editPanel);
            
            // Hide edit panel initially
            this.editPanel.Visible = false;
        }
        
        private void LoadWarehouses()
        {
            try
            {
                // Determine if user is admin (simplified check)
                bool isAdmin = currentUser.UserID == 1;
                
                List<Warehouse> warehouses;
                if (isAdmin)
                {
                    // Admin sees all warehouses
                    warehouses = WarehouseManager.GetWarehouses(true);
                }
                else
                {
                    // Non-admin users only see warehouses assigned to them
                    warehouses = WarehouseManager.GetWarehousesByUser(currentUser.UserID);
                }
                
                warehouseBindingSource.DataSource = warehouses;
                dataGridViewWarehouses.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading warehouses: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void LoadManagers()
        {
            try
            {
                // Check if UserManager.GetUsers is available
                List<User> users;
                try
                {
                    // Try to get all users who can be managers
                    users = UserManager.GetUsers();
                }
                catch (Exception)
                {
                    // Fallback to creating a single user (current user) if method not implemented yet
                    users = new List<User> { currentUser };
                }
                
                cmbManager.DisplayMember = "Name";
                cmbManager.ValueMember = "UserID";
                cmbManager.DataSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ClearForm()
        {
            txtWarehouseName.Text = string.Empty;
            txtWarehouseAddress.Text = string.Empty;
            txtWarehouseDescription.Text = string.Empty;
            chkIsActive.Checked = true;
            editingWarehouseId = null;
            
            // Default manager to current user
            foreach (var item in cmbManager.Items)
            {
                if ((item as User)?.UserID == currentUser.UserID)
                {
                    cmbManager.SelectedItem = item;
                    break;
                }
            }
        }
        
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            editingWarehouseId = null;
            editPanel.Visible = true;
        }
        
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewWarehouses.SelectedRows.Count > 0)
            {
                var selectedWarehouse = dataGridViewWarehouses.SelectedRows[0].DataBoundItem as Warehouse;
                if (selectedWarehouse != null)
                {
                    // Populate form with selected warehouse
                    txtWarehouseName.Text = selectedWarehouse.Name;
                    txtWarehouseAddress.Text = selectedWarehouse.Address;
                    txtWarehouseDescription.Text = selectedWarehouse.Description;
                    chkIsActive.Checked = selectedWarehouse.IsActive;
                    editingWarehouseId = selectedWarehouse.WarehouseID;
                    
                    // Set the manager
                    foreach (var item in cmbManager.Items)
                    {
                        if ((item as User)?.UserID == selectedWarehouse.ManagerUserID)
                        {
                            cmbManager.SelectedItem = item;
                            break;
                        }
                    }
                    
                    editPanel.Visible = true;
                }
            }
            else
            {
                MessageBox.Show("Please select a warehouse to edit.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewWarehouses.SelectedRows.Count > 0)
            {
                var selectedWarehouse = dataGridViewWarehouses.SelectedRows[0].DataBoundItem as Warehouse;
                if (selectedWarehouse != null)
                {
                    if (MessageBox.Show("Are you sure you want to deactivate this warehouse?", 
                        "Confirm Deactivation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            WarehouseManager.DeleteWarehouse(selectedWarehouse.WarehouseID);
                            LoadWarehouses();
                            MessageBox.Show("Warehouse has been deactivated.", "Success", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deactivating warehouse: {ex.Message}", "Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a warehouse to deactivate.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadWarehouses();
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(txtWarehouseName.Text))
                {
                    MessageBox.Show("Please enter a warehouse name.", "Missing Information", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (cmbManager.SelectedItem == null)
                {
                    MessageBox.Show("Please select a manager.", "Missing Information", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Get manager ID
                int managerId = ((User)cmbManager.SelectedItem).UserID;
                
                // Create warehouse object
                var warehouse = new Warehouse
                {
                    Name = txtWarehouseName.Text.Trim(),
                    Address = txtWarehouseAddress.Text.Trim(),
                    ManagerUserID = managerId,
                    Description = txtWarehouseDescription.Text.Trim(),
                    IsActive = chkIsActive.Checked
                };
                
                if (editingWarehouseId.HasValue)
                {
                    // Update existing warehouse
                    warehouse.WarehouseID = editingWarehouseId.Value;
                    WarehouseManager.UpdateWarehouse(warehouse);
                    MessageBox.Show("Warehouse updated successfully.", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Add new warehouse
                    WarehouseManager.AddWarehouse(warehouse);
                    MessageBox.Show("Warehouse added successfully.", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                // Refresh the list and hide the panel
                LoadWarehouses();
                editPanel.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving warehouse: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            editPanel.Visible = false;
            ClearForm();
        }
    }
}
