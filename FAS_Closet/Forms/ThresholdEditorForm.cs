using System;
using System.Windows.Forms;
using FASCloset.Services;
using FASCloset.Models;

namespace FASCloset.Forms
{
    public partial class ThresholdEditorForm : Form
    {
        private readonly Inventory _inventory;

        public ThresholdEditorForm(Inventory inventory)
        {
            InitializeComponent();
            _inventory = inventory;
            txtThreshold.Text = inventory.MinimumStockThreshold.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _inventory.MinimumStockThreshold = int.Parse(txtThreshold.Text);
            InventoryManager.SetMinimumStockThreshold(_inventory.ProductID, _inventory.MinimumStockThreshold);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
