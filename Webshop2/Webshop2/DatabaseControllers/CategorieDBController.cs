﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webshop2.Models;
using MySql.Data.MySqlClient;
using Webshop2.Controllers;

namespace Webshop2.DatabaseControllers
{
    public class CategorieDBController : DatabaseController
    {

        public void voegCatToe(Categorie categorie)
        {
            MySqlTransaction trans = null;

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                string InsertString = @"insert into Categorie(categorieNaam) values (@soort);";
                MySqlCommand regcmd = new MySqlCommand(InsertString, conn);


                MySqlParameter soortPara = new MySqlParameter("@soort", MySqlDbType.VarChar);

                soortPara.Value = categorie.categorieNaam;

                regcmd.Parameters.Add(soortPara);


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

        public List<Categorie> haalCatNamenOp()
        {
            List<Categorie> categorie = new List<Categorie>();
            try
            {
                conn.Open();
                string selectQuery = "SELECT * FROM Categorie ORDER BY categorieNaam ASC ";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    int ID = dataReader.GetInt32("categorieID");
                    string catNaam = dataReader.GetString("categorieNaam");
                    Categorie c = new Categorie { categorieID = ID, categorieNaam = catNaam };
                    categorie.Add(c);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return categorie;
        }


        // Kleding
        public List<Product> haalGegevensOpvanCat(string catNaam)
        {
            List<Product> producten = new List<Product>();
            try
            {
                conn.Open();
                string selectQuery = "select * from Product P Join Categorie C On  C.categorieID = P.categorieID where (C.categorieNaam like '%" + @catNaam + "%')";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                int x = dataReader.GetOrdinal("afbeeldingPath");
                string productAfbeelding;
                while (dataReader.Read())
                {
                    int ID = dataReader.GetInt32("productID");
                    string productNaam = dataReader.GetString("naam");
                    string productMerk = dataReader.GetString("merk");
                    int productPrijs = dataReader.GetInt32("prijs");
                    string productDetail = dataReader.GetString("productOmschrijving");
                    if (!dataReader.IsDBNull(x))
                    {
                        productAfbeelding = dataReader.GetString("afbeeldingPath");
                    }
                    else
                    {
                        productAfbeelding = "imagefail.jpg";
                    }
                    int categorieID = dataReader.GetInt32("categorieID");
                    string categorie = dataReader.GetString("categorieNaam");
                    Product p = new Product
                    {
                        productID = ID,
                        productNaam = productNaam,
                        productMerk = productMerk,
                        productPrijs = productPrijs,
                        productDetail = productDetail,
                        productAfbeelding = productAfbeelding,
                        productCat = categorieID

                    };
                    producten.Add(p);
                }
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

        public void WijzigenCategorie(Categorie categorie)
        {
            MySqlTransaction trans = null;

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                string InsertString = @"Update Categorie set categorieNaam = @catNaam WHERE categorieID = @CatID";
                MySqlCommand regcmd = new MySqlCommand(InsertString, conn);

                MySqlParameter Naam = new MySqlParameter("@catNaam", MySqlDbType.VarChar);
                MySqlParameter CatID = new MySqlParameter("@CatID", MySqlDbType.Double);

                Naam.Value = categorie.categorieNaam;
                CatID.Value = categorie.categorieID;

                regcmd.Parameters.Add(Naam);
                regcmd.Parameters.Add(CatID);

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
        public void VerwijderenCategorie(Categorie categorie)
        {
            MySqlTransaction trans = null;

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                string InsertString = @"DELETE FROM Categorie WHERE categorieID = @CatID";
                MySqlCommand regcmd = new MySqlCommand(InsertString, conn);

                MySqlParameter CatID = new MySqlParameter("@CatID", MySqlDbType.Double);

                CatID.Value = categorie.categorieID;

                regcmd.Parameters.Add(CatID);

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