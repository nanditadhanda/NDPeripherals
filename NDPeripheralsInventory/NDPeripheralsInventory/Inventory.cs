/*
    Author:         Nandita Dhanda
    Email:          nandita.nd64@gmail.com
    Student ID:     SUKD1702275
    Course:         TCS3274 - Windows Programming (Group C)

    Project:        Inventory Management System
    Page:           Inventory Abstract Class
    Description:    Abstract class for variables and methods that are reusable
    Date:           14/08/2020
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDPeripheralsInventory
{
    abstract class Inventory
    {
        public int itemID; //Item ID
        public String itemName; // Item Name
        public String itemCategory; // Item category
        public String itemDesc; // Item description
        public int itemOrdered; //Items Ordered
        public int itemStock; // intitially, items in stock value is same as items ordered
        public int itemSold = 0;      //  initial value of itemSold will be 0  
        public decimal supplierPrice;
        public decimal sellingPrice;
        public decimal stockValue;
        public  string lt;

    }

}

