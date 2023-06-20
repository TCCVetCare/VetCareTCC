using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VetCareTCC.Models;
using VetCareTCC.Repositories;
using static VetCareTCC.Repositories.SupplierRepository;

namespace VetCareTCC.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }


       CustomerRepositoy queryCustomer = new CustomerRepositoy();

     
        [AllowAnonymous]
        public ActionResult CadCustomer()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CadCustomer(CustomerModel customer, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                if (!ModelState.IsValid)
                    return View(customer);
            string arquivo = Path.GetFileName(file.FileName);
            string file2 = "/Files/" + Path.GetFileName(file.FileName);
            string _path = Path.Combine(Server.MapPath("/Files"), arquivo);
            file.SaveAs(_path);
            customer.imageCustomer = file2;

            string cpf = queryCustomer.SelectCPFCustomer(customer.cpfCustomer);
            string email = queryCustomer.SelectEmailCustomer(customer.emailCustomer);
            if (cpf == customer.cpfCustomer && email == customer.emailCustomer)
            {
                ViewBag.Email = "Email ja existente";
                ViewBag.CPF = "CPF já existente";
                return View(customer);
            }
            else if (cpf == customer.cpfCustomer)
            {
                ViewBag.CPF = "CPF já existente";
                return View(customer);
            }
            else if (email == customer.emailCustomer)
            {
                ViewBag.Email = "Email já existente";
                return View(customer);
            }

            CustomerModel newCustomer = new CustomerModel()
            {
                nameCustomer = customer.nameCustomer,
                cpfCustomer = customer.cpfCustomer,
                emailCustomer = customer.emailCustomer,
                passwordCustomer = customer.passwordCustomer,
                phoneCustomer = customer.phoneCustomer,
                imageCustomer = customer.imageCustomer,
                zipCode = customer.zipCode,
                streetName = customer.streetName,
                streetNumber = customer.streetNumber,
                addressComplement = customer.addressComplement,
                neighborhood = customer.neighborhood,
                city = customer.city,
                state = customer.state

            };
            //acCustomer.insertCustomer(newCustomer);
            queryCustomer.InsertCustomerWithAddress(newCustomer);
            queryCustomer.InsertAddress(newCustomer);
            return RedirectToAction("Login", "Login");
        }

        //[CustomeAuthorize(UserRole.Admin)]
        public ActionResult ListCustomer()
        {
            return View(queryCustomer.GetCustomer());
        }


        public ActionResult ListCustomerWithAddress()
        {

            List<CustomerModel> customers = queryCustomer.GetCustomersWithAddress();
            return View(customers);
        }

        public ActionResult ListCustomerWithAddressUpdate()
        {
            string name = (string)Session["name"];
            ViewBag.name = name;
            string idCustomer = (string)Session["idCustomer"];
            List<CustomerModel> customers = queryCustomer.GetCustomersWithAddress(idCustomer);


            return View(customers);
        }


        public ActionResult DeleteCustomer(int id)
        {
            queryCustomer.DeleteCustomer(id);
            return RedirectToAction("ListCustomerWithAddressUpdate");
        }

        //public ActionResult UpdateCustomer(string id)
        //{
        // return View(acCustomer.GetCustomer().Find(model => model.idCustomer == id));
        //}


        public ActionResult UpdateCustomer(string id, string idAddress)
        {
            Session["addressId"] = idAddress;
            return View(queryCustomer.GetCustomersWithAddress().Find(model => model.idCustomer == id));
        }


        [HttpPost]
        public ActionResult UpdateCustomer(int id, CustomerModel customer, HttpPostedFileBase file)
        {
            string arquivo = Path.GetFileName(file.FileName);
            string file2 = "/Files/" + Path.GetFileName(file.FileName);
            string _path = Path.Combine(Server.MapPath("/Files"), arquivo);
            file.SaveAs(_path);
            customer.imageCustomer = file2;
            string idAddress = (string)Session["addressId"];
            customer.idCustomer = id.ToString();
            queryCustomer.UpdateCustomer(customer);

            CustomerModel address = new CustomerModel();

            //address.zipCode = customer.zipCode;
            //address.streetName = customer.streetName;
            //address.streetNumber = customer.streetNumber;
            //address.city = customer.city;
            //address.state = customer.state;
            //address.addressComplement = customer.addressComplement;
            //address.neighborhood = customer.neighborhood;
            //address.idAddress = customer.idAddress;
            customer.idAddress = idAddress;
            queryCustomer.UpdateAddress(customer);
            return RedirectToAction("ListCustomerWithAddressUpdate");

        }

        public ActionResult cartaoCredito()
        {
            return View();
        }

        public ActionResult boleto()
        {
            return View();
        }
    }
}
