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
    public class EnderecosController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Enderecos
        public ActionResult Index(int? id)
        {
            return View(db.Endereco.Where(x => x.IdContato == id).ToList());
        }

        // GET: Enderecos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enderecos enderecos = db.Endereco.Find(id);
            if (enderecos == null)
            {
                return HttpNotFound();
            }
            return View(enderecos);
        }

        // GET: Enderecos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Enderecos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEndereco,Logradouro,Bairro,CEP,Cidade,Estado,IdContato")] Enderecos enderecos)
        {
            if (ModelState.IsValid)
            {
                db.Endereco.Add(enderecos);
                db.SaveChanges();

                return RedirectToAction("Create", "Contatos", new { id = enderecos.IdContato, idEndereco = enderecos.IdEndereco });
            }

            return View(enderecos);
        }

        // GET: Enderecos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enderecos enderecos = db.Endereco.Find(id);
            if (enderecos == null)
            {
                return HttpNotFound();
            }
            return View(enderecos);
        }

        // POST: Enderecos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEndereco,Logradouro,Bairro,CEP,Cidade,Estado,IdContato")] Enderecos enderecos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enderecos).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Edit", "Contatos", new { id = enderecos.IdContato, idEndereco = enderecos.IdEndereco });
            }
            return View(enderecos);
        }

        // GET: Enderecos/Delete/5
        public ActionResult Delete(int? id)
        {
            Enderecos enderecos = db.Endereco.Find(id);
            db.Endereco.Remove(enderecos);
            db.SaveChanges();

            return RedirectToAction("Edit", "Contatos", new { id = enderecos.IdContato, idEndereco = enderecos.IdEndereco });
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
