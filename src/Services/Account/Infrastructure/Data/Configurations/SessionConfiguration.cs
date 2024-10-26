using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
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
