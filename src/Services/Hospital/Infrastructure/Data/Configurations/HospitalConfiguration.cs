using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class HospitalConfiguration : IEntityTypeConfiguration<Hospital>
    {
        public void Configure(EntityTypeBuilder<Hospital> builder)
        {
            builder
                .Property(a => a.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder
                .Property(a => a.UpdatedAt)
                .HasDefaultValueSql("NOW()");
        }
    }
}
