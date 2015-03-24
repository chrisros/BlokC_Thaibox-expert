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
            int aantal = 0;
            try
            {
                conn.Open();

                string selectQuery = "select * from Bestelling where bestellingID = 3;";
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
                string selectQuery = "select A.*, B.uitvoeringID, B.aantal, U.productID, P.*  from Bestelling A left outer join BestellingProduct B on A.bestellingID = B.bestellingID" 
                +" left outer join Uitvoering U on B.uitvoeringID = U.uitvoeringID" 
                +" left outer join Product P on U.productID = P.productID where A.bestellingID = 1; ";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while(dataReader.Read())
                {
                    int ID = dataReader.GetInt32("productID");
                    string productNaam = dataReader.GetString("naam");
                    int productPrijs = dataReader.GetInt32("prijs");
                    int productaantal = dataReader.GetInt32("aantal");
                Product p = new Product { productID = ID, productDetail = "hoi", productNaam = productNaam, productPrijs = productPrijs, productAantal =  productaantal };
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