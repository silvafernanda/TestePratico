using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjetoAgenda.Models
{
    public class Emails
    {
        [Key]
        public int IdEmail { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um email válido...")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [DisplayName("Contato")]
        public int? IdContato { get; set; }

        public Contatos Contatos { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [DisplayName("Classificação")]
        public int IdClassificacaoFK { get; set; }

        [ForeignKey("IdClassificacaoFK")]
        public virtual Classificacoes Classificacoes { get; set; }
    }
}