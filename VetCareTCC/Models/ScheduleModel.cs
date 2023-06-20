using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace VetCareTCC.Models
{
    public class ScheduleModel
    {
        public string idSchedule { get; set; }

        [Display(Name = "Nome do Cliente")]
        [Required(ErrorMessage = "Informe o nome do cliente")]
        public string idCustomer { get; set; }

        [Display(Name = "Nome do Animal")]
        [Required(ErrorMessage = "Informe o nome do animal")]
        public string idAnimal { get; set; }

        [Display(Name = "Serviço oferecido")]
        [Required(ErrorMessage = "Informe o nome do serviço")]
        public string idService { get; set; }

        [Display(Name = "Data do serviço")]
        [MaxLength(10, ErrorMessage = "O nome do produto deve conter no maximo 255 caracteres")]
        [Required(ErrorMessage = "Informe o serviço")]
        public string dateSchedule { get; set; }

        [Display(Name = "Observações")]
        [MaxLength(255, ErrorMessage = "O nome do produto deve conter no maximo 255 caracteres")]
        [Required(ErrorMessage = "Informe a observação")]
        public string observations { get; set; }

        [Display(Name = "Horário")]
        [Required(ErrorMessage = "Informe o horário")]
        public string timeSchedule { get; set; }

    }
}