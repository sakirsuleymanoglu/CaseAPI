using CaseAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseAPI.Data.Configurations;

public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.Code).IsRequired();

        builder.HasOne(x => x.AppUser)
            .WithMany(x => x.Accounts)
            .HasForeignKey(x => x.AppUserId);

        builder.Property(x => x.RowVersion).IsRowVersion();
    }
}
