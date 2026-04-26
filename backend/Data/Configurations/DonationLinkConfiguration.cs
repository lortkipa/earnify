using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configurations
{
    public class DonationLinkConfiguration : IEntityTypeConfiguration<DonationLink>
    {
        public void Configure(EntityTypeBuilder<DonationLink> builder)
        {
            builder.ToTable("DonationLinks")
                .HasKey(pl => pl.Id);

            builder.Property(pl => pl.CreatorId)
                .IsRequired();

            builder.Property(pl => pl.Message)
               .IsRequired()
               .HasMaxLength(400);

            builder.HasOne(bl => bl.User)
                .WithMany(u => u.DonationLinks)
                .HasForeignKey(bl => bl.CreatorId);
        }
    }
}
