/*
    Author:         Nandita Dhanda
    Email:          nandita.nd64@gmail.com
    Student ID:     SUKD1702275
    Course:         TCS3274 - Windows Programming (Group C)

    Project:        Inventory Management System
    Page:           Class for Main System Interface
    Description:    All system functionalities take place here
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
using System.Data.SqlClient; //required to connect with SQL

namespace NDPeripheralsInventory
{
    public partial class mainSystem : Form
    {

        //create database connection
        SqlConnection dbcon = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter sda = new SqlDataAdapter();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        String query; //variable for SQL query

        //variable declaration 
        DateTime todayDate = DateTime.Today;
        DateTime timeNow = DateTime.Now;

        //item variables
        int itemID; //Item ID
        String itemName; // Item Name
        String itemCategory; // Item category
        String itemDesc; // Item description
        int itemOrdered; //Items Ordered
        int itemStock; // intitially, items in stock value is same as items ordered
        int itemSold = 0;      //  initial value of itemSold will be 0  
        decimal supplierPrice;
        decimal sellingPrice;
        decimal stockValue;
        private string lt;



        //sales variables
        int saleID;
        int saleNoOfItems = 0;
        decimal saleValue;

        //creating class instances
        item invItem = new item(); //create item object
        sales invSale = new sales();

        public mainSystem()
        {
            InitializeComponent();

            //database connection string
            dbcon.ConnectionString = (@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = 
                                C:\db\inventorydb.mdf; Integrated Security = True; Connect Timeout = 30");

        }

        private void mainSystem_Load(object sender, EventArgs e)
        {
            
            displayData(); //display datagrid
            displayItemList(); //display items in item list
            totalInventory(); //display total value of items
            totalStock(); //display total number of stock
          //  add_itemNameTxt.Focus();


            dateTxt.Text = todayDate.ToString("D");
            dateTimeTxt.Text = todayDate.ToString("dd/MM/yyyy") + timeNow.ToString("   hh:mm tt");

            
        }

        //method declaration

        private void displayData()
        {
            
            dbcon.Open(); //open database connection
            sda = new SqlDataAdapter("select * from items order by itemName ASC", dbcon); //sql query
            dt = new DataTable();
            sda.Fill(dt); // fill data table in SQL data adapter
            inventoryDataGrid.DataSource = dt; //populating datagrid with data from inventory database
            dbcon.Close();
        }


          
           private void displayItemList()
           {
            //display list of items available in inventory

            itemList.Items.Clear(); //clear any initial items

               dbcon.Open(); //open database connection
               cmd.CommandText = "select * from items"; //query
               cmd.Connection = dbcon;
               dr = cmd.ExecuteReader(); //execute query
               while (dr.Read())
               {
                   ListViewItem lv = new ListViewItem(dr[0].ToString()); //new ListViewItem object
                   lv.SubItems.Add(dr[1].ToString()); //add data to listview
                   itemList.Items.Add(lv); //populate items in listview table

               }

               dbcon.Close(); //close database connection
           }

          private void clearData()
          {
              //clear data in text boxes
              itemIDTxt.Clear();
              add_itemNameTxt.Clear();
              add_descriptionTxt.Clear();
              add_itemsOrderedTxt.Clear();
              add_supplierPriceTxt.Clear();
              add_sellingPriceTxt.Clear();
              edit_itemNameTxt.Clear();
              edit_descriptionTxt.Clear();
              edit_itemsInStockTxt.Clear();
              edit_supplierPriceTxt.Clear();
              edit_sellingPriceTxt.Clear();
            edit_itemsOrdered.Text = "";
            edit_totalStock.Text = "";
              edit_categoryTxt.SelectedItem = 1;
              oldStockVal.Text = "";
              itemOrderedOld.Text = "";
          }


        /*Search Data*/

        private void searchTxt_TextChanged(object sender, EventArgs e)
        {
            searchData(searchTxt.Text);
        }

        //search datagrid
        private void searchData(string search)
        {
            //populate datagrid with search result

            dbcon.Open(); //open DB connection
            query = "select * from items where itemName like '%" + search + "%'"; //sql query
            sda = new SqlDataAdapter(query, dbcon); //execute query
            dt = new DataTable();
            sda.Fill(dt);
            inventoryDataGrid.DataSource = dt;
            dbcon.Close();//close database connection
        }

        //search list view table
        private void searchListTxt_TextChanged(object sender, EventArgs e)
        {
            //opening connection
            dbcon.Open();
            try
            {
                //initialize a new instance of sqlcommand
                cmd = new SqlCommand();
                //set a connection used by this instance of sqlcommand
                cmd.Connection = dbcon;
                //set the sql statement to execute at the data source
                cmd.CommandText = "Select * FROM items WHERE itemName LIKE '%" + searchListTxt.Text + "%'";

                //initialize a new instance of sqlDataAdapter
                sda = new SqlDataAdapter();
                //set the sql statement or stored procedure to execute at the data source
                sda.SelectCommand = cmd;
                //initialize a new instance of DataTable
                dt = new DataTable();
                //add or resfresh rows in the certain range in the datatable to match those in the data source.
                sda.Fill(dt);
                //add the data source to display the data in the ListView
                itemList.Items.Clear();
                foreach (DataRow r in dt.Rows)
                {
                    var list = itemList.Items.Add(r.Field<int>(0).ToString());
                    list.SubItems.Add(r.Field<string>(1));
                }
            }
            catch (Exception ex)
            {
                //catching error 
                MessageBox.Show(ex.Message);
            }
            //release all resources used by the component
            sda.Dispose();
            //dr.Close();
            //clossing connection
            dbcon.Close();
        }



        /*Inventory Data*/

        private void totalInventory()
        {
            //calculate total inventory value -  total inventory is the total selling value of all of the items currently in the store
            totalInventoryTxt.Text = invItem.totalInventory(); //display total inventory value
        }

        private void totalStock()
        {
            //calculate total number of items in stock
            totalStockTxt.Text = invItem.totalStock(); //display total stock value
        }

        /*Manage Inventory*/

        private void addBtn_Click(object sender, EventArgs e)
        {
            //add new item to inventory button trigger event

            if (add_itemNameTxt.Text != "" && add_categoryTxt.Text != "" 
               && add_itemsOrderedTxt.Text != "" && add_supplierPriceTxt.Text != "" && sellingPrice.ToString() != "")
            {
                try
                {
                    //assigning data from textboxes into variables
                    itemName = add_itemNameTxt.Text; // Item Name
                    itemCategory = add_categoryTxt.Text; // Item category
                    itemDesc = add_descriptionTxt.Text; // Item description
                    itemOrdered = int.Parse(add_itemsOrderedTxt.Text); //Items Ordered
                    itemStock = int.Parse(add_itemsOrderedTxt.Text); // intitially, items in stock value is same as items ordered
                    itemSold = 0; //intitially, items sold is 0           
                    supplierPrice = decimal.Parse(add_supplierPriceTxt.Text); //supplier price 
                    sellingPrice = decimal.Parse(add_sellingPriceTxt.Text); //selling price
                    stockValue = itemStock * sellingPrice;

                    //call addItem method in invItem object
                    invItem.addItem(itemName,itemCategory,itemOrdered,itemStock, itemSold, 
                                supplierPrice, sellingPrice, stockValue, itemDesc);
                    displayData(); //method to display data from database in datagrid
                    displayItemList();// method will add item to item list
                    clearData();//clear data in text boxes
                    totalInventory(); //refresh inventory value
                    totalStock(); //refresh total number of items value

                }
                catch (Exception)
                {
                    MessageBox.Show("Error: Invalid Data Entry", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error); //error message
                }
               

            }
            else
            {
                //if there are any textboxes without data entered
                MessageBox.Show("Error: Failed to add new item. Please fill out all fields.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error); //error message

            }

        }

        private void inventoryDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //double click on row to select item

            //assign values of selected row to respective textbox fields
            itemIDTxt.Text = inventoryDataGrid.Rows[e.RowIndex].Cells[0].Value.ToString(); //item ID textbox
            edit_itemNameTxt.Text = inventoryDataGrid.Rows[e.RowIndex].Cells[1].Value.ToString(); //item name textbox 
            edit_categoryTxt.SelectedItem = inventoryDataGrid.Rows[e.RowIndex].Cells[2].Value.ToString(); //item category
            edit_itemsInStockTxt.Text = inventoryDataGrid.Rows[e.RowIndex].Cells[4].Value.ToString(); //stock value
            edit_itemsOrdered.Text = "0";
            edit_supplierPriceTxt.Text = inventoryDataGrid.Rows[e.RowIndex].Cells[6].Value.ToString(); //supplier price       
            edit_sellingPriceTxt.Text = inventoryDataGrid.Rows[e.RowIndex].Cells[7].Value.ToString(); //selling prince
            edit_descriptionTxt.Text = inventoryDataGrid.Rows[e.RowIndex].Cells[9].Value.ToString(); //item description

            oldStockVal.Text = inventoryDataGrid.Rows[e.RowIndex].Cells[4].Value.ToString(); //
            itemOrderedOld.Text = inventoryDataGrid.Rows[e.RowIndex].Cells[3].Value.ToString(); //items ordered stored in hidden label

            edit_totalStock.Text = edit_itemsInStockTxt.Text;

            edit_itemNameTxt.Focus(); //item name textbox selected
        }
        private void edit_itemsOrdered_TextChanged(object sender, EventArgs e)
        {
            if (edit_itemsOrdered.Text != "" )
            {
                try 
                { 
                    // total stock value calculation
                    edit_totalStock.Text = (int.Parse(edit_itemsOrdered.Text) + int.Parse(edit_itemsInStockTxt.Text)).ToString(); 
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //catch exception
                }
            }
            else
            {
                //if items ordered is empty
                edit_totalStock.Text = edit_itemsInStockTxt.Text; //total stock = items in stock
            }
            
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            //update item in database

            //if there is an item selected
            if (itemIDTxt.Text != "")
            {
                //try catch exception-handling and validations
                try
                {
                    //variables
                    itemID = int.Parse(itemIDTxt.Text);
                    itemName = edit_itemNameTxt.Text; // Item Name
                    itemCategory = edit_categoryTxt.Text; // Item category
                    itemDesc = edit_descriptionTxt.Text; // Item description
                    
                    itemStock = int.Parse(edit_totalStock.Text); // Stock Value = existing stock + items ordered
                    itemSold = 0; //intitially, items sold is 0           
                    supplierPrice = decimal.Parse(edit_supplierPriceTxt.Text); //supplier price 
                    sellingPrice = decimal.Parse(edit_sellingPriceTxt.Text); //selling price

                    //Items Ordered = Old Item Ordered Value + New
                    itemOrdered = int.Parse(edit_itemsOrdered.Text) + int.Parse(itemOrderedOld.Text); 

                    stockValue = itemStock * sellingPrice; //calculate total selling value of item

                    //call updateItem method
                    invItem.updateItem(itemID, itemName, itemCategory, itemOrdered, itemStock, 
                        itemSold, supplierPrice, sellingPrice, stockValue, itemDesc);

                    displayData(); //refresh data table
                    displayItemList(); //refresh item table
                    clearData(); //clear text boxes
                    totalInventory(); //refresh inventory value
                    totalStock(); //refresh total number of items value 

                }
                catch (Exception)
                {
                    MessageBox.Show("An error occured while updating item. Item not updated", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error); //error message
                }
              

            }
            else
            {
                //if no item is selected, display error message
                MessageBox.Show("Error: No item selected", "Item Selection Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void deleteBtn_Click(object sender, EventArgs e)
        {
      
            //delete item

            //if an item is selected (item ID will be populated in Item ID text box)
            if (itemIDTxt.Text != "")
            {
                //confirm if user wants to exit or not
                DialogResult result = MessageBox.Show("Are you sure you want to delete this item?", 
                    "Delete Confirmation", MessageBoxButtons.YesNo); 

                //if the user clicks on yes and confirms
                if (result == DialogResult.Yes)
                {

                    try { invItem.deleteItem(int.Parse(itemIDTxt.Text)); }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex);
                    }
                    finally
                    {
                        displayData(); //refresh data table
                        displayItemList();//refresh item list table
                        clearData(); //clear text boxes
                        totalInventory(); //refresh inventory value
                        totalStock(); //refresh total number of items value
                    }
                }          
            }
            else
            {
                MessageBox.Show("Error: No Item Selected", "Item Selection Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /*Sales Transaction*/

        private void itemList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //method called when item is selected
            try
            {
                //if no items are selected
                if (itemList.SelectedItems.Count == 0)
                    return;

                ListViewItem itemSelected = itemList.SelectedItems[0];
                itemID = int.Parse(itemSelected.Text);
                dbcon.Open();
                query = "select * from items where itemID ='" + itemID + "'";
                cmd = new SqlCommand(query, dbcon);
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    //Display Values In Textboxes
                    sale_sellingPriceTxt.Text = dr[7].ToString(); //selling price of item selected
                    sale_stockAvailableTxt.Text = dr[4].ToString(); //items in stock for item selected   
                    //maximum length of quantity that can be purchased = total number of items available in stock
                    quantityTxt.Maximum = int.Parse(sale_stockAvailableTxt.Text); //maximum quantity user can order = items in stock
                    quantityTxt.Value = 0; //initial value
                    finalPriceCalc(); //final price method call
                }


            }
            catch (Exception ex)
            {
                //catching error 
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dbcon.Close(); //close connection
            }
        }

        private void quantityTxt_ValueChanged(object sender, EventArgs e)
        {
            //calculate final price if value of quantity to purchase is changed
            finalPriceCalc();
        }
        private void quantityTxt_Click(object sender, EventArgs e)
        {
            //calculate final price if value of quantity to purchase is changed
            finalPriceCalc();
        }

        private void quantityTxt_Leave(object sender, EventArgs e)
        {
            //calculate final price if value of quantity to purchase is changed
            finalPriceCalc();
        }

        private void quantityTxt_Enter(object sender, EventArgs e)
        {
            //calculate final price if value of quantity to purchase is changed
            finalPriceCalc();
        }

        private void quantityTxt_KeyUp(object sender, KeyEventArgs e)
        {
            //calculate final price if value of quantity to purchase is changed
            finalPriceCalc();
        }

        private void finalPriceCalc()
        {
            //method to calculate final price of items based on quantity selected

            if (quantityTxt.Text != "" && sale_sellingPriceTxt.Text != "")
            {
                try
                {
                    //final price = quantity * selling price
                    finalPriceTxt.Text = (Convert.ToInt32(quantityTxt.Text) * Convert.ToDecimal(sale_sellingPriceTxt.Text)).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //error message
                }
            }
        }


        //add to cart
        private void addToCartBtn_Click(object sender, EventArgs e)
        {
            //add to cart button clicked
            try
            {
                //validation - no item selected
                if (itemList.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Error: No item selected", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning); //error message if no item is selected
                    return;
                }
                //validation - quantity value entered
                if (quantityTxt.Value != 0)
                {
                    ListViewItem itemSelected = itemList.SelectedItems[0]; // get item ID of selected item

                    //create array for list view items
                    string[] arr = new string[5];
                    arr[0] = itemSelected.Text;
                    arr[1] = itemSelected.SubItems[1].Text;
                    arr[2] = sale_sellingPriceTxt.Text;
                    arr[3] = quantityTxt.Value.ToString();
                    arr[4] = finalPriceTxt.Text;

                    //create new list view object
                    ListViewItem lv = new ListViewItem(arr);
                    purchaseList.Items.Add(lv); //add new list view object to purchaseList control    

                    //update total price in purchase summary
                    if (totalPriceTxt.Text != "")
                    {
                        //if total price text box already has a value
                        totalPriceTxt.Text = (Convert.ToDecimal(totalPriceTxt.Text) + Convert.ToDecimal(finalPriceTxt.Text)).ToString();
                    }
                    else
                    {
                        //if total price text box is empty
                        totalPriceTxt.Text = finalPriceTxt.Text;
                    }

                }
                else
                {
                    MessageBox.Show("Error: State the quantity of item to be purchased", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning); //if quantity not entered
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //error message showing exception thrown
            }

            finally
            {
                quantityTxt.Value = 0; //set quantity to 0
                finalPriceCalc(); //calculate final price
                balanceCalc(); //calculate balance
            }

        }

        /*Manage Cart*/

        private void removeFromCartBtn_Click(object sender, EventArgs e)
        {
            //method to remove item from cart

            //validation - no item selected
            if (purchaseList.SelectedItems.Count == 0)
            {             
                MessageBox.Show("Error: No item selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); //error message
            }
            else
            {
                try
                {
                    //recalculate total price
                    totalPriceTxt.Text = 
                        (Convert.ToDecimal(totalPriceTxt.Text) - Convert.ToDecimal(purchaseList.SelectedItems[0].SubItems[4].Text)).ToString();
                    //remove selected item from list
                    purchaseList.SelectedItems[0].Remove();


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //error - exception through
                }

            }
        }

        private void balanceCalc()
        {
            //calculate customer's balance payment for amount they have paid

            if (totalPriceTxt.Text != "" && tenderedTxt.Text != "")
            {
                //calculation
                try
                {
                    balanceTxt.Text = invSale.balanceCalc(Convert.ToDecimal(tenderedTxt.Text), Convert.ToDecimal(totalPriceTxt.Text));
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error:" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //error - exception through
                }
                                
            }
        }

        private void checkoutBtn_Click(object sender, EventArgs e)
        {
            //checkout button is clicked

            //if total price is 0 (meaning no item is added to cart)
            if (totalPriceTxt.Text == "0.00")
                 return;

            //if values are entereed in tendered and total price
             if (totalPriceTxt.Text != "" && tenderedTxt.Text != "")
             {
                //if total price is less greater than cash tendered
                 if (Convert.ToDecimal(totalPriceTxt.Text) > Convert.ToDecimal(tenderedTxt.Text))
                 {
                     MessageBox.Show("Error: Cash tendered is less than total price. Transaction cannot be processed." +
                         "\nPlease tender the correct amount and try again", "Error"
                         , MessageBoxButtons.OK, MessageBoxIcon.Error); //error message
                 }
                 else
                 {
                    //confirm check out
                    DialogResult result = MessageBox.Show("Please confirm if you wish to proceed with checkout",
                        "Checkout Confirmation", MessageBoxButtons.YesNo);

                    //if the user clicks on yes and confirms
                    if (result == DialogResult.Yes)
                    { 
                         try
                         {

                            for (int i = 0; i < purchaseList.Items.Count; i++)
                            {

                                itemID = Convert.ToInt32(purchaseList.Items[i].SubItems[0].Text); //item ID of item being purchased
                                int quantitySold = Convert.ToInt32(purchaseList.Items[i].SubItems[3].Text); //number of items sold
                                saleNoOfItems = saleNoOfItems + quantitySold; //total sale


                                invSale.updateStockValue(itemID, quantitySold); //call update stock value method from sales class

                                displayData(); //update values in datatable

                            }

                            saleValue = decimal.Parse(totalPriceTxt.Text); //total value of sale

                             invSale.newSale(timeNow, saleValue, saleNoOfItems); //new sale method
                            //update stock, item sold and total stock values in database for each item purchased
                            

                            //clear values in fields
                            itemList.SelectedIndices.Clear();
                             sale_sellingPriceTxt.Clear();
                             sale_stockAvailableTxt.Clear();
                             purchaseList.Items.Clear();
                             totalPriceTxt.Clear();
                             tenderedTxt.Clear();
                             balanceTxt.Clear();
                             totalInventory(); //refresh inventory value
                             totalStock(); //refresh total number of items value
                            saleNoOfItems = 0;


                         }
                         catch (Exception ex)
                         {
                             // MessageBox.Show("Error: Invalid Data Entry", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //error message

                             MessageBox.Show(ex.Message);
                         }
                    }
                }
             }
             else if (totalPriceTxt.Text != "" && tenderedTxt.Text == "" && totalPriceTxt.Text != "0.00")
             {
                 MessageBox.Show("Error: Please enter amount tendered (paid) by customer to proceed to Checkout", "Checkout Error", 
                     MessageBoxButtons.OK, MessageBoxIcon.Warning); //error message
             }
             else
             {
                 MessageBox.Show("Error: No items in purchase list", "Checkout Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); //error message

             }

            
        }

        private void totalPriceTxt_TextChanged(object sender, EventArgs e)
        {
            balanceCalc(); //calculate balance when total price value changes
        }
        private void tenderedTxt_TextChanged(object sender, EventArgs e)
        {
            balanceCalc(); //calculate balance when tendered value changes
        }

        private void tenderedTxt_Enter(object sender, EventArgs e)
        {
            balanceCalc(); //calculate balance when tendered value is entered
        }

        /*Additional Features*/

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            //log out of system
            this.Close();
            Login loginPg = new Login(); 
            loginPg.Show(); //display log in page
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            //confirm if user wants to exit or not
            DialogResult result = MessageBox.Show("Are you sure you want to exit the application?", 
                "Exit Confirmation", MessageBoxButtons.YesNo);

            if(result == DialogResult.Yes)
            {
                //exit application
                Application.Exit(); //exit application
            }
            else
            {
                return;
            }
            
        }

      /*Reports*/

        private void inventoryReportBtn_Click(object sender, EventArgs e)
        {
            //generate inventory report
            try
            {
                Report_Inventory repInv = new Report_Inventory(); //create object for report 
                repInv.Show(); //show report object

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); //error message if report cannot be generated
            }
           
        }    

        private void salesReportBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Report_Sales repSales = new Report_Sales(); //create object for report 
                repSales.Refresh();
                repSales.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); //error message if report cannot be generated
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }


}

