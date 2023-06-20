using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static VetCareTCC.Models.UserModel;
using System.Web.Security;
using VetCareTCC.ViewlModels;
using VetCareTCC.Repositories;

namespace VetCareTCC.Controllers
{
    public class LoginController : Controller
    {

        private readonly UserRepository queryUser;

        public LoginController()
        {
            string connectionString = "Server=localhost;DataBase=teste2;User=root;pwd=12345678";
            queryUser = new UserRepository(connectionString);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                //verificar as credenciais do usuÃ¡rio no banco
                var user = queryUser.GetUserByEmailAndPassword(model);

                if (user != null)
                {
                    //autenticar o usuÃ¡rio e redirecionar para a tela inicial
                    FormsAuthentication.SetAuthCookie(user.role.ToString(), false);
                    if (user.role == UserRole.Admin)
                    {
                        return RedirectToAction("ListAnimal", "Animal");
                    }
                    else if (user.role == UserRole.Customer)
                    {
                        Session["idCustomer"] = user.id;
                        Session["Name"] = user.name;
                        return RedirectToAction("CadAnimal", "Animal", new { idCustomer = user.id, Name = user.name });

                    }

                }

            }
            ModelState.AddModelError("", "Credencias invÃ¡lidas");
            return View(model);
        }


        public ActionResult Logout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmLogout()
        {
            if (Request.Form["confirm"] == "true")
            {
                // Executar o logout
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Login");
            }
            else
            {
                // Cancelar o logout
                return RedirectToAction("Index", "Home");
            }
        }



    }
}
