using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sodium;

namespace Infrastructure.Data.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder
                .HasIndex(a => a.Username)
                .IsUnique();

            builder
                .Property(a => a.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder
                .Property(a => a.UpdatedAt)
                .HasDefaultValueSql("NOW()");

            builder.HasData(new Account
            {
                Id = 1,
                LastName = "admin",
                FirstName = "admin",
                Username = "admin",
                Password = PasswordHash.ArgonHashString("admin"),
                Roles = [Role.Admin]
            }, new Account
            {
                Id = 2,
                LastName = "manager",
                FirstName = "manager",
                Username = "manager",
                Password = PasswordHash.ArgonHashString("manager"),
                Roles = [Role.Manager]
            }, new Account
            {
                Id = 3,
                LastName = "doctor",
                FirstName = "doctor",
                Username = "doctor",
                Password = PasswordHash.ArgonHashString("doctor"),
                Roles = [Role.Doctor]
            }, new Account
            {
                Id = 4,
                LastName = "user",
                FirstName = "user",
                Username = "user",
                Password = PasswordHash.ArgonHashString("user"),
                Roles = [Role.User]
            });
        }
    }
}
