using Microsoft.EntityFrameworkCore;
using MvcUnitTesting_dotnet8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace MvcUnitTesting_dotnet8.Data
{
    public class DbSeederTesting
    {
        private readonly BookDbContext _ctx;
        private readonly IWebHostEnvironment _hosting;
        private bool disposedValue;

        public DbSeederTesting(BookDbContext ctx, IWebHostEnvironment hosting)
        {
            _ctx = ctx;
            _hosting = hosting;
        }

        public void Seed()
        {
            _ctx.Database.EnsureCreated();

            if (!_ctx.Books.Any())
            {
                _ctx.Books.AddRange(new List<Book>
                {
                    new Book
                    {
                        Name = "Moby Dick",
                        Genre = "Fiction",
                        Price = 12.50m
                    },
                    new Book
                    {
                        Name = "War and Peace",
                        Genre = "Fiction",
                        Price = 17.00m
                    },
                    new Book
                    {
                        Name = "Escape from the vortex",
                        Genre = "Science Fiction",
                        Price = 12.50m
                    },
                    new Book
                    {
                        Name = "The Battle of the Somme",
                        Genre = "History",
                        Price = 22.00m
                    }
                });
                _ctx.SaveChanges();
            }
        }
    }
}
