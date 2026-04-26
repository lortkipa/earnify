using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users")
                .HasKey(u => u.Id);

            builder.Property(u => u.GoogleId)
                .IsRequired();
            builder.HasIndex(u => u.GoogleId)
                .IsUnique();

            builder.Property(u => u.GoogleToken)
               .IsRequired();

            builder.Property(u => u.Email)
              .IsRequired();
            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.FirstName)
              .IsRequired();

            builder.Property(u => u.LastName)
              .IsRequired();

            builder.Property(u => u.AvatarUrl)
              .IsRequired();

            builder.Property(u => u.CreatedAt)
              .IsRequired();

            builder.HasMany(u => u.DonationLinks)
                .WithOne(dl => dl.User)
                .HasForeignKey(dl => dl.CreatorId);
        }
    }
}
