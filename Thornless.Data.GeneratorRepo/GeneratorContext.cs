using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Thornless.Data.GeneratorRepo.DataModels;

namespace Thornless.Data.GeneratorRepo
{
    public class GeneratorContext : DbContext
    {
        // This is primaryily here to support unit testing
        public GeneratorContext(DbContextOptions<GeneratorContext> options)
            : base(options)
        {
        }

        public GeneratorContext()
            : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var assembly = Assembly.GetExecutingAssembly().Location;
                var folder = Path.GetDirectoryName(assembly);
                var dbFile = Path.Combine(folder, "data/generator.sqlite");
                optionsBuilder.UseSqlite($"DataSource={dbFile};Mode=ReadOnly");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterAncestryDto>()
                .HasIndex(x => x.Code);
        }

        internal DbSet<CharacterAncestryDto> CharacterAncestries { get; set; }
    }
}
