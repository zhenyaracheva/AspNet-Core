namespace CityInfo.Core.Entities.DbContexts
{
    using Microsoft.EntityFrameworkCore;

    public class CityInfoDbContext : DbContext
    {
        public CityInfoDbContext(DbContextOptions<CityInfoDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
            Database.Migrate();
        }

        public DbSet<City> Cities { get; set; }

        public DbSet<PointOfInterest> PointsOfInterest { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        //{
        //    optionBuilder.UseSqlServer("connectionstring");

        //    base.OnConfiguring(optionBuilder);
        //}
    }
}
