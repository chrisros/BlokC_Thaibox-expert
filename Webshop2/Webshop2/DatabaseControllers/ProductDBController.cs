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
                    Product p = new Product { productID = ID, productDetail = productDetail, productNaam = productNaam, productMerk = productMerk, productPrijs = productPrijs };
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
                    Product p = new Product { productID = ID, productNaam = productNaam, productMerk = productMerk, productPrijs = productPrijs, productDetail = productDetail, productMaat = uitvoeringMaat, productKleur = uitvoeringKleur, uitvoeringVoorraad = uitvoeringVoorraad, productUitvoeringID = productUitvoeringID };
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

    }
}