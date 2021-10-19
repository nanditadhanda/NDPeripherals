using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NDPeripheralsInventory
{
    public partial class salesReport : Form
    {
        public salesReport()
        {
            InitializeComponent();
        }

        private void salesReport_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'salesDataset.sales' table. You can move, or remove it, as needed.
            this.salesTableAdapter.Fill(this.salesDataset.sales);

            this.reportViewer1.RefreshReport();
        }
    }
}
