using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MvcDemo.Models;

namespace MvcDemo.Controllers
{
    public class MyNotesAPIController : ApiController
    {
        private MyNotesContext db = new MyNotesContext();

        // GET api/mynotesapi
        public IEnumerable<MyNotes> Get()
        {
            //return new string[] { "value1", "value2" };
            return db.Notes.ToList();
        }

        // GET api/mynotesapi/5
        public MyNotes Get(int id)
        {
            MyNotes note = db.Notes.Find(id);
            if (note == null)
            {
                return new MyNotes();
            }
            return note;
        }

        // POST api/mynotesapi
        public void Post([FromBody]MyNotes value)
        {
            db.Notes.Add(value);
            db.SaveChanges();
        }

        // PUT api/mynotesapi/5
        public void Put(int id, [FromBody]MyNotes value)
        {
            MyNotes note = db.Notes.Find(id);
            if (note != null)
            {
                note = value; //copy to!
                db.SaveChanges();
            }
        }

        // DELETE api/mynotesapi/5
        public void Delete(int id)
        {
            MyNotes note = db.Notes.Find(id);
            if (note != null)
            {
                db.Notes.Remove(note);
                db.SaveChanges();
            }
        }
    }
}
