/*
    Author:         Nandita Dhanda
    Email:          nandita.nd64@gmail.com
    Student ID:     SUKD1702275
    Course:         TCS3274 - Windows Programming (Group C)

    Project:        Inventory Management System
    Page:           Sales Class
    Description:    For sales class 
    Date:           14/08/2020
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NDPeripheralsInventory
{
    class sales : Inventory
    {
        //create database connection
        SqlConnection dbcon = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter sda = new SqlDataAdapter();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        String query; //variable for SQL query


        //sales variables
        public int saleID;
        public int saleNoOfItems = 0;
        public decimal saleValue;

        public sales() {
            //database connection string
            dbcon.ConnectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; 
            AttachDbFilename = C:\db\inventorydb.mdf; Integrated Security = True; Connect Timeout = 30");
        }

        public string balanceCalc(decimal tenderedAmount, decimal totalAmount)
        {
            if (tenderedAmount > totalAmount)
            {
                return (tenderedAmount - totalAmount).ToString();
            }

            else
            {
               return "0.00";
            }
        }
        public void newSale(DateTime todayDate, decimal saleValue, int saleNoOfItems)
        {
            // add sale data to sales SQL table
            dbcon.Open(); //open connection
            query = "insert into sales values( '" + todayDate + "','" + saleNoOfItems + "', '" + saleValue + "')";
            cmd = new SqlCommand(query, dbcon); //insert query

            try
            {
                cmd.ExecuteNonQuery(); //execute query
                MessageBox.Show("Sales Transaction Completed Successfully.\n\nNumber of Items Purchased: "
                    +saleNoOfItems+"\nTotal Sale Value: RM"+saleValue, "Purchase Summary", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information); //success message and purchase summary
            }
            catch (Exception)
            {
                MessageBox.Show("Error: An error occured while adding new item. Item not added.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error); //error message
            }
            finally
            {
                dbcon.Close(); //close database connection
                
            }


            dbcon.Close();
        }

        public void updateStockValue(int itemID, int quantitySold)
        {
            //connect to database
            dbcon.Open();
            query = "select * from items where itemID = '" + itemID + "'"; // select data from database to retrieve existing values

            cmd = new SqlCommand(query, dbcon);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                //Store values in variable
                int oldStockVal = int.Parse(dr[4].ToString()); //old stock value
                int oldSoldVal = int.Parse(dr[5].ToString()); // old value of items sold
                decimal oldTotalStockVal = decimal.Parse(dr[8].ToString()); //total stock value
                sellingPrice = decimal.Parse(dr[7].ToString());

                itemStock = oldStockVal - quantitySold; // calculate items in stock remaining
                itemSold = oldSoldVal + quantitySold;   // calculate total number of items sold
                stockValue = itemStock * sellingPrice;  // calculate total selling value of items in stock

            }
            dbcon.Close(); // close current connection

            //updating stock value in database table
            dbcon.Open(); //open new connection
            query = "update items set itemStock = '" + itemStock + "', itemSold ='" + itemSold + "', itemSellingPrice ='" 
                + sellingPrice + "', itemStockValue = '" + stockValue + "' " +
                "where itemID = '" + itemID + "'"; //query
            cmd = new SqlCommand(query, dbcon);

            try
            {
                cmd.ExecuteNonQuery(); // execute sql query


            }
            catch (Exception ex)
            {
              
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dbcon.Close(); //close database connection              

            }

        }


        
    }
}
