using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class TimetableConfiguration : IEntityTypeConfiguration<Timetable>
    {
        public void Configure(EntityTypeBuilder<Timetable> builder)
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
