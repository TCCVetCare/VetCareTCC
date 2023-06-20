using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VetCareTCC.Models
{
    public class CartModel
    {
        public string codVenda { get; set; }

        public string DtVenda { get; set; }

        public string CustomerID { get; set; }

        public string horaVenda { get; set; }

        public double ValorTotal { get; set; }

        public List<ItemCartModel> ItensPedido = new List<ItemCartModel>();
    }
}