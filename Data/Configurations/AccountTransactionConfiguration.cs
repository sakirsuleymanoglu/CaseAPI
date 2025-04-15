using CaseAPI.Entities;
using CaseAPI.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseAPI.Data.Configurations;

public sealed class AccountTransactionConfiguration : IEntityTypeConfiguration<AccountTransaction>
{
    public void Configure(EntityTypeBuilder<AccountTransaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Account)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.AccountId);

        builder.Property(x => x.Type)
        .HasConversion(
            v => v.ToString(),
            v => Enum.Parse<TransactionType>(v));

        builder.Property(x => x.Channel)
      .HasConversion(
          v => v.ToString(),
          v => Enum.Parse<TransactionChannel>(v));
    }
}