using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using VetCareTCC.Models;

namespace VetCareTCC.Repositories
{
    public class ProductRepository
    {
        DatabaseConnection con = new DatabaseConnection();

        public void insertProduct(ProductModel product)
        {
            MySqlCommand cmd = new MySqlCommand(
                "insert into tbProduct values(default, @idSupplier, @nameProduct, @descriptionProduct, @unitPrice, @imageProduct)",
                con.ConectarBD()
            );
            cmd.Parameters.Add("@idSupplier", MySqlDbType.Int64).Value = product.idSupplier;
            cmd.Parameters.Add("@nameProduct", MySqlDbType.VarChar).Value = product.nameProduct;
            cmd.Parameters.Add("@descriptionProduct", MySqlDbType.VarChar).Value =
                product.descriptionProduct;
            cmd.Parameters.Add("@unitPrice", MySqlDbType.Decimal).Value = product.unitPrice;
            cmd.Parameters.Add("@imageProduct", MySqlDbType.VarChar).Value = product.imageProduct;
            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }

        public List<ProductModel> getProduct()
        {
            List<ProductModel> listaClientes = new List<ProductModel>();
            MySqlCommand cmd = new MySqlCommand("select * from tbProduct", con.ConectarBD());

            //adapter para lista
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

            //tabela virtual
            DataTable db = new DataTable();
            adapter.Fill(db);

            con.DesconectarBD();

            //enquanto existir linhas(registros) no banco
            //o foreach irá adicionar os valors vindo do banco nos atributos da ModelCliente

            foreach (DataRow dr in db.Rows)
            {
                listaClientes.Add(
                    new ProductModel
                    {
                        idSupplier = Convert.ToString(dr["idSupplier"]),
                        idProduct = Convert.ToString(dr["idProduct"]),
                        nameProduct = Convert.ToString(dr["nameProduct"]),
                        descriptionProduct = Convert.ToString(dr["descriptionProduct"]),
                        unitPrice = Convert.ToDouble(dr["unitPrice"]),
                        imageProduct = Convert.ToString(dr["imageProduct"])
                    }
                );
            }
            return listaClientes;
        }

        // Obtém um produto por ID
        //private Produto GetProductById(string id)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings[
        //        "NomeDaConnectionString"
        //    ].ConnectionString;

        //    using (var connection = new MySqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        string query = "SELECT * FROM tbProduct WHERE IdProduct = @Id";
        //        MySqlCommand command = new MySqlCommand(query, connection);
        //        command.Parameters.AddWithValue("@Id", id);

        //        using (var reader = command.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                return new Produto
        //                {
        //                    idSupplier = Convert.ToString(dr["idSupplier"]),
        //                    idProduct = Convert.ToString(dr["idProduct"]),
        //                    nameProduct = Convert.ToString(dr["nameProduct"]),
        //                    descriptionProduct = Convert.ToString(dr["descriptionProduct"]),
        //                    unitPrice = Convert.ToDouble(dr["unitPrice"]),
        //                    image = Convert.ToString(dr["imageProduct"])
        //                };
        //            }
        //        }
        //    }

        //    return null;
        //}



        public List<ProductModel> GetConsProd(int id)
        {
            List<ProductModel> Produtoslist = new List<ProductModel>();

            MySqlCommand cmd = new MySqlCommand("select * from tbProduct where idProduct=@id", con.ConectarBD());
            cmd.Parameters.AddWithValue("@id", id);
            MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            sd.Fill(dt);
            con.DesconectarBD();

            foreach (DataRow dr in dt.Rows)
            {
                Produtoslist.Add(
                    new ProductModel
                    {
                        idSupplier = Convert.ToString(dr["idSupplier"]),
                        idProduct = Convert.ToString(dr["idProduct"]),
                        nameProduct = Convert.ToString(dr["nameProduct"]),
                        descriptionProduct = Convert.ToString(dr["descriptionProduct"]),
                        unitPrice = Convert.ToDouble(dr["unitPrice"]),
                        imageProduct = Convert.ToString(dr["imageProduct"])
                    });
            }
            return Produtoslist;
        }

        public bool deleteProduct(int id)
        {
            MySqlCommand cmd = new MySqlCommand(
                "delete from tbProduct where idProduct=@id",
                con.ConectarBD()
            );
            cmd.Parameters.AddWithValue("id", id);

            int i = cmd.ExecuteNonQuery();
            con.DesconectarBD();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool updateProduct(ProductModel product)
        {
            MySqlCommand cmd = new MySqlCommand(
                "update tbProduct set idSupplier=@idSupplier, nameProduct=@nameProduct, descriptionProduct=@descriptionProduct, unitPrice=@unitPrice,"
                    + "imageProduct=@imageProduct where "
                    + "idProduct=@idProduct",
                con.ConectarBD()
            );

            cmd.Parameters.Add("@idSupplier", MySqlDbType.Int64).Value = product.idSupplier;
            cmd.Parameters.Add("@idProduct", MySqlDbType.Int64).Value = product.idProduct;
            cmd.Parameters.Add("@nameProduct", MySqlDbType.VarChar).Value = product.nameProduct;
            cmd.Parameters.Add("@descriptionProduct", MySqlDbType.VarChar).Value =
                product.descriptionProduct;
            cmd.Parameters.Add("@unitPrice", MySqlDbType.Decimal).Value = product.unitPrice;
            cmd.Parameters.Add("@imageProduct", MySqlDbType.VarChar).Value = product.imageProduct;

            int i = cmd.ExecuteNonQuery();
            con.DesconectarBD();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
