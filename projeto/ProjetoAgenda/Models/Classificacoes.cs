using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace ProjetoAgenda.Models
{
    public class Classificacoes
    {
        [Key]
        public int IdClassificacao { get; set; }

        [DisplayName("Classificação")]
        public string NomeClassificacao { get; set; }
    }
}