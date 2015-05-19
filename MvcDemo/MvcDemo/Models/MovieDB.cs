using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MvcDemo.Models
{
    public class MovieDB
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public DateTime Date { get; set; }
    }

    public class MovieDBContext : DbContext
    {
        public MovieDBContext()
            : base("MovieDBContext")
        {
        }
        public DbSet<MovieDB> Movies { get; set; }
    }

    public class MovieViewModel
    {
        public IEnumerable<MovieDB> Movies { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int ItemsOnPage { get; set; }
    }
}
