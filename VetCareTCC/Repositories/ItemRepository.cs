using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VetCareTCC.Models;

namespace VetCareTCC.Repositories
{
    public class ItemRepository
    {
        DatabaseConnection con = new DatabaseConnection();

        public void inserirItem(ItemCartModel cm)
        {
            MySqlCommand cmd = new MySqlCommand(
                "insert into itemCart  values(default, @idCart, @idProduct, @quantidade , @valorParcial)",
                con.ConectarBD()
            );

            cmd.Parameters.Add("@idCart", MySqlDbType.VarChar).Value = cm.PedidoID;
            cmd.Parameters.Add("@idProduct", MySqlDbType.VarChar).Value = cm.ProdutoID;
            cmd.Parameters.Add("@quantidade", MySqlDbType.VarChar).Value = cm.Qtd;
            cmd.Parameters.Add("@valorParcial", MySqlDbType.VarChar).Value = cm.valorParcial;
            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }
    }
}
