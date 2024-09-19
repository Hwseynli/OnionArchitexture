using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnionArchitecture.Domain.Entities;

namespace OnionArchitecture.Persistence.EntityConfiguration.CustomerEntityTypeConfigurations;
public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");

        builder.HasKey(t => t.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id");

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasColumnName("first_name");

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasColumnName("last_name");

        builder.Property(x => x.Email)
           .IsRequired()
           .HasColumnName("mail");

        builder.HasIndex(t => t.Email)
            .IsUnique();

        builder.HasMany(x => x.AdditionDocuments)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId)
            .IsRequired();

        builder.Property(x => x.CreatedById)
            .HasColumnName("created_by_id");

        builder.Property(x => x.LastUpdateDateTime)
            .HasColumnName("last_update_date_time");

        builder.Property(x => x.UpdateById)
           .HasColumnName("update_by_id");

        builder.Property(x => x.RecordDateTime)
           .HasColumnName("record_date_time");
    }

}