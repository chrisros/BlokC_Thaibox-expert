using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Webshop2.DatabaseControllers
{
    public abstract class DatabaseController
    {
        protected MySqlConnection conn;

        public DatabaseController()
        {
            //Vul hier de juiste gegevens in!!
            conn = new MySqlConnection("Server=meru.hhs.nl;Database=14108011;Uid=14108011;Pwd=iNucairahf;");
        }
    }
}