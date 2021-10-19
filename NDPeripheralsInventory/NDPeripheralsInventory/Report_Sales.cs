/*
    Author:         Nandita Dhanda
    Email:          nandita.nd64@gmail.com
    Student ID:     SUKD1702275
    Course:         TCS3274 - Windows Programming (Group C)

    Project:        Inventory Management System
    Page:           Sales Report
    Description:    Page to generate and display sales report
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
    public partial class Report_Sales : Form
    {
        public Report_Sales()
        {
            InitializeComponent();
        }

        private void Report_Sales_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'salesDataset.sales' table. You can move, or remove it, as needed.
            this.salesTableAdapter.Fill(this.salesDataset.sales); //sales dataset

            this.reportViewer1.RefreshReport(); //refresh sales report
        }
    }
}
