using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace VetCareTCC.ViewlModels
{
    public class PlanAnimalViewModel
    {
        public string idCustomer { get; set; }
        public string idPlanAnimal { get; set; }

        public string dateSale { get; set; }

        public string idPlan { get; set; }
        [Display(Name = "Selecione o Animal")]
        //[Required(ErrorMessage = "O campo animal é obrigatório.")]
        public string idAnimal { get; set; }

        [Display(Name = "Selecione a forma de pagamento")]
        //[Required(ErrorMessage = "O campo forma de pagamento é obrigatório")]

        public string idFormOfPayment { get; set; }

        public string CreditCardNumber { get; set; }

        [RegularExpression("^(5[1-5][0-9]{14}|2(22[1-9][0-9]{12}|2[3-9][0-9]{13}|[3-6][0-9]{14}|7[0-1][0-9]{13}|720[0-9]{12}))$", ErrorMessage = "Número de Mastercard inválido")]
        public string CardNumber { get; set; }

        public string LimitDate { get; set; }


        public string nameBoleto { get; set; }

        public string cpfBoleto { get; set; }


        public string KeyPix { get; set; }


    }
}