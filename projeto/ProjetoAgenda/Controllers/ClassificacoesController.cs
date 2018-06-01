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
    public class ClassificacoesController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Classificacoes
        public ActionResult Index()
        {
            return View(db.Classificacao.ToList());
        }

        // GET: Classificacoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classificacoes classificacoes = db.Classificacao.Find(id);
            if (classificacoes == null)
            {
                return HttpNotFound();
            }
            return View(classificacoes);
        }

        // GET: Classificacoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Classificacoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdClassificacao,NomeClassificacao")] Classificacoes classificacoes)
        {
            if (ModelState.IsValid)
            {
                db.Classificacao.Add(classificacoes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(classificacoes);
        }

        // GET: Classificacoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classificacoes classificacoes = db.Classificacao.Find(id);
            if (classificacoes == null)
            {
                return HttpNotFound();
            }
            return View(classificacoes);
        }

        // POST: Classificacoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdClassificacao,NomeClassificacao")] Classificacoes classificacoes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classificacoes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(classificacoes);
        }

        // GET: Classificacoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classificacoes classificacoes = db.Classificacao.Find(id);
            if (classificacoes == null)
            {
                return HttpNotFound();
            }
            return View(classificacoes);
        }

        // POST: Classificacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Classificacoes classificacoes = db.Classificacao.Find(id);
            db.Classificacao.Remove(classificacoes);
            db.SaveChanges();
            return RedirectToAction("Index");
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
