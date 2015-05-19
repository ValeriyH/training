using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MvcDemo.Models
{
    public class MyNotes
    {
        public int ID { get; set; }
        public String Note { get; set; }
        public DateTime? CreateDate { get; set; }
    }

    public class MyNotesContext : DbContext
    {
        public MyNotesContext()
            : base("MovieDBContext")
        {
        }

        public DbSet<MyNotes> Notes { get; set; }
    }

    public class MyNotesContextInitializer : DropCreateDatabaseIfModelChanges<MyNotesContext>
    {
        protected override void Seed(MyNotesContext context)
        {
            context.Notes.Add(
                new MyNotes() { Note = "This is the sample note", CreateDate = DateTime.Now }
                );
            context.SaveChanges();
        }
    }

}