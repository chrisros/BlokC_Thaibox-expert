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
                string selectQuery = "select * from Product";
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
                    Product p = new Product { productID = ID, productDetail = productDetail, productNaam = productNaam, productMerk = productMerk, productSoort = productSoort, productPrijs = productPrijs };
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
                
                prijsPara.Value = product.productPrijs;
                naamPara.Value = product.productNaam;
                merkPara.Value = product.productMerk;
                soortPara.Value = product.productSoort;
                detailPara.Value = product.productDetail;

                regcmd.Parameters.Add(prijsPara);
                regcmd.Parameters.Add(naamPara);
                regcmd.Parameters.Add(merkPara);
                regcmd.Parameters.Add(soortPara);
                regcmd.Parameters.Add(detailPara);

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