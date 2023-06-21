using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace VetCareTCC.Models
{
    public class AnimalModel
    {
        [Display(Name = "ID")]
        public string idAnimal { get; set; }

        public string idCustomer { get; set; }

        public string idPlan { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Informe o nome do animal")]
        [MaxLength(80, ErrorMessage = "O nome deve conter no maximo 255 caracteres")]
        public string nameAnimal { get; set; }


        [Display(Name = "Raça")]
        [Required(ErrorMessage = "Escolha raça do animal")]
        public string idBreedAnimal { get; set; }

        [Display(Name = "Espécie")]
        [Required(ErrorMessage = "Escolha a espécie do animal")]
        public string idSpeciesAnimal { get; set; }

        [Display(Name = "Idade")]
        [Required(ErrorMessage = "Digite a idade do animal")]
        [RegularExpression(@"^[0-9]+${11,11}", ErrorMessage = "Somente números")]
        public string ageAnimal { get; set; }

        [Display(Name = "Gênero")]
        [Required(ErrorMessage = "Digite o gênero do animal")]
        [MaxLength(11, ErrorMessage = "O gênero do animal deve possuir no máximo 1 caracther")]
        public string idGenderAnimal { get; set; }

        [Display(Name = "Imagem")]
        [Required(ErrorMessage = "Escolha uma imagem para o animal")]
        public string imageAnimal { get; set; }

    }
}