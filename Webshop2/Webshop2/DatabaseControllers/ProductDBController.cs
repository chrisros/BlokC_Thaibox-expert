using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Webshop2.Models;

namespace Webshop2.DatabaseControllers
{
    public class ProductDBController : DatabaseController
    {
        public void Producten(Product product)
        {
            MySqlTransaction trans = null;

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                string selectStat = @"Select * from Product where productID = 1";
                MySqlCommand regcmd = new MySqlCommand(selectStat, conn);

                //MySqlParameter NaamPara = new MySqlParameter("@naam", MySqlDbType.VarChar);
                //MySqlParameter Detail = new MySqlParameter("@merk", MySqlDbType.VarChar);

                //NaamPara.Value = "Naam";
                //Detail.Value = "merk";

                //regcmd.Parameters.Add(NaamPara);
                //regcmd.Parameters.Add(Detail);
                //regcmd.Prepare();

                MySqlDataReader reader = regcmd.ExecuteReader();

                while (reader.NextResult())
                {
                    Models.Product pr = new Models.Product();
                                        
                    string naam = reader.GetString("naam");
                    string merk = reader.GetString("merk");
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

        }
    }
}