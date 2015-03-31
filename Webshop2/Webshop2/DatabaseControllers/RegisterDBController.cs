using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Webshop2.Models;

namespace Webshop2.DatabaseControllers
{
    public class RegisterDBController : DatabaseController
    {
        public void RegisterAccount(AccountModel account)
        {
            MySqlTransaction trans = null;

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                string InsertString = @"insert into Gebruiker (naam, adres, woonPostcode, woonplaats, gebruikersnaam, wachtwoord, email, 
                                           telefoonnummer) 
                                  values (@anaam, @wadres, @wpcode, @wplaats, @gnaam, @wwoord, @amail, @atel)";
                MySqlCommand regcmd = new MySqlCommand(InsertString, conn);

                MySqlParameter NaamPara = new MySqlParameter("@anaam", MySqlDbType.VarChar);
                MySqlParameter WoonPara = new MySqlParameter("@wadres", MySqlDbType.VarChar);
                MySqlParameter PostPara = new MySqlParameter("@wpcode", MySqlDbType.VarChar);
                MySqlParameter PlaatsPara = new MySqlParameter("@wplaats", MySqlDbType.VarChar);
                MySqlParameter GnaamPara = new MySqlParameter("@gnaam", MySqlDbType.VarChar);
                MySqlParameter WachtPara = new MySqlParameter("@wwoord", MySqlDbType.VarChar);
                MySqlParameter MailPara = new MySqlParameter("@amail", MySqlDbType.VarChar);
                MySqlParameter TelePara = new MySqlParameter("@atel", MySqlDbType.Int32);
                    
                //Geboortedatum, sla ik nu even over
                //MySqlParameter GebPara = new MySqlParameter("@ageb", MySqlDbType.DateTime);

                NaamPara.Value = account.Naam;
                WoonPara.Value = account.Woonadres;
                PostPara.Value = account.Woonpostcode;
                PlaatsPara.Value = account.Woonplaats;
                GnaamPara.Value = account.Gebruikersnaam;
                MailPara.Value = account.Email;
                WachtPara.Value = account.Wachtwoord;
                TelePara.Value = account.Telefoonnummer;

                regcmd.Parameters.Add(NaamPara);
                regcmd.Parameters.Add(WoonPara);
                regcmd.Parameters.Add(PostPara);
                regcmd.Parameters.Add(PlaatsPara);
                regcmd.Parameters.Add(GnaamPara);
                regcmd.Parameters.Add(MailPara);
                regcmd.Parameters.Add(WachtPara);
                regcmd.Parameters.Add(TelePara);

                regcmd.Prepare();

                regcmd.ExecuteNonQuery();

                trans.Commit();
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

