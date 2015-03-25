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
                string selectQuery = "select A.*, B.uitvoeringID, B.aantal, U.*, P.*  from Bestelling A left outer join BestellingProduct B on A.bestellingID = B.bestellingID" 
                +" left outer join Uitvoering U on B.uitvoeringID = U.uitvoeringID" 
                +" left outer join Product P on U.productID = P.productID where A.bestellingID = 1; ";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while(dataReader.Read())
                {
                    int ID = dataReader.GetInt32("productID");
                    string productNaam = dataReader.GetString("naam");
                    string kleur = dataReader.GetString("kleur");
                    string maat = dataReader.GetString("maat");
                    int productPrijs = dataReader.GetInt32("prijs");
                    int productaantal = dataReader.GetInt32("aantal");
                Product p = new Product { productID = ID, productDetail = "hoi", productNaam = productNaam +" - " + kleur +" - " + maat, productPrijs = productPrijs, productAantal =  productaantal };
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

        public void berekenTotaalPRijs()
        {
               MySqlTransaction trans = null;
            int aantal = 0;
            int prijs = 0;
            int totaalprijs = 0;
            List<int> prijzen = new List<int>();
            List<int> aantallen = new List<int>();
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string selectQuery = "select B.aantal, Be.totaalprijs, P.Prijs, P.naam from Bestelling Be join BestellingProduct B on Be.bestellingID = B.bestellingID join Uitvoering U on B.uitvoeringID = U.uitvoeringID join Product P on U.productID = P.productID where B.bestellingID = 3;";

                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    aantal = dataReader.GetInt32("aantal");
                    prijs = dataReader.GetInt32("prijs");
                    totaalprijs = dataReader.GetInt32("totaalprijs");
                    prijzen.Add(prijs);
                    aantallen.Add(aantal);
                }
                conn.Close();
                for (int i = 0; i < aantallen.Count; i++)
                {
                    totaalprijs = totaalprijs + prijzen[i] * aantallen[i];
                }

             
                   string updateQuery = "UPDATE Bestelling set totaalprijs = 15000 where bestellingID = 3";
                    conn.Open();

                    MySqlCommand cmdUpdate = new MySqlCommand(updateQuery, conn);
                    //cmdUpdate.Parameters.AddWithValue("@totprijs", totaalprijs);
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    conn.Close();

            }
            catch (Exception)
            {
                trans.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}