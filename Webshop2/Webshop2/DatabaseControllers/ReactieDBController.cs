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
        public void deleteComment(int id)
        {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                string InsertString = @"DELETE FROM Reactie WHERE reactieID = @ID LIMIT 1";
                MySqlCommand regcmd = new MySqlCommand(InsertString, conn);

                MySqlParameter sqlid = new MySqlParameter("@ID", MySqlDbType.Int16);

                sqlid.Value = id;
                regcmd.Parameters.Add(sqlid);

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

        public string getReactieAdminTable()
        {
            string table = "<hr>";
            try
            {
                conn.Open();
                string selectQuery = "select G.gebruikersnaam, R.schermNaam, R.reactie, R.rating, R.reactieID from Reactie R join Gebruiker G on R.gebruikerID = G.gebruikerID ORDER BY R.adddate DESC LIMIT 100";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    string naam1 = dataReader.GetString("gebruikersnaam");
                    string naam2 = dataReader.GetString("schermNaam");
                    string reactie = dataReader.GetString("reactie");
                    int rating = dataReader.GetInt16("rating");
                    int id = dataReader.GetInt16("reactieID");
                    string finalnaam ="";
                    if (naam1.Length > 1) { finalnaam = naam1; } else if (naam2.Length > 1) { finalnaam = naam2; } else { finalnaam = "N.V.T"; }
                    table = table + "<tr><td>" + id + "</td><td>" + finalnaam + "</td><td>" + reactie + "</td><td>" + rating + "</td><td><button type=\"submit\" name=\"submitButton\" value=\""+id+"\"/><i class=\"fa fa-trash\"></i></button></a></td></tr>";
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
                    string finalnaam = "";
                    if (naam1.Length > 1) { finalnaam = naam1; } else if (naam2.Length > 1) { finalnaam = naam2; } else { finalnaam = "N.V.T"; }
                    table = table + "<tr><td>" + finalnaam + "</td><td>" + reactie + "</td><td>" + rating + "</td></tr>";
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