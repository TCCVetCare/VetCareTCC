using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static VetCareTCC.Models.UserModel;
using static VetCareTCC.Repositories.SupplierRepository;
using VetCareTCC.Models;

namespace VetCareTCC.Controllers
{
    public class SupplierController : Controller
    {
        // GET: Supplier
        AcSupplier acSupplier = new AcSupplier();

        public ActionResult Admin()
        {
            return View();
        }

        [CustomeAuthorize(UserRole.Admin)]
        public ActionResult CadSupplier()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CadSupplier(SupplierModel supplier)
        {
            if (!ModelState.IsValid)
                if (!ModelState.IsValid)
                    return View(supplier);
            string cnpj = acSupplier.SelectCnpjSuppler(supplier.cnpjSupplier);
            if (cnpj == supplier.cnpjSupplier)
            {
                ViewBag.msg = "CNPJ já existente";
                return View(supplier);
            }
            else
            {
                acSupplier.insertSupplier(supplier);
                ViewBag.msg = "Cadastro efetuado com sucesso";
                return RedirectToAction("ListSupplier", "Supplier");
            }
        }

        [CustomeAuthorize(UserRole.Admin)]
        public ActionResult ListSupplier()
        {
            return View(acSupplier.getSupplier());
        }

        [CustomeAuthorize(UserRole.Admin)]
        public ActionResult DeleteSupplier(int id)
        {
            acSupplier.deleteSupplier(id);
            return RedirectToAction("ListSupplier");
        }

        [CustomeAuthorize(UserRole.Admin)]
        public ActionResult UpdateSupplier(string id)
        {
            return View(acSupplier.getSupplier().Find(model => model.idSupplier == id));
        }

        [HttpPost]
        public ActionResult UpdateSupplier(int id, SupplierModel supplier)
        {
            supplier.idSupplier = id.ToString();
            acSupplier.updateSupplier(supplier);
            return RedirectToAction("ListSupplier");
         

        }
    }
}
