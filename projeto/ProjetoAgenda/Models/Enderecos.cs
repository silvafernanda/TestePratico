using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjetoAgenda.Models
{
    public class Enderecos
    {
        [Key]
        public int IdEndereco { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [StringLength(20, MinimumLength = 3)]
        [DisplayName("Rua / Av / Etc")]
        public string Logradouro { get; set; }

        [StringLength(20)]
        public string Bairro { get; set; }

        [StringLength(10)]
        public string CEP { get; set; }

        [StringLength(20)]
        public string Cidade { get; set; }

        [StringLength(2)]
        public string Estado { get; set; }

        [DisplayName("Contato")]
        public int? IdContato { get; set; }

        public Contatos Contatos { get; set; }
    }
}