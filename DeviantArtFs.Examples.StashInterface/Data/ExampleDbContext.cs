using DeviantArtFs.Examples.StashInterface.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviantArtFs.Examples.StashInterface.Data
{
    public class ExampleDbContext : DbContext
    {
        public ExampleDbContext(DbContextOptions<ExampleDbContext> options) : base(options) { }

        public DbSet<StashEntry> StashEntries { get; set; }

        public DbSet<DeltaCursor> DeltaCursors { get; set; }

        public DbSet<Token> Tokens { get; set; }
    }
}
