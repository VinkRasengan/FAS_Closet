using System;
using System.ComponentModel;
using System.Windows.Forms;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public partial class ManufacturerForm : Form
    {
        private readonly Manufacturer _manufacturer;
        private readonly bool _isEditMode;
        private readonly ErrorProvider _errorProvider = new ErrorProvider();

        /// <summary>
        /// Gets the manufacturer after form is submitted
        /// </summary>
        public Manufacturer Manufacturer => _manufacturer;

        public ManufacturerForm(Manufacturer manufacturer = null)
        {
            InitializeComponent();
            _manufacturer = manufacturer ?? new Manufacturer();
            _isEditMode = manufacturer != null;
            
            // Set form title
            this.Text = _isEditMode ? "Edit Manufacturer" : "Add New Manufacturer";
            lblHeader.Text = _isEditMode ? "Edit Manufacturer" : "Add New Manufacturer";
            btnSave.Text = _isEditMode ? "Update" : "Add";
            
            // Set up validation
            txtManufacturerName.Validating += TxtManufacturerName_Validating;
            
            // Register events
            this.Load += ManufacturerForm_Load;
            this.FormClosing += ManufacturerForm_FormClosing;
        }

        private void ManufacturerForm_Load(object sender, EventArgs e)
        {
            // If editing, fill the form with manufacturer data
            if (_isEditMode)
            {
                txtManufacturerName.Text = _manufacturer.ManufacturerName;
                txtDescription.Text = _manufacturer.Description;
            }
            
            // Focus on the first field
            txtManufacturerName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                try
                {
                    // Check if manufacturer name already exists (only for new manufacturers)
                    if (!_isEditMode && ProductManager.IsManufacturerNameTaken(txtManufacturerName.Text.Trim()))
                    {
                        _errorProvider.SetError(txtManufacturerName, "Manufacturer name already exists");
                        return;
                    }
                    
                    // Update manufacturer object
                    _manufacturer.ManufacturerName = txtManufacturerName.Text.Trim();
                    _manufacturer.Description = txtDescription.Text.Trim();
                    
                    if (_isEditMode)
                    {
                        // Update existing manufacturer
                        ProductManager.UpdateManufacturer(_manufacturer);
                    }
                    else
                    {
                        // Add new manufacturer
                        ProductManager.AddManufacturer(_manufacturer);
                    }
                    
                    DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving manufacturer: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        
        private void TxtManufacturerName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtManufacturerName.Text))
            {
                _errorProvider.SetError(txtManufacturerName, "Manufacturer name is required");
                e.Cancel = true;
            }
            else
            {
                _errorProvider.SetError(txtManufacturerName, "");
            }
        }

        private void ManufacturerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _errorProvider.Dispose();
        }
    }
}