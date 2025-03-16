using System;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class StockEditorForm : Form
    {
        private readonly Inventory _inventory;

        public StockEditorForm(Inventory inventory)
        {
            InitializeComponent();
            _inventory = inventory;
            txtStockQuantity.Text = inventory.StockQuantity.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _inventory.StockQuantity = int.Parse(txtStockQuantity.Text);
            InventoryManager.UpdateStock(_inventory.ProductID, _inventory.StockQuantity);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