        public AccountModel LoginCheck(String email) 
        {
            AccountModel account = new AccountModel();
            try
            {
                conn.Open();
                string selectquery = @"select * from Gebruiker where email = @mail";
                MySqlCommand selcmd = new MySqlCommand(selectquery, conn);

                MySqlParameter mailpara = new MySqlParameter("@mail", MySqlDbType.VarChar);
                mailpara.Value = email;

                selcmd.Parameters.Add(mailpara);

                MySqlDataReader reader = selcmd.ExecuteReader();


                while(reader.Read())
                {
                    account.GebruikerID = reader.GetInt32("gebruikerID");
                    account.Email = reader.GetString("email");
                    account.Wachtwoord = reader.GetString("wachtwoord");
                    account.Naam = reader.GetString("naam");
                    account.Woonadres = reader.GetString("adres");
                    account.Woonpostcode = reader.GetString("woonPostcode");
                    account.Woonplaats = reader.GetString("woonplaats");
                    account.Gebruikersnaam = reader.GetString("gebruikersnaam");
                    account.Telefoonnummer = reader.GetInt32("telefoonnummer");
                }
                
            }
            catch (Exception)
            {
                
            }
            finally 
            {
                conn.Close();
            }
            return account;
        }
        //Kijkt of de email al bestaat in de database returned true als de email nog niet in de DB voorkomt
        public Boolean isNewEmail(String email)
        {
            Boolean isNewEmail = false;
            int rowCount = 1;
            try
            {
                conn.Open();

                string selectQuery = @"select count(email) as rows from Gebruiker where email = @mail";
                MySqlCommand selcmd = new MySqlCommand(selectQuery, conn);

                MySqlParameter mailpara = new MySqlParameter("@mail", MySqlDbType.VarChar);
                mailpara.Value = email;

                selcmd.Parameters.Add(mailpara);

                MySqlDataReader dataReader = selcmd.ExecuteReader();

                while (dataReader.Read())
                {
                    rowCount = dataReader.GetInt16("rows");
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                conn.Close();
            }

            if (rowCount == 0)
            {
                isNewEmail = true;
            }

            return isNewEmail;
        }

        public AdminModel AdminLoginCheck(String gebruiker) 
        {
            AdminModel admin = new AdminModel();
            try{
                conn.Open();

                string select = @"select * from Admin where gebruikersnaam = @user";
                MySqlCommand selectcmd = new MySqlCommand(select, conn);

                MySqlParameter Userpara = new MySqlParameter("@user", MySqlDbType.VarChar);
                Userpara.Value = gebruiker;

                selectcmd.Parameters.Add(Userpara);

                MySqlDataReader reader = selectcmd.ExecuteReader();


                while(reader.Read())
                {
                    admin.GebruikerID = reader.GetInt32("adminID");
                    admin.Naam = reader.GetString("gebruikersnaam");
                    admin.Wachtwoord = reader.GetString("wachtwoord");
                    admin.email = reader.GetString("email");
                    string functie = reader.GetString("functie");
                    if (functie.Equals("w")) {
                        admin.Functie = "beheerder";
                    }
                    else if (functie.Equals("f")) {
                        admin.Functie = "manager";
                    }
                }
                
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                conn.Close();
            }
            
            return admin;
        }

        public List<Bestelling> MaandOverzicht() 
        {
            List<Bestelling> Overzicht = new List<Bestelling>();
            conn.Open();
            string overselect = @"select * from Bestelling where betaald = '1' AND Month(bestelDatum) = Month(CURRENT_DATE) AND Year(bestelDatum) = Year(CURRENT_DATE)";
            MySqlCommand overcmd = new MySqlCommand(overselect, conn);

            MySqlDataReader reader = overcmd.ExecuteReader();

            while (reader.Read()) 
            {
                Bestelling b = new Bestelling();
                b.bestellingID = reader.GetInt32("bestellingID");
                b.gebruiker = reader.GetInt32("gebruiker");
                b.totaalPrijs = reader.GetInt32("totaalPrijs");
                b.bestellingStatus = reader.GetString("bestellingStatus");
                b.bestelDatum = reader.GetDateTime("bestelDatum");
                b.bezorgDatum = reader.GetDateTime("bezorgDatum");
                Overzicht.Add(b);
            }
            return (Overzicht);
        }

        public List<Product> ProductOverzicht() {

            List<Product> goedverkocht = new List<Product>();
            conn.Open();
            string poselect = @"select uitvoeringID, sum(aantal) as totaalAantal from BestellingProduct
                        where aantal in (select aantal from BestellingProduct) group by uitvoeringID 
                        order by totaalAantal DESC Limit 5";
            MySqlCommand pocmd = new MySqlCommand(poselect, conn);

            MySqlDataReader reader = pocmd.ExecuteReader();

            while (reader.Read())
            { 
                Product p = new Product();
                p.productUitvoeringID = reader.GetInt32("uitvoeringID");
                p.productAantal = reader.GetInt32("totaalAantal");
                goedverkocht.Add(p);
            }
            return goedverkocht;
        }
    }
}