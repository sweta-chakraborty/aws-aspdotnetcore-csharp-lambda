using Microsoft.EntityFrameworkCore;
using DemoLambdaFunction.Models;

namespace DemoLambdaFunction.Data
{
    public class SurveyDbContext : DbContext
    {
        public SurveyDbContext(DbContextOptions<SurveyDbContext> options)
            : base(options)
        {
        }

        public DbSet<SurveyResponse> SurveyResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SurveyResponse>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();  // Add unique constraint
                entity.Property(e => e.RoleSatisfaction).IsRequired();
                entity.Property(e => e.ManagerSupport).IsRequired();
                entity.Property(e => e.ValueRecognition).IsRequired();
                entity.Property(e => e.GrowthOpportunities).IsRequired();
                entity.Property(e => e.CompanyRecommendation).IsRequired();
            });
        }
    }
}
