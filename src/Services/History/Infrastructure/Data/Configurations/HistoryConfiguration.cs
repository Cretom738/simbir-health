using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class HistoryConfiguration : IEntityTypeConfiguration<History>
    {
        public void Configure(EntityTypeBuilder<History> builder)
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
