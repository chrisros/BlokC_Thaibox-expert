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
                int x = dataReader.GetOrdinal("afbeeldingPath");
                string productAfbeelding;
                while (dataReader.Read())
                {
                    int ID = dataReader.GetInt32("productID");
                    string productNaam = dataReader.GetString("naam");
                    string productMerk = dataReader.GetString("merk");
                    double productPrijs = dataReader.GetDouble("prijs");
                    string productDetail = dataReader.GetString("productOmschrijving");
           //         string productSoort = dataReader.GetString("soort");
                    
                    if (!dataReader.IsDBNull(x))
                    {
                        productAfbeelding = dataReader.GetString("afbeeldingPath");
                    }
                    else 
                    {
                        productAfbeelding = "imagefail.jpg";
                    }
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
                int x = dataReader.GetOrdinal("afbeeldingPath");
                string productAfbeelding;
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
                    if (!dataReader.IsDBNull(x))
                    {
                        productAfbeelding = dataReader.GetString("afbeeldingPath");
                    }
                    else
                    {
                        productAfbeelding = "imagefail.jpg";
                    }
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



        public void WijzigenProduct(Product product, int id)
        {
            MySqlTransaction trans = null;

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                string InsertString = @"Update Product set prijs = @prijs, naam = @naam, merk = @merk, productOmschrijving = @detail WHERE productID = @prodID";
                MySqlCommand regcmd = new MySqlCommand(InsertString, conn);

                MySqlParameter naamPara = new MySqlParameter("@naam", MySqlDbType.VarChar);
                MySqlParameter prijsPara = new MySqlParameter("@prijs", MySqlDbType.Double);
                MySqlParameter merkPara = new MySqlParameter("@merk", MySqlDbType.VarChar);
         //       MySqlParameter Soort = new MySqlParameter("@soort", MySqlDbType.VarChar);
                MySqlParameter detailPara = new MySqlParameter("@detail", MySqlDbType.VarChar);
                MySqlParameter ID = new MySqlParameter("@prodID", MySqlDbType.VarChar);


                prijsPara.Value = product.productPrijs;
                naamPara.Value = product.productNaam;
                merkPara.Value = product.productMerk;
                detailPara.Value = product.productDetail;
                ID.Value = id;

                regcmd.Parameters.Add(naamPara);
                regcmd.Parameters.Add(prijsPara);
           //     regcmd.Parameters.Add(Soort);
                regcmd.Parameters.Add(detailPara);
                regcmd.Parameters.Add(merkPara);
                regcmd.Parameters.Add(ID);


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

                string InsertString = @"insert into Product (prijs, naam, merk, productOmschrijving, categorieID, afbeeldingPath, geslacht) 
                                  values (@prijs, @naam, @merk, @detail, @catID, @afbeeldingPath, @geslacht)";
                MySqlCommand regcmd = new MySqlCommand(InsertString, conn);

                MySqlParameter prijsPara = new MySqlParameter("@prijs", MySqlDbType.VarChar);
                MySqlParameter naamPara = new MySqlParameter("@naam", MySqlDbType.VarChar);
                MySqlParameter merkPara = new MySqlParameter("@merk", MySqlDbType.VarChar);
                MySqlParameter detailPara = new MySqlParameter("@detail", MySqlDbType.VarChar);
                MySqlParameter catPar = new MySqlParameter("@catID", MySqlDbType.Int32);
                MySqlParameter imagedataPara = new MySqlParameter("@afbeeldingPath", MySqlDbType.VarChar);
                MySqlParameter geslPara = new MySqlParameter("@geslacht", MySqlDbType.VarChar);

                prijsPara.Value = product.productPrijs;
                naamPara.Value = product.productNaam;
                merkPara.Value = product.productMerk;
                detailPara.Value = product.productDetail;
                catPar.Value = product.productCat;
                imagedataPara.Value = product.ImageData;
                geslPara.Value = product.productGeslacht;

                regcmd.Parameters.Add(prijsPara);
                regcmd.Parameters.Add(naamPara);
                regcmd.Parameters.Add(merkPara);
                regcmd.Parameters.Add(detailPara);
                regcmd.Parameters.Add(catPar);
                regcmd.Parameters.Add(imagedataPara);
                regcmd.Parameters.Add(geslPara);

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

        public Int32 getLastProductID()
        {
            int productID = 0;
            try
            {
                string selectQuery = "select productID from Product order by productID desc limit 1";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlParameter prodIDPara = new MySqlParameter("@prodID", MySqlDbType.Int32);
                prodIDPara.Value = productID;
                cmd.Parameters.Add(prodIDPara);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    productID = dataReader.GetInt32("productID");
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return productID;
        }
        
        public void registerUitvoering(string maat, string kleur, int voorraad)
            {
            MySqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();
                 
                string InsertString = @"insert into Uitvoering (productID, voorraad, maat, kleur) 
                                  values (@productID,  @voorraad, @maat, @kleur)";
                MySqlCommand regcmd = new MySqlCommand(InsertString, conn);

                MySqlParameter prodIDPara = new MySqlParameter("@productID", MySqlDbType.Int32);
                MySqlParameter voorraadPara = new MySqlParameter("@voorraad", MySqlDbType.Int32);
                MySqlParameter maatPara = new MySqlParameter("@maat", MySqlDbType.VarChar);
                MySqlParameter kleurPara = new MySqlParameter("@kleur", MySqlDbType.VarChar);

                prodIDPara.Value = getLastProductID();
                voorraadPara.Value = voorraad;
                maatPara.Value = maat;
                kleurPara.Value = kleur;

                regcmd.Parameters.Add(prodIDPara);
                regcmd.Parameters.Add(voorraadPara);
                regcmd.Parameters.Add(maatPara);
                regcmd.Parameters.Add(kleurPara);

                regcmd.Prepare();

                regcmd.ExecuteNonQuery();

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
                int x = dataReader.GetOrdinal("afbeeldingPath");
                string productAfbeelding;
                while (dataReader.Read())
                {
                    int ID = dataReader.GetInt32("productID");
                    string productNaam = dataReader.GetString("naam");
                    string productMerk = dataReader.GetString("merk");
                    double productPrijs = dataReader.GetDouble("prijs");
                    string productDetail = dataReader.GetString("productOmschrijving");
           //         string productSoort = dataReader.GetString("soort");
                    if (!dataReader.IsDBNull(x))
                    {
                        productAfbeelding = dataReader.GetString("afbeeldingPath");
                    }
                    else
                    {
                        productAfbeelding = "imagefail.jpg";
                    }
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
                int x = dataReader.GetOrdinal("afbeeldingPath");
                string productAfbeelding;
                while (dataReader.Read())
                {
                    {
                        int ID = dataReader.GetInt32("productID");
                        string productNaam = dataReader.GetString("naam");
                        string productMerk = dataReader.GetString("merk");
                        double productPrijs = dataReader.GetDouble("prijs");
                        string productDetail = dataReader.GetString("productOmschrijving");
                        //         string productSoort = dataReader.GetString("soort");
                        if (!dataReader.IsDBNull(x))
                        {
                            productAfbeelding = dataReader.GetString("afbeeldingPath");
                        }
                        else 
                        {
                            productAfbeelding = "imagefail.jpg";
                        }
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
            string selectQuery = "select distinct merk from Product ORDER BY merk ASC";
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

        public List<string> getGeslacht()
        {
            string selectQuery = "select distinct geslacht from Product P join Uitvoering U on P.productID = U.productID";
            List<string> geslachten = new List<string>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    string maat = dataReader.GetString("geslacht"); 
                    geslachten.Add(maat);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return geslachten;
        }

        public List<Product> uitgelichteProducten() 
        {
            List<Product> uitgelicht = new List<Product>();

            try
            {
                conn.Open();

                string selectuit = @"select * from Product ORDER BY RAND() LIMIT 3";
                MySqlCommand uitcmd = new MySqlCommand(selectuit, conn);
                MySqlDataReader reader = uitcmd.ExecuteReader();
                int x = reader.GetOrdinal("afbeeldingPath");
                while (reader.Read())
                {
                    Product p = new Product();
                    p.productID = reader.GetInt32("productID");
                    p.productNaam = reader.GetString("naam");
                    p.productPrijs = reader.GetDouble("prijs");
                    if (!reader.IsDBNull(x))
                    {
                        p.productAfbeelding = reader.GetString("afbeeldingPath");
                    }
                    else 
                    {
                        p.productAfbeelding = "imagefail.jpg";
                    }
                    uitgelicht.Add(p);
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

            return uitgelicht;
        }
        public void VerwijderenProduct(Product product)
        {
            MySqlTransaction trans = null;

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                string InsertString = @"DELETE FROM Product WHERE productID = @prodID";
                MySqlCommand regcmd = new MySqlCommand(InsertString, conn);

                MySqlParameter prodID = new MySqlParameter("@prodID", MySqlDbType.Double);

                prodID.Value = product.productID;

                regcmd.Parameters.Add(prodID);

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