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
                var folder = Path.GetDirectoryName(assembly) ?? string.Empty;
                var dbFile = Path.Combine(folder, "data/generator.sqlite");
                optionsBuilder.UseSqlite($"DataSource={dbFile};Mode=ReadOnly");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterAncestryDto>()
                .HasIndex(x => x.Code);
        }

        internal DbSet<CharacterAncestryDto> CharacterAncestries => Set<CharacterAncestryDto>();
        internal DbSet<BuildingTypeDto> BuildingTypes => Set<BuildingTypeDto>();
        internal DbSet<BuildingNamePartDto> BuildingNameParts => Set<BuildingNamePartDto>();
        internal DbSet<SettlementTypeDto> SettlementTypes => Set<SettlementTypeDto>();
    }
}
