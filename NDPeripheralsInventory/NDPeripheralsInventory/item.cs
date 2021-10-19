/*
    Author:         Nandita Dhanda
    Email:          nandita.nd64@gmail.com
    Student ID:     SUKD1702275
    Course:         TCS3274 - Windows Programming (Group C)

    Project:        Inventory Management System
    Page:           item class
    Description:    for item class 
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
using System.Runtime.InteropServices.WindowsRuntime;

namespace NDPeripheralsInventory
{
    class item : Inventory
    {
        //create database connection
        SqlConnection dbcon = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter sda = new SqlDataAdapter();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        String query; //variable for SQL query


       
        public item() {
            //database connection string
            dbcon.ConnectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = 
            C:\db\inventorydb.mdf; Integrated Security = True; Connect Timeout = 30");
        }
        public String totalStock()
        {
            //calculate total number of items in stock

            string query = "SELECT SUM (itemStock) FROM items"; //query
            sda = new SqlDataAdapter(query, dbcon);
            DataTable source = new DataTable();
            sda.Fill(source);
            string totalStockVal = source.Rows[0][0].ToString() + " items"; //total stock variable data
            return totalStockVal; //display result
        }
        public String totalInventory()
        {
            //calculate total inventory value -  total inventory is the total selling value of all of the items currently in the store

            string query = "SELECT SUM (itemStockValue) FROM items"; //query
            sda = new SqlDataAdapter(query, dbcon);
            DataTable source = new DataTable();
            sda.Fill(source);
            String totalInventory = source.Rows[0][0].ToString(); //display resul
            return totalInventory;
        }

        public void addItem(string itemName, string itemCategory,int itemOrdered,
                            int itemStock, int itemSold, decimal supplierPrice, 
                            decimal sellingPrice, decimal stockValue, string itemDesc)
        {
            //method to add item to database
            
            //Inserting data into SQL Database
            dbcon.Open(); //open database connection
            cmd = new SqlCommand("insert into items values( '" + itemName + "','"+ itemCategory + "', '" 
                                                              + itemOrdered + "','" + itemStock + "', '" 
                                                              + itemSold + "', '" + supplierPrice + 
                                                          "', '" + sellingPrice + "', '" + stockValue + 
                                                          "','" + itemDesc + "')", dbcon); //insert query

            try
            {
                cmd.ExecuteNonQuery(); //execute query
                MessageBox.Show("Item successfully added", "Successful Entry", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information); //success message
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

        }
        public void updateItem(int itemID, string itemName, string itemCategory, 
            int itemOrdered,int  itemStock, int itemSold, decimal supplierPrice, 
            decimal sellingPrice, decimal stockValue, string itemDesc)
        {
            //method to update item in database

            dbcon.Open(); //open database connection

            query = "update items set itemName ='" + itemName + "', itemCat ='" 
                     + itemCategory + "', itemOrdered ='" + itemOrdered + "', itemStock = '" 
                     + itemStock + "', itemSupplierPrice ='" + supplierPrice + "', itemSellingPrice ='"
                     + sellingPrice + "', itemStockValue = '" + stockValue + "', itemDesc ='" + itemDesc 
                     + "' where itemID = '" + itemID + "'"; //query

            cmd = new SqlCommand(query, dbcon); 
    
            try
            {
                cmd.ExecuteNonQuery(); // execute sql query
                MessageBox.Show("Item successfully updated", "Update Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information); //success message

            }
            catch (Exception)
            {
                MessageBox.Show("An error occured while updating item. Item not updated", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error); //error message
            }
            finally
            {
                dbcon.Close(); //close database
            }
        }

        public void deleteItem(int id)
        {
            //method to delete item from database

            dbcon.Open(); //open database connection
            query = "delete from items where itemID = '" + id + "'"; //delete query
            cmd = new SqlCommand(query, dbcon);

            try
            {
                cmd.ExecuteNonQuery(); //execute sql query
                MessageBox.Show("Item Deleted Successfully", "Delete Successful", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information); //success message
            }
            catch (Exception)
            {
                MessageBox.Show("An error occured while trying to delete item. Item not deleted", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //error message
            }
            finally
            {
                dbcon.Close(); //close database connection
                
            }
        }

       
    }

}
