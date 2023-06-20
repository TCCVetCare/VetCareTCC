using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static VetCareTCC.Models.UserModel;
using VetCareTCC.Models;
using VetCareTCC.ViewlModels;
using VetCareTCC.Repositories;

namespace VetCareTCC.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        ProductRepository queryProduct = new ProductRepository();

        public ActionResult Admin()
        {
            return View();
        }

        public void loadSupplier()
        {
            List<SelectListItem> supplier = new List<SelectListItem>();

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbSupplier", con);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    supplier.Add(
                        new SelectListItem
                        {
                            Text = rdr[1].ToString(), //nome
                            Value = rdr[0].ToString() //id do autor
                        }
                    );
                }
                con.Close(); //fechando conexÃ£o
            }

            ViewBag.supplier = new SelectList(supplier, "Value", "Text");
        }

        [CustomeAuthorize(UserRole.Admin)]
        public ActionResult CadProduct()
        {
            loadSupplier();
            return View();
        }

        [HttpPost]
        public ActionResult CadProduct(ProductModel product, HttpPostedFileBase file)
        {
            loadSupplier();
            string arquivo = Path.GetFileName(file.FileName);
            string file2 = "/Files/" + Path.GetFileName(file.FileName);
            string _path = Path.Combine(Server.MapPath("/Files"), arquivo);
            file.SaveAs(_path);
            product.imageProduct = file2;
            product.idSupplier = Request["supplier"];

            if (ModelState.IsValid)
            {
                ViewBag.msg = "Erro ao realizar cadastro do produto";
                return View(product);
            }
            else
            {
                queryProduct.insertProduct(product);
                ViewBag.msg = "Cadastro efetuado com sucesso";
                return RedirectToAction("ListProductAdmin", "Product");
            }
        }

        public ActionResult ListProduct()
        {
            return View(queryProduct.getProduct());
        }

        public ActionResult ListProductAdmin()
        {
            return View(queryProduct.getProduct());
        }

        //public ActionResult Carrinho(string id)
        //{
        //    Carrinho carrinho = Session["Carrinho"] as Carrinho;
        //    if (carrinho == null)
        //    {
        //        carrinho = new Carrinho();
        //        Session["Carrinho"] = carrinho;
        //    }
        //    Produto produto = acProduct.GetProductById(id);
        //    carrinho.Add(produto);

        //    return View();
        //}


        public ActionResult DeleteProduct(int id)
        {
            queryProduct.deleteProduct(id);
            return RedirectToAction("ListProductAdmin");
        }

        public ActionResult UpdateProduct(string id)
        {
            loadSupplier();
            return View(queryProduct.getProduct().Find(model => model.idProduct == id));
        }

        [HttpPost]
        public ActionResult UpdateProduct(int id, ProductModel product, HttpPostedFileBase file)
        {
            string arquivo = Path.GetFileName(file.FileName);
            string file2 = "/Files/" + Path.GetFileName(file.FileName);
            string _path = Path.Combine(Server.MapPath("/Files"), arquivo);
            file.SaveAs(_path);
            product.imageProduct = file2;
            product.idSupplier = Request["supplier"];
            loadSupplier();
            product.idProduct = id.ToString();
            queryProduct.updateProduct(product);

            return RedirectToAction("ListProductAdmin");
        }

        public ActionResult Search(string searchTerm)
        {
            IEnumerable<ProductModel> searchResults = Enumerable.Empty<ProductModel>();

            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("ListProduct");
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                string connectionString = "Server=localhost;DataBase=teste2;User=root;pwd=12345678"; // Substitua pela sua string de conexão real
                string query = "SELECT * FROM tbProduct WHERE nameProduct LIKE @searchTerm";


                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            List<ProductModel> products = new List<ProductModel>();
                            while (reader.Read())
                            {
                                ProductModel product = new ProductModel
                                {
                                    nameProduct = reader["nameProduct"].ToString(),
                                    idSupplier = Convert.ToString(reader["idSupplier"]),
                                    idProduct = Convert.ToString(reader["idProduct"]),
                                    descriptionProduct = Convert.ToString(reader["descriptionProduct"]),
                                    unitPrice = Convert.ToDouble(reader["unitPrice"]),
                                    imageProduct = Convert.ToString(reader["imageProduct"])

                                    // Preencha os demais campos do produto conforme necessário
                                };

                                products.Add(product);
                            }

                            searchResults = products;
                        }
                    }
                }
            }
            if (!searchResults.Any())
            {
                ViewBag.ErrorMessage = "Nenhum resultado encontrado.";
            }
            return PartialView("SearchResults", searchResults);
        }


        public List<PlanAnimalViewModel> GetSale()
        {

            List<PlanAnimalViewModel> sales = new List<PlanAnimalViewModel>();
            string idCustomer = (string)Session["idCustomer"];

            // Consulta SQL com INNER JOIN
            string query =
                @"SELECT tbSale.idSale, tbCustomer.nameCustomer, tbPayment.nameFormOfPayment, tbSale.date_sale
                FROM tbSale
                INNER JOIN tbCustomer ON tbSale.idCustomer = tbCustomer.idCustomer
                INNER JOIN tbPayment ON tbSale.idFormOfPayment = tbPayment.idPayment;";

            // Crie uma conexão com o banco de dados e execute a consulta
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;DataBase=teste2;User=root;pwd=12345678"))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlanAnimalViewModel pag = new PlanAnimalViewModel();
                            pag.idCustomer = Convert.ToString(reader["nameCustomer"]);
                            pag.idFormOfPayment = Convert.ToString(reader["nameFormOfPayment"]);
                            pag.dateSale = Convert.ToString(reader["date_sale"]);



                            sales.Add(pag);
                        }
                    }
                }
            }
            return sales;
        }

        [CustomeAuthorize(UserRole.Admin)]
        public ActionResult Sales()
        {
            return View(GetSale());
        }

    }
}
