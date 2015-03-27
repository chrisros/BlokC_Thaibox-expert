using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using Webshop2.Models;

namespace Webshop2.DatabaseControllers
{
    public class ordermailDBController : DatabaseController
    {
        public string getbestelling(string orderid)
        {
            int orderidint = Int32.Parse(orderid);
            string bestellijst = "";
            try
            {
                conn.Open();
                List<int> prijzen = new List<int>();
                List<int> aantallen = new List<int>();
                string selectQuery = "select uitvoeringID, aantal from BestellingProduct where bestellingID = @ID";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter idPara = new MySqlParameter("@ID", MySqlDbType.Int32);
                idPara.Value = orderidint;
                cmd.Parameters.Add(idPara);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    int uitvoeringID = dataReader.GetInt32("uitvoeringID");
                    int aantal = dataReader.GetInt32("aantal");
                    bestellijst = bestellijst + "<tr><td width=\"100px\">Product: </td><td width=\"100px\">" + uitvoeringID + "</td><td width=\"100px\">Aantal:</td><td width=\"100px\">" + aantal + "</td></tr>";      
                }
                
            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return bestellijst;
        }

        public int getorderperson(string orderid)
        {
            int orderidint = Int32.Parse(orderid);
            int orderperson = 0;
            try
            {
                conn.Open();
                List<int> userid = new List<int>();
                string selectQuery = "select gebruiker from Bestelling where bestellingID = @ID";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter idPara = new MySqlParameter("@ID", MySqlDbType.Int32);
                idPara.Value = orderidint;
                cmd.Parameters.Add(idPara);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    
                    int value = dataReader.GetInt32("gebruiker");
                    orderperson = value;
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return orderperson;
        }

        public string getPersoonGegevens(int orderid)
        {
            int orderidint = orderid;
            string adreslijst = "";
            try
            {
                conn.Open();
                List<int> prijzen = new List<int>();
                List<int> aantallen = new List<int>();
                string selectQuery = "select naam, adres, woonPostcode, woonplaats, telefoonnummer from Gebruiker where gebruikerID = @ID";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter idPara = new MySqlParameter("@ID", MySqlDbType.Int32);
                idPara.Value = orderidint;
                cmd.Parameters.Add(idPara);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    string adres = dataReader.GetString("adres");
                    string postcode = dataReader.GetString("woonPostcode");
                    string woonplaats = dataReader.GetString("woonplaats");
                    string naam = dataReader.GetString("naam");
                    string telefoon = dataReader.GetString("telefoonnummer");
                    
                    adreslijst = "<tr><td width=\"200px\">Straat: </td><td width=\"200px\">" + adres + "</td></tr>"+
                                 "<tr><td width=\"200px\">Postcode:</td><td width=\"200px\">" + postcode + "</td></tr>"+
                                 "<tr><td width=\"200px\">Woonplaats: </td><td width=\"200px\">" + woonplaats + "</td></tr>"+
                                 "<tr><td width=\"200px\">T.A.V: </td><td width=\"200px\">" + naam + "</td></tr>"+
                                 "<tr><td width=\"200px\">Telefoon: </td><td width=\"200px\">" + telefoon + "</td></tr>";
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return adreslijst;
        }

        public Boolean isGoldCustomer(int userid)
        {
            Boolean goldCustomer = false;
            try
            {
                conn.Open();
               
                string selectQuery = "select sum(totaalPrijs) as total from Bestelling where gebruiker = @ID";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter idPara = new MySqlParameter("@ID", MySqlDbType.Int32);
                idPara.Value = userid;
                cmd.Parameters.Add(idPara);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    double total = dataReader.GetDouble("total");
                    int total2 = Convert.ToInt32(total);
                    if (total2 > 500) { goldCustomer = true; }
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return goldCustomer;
        }

        public string getEmail(int userid)
        {
            string email = "";
            try
            {
                conn.Open();

                string selectQuery = "select email from Gebruiker where gebruikerID = @ID";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter idPara = new MySqlParameter("@ID", MySqlDbType.Int32);
                idPara.Value = userid;
                cmd.Parameters.Add(idPara);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    email = dataReader.GetString("email");
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return email;
        }

        public string getNaam(int userid)
        {
            string naam = "";
            try
            {
                conn.Open();

                string selectQuery = "select naam from Gebruiker where gebruikerID = @ID";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter idPara = new MySqlParameter("@ID", MySqlDbType.Int32);
                idPara.Value = userid;
                cmd.Parameters.Add(idPara);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    naam = dataReader.GetString("naam");
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return naam;
        }

    }

 

}