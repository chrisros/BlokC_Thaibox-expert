using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webshop2.Models;
using MySql.Data.MySqlClient;

namespace Webshop2.DatabaseControllers
{
    public class CategorieDBController : DatabaseController
    {

        public List<Product> haalKledingGegevensOp()
        {
            List<Product> producten = new List<Product>();
            try
            {
                conn.Open();
                string selectQuery = "SELECT * FROM Product " + 
                    " WHERE (soort like 'Kleding') " + 
                    "|| (soort like 'shirts') " + 
                    "||(soort like 'Schoenen')" + 
                    "|| (soort like 'Sokken') " + 
                    "||(soort like 'Ondergoed') " + 
                    "|| (soort like 'Broeken') ";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    int ID = dataReader.GetInt32("productID");
                    string productNaam = dataReader.GetString("naam");
                    string productMerk = dataReader.GetString("merk");
                    int productPrijs = dataReader.GetInt32("prijs");
                    string productDetail = dataReader.GetString("productOmschrijving");
                    string productSoort = dataReader.GetString("soort");
                    Product p = new Product { productID = ID, productDetail = productDetail, productNaam = productNaam, productMerk = productMerk, productSoort = productSoort, productPrijs = productPrijs };
                    producten.Add(p);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return producten;
        }

        public List<Product> haalShirtsOp()
        {
            List<Product> producten = new List<Product>();
            try
            {
                conn.Open();
                string selectQuery = "SELECT * FROM Product WHERE (soort like 'shirt') ";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    int ID = dataReader.GetInt32("productID");
                    string productNaam = dataReader.GetString("naam");
                    string productMerk = dataReader.GetString("merk");
                    int productPrijs = dataReader.GetInt32("prijs");
                    string productDetail = dataReader.GetString("productOmschrijving");
                    string productSoort = dataReader.GetString("soort");
                    Product p = new Product { productID = ID, productDetail = productDetail, productNaam = productNaam, productMerk = productMerk, productSoort = productSoort, productPrijs = productPrijs };
                    producten.Add(p);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return producten;
        }


    }
}