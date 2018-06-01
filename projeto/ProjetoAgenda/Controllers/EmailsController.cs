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
    public class EmailsController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Emails
        public ActionResult Index(int? id)
        {
            List<Emails> email = db.Email.Include(e => e.Classificacoes).Where(x => x.IdContato == id).ToList();

            this.Session.Add("IdContatoEditEmail", id);

            return View(email);
        }

        // GET: Emails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emails emails = db.Email.Find(id);
            if (emails == null)
            {
                return HttpNotFound();
            }
            return View(emails);
        }

        // GET: Emails/Create
        public ActionResult Create(int? id)
        {
            Emails emails = new Emails();

            if (id != null)
                emails.IdContato = (int)id;

            ViewBag.IdClassificacaoFK = new List<SelectListItem>
            {
                new SelectListItem { Text = "Casa", Value = "1" },
                new SelectListItem { Text = "Trabalho", Value = "2" },
                new SelectListItem { Text = "Outro", Value = "3" }
            };

            return View(emails);
        }

        // POST: Emails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEmail,Email,IdContato,IdClassificacaoFK")] Emails emails, string ed)
        {
            if (ModelState.IsValid)
            {
                ConsultaInsereClassificacao(emails);

                emails.IdContato = string.IsNullOrEmpty(ed) ? null : (int?)this.Session["IdContatoEditEmail"];
                db.Email.Add(emails);
                db.SaveChanges();

                string pagina = string.IsNullOrEmpty(ed) ? "Create" : "Edit";

                return RedirectToAction(pagina, "Contatos", new { id = emails.IdContato, idEmail = emails.IdEmail });
            }

            ViewBag.IdClassificacaoFK = new SelectList(db.Classificacao, "IdClassificacao", "NomeClassificacao", emails.IdClassificacaoFK);
            return View(emails);
        }

        // GET: Emails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emails emails = db.Email.Find(id);
            if (emails == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdClassificacaoFK = new List<SelectListItem>
            {
                new SelectListItem { Text = "Casa", Value = "1" },
                new SelectListItem { Text = "Trabalho", Value = "2" },
                new SelectListItem { Text = "Outro", Value = "3" }
            };

            return View(emails);
        }

        // POST: Emails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEmail,Email,IdContato,IdClassificacaoFK")] Emails emails)
        {
            if (ModelState.IsValid)
            {
                ConsultaInsereClassificacao(emails);

                db.Entry(emails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Contatos", new { id = emails.IdContato, idEmail = emails.IdEmail });
            }
            ViewBag.IdClassificacaoFK = new SelectList(db.Classificacao, "IdClassificacao", "NomeClassificacao", emails.IdClassificacaoFK);
            return View(emails);
        }

        // GET: Emails/Delete/5
        public ActionResult Delete(int? id)
        {
            Emails emails = db.Email.Find(id);
            db.Email.Remove(emails);
            db.SaveChanges();
            return RedirectToAction("Edit", "Contatos", new { id = emails.IdContato, idEmail = emails.IdEmail });
        }

        private void ConsultaInsereClassificacao(Emails emails)
        {
            Classificacoes classificacaoAux = db.Classificacao.Where(x => x.IdClassificacao == emails.IdClassificacaoFK).FirstOrDefault();
            if (classificacaoAux == null)
            {
                classificacaoAux = new Classificacoes();
                classificacaoAux.IdClassificacao = emails.IdClassificacaoFK;
                classificacaoAux.NomeClassificacao = emails.IdClassificacaoFK == 1 ? "Casa" : emails.IdClassificacaoFK == 2 ? "Trabalho" : "Outro";

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
