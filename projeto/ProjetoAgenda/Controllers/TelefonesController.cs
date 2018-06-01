using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjetoAgenda.Models;

namespace ProjetoAgenda.Controllers
{
    public class TelefonesController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Telefones
        public ActionResult Index(int? id)
        {
            List<Telefones> telefones = db.Telefone.Include(t => t.Classificacoes).Where(x => x.IdContato == id).ToList();

            this.Session.Add("IdContatoEditTel", id);

            return View(telefones);
        }

        // GET: Telefones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telefones telefones = db.Telefone.Find(id);
            if (telefones == null)
            {
                return HttpNotFound();
            }
            return View(telefones);
        }

        // GET: Telefones/Create
        public ActionResult Create(int? id)
        {
            Telefones telefones = new Telefones();

            if (id != null)
                telefones.IdContato = (int)id;

            ViewBag.IdClassificacaoFK = new List<SelectListItem>
            {
                new SelectListItem { Text = "Casa", Value = "1" },
                new SelectListItem { Text = "Trabalho", Value = "2" },
                new SelectListItem { Text = "Outro", Value = "3" }
            };
            return View(telefones);
        }

        // POST: Telefones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTelefone,IdContato,NumeroTelefone,IdClassificacaoFK")] Telefones telefones, string ed)
        {
            if (ModelState.IsValid)
            {
                ConsultaInsereClassificacao(telefones);

                telefones.IdContato = string.IsNullOrEmpty(ed) ? null : (int?)this.Session["IdContatoEditTel"];
                db.Telefone.Add(telefones);
                db.SaveChanges();

                string pagina = string.IsNullOrEmpty(ed) ? "Create" : "Edit";

                return RedirectToAction(pagina, "Contatos", new { id = telefones.IdContato, idTelefone = telefones.IdTelefone });
            }

            ViewBag.IdClassificacaoFK = new SelectList(db.Classificacao, "IdClassificacao", "NomeClassificacao", telefones.IdClassificacaoFK);
            return View(telefones);
        }

        // GET: Telefones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telefones telefones = db.Telefone.Find(id);
            if (telefones == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdClassificacaoFK = new List<SelectListItem>
            {
                new SelectListItem { Text = "Casa", Value = "1" },
                new SelectListItem { Text = "Trabalho", Value = "2" },
                new SelectListItem { Text = "Outro", Value = "3" }
            };

            return View(telefones);
        }

        // POST: Telefones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTelefone,IdContato,NumeroTelefone,IdClassificacaoFK")] Telefones telefones)
        {
            if (ModelState.IsValid)
            {
                ConsultaInsereClassificacao(telefones);

                db.Entry(telefones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Contatos", new { id = telefones.IdContato, idTelefone = telefones.IdTelefone });
            }

            ViewBag.IdClassificacaoFK = new SelectList(db.Classificacao, "IdClassificacao", "NomeClassificacao", telefones.IdClassificacaoFK);
            return View(telefones);
        }

        // GET: Telefones/Delete/5
        public ActionResult Delete(int? id)
        {
            Telefones telefones = db.Telefone.Find(id);
            db.Telefone.Remove(telefones);
            db.SaveChanges();
            return RedirectToAction("Edit", "Contatos", new { id = telefones.IdContato, idTelefone = telefones.IdTelefone });
        }

        private void ConsultaInsereClassificacao(Telefones telefones)
        {
            Classificacoes classificacaoAux = db.Classificacao.Where(x => x.IdClassificacao == telefones.IdClassificacaoFK).FirstOrDefault();
            if (classificacaoAux == null)
            {
                classificacaoAux = new Classificacoes();
                classificacaoAux.IdClassificacao = telefones.IdClassificacaoFK;
                classificacaoAux.NomeClassificacao = telefones.IdClassificacaoFK == 1 ? "Casa" : telefones.IdClassificacaoFK == 2 ? "Trabalho" : "Outro";

                db.Classificacao.Add(classificacaoAux);
                db.SaveChanges();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
