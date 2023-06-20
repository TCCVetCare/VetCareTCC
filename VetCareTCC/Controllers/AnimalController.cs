using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class AnimalController : Controller
    {
        // GET: Animal
        AnimalRepository queryAnimal = new AnimalRepository();
        CustomerRepositoy queryCustomer = new CustomerRepositoy();

        public ActionResult Index()
        {
            return View();
        }

        public void loadCustomer(string id)
        {
            List<SelectListItem> customer = new List<SelectListItem>();

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM tbCustomer WHERE idCustomer = @idCustomer",
                    con
                );
                cmd.Parameters.AddWithValue("@idCustomer", id);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    customer.Add(
                        new SelectListItem
                        {
                            Text = rdr[1].ToString(), // nome
                            Value = rdr[0].ToString() // id do autor
                        }
                    );
                }
                con.Close(); // fechando conexão
            }

            ViewBag.customer = new SelectList(customer, "Value", "Text");
        }


        public void loadGenderPet()
        {
            List<SelectListItem> genderPet = new List<SelectListItem>();

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from GenderAnimal", con);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    genderPet.Add(
                        new SelectListItem
                        {
                            Text = rdr[1].ToString(), //nome
                            Value = rdr[0].ToString() //id do autor
                        }
                    );
                }
                con.Close(); //fechando conexÃ£o
            }

            ViewBag.genderPet = new SelectList(genderPet, "Value", "Text");
        }

        public void loadBreedAnimal()
        {
            List<SelectListItem> breedAnimal = new List<SelectListItem>();

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbBreedAnimal", con);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    breedAnimal.Add(
                        new SelectListItem
                        {
                            Text = rdr[1].ToString(), //nome
                            Value = rdr[0].ToString() //id do autor
                        }
                    );
                }
                con.Close(); //fechando conexÃ£o
            }

            ViewBag.breedAnimal = new SelectList(breedAnimal, "Value", "Text");
        }

        public void loadSpeciesAnimal()
        {
            List<SelectListItem> speciesAnimal = new List<SelectListItem>();

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbSpeciesAnimal", con);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    speciesAnimal.Add(
                        new SelectListItem
                        {
                            Text = rdr[1].ToString(), //nome
                            Value = rdr[0].ToString() //id do autor
                        }
                    );
                }
                con.Close(); //fechando conexÃ£o
            }

            ViewBag.speciesAnimal = new SelectList(speciesAnimal, "Value", "Text");
        }

        public ActionResult CadAnimal(string name)
        {
            string idCustomer = (string)Session["idCustomer"];
            Session["name"] = name;
            loadCustomer(idCustomer);
            loadBreedAnimal();
            loadSpeciesAnimal();
            loadGenderPet();
            // Verifica se o usuário está autenticado
            if (!User.Identity.IsAuthenticated)
            {
                // Se não estiver autenticado, redireciona para a página de login
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult CadAnimal(AnimalModel animal, HttpPostedFileBase file, string email) //string idCustomer
        {
            if (!ModelState.IsValid)
            {
                string idCustomer = (string)Session["idCustomer"];
                string name = (string)Session["name"];
                loadCustomer(idCustomer);
                loadBreedAnimal();
                loadSpeciesAnimal();
                loadGenderPet();
                animal.idCustomer = Request["customer"];
                animal.idBreedAnimal = Request["breedAnimal"];
                animal.idSpeciesAnimal = Request["speciesAnimal"];
                animal.idGenderAnimal = Request["genderPet"];

                Session["idBreedAnimal"] = animal.idBreedAnimal;
                Session["idSpeciesAnimal"] = animal.idSpeciesAnimal;
                Session["idGenderAnimal"] = animal.idGenderAnimal;
                animal.idCustomer = idCustomer;
                string arquivo = Path.GetFileName(file.FileName);
                string file2 = "/Files/" + Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("/Files"), arquivo);
                file.SaveAs(_path);
                animal.imageAnimal = file2;

                queryAnimal.insertAnimal(animal);
                string idAnimal = animal.idAnimal;
                ViewBag.msg = "Cadastro efetuado com sucesso";
                return RedirectToAction("ListAnimalCustomer", "Animal");
            }
            else
            {
                ViewBag.msg = "Erro ao realizar cadastro do animal";
                return View(animal);
            }
        }

        public AnimalModel GetNameCustomerById()
        {
            string idCustomer = (string)Session["idCustomer"];

            // Consulta SQL com INNER JOIN
            string query =
                @"SELECT tbCustomer.nameCustomer
                     FROM tbCustomer
                     JOIN tbPet ON tbPet.idCustomer = tbCustomer.idCustomer
                     WHERE tbCustomer.idCustomer = @IdCustomer";

            // Crie uma conexão com o banco de dados e execute a consulta
            using (
                MySqlConnection connection = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdCustomer", idCustomer);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nameCustomer = reader["nameCustomer"].ToString();
                            ViewBag.nameCustomer = nameCustomer;

                            // Crie uma instância de Animal e defina o nome do cliente
                            AnimalModel animal = new AnimalModel();
                            animal.idCustomer = nameCustomer;

                            return animal;
                        }
                    }
                }
                // Caso o cliente não seja encontrado, retorne uma mensagem de erro ou faça algo apropriado
                ViewBag.customer = "Cliente não encontrado";
            }
            return null;
        }


        public List<PlanAnimalViewModel> GetNamePlanById()
        {
            string idCustomer = (string)Session["idCustomer"];
            List<AnimalModel> listaAnimais = queryAnimal.getAnimalByIdCustomer(idCustomer);
            List<string> listaIdsAnimais = (from a in listaAnimais
                                            select a.idAnimal).ToList();
            List<string> listaIdsAnimaisSessao = new List<string>(); // Lista para armazenar os IDs na sessão

            foreach (string idAnimal in listaIdsAnimais)
            {
                listaIdsAnimaisSessao.Add(idAnimal); // Adiciona o ID à lista da sessão
            }

            Session["listaIdsAnimais"] = listaIdsAnimaisSessao;

            // Consulta SQL com INNER JOIN
            string query =
                @"SELECT tbPet.idAnimal, tbPlan.namePlan FROM tbPet INNER JOIN tbPlan ON tbPet.idPlan = tbPlan.idPlan WHERE tbPet.idAnimal = @idPet";

            // Crie uma conexão com o banco de dados e execute a consulta
            using (MySqlConnection connection = new MySqlConnection("Server =localhost;DataBase=teste2;User=root;pwd=12345678"))
            {
                connection.Open();
                List<PlanAnimalViewModel> resultadoAnimais = new List<PlanAnimalViewModel>(); // Lista para armazenar os resultados
                Dictionary<string, string> animalPlanMap = new Dictionary<string, string>(); // Dicionário para mapear ID do animal ao valor da raça

                foreach (string idPet in (List<string>)Session["listaIdsAnimais"])
                {
                    // Crie uma nova instância de MySqlCommand para cada iteração do loop
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPet", idPet);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string namePlan = reader["namePlan"].ToString();

                                // Crie uma instância de Animal e defina o valor da propriedade idBreedAnimal
                                PlanAnimalViewModel pet = new PlanAnimalViewModel();
                                pet.idAnimal = reader["idAnimal"].ToString();
                                pet.idPlan = namePlan;

                                resultadoAnimais.Add(pet);

                                // Mapeie o ID do animal ao valor da raça
                                animalPlanMap[pet.idAnimal] = namePlan;
                            }
                        }
                    }
                    ViewBag.PlanMap = animalPlanMap;
                }


                return resultadoAnimais;
            }
        }

        public PlanAnimalViewModel GetNamePlanById2()
        {
            string idPlan = (string)Session["idPlan"];

            // Consulta SQL com INNER JOIN
            string query =
                @"SELECT tbPlan.namePlan
                      FROM PlanAnimal
                      JOIN tbPlan  ON PlanAnimal.idPlan = tbPlan.idPlan
                      WHERE tbPet.idAnimal = @IdPlan";

            // Crie uma conexão com o banco de dados e execute a consulta
            using (
                MySqlConnection connection = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPlan", idPlan);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string namePlan = reader["namePlan"].ToString();
                            ViewBag.namePlan = namePlan;

                            // Crie uma instância de Animal e defina o nome do cliente
                            PlanAnimalViewModel planAnimal = new PlanAnimalViewModel();
                            planAnimal.idPlan = namePlan;

                            return planAnimal;
                        }
                    }
                }
                // Caso o cliente não seja encontrado, retorne uma mensagem de erro ou faça algo apropriado
                ViewBag.namePlan = "plano não encontrado";
            }
            return null;
        }


        public List<AnimalModel> GetPetBreedNameById()
        {
            string idCustomer = (string)Session["idCustomer"];
            List<AnimalModel> listaAnimais = queryAnimal.getAnimalByIdCustomer(idCustomer);
            List<string> listaIdsAnimais = (from a in listaAnimais
                                            select a.idAnimal).ToList();
            List<string> listaIdsAnimaisSessao = new List<string>(); // Lista para armazenar os IDs na sessão

            foreach (string idAnimal in listaIdsAnimais)
            {
                listaIdsAnimaisSessao.Add(idAnimal); // Adiciona o ID à lista da sessão
            }

            Session["listaIdsAnimais"] = listaIdsAnimaisSessao;

            // Consulta SQL com INNER JOIN
            string query =
                @"SELECT tbPet.idAnimal, tbBreedAnimal.nameBreed
               FROM tbPet
               INNER JOIN tbBreedAnimal ON tbPet.idBreedAnimal = tbBreedAnimal.idBreedAnimal
               WHERE tbPet.idAnimal = @IdPet";

            // Crie uma conexão com o banco de dados e execute a consulta
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;DataBase=teste2;User=root;pwd=12345678"))
            {
                connection.Open();
                List<AnimalModel> resultadoAnimais = new List<AnimalModel>(); // Lista para armazenar os resultados
                Dictionary<string, string> animalBreedMap = new Dictionary<string, string>(); // Dicionário para mapear ID do animal ao valor da raça

                foreach (string idPet in (List<string>)Session["listaIdsAnimais"])
                {
                    // Crie uma nova instância de MySqlCommand para cada iteração do loop
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPet", idPet);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string nameBreed = reader["nameBreed"].ToString();

                                // Crie uma instância de Animal e defina o valor da propriedade idBreedAnimal
                                AnimalModel pet = new AnimalModel();
                                pet.idAnimal = reader["idAnimal"].ToString();
                                pet.idBreedAnimal = nameBreed;

                                resultadoAnimais.Add(pet);

                                // Mapeie o ID do animal ao valor da raça
                                animalBreedMap[pet.idAnimal] = nameBreed;
                            }
                        }
                    }
                    ViewBag.AnimalBreedMap = animalBreedMap;
                }


                return resultadoAnimais;
            }
        }


        public List<AnimalModel> GetPetGenderNameById()
        {
            string idCustomer = (string)Session["idCustomer"];
            List<AnimalModel> listaAnimais = queryAnimal.getAnimalByIdCustomer(idCustomer);
            List<string> listaIdsAnimais = (from a in listaAnimais
                                            select a.idAnimal).ToList();
            List<string> listaIdsAnimaisSessao = new List<string>(); // Lista para armazenar os IDs na sessão

            foreach (string idAnimal in listaIdsAnimais)
            {
                listaIdsAnimaisSessao.Add(idAnimal); // Adiciona o ID à lista da sessão
            }

            Session["listaIdsAnimais"] = listaIdsAnimaisSessao;

            // Consulta SQL com INNER JOIN
            string query =
                @"SELECT tbPet.idAnimal, GenderAnimal.gender
                FROM tbPet
                INNER JOIN GenderAnimal ON tbPet.idGenderAnimal = GenderAnimal.idGenderAnimal
                WHERE tbPet.idAnimal = @IdPet";

            // Crie uma conexão com o banco de dados e execute a consulta
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;DataBase=teste2;User=root;pwd=12345678"))
            {
                connection.Open();
                List<AnimalModel> resultadoAnimais = new List<AnimalModel>(); // Lista para armazenar os resultados
                Dictionary<string, string> animalGenderMap = new Dictionary<string, string>(); // Dicionário para mapear ID do animal ao valor da raça

                foreach (string idPet in (List<string>)Session["listaIdsAnimais"])
                {
                    // Crie uma nova instância de MySqlCommand para cada iteração do loop
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPet", idPet);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string gender = reader["gender"].ToString();

                                // Crie uma instância de Animal e defina o valor da propriedade idBreedAnimal
                                AnimalModel pet = new AnimalModel();
                                pet.idAnimal = reader["idAnimal"].ToString();
                                pet.idGenderAnimal = gender;

                                resultadoAnimais.Add(pet);

                                // Mapeie o ID do animal ao valor da raça
                                animalGenderMap[pet.idAnimal] = gender;
                            }
                        }
                    }
                    ViewBag.AnimalGenderMap = animalGenderMap;
                }


                return resultadoAnimais;
            }
        }


        public List<AnimalModel> GetPetSpeciesNameById()
        {
            string idCustomer = (string)Session["idCustomer"];
            List<AnimalModel> listaAnimais = queryAnimal.getAnimalByIdCustomer(idCustomer);
            List<string> listaIdsAnimais = (from a in listaAnimais
                                            select a.idAnimal).ToList();
            List<string> listaIdsAnimaisSessao = new List<string>(); // Lista para armazenar os IDs na sessão

            foreach (string idAnimal in listaIdsAnimais)
            {
                listaIdsAnimaisSessao.Add(idAnimal); // Adiciona o ID à lista da sessão
            }

            Session["listaIdsAnimais"] = listaIdsAnimaisSessao;

            // Consulta SQL com INNER JOIN
            string query =
                @"SELECT  tbPet.idAnimal, tbSpeciesAnimal.nameSpeciesAnimal
                FROM tbPet
                INNER JOIN tbSpeciesAnimal ON tbPet.idSpeciesAnimal = tbSpeciesAnimal.idSpeciesAnimal
                WHERE tbPet.idAnimal = @IdPet";

            // Crie uma conexão com o banco de dados e execute a consulta
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;DataBase=teste2;User=root;pwd=12345678"))
            {
                connection.Open();
                List<AnimalModel> resultadoAnimais = new List<AnimalModel>(); // Lista para armazenar os resultados
                Dictionary<string, string> animalSpeciesMap = new Dictionary<string, string>(); // Dicionário para mapear ID do animal ao valor da raça

                foreach (string idPet in (List<string>)Session["listaIdsAnimais"])
                {
                    // Crie uma nova instância de MySqlCommand para cada iteração do loop
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPet", idPet);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string nameSpeciesAnimal = reader["nameSpeciesAnimal"].ToString();

                                // Crie uma instância de Animal e defina o valor da propriedade idBreedAnimal
                                AnimalModel pet = new AnimalModel();
                                pet.idAnimal = reader["idAnimal"].ToString();
                                pet.idSpeciesAnimal = nameSpeciesAnimal;

                                resultadoAnimais.Add(pet);

                                // Mapeie o ID do animal ao valor da raça
                                animalSpeciesMap[pet.idAnimal] = nameSpeciesAnimal;
                            }
                        }
                    }
                    ViewBag.AnimalSpeciesMap = animalSpeciesMap;
                }


                return resultadoAnimais;
            }
        }


        public List<AnimalModel> GetPetBreedNameById2()
        {
            string idCustomer = (string)Session["idCustomer"];
            List<AnimalModel> listaAnimais = queryAnimal.getAnimalByIdCustomer(idCustomer);

            string query =
                @"SELECT p.idAnimal, b.nameBreed
               FROM tbPet p
               INNER JOIN tbBreedAnimal b ON p.idBreedAnimal = b.idBreedAnimal
               WHERE p.idCustomer = @IdCustomer";

            using (MySqlConnection connection = new MySqlConnection("Server=localhost;DataBase=teste2;User=root;pwd=12345678"))
            {
                connection.Open();
                List<AnimalModel> resultadoAnimais = new List<AnimalModel>();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdCustomer", idCustomer);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string idAnimal = reader["idAnimal"].ToString();
                            string nameBreed = reader["nameBreed"].ToString();

                            AnimalModel pet = new AnimalModel();
                            pet.idAnimal = idAnimal;
                            pet.idBreedAnimal = nameBreed;

                            resultadoAnimais.Add(pet);
                        }
                    }
                }

                // Armazenar os valores de nameBreed em uma ViewBag
                List<string> nameBreeds = resultadoAnimais.Select(a => a.idBreedAnimal).ToList();
                ViewBag.NameBreeds = nameBreeds;

                return listaAnimais;
            }
        }


       DatabaseConnection con = new DatabaseConnection();
        public List<AnimalModel> getAnimalByIdCustomer2()
        {
            List<AnimalModel> listaAnimais = new List<AnimalModel>();
            MySqlCommand cmd = new MySqlCommand(
                "SELECT tbPet.*, tbSpeciesAnimal.nameSpeciesAnimal, tbBreedAnimal.nameBreed, GenderAnimal.gender " +
                 "FROM tbPet " +
                 "INNER JOIN tbSpeciesAnimal ON tbPet.idSpeciesAnimal = tbSpeciesAnimal.idSpeciesAnimal " +
                 "INNER JOIN tbBreedAnimal ON tbPet.idBreedAnimal = tbBreedAnimal.idBreedAnimal " +
                 "INNER JOIN GenderAnimal ON tbPet.idGenderAnimal = GenderAnimal.idGenderAnimal",
                con.ConectarBD()
            );


            // adapter para lista
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

            // tabela virtual
            DataTable db = new DataTable();
            adapter.Fill(db);

            con.DesconectarBD();

            // enquanto existir linhas (registros) no banco
            // o foreach irá adicionar os valores vindos do banco nos atributos da classe Animal

            foreach (DataRow dr in db.Rows)
            {
                listaAnimais.Add(
                    new AnimalModel
                    {
                        idAnimal = Convert.ToString(dr["idAnimal"]),
                        idCustomer = Convert.ToString(dr["idCustomer"]),
                        nameAnimal = Convert.ToString(dr["nameAnimal"]),
                        idBreedAnimal = Convert.ToString(dr["idBreedAnimal"]),
                        idSpeciesAnimal = Convert.ToString(dr["idSpeciesAnimal"]),
                        idGenderAnimal = Convert.ToString(dr["idGenderAnimal"]),
                        ageAnimal = Convert.ToString(dr["ageAnimal"]),
                        imageAnimal = Convert.ToString(dr["imageAnimal"])

                    }
                );
                ViewBag.NameSpeciesAnimal = Convert.ToString(dr["idSpeciesAnimal"]);

                // Adicionar idBreedAnimal em uma ViewBag
                ViewBag.NameBreed = Convert.ToString(dr["idBreedAnimal"]);
                ViewBag.Gender = Convert.ToString(dr["idGenderAnimal"]);
            }

            return listaAnimais;
        }




        public ActionResult ListAnimalCustomer(string idAnimal)
        {
            // Verifica se o usuário está autenticado
            //if (User.Identity.IsAuthenticated)
            //{
                //Obtém o ID do usuário logado
                //string userId = GetCurrentUserId();
                string idCustomer = (string)Session["idCustomer"];
                string idPlan = (string)Session["idPlan"];

                string idBreedAnimal = (string)Session["idBreedAnimal"];
                string idSpeciesAnimal = (string)Session["idSpeciesAnimal"];

                // Obtém os animais cadastrados com a chave estrangeira igual ao ID do usuário
                //List<Animal> animais = acAnimal.getAnimalByIdCustomer(idCustomer);

                List<AnimalModel> listaAnimais = queryAnimal.getAnimalByIdCustomer(idCustomer);

                GetPetBreedNameById();
                GetPetSpeciesNameById();
                GetPetGenderNameById();

                ViewBag.customer = idCustomer;
                ViewBag.breed = idBreedAnimal;
                ViewBag.species = idSpeciesAnimal;

                AnimalModel animal = GetNameCustomerById();
                GetNamePlanById();


                // Retorna a lista de animais para a view
                return View(listaAnimais);
            //}

            // Redireciona para a página de login se o usuário não estiver autenticado
            //return RedirectToAction("Login", "Login");
        }

        public ActionResult DeleteAnimal(int id)
        {
            queryAnimal.deleteAnimal(id);
            return RedirectToAction("ListAnimalCustomer");
        }

        public ActionResult UpdateAnimal(string id)
        {
            return View(queryAnimal.getAnimal().Find(model => model.idAnimal == id));
        }

        [HttpPost]
        public ActionResult UpdateAnimal(int id, AnimalModel animal, HttpPostedFileBase file)
        {
            string arquivo = Path.GetFileName(file.FileName);
            string file2 = "/Files/" + Path.GetFileName(file.FileName);
            string _path = Path.Combine(Server.MapPath("/Files"), arquivo);
            loadBreedAnimal();
            loadSpeciesAnimal();
            loadGenderPet();
            animal.idBreedAnimal = Request["speciesAnimal"];
            animal.idSpeciesAnimal = Request["breedAnimal"];
            animal.idGenderAnimal = Request["genderPet"];
            file.SaveAs(_path);
            animal.imageAnimal = file2;
            string idCustomer = (string)Session["idCustomer"];
            loadCustomer(idCustomer);
            animal.idCustomer = Request["customer"];
            animal.idCustomer = idCustomer;

            animal.idAnimal = id.ToString();
            queryAnimal.atualizarAnimal(animal);
            return RedirectToAction("ListAnimalCustomer");

        }

        [CustomeAuthorize(UserRole.Admin)]
        public ActionResult ListAnimal()
        {
            return View(getAnimalByIdCustomer2());
        }
    }
}
