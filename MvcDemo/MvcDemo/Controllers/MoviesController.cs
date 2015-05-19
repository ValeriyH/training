using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcDemo.Models;
using MvcDemo.Filters;

namespace MvcDemo.Controllers
{
    [LogRequests]
    public class MoviesController : Controller
    {
        private MovieDBContext db = new MovieDBContext();
        private int itemsPerPage = 10;
        private int currentPage = 0;

        //
        // GET: /Movies/

        public ActionResult Index(int page = 0, int test = 0)
        {
            int count = db.Movies.Count();

            if (page < 0 || page * itemsPerPage > count)
            {
                return HttpNotFound();
            }
            else
            {
                currentPage = page;
            }

            //TODO Get range from Movies Enumerable not from List
            //return View(db.Movies.Where((model, index) => index > page * itemsOnPage && index <= page * itemsOnPage + itemsOnPage).ToList());

            //Note: In theory pagination should be on the View side.
            int start = page * itemsPerPage;
            int items = itemsPerPage;
            if (start + items > count)
            {
                items = count - start;
            }

            MovieViewModel view = new MovieViewModel();
            view.Movies = db.Movies.ToList().GetRange(start, items);
            view.CurrentPage = page;
            view.ItemsOnPage = items;
            view.PageCount = count / itemsPerPage;
            if (view.PageCount * itemsPerPage < count )
            {
                view.PageCount++;
            }

            return View(view);
        }

        //
        // GET: /Movies/Details/5

        public ActionResult Details(int id = 0)
        {
            MovieDB moviedb = db.Movies.Find(id);
            if (moviedb == null)
            {
                return HttpNotFound();
            }
            return View(moviedb);
        }

        //
        // GET: /Movies/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Movies/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovieDB moviedb)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(moviedb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(moviedb);
        }

        //
        // GET: /Movies/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MovieDB moviedb = db.Movies.Find(id);
            if (moviedb == null)
            {
                return HttpNotFound();
            }
            return View(moviedb);
        }

        //
        // POST: /Movies/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MovieDB moviedb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(moviedb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(moviedb);
        }

        //
        // GET: /Movies/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MovieDB moviedb = db.Movies.Find(id);
            if (moviedb == null)
            {
                return HttpNotFound();
            }
            return View(moviedb);
        }

        //
        // POST: /Movies/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MovieDB moviedb = db.Movies.Find(id);
            db.Movies.Remove(moviedb);
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