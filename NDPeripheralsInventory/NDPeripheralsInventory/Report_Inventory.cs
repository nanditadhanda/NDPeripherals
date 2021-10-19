/*
    Author:         Nandita Dhanda
    Email:          nandita.nd64@gmail.com
    Student ID:     SUKD1702275
    Course:         TCS3274 - Windows Programming (Group C)

    Project:        Inventory Management System
    Page:           Inventory Report
    Description:    Page to generate and display inventory report
    Date:           14/08/2020
 */

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
    public partial class Report_Inventory : Form
    {
        public Report_Inventory()
        {
            InitializeComponent();
        }

        private void Report_Inventory_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'inventoryDataset.items' table. You can move, or remove it, as needed.
            this.itemsTableAdapter.Fill(this.inventoryDataset.items); //connect with dataset

            this.reportViewer1.RefreshReport(); //display inventory report in report viewer elemnt
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }
    }
}
