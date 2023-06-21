using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Web;
using System.Web.Mvc;
using static VetCareTCC.Models.UserModel;
using VetCareTCC.Models;
using VetCareTCC.ViewlModels;
using VetCareTCC.Repositories;

namespace VetCareTCC.Controllers
{
    public class PlanController : Controller
    {
        // GET: Plan

        PlanRepository queryRepository = new PlanRepository();
        public void loadFormOfPayment()
        {
            List<SelectListItem> payments = new List<SelectListItem>();

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbPayment", con);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    payments.Add(
                        new SelectListItem
                        {
                            Text = rdr[1].ToString(), //nome
                            Value = rdr[0].ToString() //id do autor
                        }
                    );
                }
                con.Close(); //fechando conexÃ£o
            }

            ViewBag.payments = new SelectList(payments, "Value", "Text");
        }

        public void loadAnimal(string idCustomer)
        {
            List<SelectListItem> animal = new List<SelectListItem>();

            using (
                MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM tbPet WHERE idCustomer = @idCustomer",
                    con
                );
                cmd.Parameters.AddWithValue("@idCustomer", idCustomer);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    animal.Add(
                        new SelectListItem { Text = rdr[1].ToString(), Value = rdr[0].ToString() }
                    );
                }
                con.Close();
            }

            ViewBag.animal = new SelectList(animal, "Value", "Text");
        }



        public ActionResult Admin()
        {
            return View();
        }

        //[CustomeAuthorize(UserRole.Admin)]
        public ActionResult CadPlan()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CadPlan(PlanModel plan, HttpPostedFileBase file)
        {
            string arquivo = Path.GetFileName(file.FileName);
            string file2 = "/Images/" + Path.GetFileName(file.FileName);
            string _path = Path.Combine(Server.MapPath("/Images"), arquivo);
            file.SaveAs(_path);
            plan.imagePlan = file2;
            if (!ModelState.IsValid)
            {
                ViewBag.msg = "Erro ao realizar cadastro do plano";
                return View(plan);
            }
            else
            {
                queryRepository.insertPlan(plan);
                ViewBag.msg = "Cadastro efetuado com sucesso";
                return RedirectToAction("ListPlan", "Plan");
            }
        }

        public ActionResult ListPlan()
        {
            return View(queryRepository.getPlan());
        }



        public ActionResult ListPlanCustomer()
        {
            return View(queryRepository.getPlan());
        }


        //[CustomeAuthorize(UserRole.Admin)]
        public ActionResult DeletePlan(int id)
        {
            queryRepository.deletePlan(id);
            return RedirectToAction("ListPlan");
        }

        //[CustomeAuthorize(UserRole.Admin)]
        public ActionResult UpdatePlan(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            return View(queryRepository.getPlan().Find(model => model.idPlan == id));
        }

        [HttpPost]
        public ActionResult UpdatePlan(int id, PlanModel plan, HttpPostedFileBase file)
        {
            string arquivo = Path.GetFileName(file.FileName);
            string file2 = "/Images/" + Path.GetFileName(file.FileName);
            string _path = Path.Combine(Server.MapPath("/Images"), arquivo);
            file.SaveAs(_path);
            plan.imagePlan = file2;
            if (ModelState.IsValid)
            {
                plan.idPlan = id.ToString();
                queryRepository.updatePlan(plan);
                return RedirectToAction("ListPlan", "Plan");
            }
            return View(plan);
        }

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult CadPlanAnimal(string id)
        {
            string idCustomer = (string)Session["idCustomer"];
            loadAnimal(idCustomer);
            Session["idPlan"] = id;
            loadFormOfPayment();
            return View();
        }

        [HttpPost]
        public ActionResult CadPlanAnimal(PlanAnimalViewModel planAnimal)
        {
            string idPlan = (string)Session["idPlan"];
            planAnimal.idPlan = idPlan;
            planAnimal.idAnimal = Request["animal"];

            string idAnimal = planAnimal.idAnimal;

            queryRepository.UpdateIdPlan(idPlan, idAnimal);
            planAnimal.idFormOfPayment = Request["payments"];
            string idCustomer = (string)Session["idCustomer"];
            loadAnimal(idCustomer);
            loadFormOfPayment();


            PlanAnimalViewModel plan = new PlanAnimalViewModel()
            {
                idPlan = idPlan,
                idAnimal = planAnimal.idAnimal,
                idFormOfPayment = planAnimal.idFormOfPayment

            };

            if (ModelState.IsValid)
            {
                queryRepository.insertPlanAnimal(planAnimal);
                ViewBag.msg = "Compra efetuada com sucesso";
            }
            else
            {
                ViewBag.msg = "Erro ao finalizar a compra";
                return View(planAnimal);
            }
            return View();

        }

    }
}
