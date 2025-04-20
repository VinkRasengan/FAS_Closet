using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using FASCloset.Models;
using FASCloset.Services;

namespace FASCloset.Forms
{
    public class InventoryItemForm : Form
    {
        private readonly ErrorProvider _errorProvider = new ErrorProvider();
        private bool _isEditMode = false;
        private Product _product;
        private List<Category> _categories;
        private List<Manufacturer> _manufacturers;

        public Product Product => _product;

        public InventoryItemForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;

            // Load categories and manufacturers
            LoadCategoriesAndManufacturers();

            _product = new Product();
            this.Text = "Thêm sản phẩm mới";

            this.FormClosing += InventoryItemForm_FormClosing;
        }

        public InventoryItemForm(Product product) : this()
        {
            _isEditMode = true;
            _product = product;
            this.Text = "Chỉnh sửa sản phẩm";
            LoadProductData();
        }

        private TextBox txtProductName;
        private TextBox txtProductPrice;
        private TextBox txtProductStock;
        private TextBox txtProductDescription;
        private ComboBox cmbCategory;
        private ComboBox cmbManufacturer;
        private CheckBox chkIsActive;
        private Button btnSave;
        private Button btnCancel;

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Size = new Size(500, 500);
            this.BackColor = Color.White;

            // Create labels
            Label lblProductName = new Label
            {
                Text = "Tên sản phẩm:",
                Location = new Point(30, 30),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 9.5F)
            };

            Label lblCategory = new Label
            {
                Text = "Danh mục:",
                Location = new Point(30, 70),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 9.5F)
            };

            Label lblManufacturer = new Label
            {
                Text = "Nhà sản xuất:",
                Location = new Point(30, 110),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 9.5F)
            };

            Label lblPrice = new Label
            {
                Text = "Giá:",
                Location = new Point(30, 150),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 9.5F)
            };

            Label lblStock = new Label
            {
                Text = "Số lượng tồn:",
                Location = new Point(30, 190),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 9.5F)
            };

            Label lblDescription = new Label
            {
                Text = "Mô tả:",
                Location = new Point(30, 230),
                Size = new Size(120, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 9.5F)
            };

            // Create controls
            txtProductName = new TextBox
            {
                Location = new Point(150, 30),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 9.5F)
            };
            txtProductName.Validating += TxtProductName_Validating;

            cmbCategory = new ComboBox
            {
                Location = new Point(150, 70),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 9.5F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategory.Validating += CmbCategory_Validating;

            cmbManufacturer = new ComboBox
            {
                Location = new Point(150, 110),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 9.5F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            txtProductPrice = new TextBox
            {
                Location = new Point(150, 150),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 9.5F)
            };
            txtProductPrice.Validating += TxtProductPrice_Validating;

            txtProductStock = new TextBox
            {
                Location = new Point(150, 190),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 9.5F)
            };
            txtProductStock.Validating += TxtProductStock_Validating;

            txtProductDescription = new TextBox
            {
                Location = new Point(150, 230),
                Size = new Size(300, 100),
                Font = new Font("Segoe UI", 9.5F),
                Multiline = true
            };
            txtProductDescription.Validating += TxtProductDescription_Validating;

            chkIsActive = new CheckBox
            {
                Text = "Sản phẩm đang hoạt động",
                Location = new Point(150, 340),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 9.5F),
                Checked = true
            };

            // Save button
            btnSave = new Button
            {
                Text = "Lưu",
                Location = new Point(150, 380),
                Size = new Size(130, 35),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            // Cancel button
            btnCancel = new Button
            {
                Text = "Hủy",
                Location = new Point(300, 380),
                Size = new Size(130, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += BtnCancel_Click;

            // Add controls to form
            this.Controls.Add(lblProductName);
            this.Controls.Add(txtProductName);
            this.Controls.Add(lblCategory);
            this.Controls.Add(cmbCategory);
            this.Controls.Add(lblManufacturer);
            this.Controls.Add(cmbManufacturer);
            this.Controls.Add(lblPrice);
            this.Controls.Add(txtProductPrice);
            this.Controls.Add(lblStock);
            this.Controls.Add(txtProductStock);
            this.Controls.Add(lblDescription);
            this.Controls.Add(txtProductDescription);
            this.Controls.Add(chkIsActive);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadCategoriesAndManufacturers()
        {
            try
            {
                // Load categories
                _categories = CategoryManager.GetCategories();
                cmbCategory.DataSource = null;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryID";
                cmbCategory.DataSource = _categories;
                if (_categories.Count > 0)
                    cmbCategory.SelectedIndex = 0;

                // Load manufacturers
                _manufacturers = ManufacturerManager.GetManufacturers();
                cmbManufacturer.DataSource = null;
                cmbManufacturer.DisplayMember = "ManufacturerName";
                cmbManufacturer.ValueMember = "ManufacturerID";
                cmbManufacturer.DataSource = _manufacturers;
                if (_manufacturers.Count > 0)
                    cmbManufacturer.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProductData()
        {
            if (_product != null)
            {
                txtProductName.Text = _product.ProductName;
                txtProductPrice.Text = _product.Price.ToString("N0");
                txtProductStock.Text = _product.Stock.ToString();
                txtProductDescription.Text = _product.Description;
                chkIsActive.Checked = _product.IsActive;

                // Select the correct category
                for (int i = 0; i < cmbCategory.Items.Count; i++)
                {
                    var category = cmbCategory.Items[i] as Category;
                    if (category != null && category.CategoryID == _product.CategoryID)
                    {
                        cmbCategory.SelectedIndex = i;
                        break;
                    }
                }

                // Select the correct manufacturer
                for (int i = 0; i < cmbManufacturer.Items.Count; i++)
                {
                    var manufacturer = cmbManufacturer.Items[i] as Manufacturer;
                    if (manufacturer != null && manufacturer.ManufacturerID == _product.ManufacturerID)
                    {
                        cmbManufacturer.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                try
                {
                    _product.ProductName = txtProductName.Text.Trim();
                    _product.CategoryID = Convert.ToInt32(cmbCategory.SelectedValue);
                    _product.ManufacturerID = Convert.ToInt32(cmbManufacturer.SelectedValue);
                    _product.Price = decimal.Parse(txtProductPrice.Text.Replace(",", ""));
                    _product.Stock = int.Parse(txtProductStock.Text);
                    _product.Description = txtProductDescription.Text.Trim();
                    _product.IsActive = chkIsActive.Checked;

                    if (_isEditMode)
                    {
                        // Update existing product
                        ProductManager.UpdateProduct(_product);
                    }
                    else
                    {
                        // Add new product
                        ProductManager.AddProduct(_product);
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void InventoryItemForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _errorProvider.Clear();
        }

        #region Validation Methods
        private void TxtProductName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                _errorProvider.SetError(txtProductName, "Vui lòng nhập tên sản phẩm");
                e.Cancel = true;
            }
            else
            {
                _errorProvider.SetError(txtProductName, "");
            }
        }

        private void CmbCategory_Validating(object sender, CancelEventArgs e)
        {
            if (cmbCategory.SelectedIndex == -1)
            {
                _errorProvider.SetError(cmbCategory, "Vui lòng chọn danh mục");
                e.Cancel = true;
            }
            else
            {
                _errorProvider.SetError(cmbCategory, "");
            }
        }

        private void TxtProductPrice_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductPrice.Text))
            {
                _errorProvider.SetError(txtProductPrice, "Vui lòng nhập giá sản phẩm");
                e.Cancel = true;
            }
            else
            {
                decimal price;
                if (!decimal.TryParse(txtProductPrice.Text.Replace(",", ""), out price) || price <= 0)
                {
                    _errorProvider.SetError(txtProductPrice, "Giá không hợp lệ. Vui lòng nhập số dương");
                    e.Cancel = true;
                }
                else
                {
                    _errorProvider.SetError(txtProductPrice, "");
                }
            }
        }

        private void TxtProductStock_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductStock.Text))
            {
                _errorProvider.SetError(txtProductStock, "Vui lòng nhập số lượng tồn");
                e.Cancel = true;
            }
            else
            {
                int stock;
                if (!int.TryParse(txtProductStock.Text, out stock) || stock < 0)
                {
                    _errorProvider.SetError(txtProductStock, "Số lượng không hợp lệ. Vui lòng nhập số không âm");
                    e.Cancel = true;
                }
                else
                {
                    _errorProvider.SetError(txtProductStock, "");
                }
            }
        }

        private void TxtProductDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductDescription.Text))
            {
                _errorProvider.SetError(txtProductDescription, "Vui lòng nhập mô tả sản phẩm");
                e.Cancel = true;
            }
            else
            {
                _errorProvider.SetError(txtProductDescription, "");
            }
        }
        #endregion
    }
}