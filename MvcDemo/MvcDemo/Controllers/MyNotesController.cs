using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcDemo.Models;

namespace MvcDemo.Controllers
{
    public class MyNotesController : Controller
    {
        private MyNotesContext db = new MyNotesContext();

        //
        // GET: /MyTest/

        public ActionResult Index()
        {
            return View(db.Notes.ToList());
        }

        //
        // GET: /MyTest/Help

        public ActionResult Help()
        {
            //Use Add view from popup!!!
            return View();
        }


        //mvcaction4 will insert action method to controller
        public ActionResult Action()
        {
            return View();
        }

        //
        // GET: /MyTest/Details/5

        public ActionResult Details(int id = 0)
        {
            MyNotes note = db.Notes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        //
        // GET: /MyTest/Create

        public ActionResult Create()
        {
            MyNotes note = new MyNotes();
            note.ID = 0;
            note.Note = "Write your note here";
            note.CreateDate = DateTime.Now.Date;
            return View(note);
        }

        //
        // POST: /MyTest/Create

        [HttpPost]
        public ActionResult Create(MyNotes note)
        {
            ModelState value;
            if (ModelState.IsValid)
            {
                note.CreateDate = DateTime.Now;
                db.Notes.Add(note);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else if (ModelState.TryGetValue("Note", out value))
            {
                note.Note = value.Value.AttemptedValue;
                note = new MyNotes();
                note.CreateDate = DateTime.Now;
                db.Notes.Add(note);
                db.SaveChanges();

            }

            return View(note);
        }

        //
        // GET: /MyTest/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MyNotes note = db.Notes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        //
        // POST: /MyTest/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MyNotes note)
        {
            if (ModelState.IsValid)
            {
                db.Entry(note).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(note);
        }

        //
        // GET: /MyTest/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MyNotes note = db.Notes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        //
        // POST: /MyTest/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MyNotes note = db.Notes.Find(id);
            db.Notes.Remove(note);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}