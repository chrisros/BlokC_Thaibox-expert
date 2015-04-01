using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Webshop2.DatabaseControllers
{
    public class ReactieDBController : DatabaseController
    {
        public string getReactieTable()
        {
            string table = "<hr>";
            try
            {
                conn.Open();
                string selectQuery = "select G.gebruikersnaam, R.schermNaam, R.reactie, R.rating from Reactie R join Gebruiker G on R.gebruikerID = G.gebruikerID ORDER BY R.adddate DESC LIMIT 5";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    string naam1 = dataReader.GetString("gebruikersnaam");
                    string naam2 = dataReader.GetString("schermNaam");
                    string reactie = dataReader.GetString("reactie");
                    int rating = dataReader.GetInt16("rating");
                    string finalnaam ="";
                    if (naam1.Length > 1) { finalnaam = naam1; } else if (naam2.Length > 1) { finalnaam = naam2; } else { finalnaam = "N.V.T"; }
                    table = table + "<tr><td width=\"100px\">Gebruiker: </td><td width=\"100px\">" + finalnaam + "</td><td width=\"100px\">Rating: </td><td width=\"10px\">" + rating + "</td></tr><tr><td width=\"50px\"> Reactie: </td><td width=\"350px\">" + reactie + "</td></tr><tr><td><hr></td></tr>";
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return table;
        }

        public void RegisterReactie(int gebr, string scher, string text, int rating)
        {
            MySqlTransaction trans = null;
            if (scher == null) { scher = ""; }
            
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                string InsertString = @"insert into Reactie (gebruikerID, schermNaam, reactie, rating) 
                                  values (@gebr, @scher, @text, @rating)";
                MySqlCommand regcmd = new MySqlCommand(InsertString, conn);

                MySqlParameter gebr2 = new MySqlParameter("@gebr", MySqlDbType.Int16);
                MySqlParameter scher2 = new MySqlParameter("@scher", MySqlDbType.VarChar);
                MySqlParameter text2 = new MySqlParameter("@text", MySqlDbType.Text);
                MySqlParameter rating2 = new MySqlParameter("@rating", MySqlDbType.Int16);

                gebr2.Value = gebr;
                scher2.Value = scher;
                text2.Value = text;
                rating2.Value = rating;


                regcmd.Parameters.Add(gebr2);
                regcmd.Parameters.Add(scher2);
                regcmd.Parameters.Add(text2);
                regcmd.Parameters.Add(rating2);

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