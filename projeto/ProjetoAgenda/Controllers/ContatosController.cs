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
    public class ContatosController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Contatos
        public ActionResult Index(string pesquisa)
        {
            List<Contatos> contatos = new List<Contatos>();
            contatos = db.Contato.ToList();

            foreach (var item in contatos)
            {
                List<Telefones> telefones = db.Telefone.Where(x => x.IdContato == item.IdContato).ToList();
                item.Telefones = new List<Telefones>();
                item.Telefones.AddRange(telefones);

                List<Emails> emails = db.Email.Where(x => x.IdContato == item.IdContato).ToList();
                item.Emails = new List<Emails>();
                item.Emails.AddRange(emails);

                Enderecos endereco = db.Endereco.Where(x => x.IdContato == item.IdContato).FirstOrDefault();
                item.Enderecos = new Enderecos();
                item.Enderecos = endereco;
            }

            if (!string.IsNullOrEmpty(pesquisa))
            {
                Contatos modelContato = new Contatos();
                List<Contatos> resultPesquisa = new List<Contatos>();
                List<Telefones> resultTelefones = new List<Telefones>();
                List<Emails> resultEmails = new List<Emails>();

                foreach (var item in contatos)
                {
                    bool resultNome = item.Nome.Contains(pesquisa);
                    resultTelefones = item.Telefones.Where(x => x.NumeroTelefone.Contains(pesquisa)).ToList();
                    resultEmails = item.Emails.Where(x => x.Email.Contains(pesquisa)).ToList();

                    if (resultNome || resultTelefones.Count > 0 || resultEmails.Count > 0)
                    {
                        modelContato.Telefones = new List<Telefones>();
                        modelContato.Emails = new List<Emails>();
                        modelContato.Enderecos = new Enderecos();

                        modelContato.Empresa = item.Empresa;
                        modelContato.IdContato = item.IdContato;
                        modelContato.Nome = item.Nome;
                        modelContato.Enderecos.Logradouro = item.Enderecos == null ? string.Empty : item.Enderecos.Logradouro;
                        modelContato.Telefones.AddRange(resultTelefones.Count == 0 ? item.Telefones : resultTelefones);
                        modelContato.Emails.AddRange(resultEmails.Count == 0 ? item.Emails : resultEmails);
                        resultPesquisa.Add(modelContato);
                    }
                }

                contatos = new List<Contatos>();
                contatos = resultPesquisa;
            }

            return View(contatos.OrderBy(x => x.Nome));
        }

        // GET: Contatos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contatos contatos = db.Contato.Find(id);
            if (contatos == null)
            {
                return HttpNotFound();
            }

            List<Telefones> telefones = db.Telefone.Where(x => x.IdContato == id).Include(t => t.Classificacoes).ToList();
            List<Classificacoes> classificacoes = db.Classificacao.ToList();
            contatos.Telefones = new List<Telefones>();
            contatos.Telefones.AddRange(telefones);

            foreach (var item in contatos.Telefones)
            {
                item.NumeroTelefone = item.NumeroTelefone + "  -  " + item.Classificacoes.NomeClassificacao;
            }

            List<Emails> emails = db.Email.Where(x => x.IdContato == id).Include(t => t.Classificacoes).ToList();
            contatos.Emails = new List<Emails>();
            contatos.Emails.AddRange(emails);

            foreach (var itemEmail in contatos.Emails)
            {
                itemEmail.Email = itemEmail.Email + "  -  " + itemEmail.Classificacoes.NomeClassificacao;
            }

            Enderecos endereco = db.Endereco.Where(x => x.IdContato == id).FirstOrDefault();
            contatos.Enderecos = new Enderecos();
            contatos.Enderecos.Logradouro = "Logradouro: " + endereco.Logradouro + "  -  Bairro: " + endereco.Bairro + "  -  CEP: " + endereco.CEP + "  -  Cidade: " + endereco.Cidade + "  -  Estado: " + endereco.Estado;

            return View(contatos);
        }

        // GET: Contatos/Create
        public ActionResult Create(int? idTelefone, int? idEmail, int? idEndereco)
        {
            List<Telefones> listTelefonesSession = new List<Telefones>();
            List<Emails> listEmailsSession = new List<Emails>();
            string enderecoLogradouro = string.Empty;

            if (idTelefone != null)
            {
                listTelefonesSession = (List<Telefones>)this.Session["ListTelefones"] == null ? new List<Telefones>() : (List<Telefones>)this.Session["ListTelefones"];

                Telefones telefones = db.Telefone.Find(idTelefone);

                listTelefonesSession.Add(telefones);

                this.Session.Add("ListTelefones", listTelefonesSession);
            }

            if (idEmail != null)
            {
                listEmailsSession = (List<Emails>)this.Session["ListEmails"] == null ? new List<Emails>() : (List<Emails>)this.Session["ListEmails"];

                Emails emails = db.Email.Find(idEmail);

                listEmailsSession.Add(emails);

                this.Session.Add("ListEmails", listEmailsSession);
            }

            idEndereco = idEndereco == null ? (int?)this.Session["Endereco"] : idEndereco;

            if (idEndereco != null)
            {
                this.Session.Add("Endereco", idEndereco);

                Enderecos endereco = new Enderecos();
                endereco = db.Endereco.Where(x => x.IdEndereco == idEndereco).FirstOrDefault();
                enderecoLogradouro = endereco.Logradouro;
            }

            listTelefonesSession = (List<Telefones>)this.Session["ListTelefones"] == null ? listTelefonesSession : (List<Telefones>)this.Session["ListTelefones"];
            listEmailsSession = (List<Emails>)this.Session["ListEmails"] == null ? listEmailsSession : (List<Emails>)this.Session["ListEmails"];

            ViewBag.IdTelefone = new SelectList(listTelefonesSession, "IdTelefone", "NumeroTelefone");
            ViewBag.IdEmail = new SelectList(listEmailsSession, "IdEmail", "Email");
            ViewBag.Logradouro = enderecoLogradouro;

            return View();
        }

        // POST: Contatos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdContato,Nome,Empresa,IdTelefone,IdEndereco,IdEmail")] Contatos contatos)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Contato.Add(contatos);
                    db.SaveChanges();

                    List<Telefones> listTelefonesSession = new List<Telefones>();
                    listTelefonesSession = (List<Telefones>)this.Session["ListTelefones"] == null ? new List<Telefones>() : (List<Telefones>)this.Session["ListTelefones"];

                    foreach (var itemTelefone in listTelefonesSession)
                    {
                        itemTelefone.IdContato = contatos.IdContato;
                        db.Entry(itemTelefone).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    List<Emails> listEmailsSession = new List<Emails>();
                    listEmailsSession = (List<Emails>)this.Session["ListEmails"] == null ? new List<Emails>() : (List<Emails>)this.Session["ListEmails"];

                    foreach (var itemEmails in listEmailsSession)
                    {
                        itemEmails.IdContato = contatos.IdContato;
                        db.Entry(itemEmails).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    if (this.Session["Endereco"] != null)
                    {
                        int idEnderecoAux = Convert.ToInt32(this.Session["Endereco"]);
                        Enderecos endereco = new Enderecos();
                        endereco = db.Endereco.Where(x => x.IdEndereco == idEnderecoAux).FirstOrDefault();
                        endereco.IdContato = contatos.IdContato;
                        db.Entry(endereco).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    Session.RemoveAll();

                    return RedirectToAction("Index");
                }

                return View(contatos);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

        // GET: Contatos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contatos contatos = db.Contato.Find(id);
            if (contatos == null)
            {
                return HttpNotFound();
            }

            List<Telefones> listTelefones = new List<Telefones>();
            listTelefones = db.Telefone.Where(x => x.IdContato == id).ToList();
            ViewBag.IdTelefone = new SelectList(listTelefones, "IdTelefone", "NumeroTelefone");

            List<Emails> listEmails = new List<Emails>();
            listEmails = db.Email.Where(x => x.IdContato == id).ToList();
            ViewBag.IdEmail = new SelectList(listEmails, "IdEmail", "Email");

            Enderecos endereco = new Enderecos();
            endereco = db.Endereco.Where(x => x.IdContato == id).FirstOrDefault();
            ViewBag.Logradouro = endereco.Logradouro;

            return View(contatos);
        }

        // POST: Contatos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdContato,Nome,Empresa,IdTelefone,IdEndereco,IdEmail")] Contatos contatos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contatos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contatos);
        }

        // GET: Contatos/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                Contatos contatos = db.Contato.Find(id);
                if (contatos != null)
                {
                    db.Contato.Remove(contatos);
                    db.SaveChanges();
                }

                Emails emails = db.Email.Find(id);
                if (emails != null)
                {
                    db.Email.Remove(emails);
                    db.SaveChanges();
                }

                Enderecos enderecos = db.Endereco.Find(id);
                if (enderecos != null)
                {
                    db.Endereco.Remove(enderecos);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                return HttpNotFound();
            }

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
