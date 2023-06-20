using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static VetCareTCC.Models.UserModel;
using System.Web.Mvc;

namespace VetCareTCC.Models
{
    public class CustomeAuthorizeAttribute : AuthorizeAttribute
    {

        // [CustomeAuthorize(UserRole.Admin)]

        private readonly UserRole roleAutorizada;

        public CustomeAuthorizeAttribute(UserRole role)
        {
            roleAutorizada = role;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Verificar se o usuário está autenticado
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            // Verificar se o usuário tem a role autorizada
            var userRole = httpContext.User.Identity.Name;

            // Verificar se o usuário é admin ou tem a role autorizada
            if (userRole == UserRole.Admin.ToString() && userRole == roleAutorizada.ToString())
                return true;

            return false;
        }


    }
}