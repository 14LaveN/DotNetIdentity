using DotNetIdentity.Domain.Entities;
using DotNetIdentity.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetIdentity.Database.Identity.Data;

/// <summary>
/// Represents the configuration for the <see cref="User"/> entity.
/// </summary>
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        builder.HasIndex(x => x.Id)
            .HasDatabaseName("IdUserIndex");

        //TODO builder.HasData( new EmailAddress("dfsdfsdfdsf"),
        //TODO     new User(FirstName.Create("dfsdf").Value,
        //TODO     LastName.Create("fdfsdfsf").Value,
        //TODO     new EmailAddress("dfsdfsdfdsf"),
        //TODO     "Sdfdsf_2008",
        //TODO     Guid.Empty));
        
        builder.HasKey(user => user.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        builder.OwnsOne(user => user.FirstName, firstNameBuilder =>
        {
            firstNameBuilder.WithOwner();

            firstNameBuilder.Property(firstName => firstName.Value)
                .HasColumnName(nameof(User.FirstName))
                .HasMaxLength(FirstName.MaxLength)
                .IsRequired();
        });


        builder.OwnsOne(user => user.LastName, lastNameBuilder =>
        {
            lastNameBuilder.WithOwner();

            lastNameBuilder.Property(lastName => lastName.Value)
                .HasColumnName(nameof(User.LastName))
                .HasMaxLength(LastName.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(user => user.EmailAddress, emailBuilder =>
        {
            emailBuilder.WithOwner();

            emailBuilder.Property(email => email.Value)
                .HasColumnName(nameof(User.EmailAddress))
                .HasMaxLength(EmailAddress.MaxLength)
                .IsRequired();
        });

        builder.Property(user => user.CreatedOnUtc).IsRequired();

        builder.Property(user => user.ModifiedOnUtc);

        builder.Property(user => user.DeletedOnUtc);

        builder.Property(user => user.Deleted).HasDefaultValue(false);

        builder.HasQueryFilter(user => !user.Deleted);

        builder.Ignore(user => user.FullName);
    }
}