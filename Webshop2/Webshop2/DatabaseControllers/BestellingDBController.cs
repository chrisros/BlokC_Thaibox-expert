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
             public Int32 HaalBestellingTotaalPrijsOpUser()
        {
            berekenTotaalPRijsUser();
            int prijs=0;
            int aantal = 0;
            try
            {
                conn.Open();
                int ID = (int)System.Web.HttpContext.Current.Session["gebruikerID"];
                string selectQuery = "select * from Bestelling where bestellingID = @ID and betaald = 0";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter idPara = new MySqlParameter("@ID", MySqlDbType.Int32);
                idPara.Value = ID;
                cmd.Parameters.Add(idPara);
                cmd.Prepare();
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

        public List<Product> haalProductGegevensOpVoorGebruiker()
        {
            List<Product> producten = new List<Product>();
            try
            {
                int userID = (int)System.Web.HttpContext.Current.Session["gebruikerID"];

                conn.Open();
                string selectQuery = "select A.*, B.uitvoeringID, B.aantal, U.*, P.*  from Bestelling A left outer join BestellingProduct B on A.bestellingID = B.bestellingID" 
                +" left outer join Uitvoering U on B.uitvoeringID = U.uitvoeringID" 
                +" left outer join Product P on U.productID = P.productID where A.bestellingID = @ID and A.betaald = 0; ";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter idPara = new MySqlParameter("@ID", MySqlDbType.Int32);
                idPara.Value = userID;
                cmd.Parameters.Add(idPara);
                cmd.Prepare();
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

        public void updateTotaalPRijsUser(int totPrijs)
        {
            
            
            int ID = (int)System.Web.HttpContext.Current.Session["gebruikerID"];

            MySqlTransaction trans = null;
            conn.Open();
            trans = conn.BeginTransaction();
            MySqlCommand cmd = new MySqlCommand("update Bestelling Set totaalprijs = @prijs where bestellingID = @ID and betaald = 0", conn);
            MySqlParameter prijsPara = new MySqlParameter("@prijs", MySqlDbType.Int32);
            MySqlParameter IDPara = new MySqlParameter("@ID", MySqlDbType.Int32);
            prijsPara.Value = totPrijs;
            IDPara.Value = ID;
            cmd.Parameters.Add(prijsPara);
            cmd.Parameters.Add(IDPara);
            cmd.ExecuteNonQuery();
            trans.Commit();
            conn.Close();
        }

        public void berekenTotaalPRijsUser()
        {
            int totaalprijs = 0;
            int ID = (int)System.Web.HttpContext.Current.Session["gebruikerID"];

            try
            {
            conn.Open();
            List<int> prijzen = new List<int>();
            List<int> aantallen = new List<int>();
                string selectQuery = "select B.aantal, Be.totaalprijs, P.Prijs, P.naam from Bestelling Be join BestellingProduct B on Be.bestellingID = B.bestellingID join Uitvoering U on B.uitvoeringID = U.uitvoeringID join Product P on U.productID = P.productID where B.bestellingID = @ID and Be.betaald = 0";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter idPara = new MySqlParameter("@ID", MySqlDbType.Int32);
                idPara.Value = ID;
                cmd.Parameters.Add(idPara);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while(dataReader.Read())
                {
                    int prijs = dataReader.GetInt32("prijs");
                    int aantal = dataReader.GetInt32("aantal");
                    prijzen.Add(prijs);
                    aantallen.Add(aantal);
                }
                for (int i = 0; i < aantallen.Count; i++) 
                {
                    totaalprijs = totaalprijs + prijzen[i] * aantallen[i];
                }
            }
            catch(Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            updateTotaalPRijsUser(totaalprijs);
        }

        public Product haalProductGegevensOp(int prodID, int aantal)
        {
            Product p = new Product();
            try
            {
            conn.Open();
            string selectQuery = "select * from Product where productID = @prodID ";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter prodIDPara = new MySqlParameter("@prodID", MySqlDbType.Int32);
                prodIDPara.Value = prodID;
                cmd.Parameters.Add(prodIDPara);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while(dataReader.Read())
                {
                    int prijs = dataReader.GetInt32("prijs");
                    string naam = dataReader.GetString("naam");
                    p = new Product { productID = prodID, productNaam = naam, productPrijs = prijs, productAantal = aantal };
                }
           }
            catch(Exception)
            {
                
            }
            finally
            {
                conn.Close();
            }
            return p;
        }
    }
}