/*
    Author:         Nandita Dhanda
    Email:          nandita.nd64@gmail.com
    Student ID:     SUKD1702275
    Course:         TCS3274 - Windows Programming (Group C)

    Project:        Inventory Management System
    Page:           Class for Login page
    Description:    Allow authorized user to log into the system  
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
using System.Data.SqlClient;

namespace NDPeripheralsInventory
{
    public partial class Login : Form
    {

        //create database connection
        SqlConnection dbcon = new SqlConnection();
        SqlCommand cmd = new SqlCommand();

        public Login()
        {
            InitializeComponent();

            //database connection string
            dbcon.ConnectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\db\inventorydb.mdf; Integrated Security = True; Connect Timeout = 30");
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {

            dbcon.Open(); //open database connection
            cmd.Connection = dbcon;
            cmd.CommandText = "SELECT * FROM login"; //sql query
            SqlDataReader dbdata = cmd.ExecuteReader(); //read data

            //if system is able to read data from database
            if (dbdata.Read())
            {
                //username and password match user account
                if (usernameTxt.Text.Equals(dbdata["username"].ToString()) 
                    && passwordTxt.Text.Equals(dbdata["password"].ToString()))
                {
                    //login successfull - go to main page
                    //  ndpmain mainpage = new ndpmain();
                    //  mainpage.Show();

                    mainSystem obj = new mainSystem(); //create new object for dashboard
                    obj.Show();
                    this.Hide();

                    //clear textbox entries
                    usernameTxt.Clear();
                    passwordTxt.Clear();
                }
                else
                {
                    //if log in attempt fails
                    MessageBox.Show("Error: Invalid username and password entered", 
                                    "Invalid Login Details", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    usernameTxt.Clear(); //clear username
                    usernameTxt.Focus(); //make cursor focused on username field
                    passwordTxt.Clear(); //clear password field
                }
            }        
            dbcon.Close(); //close database connection
        }

        private void Login_Load(object sender, EventArgs e)
        {
            //default button if enter key is pressed
            this.AcceptButton = this.loginBtn;

        }

        private void clear_btn_Click(object sender, EventArgs e)
        {
            //clear textbox entries
            usernameTxt.Clear();
            passwordTxt.Clear();

        }

        private void exit_Click(object sender, EventArgs e)
        {
            //confirm if user wants to exit or not
            DialogResult result = MessageBox.Show("Are you sure you want to exit the application?", "Exit Confirmation", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                //exit application
                Application.Exit(); //exit application
            }
            else
            {
                return;
            }
        }


    }
}
