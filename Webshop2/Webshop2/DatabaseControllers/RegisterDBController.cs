﻿using System;
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

                string InsertString = @"insert into Gebruiker (naam, adres, woonPostcode, gebruikersnaam, wachtwoord, email, 
                                           telefoonnummer, ) 
                                  values (@anaam, @wadres, @wpcode, @gnaam, @wwoord, @amail, @atel)";
                MySqlCommand regcmd = new MySqlCommand(InsertString, conn);

                MySqlParameter NaamPara = new MySqlParameter("@anaam", MySqlDbType.VarChar);
                MySqlParameter WoonPara = new MySqlParameter("@wadres", MySqlDbType.VarChar);
                MySqlParameter PostPara = new MySqlParameter("@wpcode", MySqlDbType.VarChar);
                MySqlParameter GnaamPara = new MySqlParameter("@gnaam", MySqlDbType.VarChar);
                MySqlParameter WachtPara = new MySqlParameter("@wwoord", MySqlDbType.VarChar);
                MySqlParameter MailPara = new MySqlParameter("@amail", MySqlDbType.VarChar);
                MySqlParameter TelePara = new MySqlParameter("@anaam", MySqlDbType.Int32);

                //Geboortedatum, sla ik nu even over
                //MySqlParameter GebPara = new MySqlParameter("@ageb", MySqlDbType.DateTime);

                NaamPara.Value = account.Naam;
                WoonPara.Value = account.Woonadres;
                PostPara.Value = account.Woonpostcode;
                GnaamPara.Value = account.Gebruikersnaam;
                MailPara.Value = account.Email;
                WachtPara.Value = account.Wachtwoord;
                TelePara.Value = account.Telefoonnummer;

                regcmd.Parameters.Add(NaamPara);
                regcmd.Parameters.Add(WoonPara);
                regcmd.Parameters.Add(PostPara);
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
    }
}