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
        static int bestelID = 0;
        int id = 0;
        public double HaalBestellingTotaalPrijsOpUser()
        {
            //berekenTotaalPRijsUser();
            double prijs = 0;
            int aantal = 0;
            try
            {
                conn.Open();
                int ID = (int)System.Web.HttpContext.Current.Session["gebruikerID"];
                string selectQuery = "select * from Bestelling where gebruiker = @ID and betaald = 0 order by bestellingID desc limit 1";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter idPara = new MySqlParameter("@ID", MySqlDbType.Int32);
                idPara.Value = ID;
                cmd.Parameters.Add(idPara);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    prijs = dataReader.GetDouble("totaalprijs");
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
                + " left outer join Uitvoering U on B.uitvoeringID = U.uitvoeringID"
                + " left outer join Product P on U.productID = P.productID where A.bestellingID = @bestelID and A.betaald = 0; ";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter bestelidPara = new MySqlParameter("@bestelID", MySqlDbType.Int32);
                bestelidPara.Value = getBestelID();
                cmd.Parameters.Add(bestelidPara);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    string firstItem = Convert.ToString(dataReader["productID"]);
                    if (!string.IsNullOrEmpty(firstItem))
                    {


                        int ID = dataReader.GetInt32("productID");
                        bestelID = dataReader.GetInt32("bestellingID");
                        string productNaam = dataReader.GetString("naam");
                        string kleur = dataReader.GetString("kleur");
                        string maat = dataReader.GetString("maat");
                        double productPrijs = dataReader.GetDouble("prijs");
                        int productaantal = dataReader.GetInt32("aantal");

                        Product p = new Product { productID = ID, productDetail = "hoi", productNaam = productNaam + " - " + kleur + " - " + maat, productPrijs = productPrijs, productAantal = productaantal, productMaat = maat, productKleur = kleur };
                        producten.Add(p);
                    }
                }
            }
            catch (NullReferenceException)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return producten;
        }

        public void updateTotaalPRijsUser(double totPrijs)
        {


            int ID = getBestelID();

            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                MySqlCommand cmd = new MySqlCommand("update Bestelling Set totaalprijs = @prijs where bestellingID = @ID and betaald = 0", conn);
                MySqlParameter prijsPara = new MySqlParameter("@prijs", MySqlDbType.Double);
                MySqlParameter IDPara = new MySqlParameter("@ID", MySqlDbType.Int32);
                prijsPara.Value = totPrijs;
                IDPara.Value = ID;
                cmd.Parameters.Add(prijsPara);
                cmd.Parameters.Add(IDPara);
                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public void berekenTotaalPRijsUser()
        {
            double totaalprijs = 0.0;
            int ID = getBestelID();

            try
            {
                conn2.Open();
                List<int> prijzen = new List<int>();
                List<int> aantallen = new List<int>();
                string selectQuery = "select B.aantal, Be.totaalprijs, P.Prijs, P.naam from Bestelling Be join BestellingProduct B on Be.bestellingID = B.bestellingID join Uitvoering U on B.uitvoeringID = U.uitvoeringID join Product P on U.productID = P.productID where B.bestellingID = @ID and Be.betaald = 0";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn2);
                MySqlParameter idPara = new MySqlParameter("@ID", MySqlDbType.Int32);
                idPara.Value = ID;
                cmd.Parameters.Add(idPara);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
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
            finally
            {
                conn2.Close();
            }
            DatabaseControllers.ordermailDBController ordercont = new DatabaseControllers.ordermailDBController();
            if ((ordercont.isGoldCustomer((int)System.Web.HttpContext.Current.Session["gebruikerID"]) == true))
            {
                double totprijs = totaalprijs * 0.96;
                updateTotaalPRijsUser(totprijs);
            }
            else
            {
                updateTotaalPRijsUser(totaalprijs);
            }
        }

        public Product haalProductGegevensOp(int prodID, int aantal)
        {
            Product p = new Product();
            try
            {
                conn.Open();
                string selectQuery = "select P.*, U.* from Product P join Uitvoering U on P.productID = U.productID where P.productID = @prodID;";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter prodIDPara = new MySqlParameter("@prodID", MySqlDbType.Int32);
                prodIDPara.Value = prodID;
                cmd.Parameters.Add(prodIDPara);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    double prijs = dataReader.GetDouble("prijs");
                    string naam = dataReader.GetString("naam");
                    string maat = dataReader.GetString("maat");
                    string kleur = dataReader.GetString("kleur");
                    p = new Product { productID = prodID, productNaam = naam, productPrijs = prijs, productAantal = aantal, productKleur = kleur, productMaat = maat };
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return p;
        }

        public void NieuweBestellingGebruiker()
        {
            int gebruiker = (int)System.Web.HttpContext.Current.Session["gebruikerID"];
            MySqlTransaction trans = null;
            string insertQuery = @"insert into Bestelling(totaalPrijs, bestellingStatus, betaald, bezorgDatum, gebruiker, bestelDatum)
                                    values(@totprijs, @bestellingstatus, 0, @bezorgDatum, @gebruiker, @bestelDatum)";
            try
            {
                conn2.Open();
                trans = conn2.BeginTransaction();
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn2);
                MySqlParameter totPrijsPara = new MySqlParameter("@totprijs", MySqlDbType.Double);
                MySqlParameter bestStatPara = new MySqlParameter("@bestellingStatus", MySqlDbType.VarChar);
                MySqlParameter bezorgDatumPara = new MySqlParameter("@bezorgDatum", MySqlDbType.Date);
                MySqlParameter gebruikerPara = new MySqlParameter("@gebruiker", MySqlDbType.Int32);
                MySqlParameter besteldatumPara = new MySqlParameter("@bestelDatum", MySqlDbType.Date);

                totPrijsPara.Value = HaalBestellingTotaalPrijsOpUser();
                bestStatPara.Value = "In Behandeling";
                bezorgDatumPara.Value = "2020-01-20";
                gebruikerPara.Value = gebruiker;
                besteldatumPara.Value = DateTime.Now;
                cmd.Parameters.Add(totPrijsPara);
                cmd.Parameters.Add(bestStatPara);
                cmd.Parameters.Add(bezorgDatumPara);
                cmd.Parameters.Add(gebruikerPara);
                cmd.Parameters.Add(besteldatumPara);

                cmd.Prepare();

                cmd.ExecuteNonQuery();

                trans.Commit();


            }
            finally
            {
                conn2.Close();
            }
        }

        public void productToevoegenWinkelWagenGebruiker(int aantal, int uitvoeringsID)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                string insertString = "insert into BestellingProduct(uitvoeringID, bestellingID, aantal) values(@uitvoeringID, @bestellingID, @aantal)";

                MySqlCommand cmd = new MySqlCommand(insertString, conn);
                MySqlParameter bestIDPara = new MySqlParameter("@bestellingID", MySqlDbType.Int32);
                MySqlParameter uitvoerIDPara = new MySqlParameter("@uitvoeringID", MySqlDbType.Int32);
                MySqlParameter aantPara = new MySqlParameter("@aantal", MySqlDbType.Int32);

                aantPara.Value = aantal;
                bestIDPara.Value = getBestelID();
                uitvoerIDPara.Value = uitvoeringsID;
                cmd.Parameters.Add(aantPara);
                cmd.Parameters.Add(uitvoerIDPara);
                cmd.Parameters.Add(bestIDPara);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                trans.Commit();



            }
            catch (Exception)
            {
                trans.Rollback();
            }

            finally
            {
                conn.Close();
                berekenTotaalPRijsUser();
            }
        }

        public Int32 haalUitvoeringsIDOp(int productID, string maat, string kleur)
        {
            int uitvoerID = 3;
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                int prodID = productID;
                string maattest = maat;
                string kleurtest = kleur;
                string selectQuery = "select uitvoeringID from Uitvoering where productID = @prodID and maat = @maat and kleur = @kleur";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter prodIDPara = new MySqlParameter("@prodID", MySqlDbType.Int32);
                MySqlParameter maatPara = new MySqlParameter("@maat", MySqlDbType.VarChar);
                MySqlParameter kleurPara = new MySqlParameter("@kleur", MySqlDbType.VarChar);
                prodIDPara.Value = productID;
                maatPara.Value = maat;
                kleurPara.Value = kleur;
                cmd.Parameters.Add(prodIDPara);
                cmd.Parameters.Add(maatPara);
                cmd.Parameters.Add(kleurPara);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    uitvoerID = dataReader.GetInt32("uitvoeringID");
                }
            }
            catch (Exception)
            {
                trans.Rollback();
            }
            finally
            {
                conn.Close();
            }
            return uitvoerID;
        }

        public void editAantalWinkelmandGebruiker(int aantal, int uitvoeringID)
        {
            MySqlTransaction trans = null;
            try
            {

                conn.Open();
                trans = conn.BeginTransaction();
                MySqlCommand cmd = new MySqlCommand("update BestellingProduct Set aantal = @aant where uitvoeringID = @ID and bestellingID = @bestelID", conn);
                MySqlParameter aantPara = new MySqlParameter("@aant", MySqlDbType.Int32);
                MySqlParameter uitvoeringPara = new MySqlParameter("@ID", MySqlDbType.Int32);
                MySqlParameter bestelPara = new MySqlParameter("@bestelID", MySqlDbType.Int32);
                aantPara.Value = aantal;
                uitvoeringPara.Value = uitvoeringID;
                bestelPara.Value = getBestelID();
                cmd.Parameters.Add(aantPara);
                cmd.Parameters.Add(uitvoeringPara);
                cmd.Parameters.Add(bestelPara);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            finally
            {
                conn.Close();
                berekenTotaalPRijsUser();
            }
        }

        public Int32 getBestelID()
        {
            conn2.Open();
            int gebruikerID = (int)System.Web.HttpContext.Current.Session["gebruikerID"];
            string selectQuery = "select * from Bestelling where gebruiker = @gebruiker and betaald = 0 order by bestellingID desc limit 1";
            MySqlCommand cmd = new MySqlCommand(selectQuery, conn2);
            MySqlParameter gebruikerPara = new MySqlParameter("@gebruiker", MySqlDbType.Int16);
            gebruikerPara.Value = gebruikerID;
            cmd.Parameters.Add(gebruikerPara);
            cmd.Prepare();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                string firstItem = Convert.ToString(dataReader["bestellingID"]);
                if (!string.IsNullOrEmpty(firstItem))
                {
                    bestelID = dataReader.GetInt32("bestellingID");
                }
                else
                {
                    NieuweBestellingGebruiker();
                }
            }
            dataReader.Close();
            conn2.Close();
            return bestelID;
        }

        public void deleteWinkelMandProductGebruiker(int uitvoeringID)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                int uitvoID = uitvoeringID;
                MySqlCommand cmd = new MySqlCommand("delete from BestellingProduct where uitvoeringID = @uitvoeringID and bestellingID = @bestelID ", conn);
                MySqlParameter uitvoerIDPara = new MySqlParameter("@uitvoeringID", MySqlDbType.Int32);
                MySqlParameter bestelIDPara = new MySqlParameter("@bestelID", MySqlDbType.Int32);

                bestelIDPara.Value = getBestelID();
                uitvoerIDPara.Value = uitvoeringID;

                cmd.Parameters.Add(bestelIDPara);
                cmd.Parameters.Add(uitvoerIDPara);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                trans.Commit();


            }
            catch (Exception)
            {
                trans.Rollback();
            }
            finally
            {
                conn.Close();
                berekenTotaalPRijsUser();
            }
        }

        public List<Product> getBetaaldeProducten()
        {
            List<Product> producten = new List<Product>();
            try
            {
                int gebruikerID = (int)System.Web.HttpContext.Current.Session["gebruikerID"];


                string selectQuery = "select B.*, BP.*, U.*, P.naam, P.prijs from Bestelling B join BestellingProduct BP on B.bestellingID = BP.bestellingID join Uitvoering U on BP.uitvoeringID = U.uitvoeringID join Product P on U.productID = P.productID where betaald = 1 and B.gebruiker = @gebruiker";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter gebruikerPara = new MySqlParameter("@gebruiker", MySqlDbType.Int16);
                gebruikerPara.Value = gebruikerID;
                cmd.Parameters.Add(gebruikerPara);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {


                    string productNaam = dataReader.GetString("naam");
                    string productKleur = dataReader.GetString("kleur");
                    string productMaat = dataReader.GetString("maat");
                    int productAantal = dataReader.GetInt16("aantal");
                    double productPrijs = dataReader.GetDouble("prijs");
                    Product p = new Product { productNaam = productNaam + " - " + productKleur + " - " + productMaat, productPrijs = productPrijs, productAantal = productAantal };
                    producten.Add(p);
                }
                dataReader.Close();

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

        public void setBetaald()
        {
            MySqlTransaction trans = null;
            try
            {

                conn.Open();
                trans = conn.BeginTransaction();
                MySqlCommand cmd = new MySqlCommand("update Bestelling Set betaald = 1 where bestellingID = @bestelID", conn);
                MySqlParameter bestelPara = new MySqlParameter("@bestelID", MySqlDbType.Int32);
                bestelPara.Value = getBestelID();
                cmd.Parameters.Add(bestelPara);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}