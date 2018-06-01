using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjetoAgenda.Models
{
    public class Telefones
    {
        [Key]
        public int IdTelefone { get; set; }

        [DisplayName("Contato")]
        public int? IdContato { get; set; }
        
        public Contatos Contatos { get; set; }

        [Required(ErrorMessage = "Campo obrigatório. Somente Números")]
        [StringLength(20, MinimumLength = 8)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Informar Apenas Números")]
        [DisplayName("Número")]
        public string NumeroTelefone { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [DisplayName("Classificação")]
        public int IdClassificacaoFK { get; set; }

        [ForeignKey("IdClassificacaoFK")] 
        public virtual Classificacoes Classificacoes { get; set; }
    }
}