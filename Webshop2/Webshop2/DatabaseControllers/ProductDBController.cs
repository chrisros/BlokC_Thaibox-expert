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
        public List<Product> haalProductGegevensOp()
        {
            List<Product> producten = new List<Product>();
            try
            {
                conn.Open();
                string selectQuery = "select *  from Product";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    int ID = dataReader.GetInt32("productID");
                    string productNaam = dataReader.GetString("naam");
                    string productMerk = dataReader.GetString("merk");
                    double productPrijs = dataReader.GetDouble("prijs");
                    string productDetail = dataReader.GetString("productOmschrijving");
           //         string productSoort = dataReader.GetString("soort");
                    string productAfbeelding = dataReader.GetString("afbeeldingPath");
                    Product p = new Product { productAfbeelding = productAfbeelding, productID = ID, productDetail = productDetail, productNaam = productNaam, productMerk = productMerk, productPrijs = productPrijs };
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

        public List<Product> haalProductDetailGegevensOp(int productID)
        {
            int prodID = productID;
            List<Product> producten = new List<Product>();
            try
            {
                conn.Open();
                string selectQuery = "select P.*, U.* from Product P join Uitvoering U on P.productID = U.productID  where (P.productID = @prodID)";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter prodIDPara = new MySqlParameter("@prodID", MySqlDbType.Int32);
                prodIDPara.Value = productID;
                cmd.Parameters.Add(prodIDPara);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    int ID = dataReader.GetInt32("productID");
                    string productNaam = dataReader.GetString("naam");
                    string productMerk = dataReader.GetString("merk");
                    double productPrijs = dataReader.GetDouble("prijs");
                    string productDetail = dataReader.GetString("productOmschrijving");
                 //   string productSoort = dataReader.GetString("soort");
                    string uitvoeringMaat = dataReader.GetString("maat");
                    string uitvoeringKleur = dataReader.GetString("kleur");
                    int uitvoeringVoorraad = dataReader.GetInt16("voorraad");
                    int productUitvoeringID = dataReader.GetInt32("uitvoeringID");
                    string productAfbeelding = dataReader.GetString("afbeeldingPath");
                    Product p = new Product { productAfbeelding = productAfbeelding, productID = ID, productNaam = productNaam, productMerk = productMerk, productPrijs = productPrijs, productDetail = productDetail, productMaat = uitvoeringMaat, productKleur = uitvoeringKleur, uitvoeringVoorraad = uitvoeringVoorraad, productUitvoeringID = productUitvoeringID };
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

        public List<string> getUitvoeringenKleur(int productID)
        {
            int prodID = productID;
            List<string> kleuren = new List<string>();
            try
            {
                conn.Open();

                string selectQuery = "select kleur from Uitvoering where (productID = @prodID)";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter prodIDPara = new MySqlParameter("@prodID", MySqlDbType.Int32);
                prodIDPara.Value = productID;
                cmd.Parameters.Add(prodIDPara);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    string kleur = dataReader.GetString("kleur");
                    kleuren.Add(kleur);
                }
            }
            catch (Exception)
            {

            }

            finally
            {
                conn.Close();
            }
            return kleuren;
        }



        public void WijzigenProduct(Product product)
        {
            MySqlTransaction trans = null;

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                string InsertString = @"Update Product set prijs = @prijs, naam = @naam, merk = @merk, productOmschrijving = @detail WHERE productID BETWEEN 13 AND 15";
                MySqlCommand regcmd = new MySqlCommand(InsertString, conn);

                MySqlParameter Naam = new MySqlParameter("@anaam", MySqlDbType.VarChar);
                MySqlParameter Prijs = new MySqlParameter("@prijs", MySqlDbType.Double);
                MySqlParameter Merk = new MySqlParameter("@merk", MySqlDbType.VarChar);
         //       MySqlParameter Soort = new MySqlParameter("@soort", MySqlDbType.VarChar);
                MySqlParameter Detail = new MySqlParameter("@detail", MySqlDbType.VarChar);

                Naam.Value = product.productNaam;
                Prijs.Value = product.productPrijs;
                Merk.Value = product.productMerk;
           //     Soort.Value = product.productSoort;
                Detail.Value = product.productDetail;

                regcmd.Parameters.Add(Naam);
                regcmd.Parameters.Add(Prijs);
           //     regcmd.Parameters.Add(Soort);
                regcmd.Parameters.Add(Detail);
                regcmd.Parameters.Add(Merk);

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


        public void RegisterProduct(Product product)
        {
            MySqlTransaction trans = null;

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                string InsertString = @"insert into Product (prijs, naam, merk, productOmschrijving, categorieID, afbeeldingPath) 
                                  values (@prijs, @naam, @merk, @detail, @catID, @afbeeldingPath)";
                MySqlCommand regcmd = new MySqlCommand(InsertString, conn);

                MySqlParameter prijsPara = new MySqlParameter("@prijs", MySqlDbType.VarChar);
                MySqlParameter naamPara = new MySqlParameter("@naam", MySqlDbType.VarChar);
                MySqlParameter merkPara = new MySqlParameter("@merk", MySqlDbType.VarChar);
                //MySqlParameter soortPara = new MySqlParameter("@soort", MySqlDbType.VarChar);
                MySqlParameter detailPara = new MySqlParameter("@detail", MySqlDbType.VarChar);
                MySqlParameter catPar = new MySqlParameter("@catID", MySqlDbType.Int32);
                MySqlParameter imagedataPara = new MySqlParameter("@afbeeldingPath", MySqlDbType.VarChar);
                //MySqlParameter imagemimetypePara = new MySqlParameter("@imagemimetype", MySqlDbType.VarChar);

                prijsPara.Value = product.productPrijs;
                naamPara.Value = product.productNaam;
                merkPara.Value = product.productMerk;
                //soortPara.Value = product.productSoort;
                detailPara.Value = product.productDetail;
                catPar.Value = 1;
                imagedataPara.Value = product.ImageData;
                //imagemimetypePara.Value = product.ImageMimeType;

                regcmd.Parameters.Add(prijsPara);
                regcmd.Parameters.Add(naamPara);
                regcmd.Parameters.Add(merkPara);
                //regcmd.Parameters.Add(soortPara);
                regcmd.Parameters.Add(detailPara);
                regcmd.Parameters.Add(catPar);
                regcmd.Parameters.Add(imagedataPara);
                //regcmd.Parameters.Add(imagemimetypePara);

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

        public List<Product> zoekProduct(string zoekTerm)
        {
            List<Product> producten = new List<Product>();
            try
            {
                conn.Open();
                string selectQuery = "select * from Product where (naam like '%" +@zoekTerm +"%')";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                //MySqlParameter termPara = new MySqlParameter("@term", MySqlDbType.Int32);
                //termPara.Value = zoekTerm;
                //cmd.Parameters.Add(termPara);
                //cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    int ID = dataReader.GetInt32("productID");
                    string productNaam = dataReader.GetString("naam");
                    string productMerk = dataReader.GetString("merk");
                    double productPrijs = dataReader.GetDouble("prijs");
                    string productDetail = dataReader.GetString("productOmschrijving");
           //         string productSoort = dataReader.GetString("soort");
                    string productAfbeelding = dataReader.GetString("afbeeldingPath");
                    Product p = new Product { productAfbeelding = productAfbeelding, productID = ID, productDetail = productDetail, productNaam = productNaam, productMerk = productMerk, productPrijs = productPrijs };
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

        public List<Product> filterProduct(string filterSoort, string filter)
        {
            List<Product> producten = new List<Product>();
            try
            {

                conn.Open();
                string selectQuery = "select distinct P.*, U.maat from Product P join Uitvoering U on P.productID = U.productID where (P." + filterSoort + " like '%" + @filter + "%') group by P.productID";

                if(filterSoort.Equals("maat"))
                {
                    selectQuery = "select distinct P.*, U.maat from Product P join Uitvoering U on P.productID = U.productID where (U." + filterSoort + " like '%" + @filter + "%') group by P.productID";
                }
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                //MySqlParameter filterSoortPara = new MySqlParameter("filterSoort", MySqlDbType.VarChar);
                //MySqlParameter filterPara = new MySqlParameter("filter", MySqlDbType.VarChar);
                //filterSoortPara.Value = filterSoort;
                ////filterPara.Value = filter;
                //cmd.Parameters.Add(filterSoortPara);
                ////cmd.Parameters.Add(filterPara);
                //cmd.Prepare();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    {
                        int ID = dataReader.GetInt32("productID");
                        string productNaam = dataReader.GetString("naam");
                        string productMerk = dataReader.GetString("merk");
                        double productPrijs = dataReader.GetDouble("prijs");
                        string productDetail = dataReader.GetString("productOmschrijving");
                        //         string productSoort = dataReader.GetString("soort");
                        string productAfbeelding = dataReader.GetString("afbeeldingPath");
                        Product p = new Product { productAfbeelding = productAfbeelding, productID = ID, productDetail = productDetail, productNaam = productNaam, productMerk = productMerk, productPrijs = productPrijs };
                        producten.Add(p);
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
            return producten;
        }
      
        public List<string> getMerken()
        {
            string selectQuery = "select distinct merk from Product";
            List<string> merken = new List<string>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    string merk = dataReader.GetString("merk");
                    merken.Add(merk);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return merken;

        }
        public List<string> getMaten()
        {
            string selectQuery = "select distinct maat from Product P join Uitvoering U on P.productID = U.productID";
            List<string> maten = new List<string>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    string maat = dataReader.GetString("maat");
                    maten.Add(maat);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return maten;
        }
    
    }
}