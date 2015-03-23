using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using Webshop2.Models;

namespace Webshop2.DatabaseControllers
{
    public class BestellingDBController : DatabaseController
    {
             public Int32 HaalBestellingTotaalPrijsOp()
        {
            int prijs=0;
            try
            {
                conn.Open();

                string selectQuery = "select * from Bestelling where bestellingID = 3";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    prijs = dataReader.GetInt32("totaalprijs");
                }
            }
            catch (Exception e)
            {
                
            }
            finally
            {
                conn.Close();
            }

            return prijs;
        }

        public List<Product> haalProductGegevensOp()
        {
            List<Product> producten = new List<Product>();
            try
            {
                conn.Open();
                string selectQuery = "select * from Product where productID in(select U.productID from Uitvoering U left outer join BestellingProduct B on U.uitvoeringID=B.uitvoeringID where bestellingID = 3)";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while(dataReader.Read())
                {
                    int ID = dataReader.GetInt32("productID");
                    string productNaam = dataReader.GetString("naam");
                    int productPrijs = dataReader.GetInt32("prijs");
                Product p = new Product { productID = ID, productDetail = "hoi", productNaam = productNaam, productPrijs = productPrijs };
                producten.Add(p);
                }
            }
            catch(Exception)
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