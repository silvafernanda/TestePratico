using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjetoAgenda.Models
{
    public class Contatos
    {
        [Key]
        public int IdContato { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [StringLength(20, MinimumLength = 3)]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [StringLength(20)]
        public string Empresa { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [DisplayName("Telefone(s)")]
        public int IdTelefone { get; set; }

        [DisplayName("Telefone(s)")]
        public List<Telefones> Telefones { get; set; }

        [DisplayName("Endereço")]
        public int? IdEndereco { get; set; }

        [DisplayName("Endereço")]
        public Enderecos Enderecos { get; set; }

        [DisplayName("E-mail(s)")]
        public int? IdEmail { get; set; }

        [DisplayName("E-mail(s)")]
        public List<Emails> Emails { get; set; }
    }
}