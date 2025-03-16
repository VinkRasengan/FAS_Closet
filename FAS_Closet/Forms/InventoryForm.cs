using System;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class InventoryForm : Form
    {
        public InventoryForm()
        {
            InitializeComponent();
            LoadInventory();
        }

        private void LoadInventory()
        {
            var inventory = InventoryManager.GetLowStockProducts();
            if (inventory != null)
            {
                dataGridViewInventory.DataSource = inventory;
            }
        }

        private void btnUpdateStock_Click(object sender, EventArgs e)
        {
            if (dataGridViewInventory.SelectedRows.Count > 0)
            {
                var selectedInventory = dataGridViewInventory.SelectedRows[0].DataBoundItem as Inventory;
                if (selectedInventory != null)
                {
                    var stockEditorForm = new StockEditorForm(selectedInventory);
                    if (stockEditorForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadInventory();
                    }
                }
            }
        }

        private void btnSetThreshold_Click(object sender, EventArgs e)
        {
            if (dataGridViewInventory.SelectedRows.Count > 0)
            {
                var selectedInventory = dataGridViewInventory.SelectedRows[0].DataBoundItem as Inventory;
                if (selectedInventory != null)
                {
                    var thresholdEditorForm = new ThresholdEditorForm(selectedInventory);
                    if (thresholdEditorForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadInventory();
                    }
                }
            }
        }

        private void btnCheckLowStock_Click(object sender, EventArgs e)
        {
            LoadInventory();
        }
    }
}
