using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VetCareTCC.Models;

namespace VetCareTCC.Repositories
{
    public class CartRepository
    {
         DatabaseConnection con = new DatabaseConnection();
        MySqlConnection cn = new MySqlConnection(
            "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
        );

        public void inserirNoCarrinho(CartModel cm)
        {
            MySqlCommand cmd = new MySqlCommand(
                "insert into tbCart values(default, @idCustomer, @dataCart, @horaCart , @valorFinal)",
                con.ConectarBD()
            );

            cmd.Parameters.Add("@idCustomer", MySqlDbType.VarChar).Value = cm.CustomerID;
            cmd.Parameters.Add("@dataCart", MySqlDbType.VarChar).Value = cm.DtVenda;
            cmd.Parameters.Add("@horaCart", MySqlDbType.VarChar).Value = cm.horaVenda;
            cmd.Parameters.Add("@valorFinal", MySqlDbType.VarChar).Value = cm.ValorTotal;
            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }



        MySqlDataReader dr;

        public void buscaIdVenda(CartModel vend)
        {
            MySqlCommand cmd = new MySqlCommand(
                "SELECT idCart FROM tbCart ORDER BY idCart DESC limit 1",
                con.ConectarBD()
            );
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                vend.codVenda = dr[0].ToString();
            }
            con.DesconectarBD();
        }
    }
}
