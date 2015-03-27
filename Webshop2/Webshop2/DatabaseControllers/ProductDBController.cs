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
                    int productPrijs = dataReader.GetInt32("prijs");
                    string productDetail = dataReader.GetString("productOmschrijving");
                    string productSoort = dataReader.GetString("soort");
                    Product p = new Product { productID = ID, productDetail = productDetail, productNaam = productNaam, productMerk = productMerk, productSoort = productSoort, productPrijs = productPrijs};
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
            List<Product> producten = new List<Product>();
            try
            {
                conn.Open();
                string selectQuery = "select P.*, U.* from Product P join Uitvoering U on P.productID = U.productID  where (P.productID = " + productID + ")";
                //string selectQuery = "select * from Product where (productID = " + productID + ")";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    int ID = dataReader.GetInt32("productID");
                    string productNaam = dataReader.GetString("naam");
                    string productMerk = dataReader.GetString("merk");
                    int productPrijs = dataReader.GetInt32("prijs");
                    string productDetail = dataReader.GetString("productOmschrijving");
                    string productSoort = dataReader.GetString("soort");
                    string uitvoeringMaat = dataReader.GetString("maat");
                    string uitvoeringKleur = dataReader.GetString("kleur");
                    int uitvoeringVoorraad = dataReader.GetInt16("voorraad");
                    int productUitvoeringID = dataReader.GetInt32("uitvoeringID");
                    Product pr = new Product { productNaam = productNaam, productMerk = productMerk, productPrijs = productPrijs, productDetail = productDetail, uitvoeringMaat = uitvoeringMaat, uitvoeringKleur = uitvoeringKleur, uitvoeringVoorraad = uitvoeringVoorraad, productUitvoeringID = productUitvoeringID };
                    //Product p = new Product { productID = ID, productDetail = productDetail, productNaam = productNaam, productMerk = productMerk, productSoort = productSoort, productPrijs = productPrijs };
                    producten.Add(pr);

                    //string maat = dataReader.GetString("maat");
                    //string kleur = dataReader.GetString("kleur");
                    //int uitvoeringsID = dataReader.GetInt32("uitvoeringID");
                    if (productSoort == "kleding" || productSoort == "shirt" || productSoort == "broek" || productSoort == "schoenen" || productSoort == "sokken" || productSoort == "ondergoed")
                    {
                        string selectQquery = "select * from Kleding where (productID = " + productID + ")";
                        MySqlCommand cmdd = new MySqlCommand(selectQuery, conn);
                        MySqlDataReader dattaReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            string productGeslacht = dataReader.GetString("geslacht");
                            string productMateriaal = dataReader.GetString("materiaal");
                            Product p = new Product { productID = ID, productDetail = productDetail, productNaam = productNaam, productMerk = productMerk, productSoort = productSoort, productPrijs = productPrijs, productGeslacht = productGeslacht, productMateriaal = productMateriaal };
                            producten.Add(p);
                        }
                    }
                    else if (productSoort == "LesMateriaal")
                    {
                        string selectQquery = "select * from LesMateriaal where (productID = " + productID + ")";
                        MySqlCommand cmdd = new MySqlCommand(selectQuery, conn);
                        MySqlDataReader dattaReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            string productAuteur = dataReader.GetString("auteur");
                            string productUitgever = dataReader.GetString("uitgever");
                            Product p = new Product { productID = ID, productDetail = productDetail, productNaam = productNaam, productMerk = productMerk, productSoort = productSoort, productPrijs = productPrijs, productAuteur = productAuteur, productUitgever = productUitgever };
                            producten.Add(p);
                        }
                    }
                    else
                    {
                        string selectQquery = "select * from OefenMateriaal where (productID = " + productID + ")";
                        MySqlCommand cmdd = new MySqlCommand(selectQuery, conn);
                        MySqlDataReader dattaReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            string productGewicht = dataReader.GetString("gewicht");
                            string productMateriaal = dataReader.GetString("materiaal");
                            Product p = new Product { productID = ID, productDetail = productDetail, productNaam = productNaam, productMerk = productMerk, productSoort = productSoort, productPrijs = productPrijs, productGewicht = productGewicht, productMateriaal = productMateriaal };
                            producten.Add(p);
                        }
                    }
                 //   string selectQQuery = "select P.*, U.* from Product P join Uitvoering U on P.productID = U.productID  where (P.productID = " + productID + ")";

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
            List<string> kleuren = new List<string>();
            try
            {
                conn.Open();
            
                string selectQuery = "select kleur from Uitvoering where (productID = " + productID + ")";
                MySqlCommand cmd = new MySqlCommand(selectQuery, conn);
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

                string InsertString = @"Update Product set prijs = @prijs, naam = @naam, merk = @merk, soort = @soort, productOmschrijving = @detail WHERE productID BETWEEN 13 AND 15";
                MySqlCommand regcmd = new MySqlCommand(InsertString, conn);

                MySqlParameter Naam = new MySqlParameter("@anaam", MySqlDbType.VarChar);
                MySqlParameter Prijs = new MySqlParameter("@prijs", MySqlDbType.Int32);
                MySqlParameter Merk = new MySqlParameter("@merk", MySqlDbType.VarChar);
                MySqlParameter Soort = new MySqlParameter("@soort", MySqlDbType.VarChar);
                MySqlParameter Detail = new MySqlParameter("@detail", MySqlDbType.VarChar);

                Naam.Value = product.productNaam;
                Prijs.Value = product.productPrijs;
                Merk.Value = product.productMerk;
                Soort.Value = product.productSoort;
                Detail.Value = product.productDetail;

                regcmd.Parameters.Add(Naam);
                regcmd.Parameters.Add(Prijs);
                regcmd.Parameters.Add(Soort);
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

                string InsertString = @"insert into Product (prijs, naam, merk, soort, productOmschrijving) 
                                  values (@prijs, @naam, @merk, @soort, @detail)";
                MySqlCommand regcmd = new MySqlCommand(InsertString, conn);

                MySqlParameter prijsPara = new MySqlParameter("@prijs", MySqlDbType.VarChar);
                MySqlParameter naamPara = new MySqlParameter("@naam", MySqlDbType.VarChar);
                MySqlParameter merkPara = new MySqlParameter("@merk", MySqlDbType.VarChar);
                MySqlParameter soortPara = new MySqlParameter("@soort", MySqlDbType.VarChar);
                MySqlParameter detailPara = new MySqlParameter("@detail", MySqlDbType.VarChar);

                //MySqlParameter imagedataPara = new MySqlParameter("@imagedata", MySqlDbType.VarBinary);
                //MySqlParameter imagemimetypePara = new MySqlParameter("@imagemimetype", MySqlDbType.VarChar);
                
                prijsPara.Value = product.productPrijs;
                naamPara.Value = product.productNaam;
                merkPara.Value = product.productMerk;
                soortPara.Value = product.productSoort;
                detailPara.Value = product.productDetail;
                //imagedataPara.Value = product.ImageData;
                //imagemimetypePara.Value = product.ImageMimeType;

                regcmd.Parameters.Add(prijsPara);
                regcmd.Parameters.Add(naamPara);
                regcmd.Parameters.Add(merkPara);
                regcmd.Parameters.Add(soortPara);
                regcmd.Parameters.Add(detailPara);
                //regcmd.Parameters.Add(imagedataPara);
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

        public Product getImageOutDB()
        {

            try
            {
                conn.Open();
                string select = @"select imagedata, imagemimetype from Afbeelding";

                MySqlCommand cmd = new MySqlCommand(select, conn);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    byte[] imageData = null;
                    //Check, want wanneer niet van het type Byte[] wordt er een exceptie gegooid.
                    if (dataReader["imagedata"] is System.Byte[])
                    {
                        imageData = (byte[])dataReader["imagedata"];
                    }


                    string imageMimeType = dataReader.GetString("imagemimetype");

                    return new Product
                    {
                        ImageData = imageData,
                        ImageMimeType = imageMimeType
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }


    }
}