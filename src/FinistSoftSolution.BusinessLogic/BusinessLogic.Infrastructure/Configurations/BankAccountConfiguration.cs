using BusinessLogic.Core.Entities;
using BusinessLogic.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessLogic.Infrastructure.Configurations;

internal sealed class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.Property(e => e.AccountNumber)
           .HasConversion(
               e => e.Value,
               v => new AccountNumber(v));

        builder.HasIndex(e => e.AccountNumber).IsUnique();
    }
}