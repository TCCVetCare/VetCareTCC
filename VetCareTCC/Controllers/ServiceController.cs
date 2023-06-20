using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static VetCareTCC.Models.UserModel;
using System.Web.Services.Description;
using VetCareTCC.Models;
using VetCareTCC.Repositories;

namespace VetCareTCC.Controllers
{
    public class ServiceController : Controller
    {
        // GET: Service
        ServiceRepository queryService  = new ServiceRepository();

        public ActionResult Admin()
        {
            return View();
        }

        [CustomeAuthorize(UserRole.Admin)]
        public ActionResult CadService()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CadService(ServiceModel service)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.msg = "Erro ao realizar cadastro do serviço";
                return View(service);
            }
            else
            {
                queryService.insertService(service);
                ViewBag.msg = "Cadastro efetuado com sucesso";
                return RedirectToAction("ListService", "Service");
            }
        }

        [CustomeAuthorize(UserRole.Admin)]
        public ActionResult ListService()
        {
            return View(queryService.getService());
        }

        [CustomeAuthorize(UserRole.Admin)]
        public ActionResult DeleteService(int id)
        {
            queryService.deleteService(id);
            return RedirectToAction("ListService");
        }

        [CustomeAuthorize(UserRole.Admin)]
        public ActionResult UpdateService(string id)
        {
            return View(queryService.getService().Find(model => model.idService == id));
        }

        [HttpPost]
        public ActionResult UpdateService(int id, ServiceModel service)
        {
            service.idService = id.ToString();
            queryService.updateService(service);
            return RedirectToAction("ListService");

        }
    }
}
