using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ProjetoAgenda.Models
{
    public class Contexto : DbContext
    {
        public Contexto() : base ("name = DefaultConnection")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Contatos> Contato { get; set; }

        public DbSet<Classificacoes> Classificacao { get; set; }

        public DbSet<Emails> Email { get; set; }

        public DbSet<Telefones> Telefone { get; set; }

        public DbSet<Enderecos> Endereco { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contatos>().HasOptional(p => p.Emails).WithOptionalDependent();
            modelBuilder.Entity<Contatos>().HasOptional(p => p.Enderecos).WithOptionalDependent();
            modelBuilder.Entity<Contatos>().HasOptional(p => p.Telefones).WithOptionalDependent();

            //modelBuilder.Entity<Telefones>()
            //    .HasRequired(c => c.Classificacoes)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Emails>()
            //    .HasRequired(s => s.Classificacoes)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Telefones>()
            //    .HasRequired(c => c.Contatos)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Emails>()
            //    .HasRequired(s => s.Contatos)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Enderecos>()
            //    .HasRequired(s => s.Contatos)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);
        }
    }
}