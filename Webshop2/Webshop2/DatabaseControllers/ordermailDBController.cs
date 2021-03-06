﻿using System;
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
        //maakt een tabel voor het versturen van een order 
        public string getbestelling(string orderid)
        {
            int orderidint = Int32.Parse(orderid);
            string bestellijst = "";
            try
            {
                conn2.Open();
                List<int> prijzen = new List<int>();
                List<int> aantallen = new List<int>();
                string selectQuery = "select uitvoeringID, aantal from BestellingProduct where bestellingID = @ID";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn2);
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
                conn2.Close();
            }
            return bestellijst;
        }
        //haalt de gebruiker van een bepaalde bestelling op voor het ophalen van verdere gegevens
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
        //haalt een tabel met alle persoonsgegevens op voor het invoegin in de mail
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
        //kijkt of iemand in aanmerking komt om golduser te worden (kijkt of er meer als 500 eur is uitgegeven)
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
        //haalt de email van een gebruiker uit de database
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
        //returned TRUE als iemand in de DB als goldcustomer staat.
        public Boolean knownGoldCustomer(int userid)
        {
            Boolean gold = false;
            int data = 0;
            try
            {
                conn.Open();

                string selectQuery = "select isGoldCustomer from Gebruiker where gebruikerID = @ID";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter idPara = new MySqlParameter("@ID", MySqlDbType.Int32);
                idPara.Value = userid;
                cmd.Parameters.Add(idPara);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    data = dataReader.GetInt32("isGoldCustomer");
                    if (data==1)
                    {
                        gold = true;

                    }
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return gold;
        }

        public void updateGoldCustomerStatus(int userid)
        {
            try
            {
                         conn.Open();
                        MySqlTransaction trans = null;
                        trans = conn.BeginTransaction();
                        string insertQuery = "update Gebruiker set isGoldCustomer=1 where gebruikerID = @ID";
                        MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                        MySqlParameter idPara = new MySqlParameter("@ID", MySqlDbType.Int32);
                        idPara.Value = userid;
                        cmd.Parameters.Add(idPara);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        trans.Commit();
            
            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
        }
        //haalt de naam van een persoon op om te gebruiken in een mai
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