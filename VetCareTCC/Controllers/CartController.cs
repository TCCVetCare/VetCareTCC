using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using VetCareTCC.Models;
using VetCareTCC.Repositories;

namespace VetCareTCC.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        CartRepository  queryCart = new CartRepository();
        ProductRepository queryProduct = new ProductRepository();
        ItemRepository queryItem = new ItemRepository();

       DatabaseConnection con = new DatabaseConnection();
        MySqlConnection cn = new MySqlConnection(
            "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
        );
        public static string codigo;

        public ActionResult AdicionarCarrinho(int id, double pre)
        {
            CartModel carrinho =
                Session["Carrinho"] != null ? (CartModel)Session["Carrinho"] : new CartModel();
            var produto = queryProduct.GetConsProd(id);
            codigo = id.ToString();

            ProductModel prod = new ProductModel();

            if (produto != null)
            {
                var itemPedido = new ItemCartModel();
                itemPedido.ItemPedidoID = Guid.NewGuid();
                itemPedido.ProdutoID = id.ToString();
                itemPedido.Produto = produto[0].nameProduct;
                itemPedido.imageProduct = produto[0].imageProduct;
                itemPedido.Qtd = 1;
                itemPedido.valorUnit = pre;

                List<ItemCartModel> x = carrinho.ItensPedido.FindAll(
                    l => l.Produto == itemPedido.Produto
                );

                if (x.Count != 0)
                {
                    carrinho.ItensPedido
                        .FirstOrDefault(p => p.Produto == produto[0].nameProduct)
                        .Qtd += 1;
                    itemPedido.valorParcial = itemPedido.Qtd * itemPedido.valorUnit;
                    carrinho.ValorTotal += itemPedido.valorParcial;
                    carrinho.ItensPedido
                        .FirstOrDefault(p => p.Produto == produto[0].nameProduct)
                        .valorParcial =
                        carrinho.ItensPedido
                            .FirstOrDefault(p => p.Produto == produto[0].nameProduct)
                            .Qtd * itemPedido.valorUnit;
                }
                else
                {
                    itemPedido.valorParcial = itemPedido.Qtd * itemPedido.valorUnit;
                    carrinho.ValorTotal += itemPedido.valorParcial;
                    carrinho.ItensPedido.Add(itemPedido);
                }

                /*carrinho.ValorTotal = carrinho.ItensPedido.Select(i => i.Produto).Sum(d => d.Valor);*/

                Session["Carrinho"] = carrinho;
            }

            return RedirectToAction("Carrinho");
        }

        public ActionResult Carrinho()
        {
            CartModel carrinho =
                Session["Carrinho"] != null ? (CartModel)Session["Carrinho"] : new CartModel();
                return View(carrinho);
        }

        public ActionResult ExcluirItem(Guid id)
        {
            var carrinho =
                Session["Carrinho"] != null ? (CartModel)Session["Carrinho"] : new CartModel();
            var itemExclusao = carrinho.ItensPedido.FirstOrDefault(i => i.ItemPedidoID == id);

            carrinho.ValorTotal -= itemExclusao.valorParcial;

            carrinho.ItensPedido.Remove(itemExclusao);

            Session["Carrinho"] = carrinho;
            return RedirectToAction("Carrinho");
        }

        public ActionResult savCart(string id)
        {
            return View();
        }

        public void insertSale()
        {
            MySqlCommand cmd = new MySqlCommand(
                "insert into tbSale(idCustomer, idFormOfPayment) values(@idCustomer, @idFormOfPayment)",
                con.ConectarBD()
            );

            string idCustomer = (string)Session["idCustomer"];
            int idFormOfPayment = 1;


            cmd.Parameters.AddWithValue("@idCustomer", idCustomer);
            cmd.Parameters.AddWithValue("@idFormOfPayment", idFormOfPayment);
            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }

        [HttpPost]
        public ActionResult savCart(CartModel x, string IdCart)
        {
            //if ((Session["usuarioLogado"] == null) || (Session["senhaLogado"] == null))

            //{
            //    return RedirectToAction("Index2", "Login");
            //}
            //else
            //{

            string idCustomer = (string)Session["idCustomer"];
            var carrinho =
                Session["Carrinho"] != null ? (CartModel)Session["Carrinho"] : new CartModel();

            CartModel md = new CartModel();
            ItemCartModel mdV = new ItemCartModel();

            md.DtVenda = DateTime.Now.ToLocalTime().ToString("dd/MM/yyyy");
            md.horaVenda = DateTime.Now.ToLocalTime().ToString("HH:mm");
            md.CustomerID = Session["idCustomer"].ToString();
            md.ValorTotal = carrinho.ValorTotal;

            queryCart.inserirNoCarrinho(md);

            queryCart.buscaIdVenda(x);

            for (int i = 0; i < carrinho.ItensPedido.Count; i++)
            {
                mdV.PedidoID = x.codVenda;
                mdV.ProdutoID = carrinho.ItensPedido[i].ProdutoID;
                mdV.Qtd = carrinho.ItensPedido[i].Qtd;
                mdV.valorParcial = carrinho.ItensPedido[i].valorParcial;
                queryItem.inserirItem(mdV);
            }

            carrinho.ValorTotal = 0;
            carrinho.ItensPedido.Clear();

            insertSale();

            return RedirectToAction("confCarrinho");
        }

        //}

        public ActionResult confCarrinho()
        {
            return View();
        }
    }

}
