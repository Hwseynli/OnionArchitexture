using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnionArchitecture.Domain.Entities.Documents;

namespace OnionArchitecture.Persistence.EntityConfiguration.DocumentEntityTypeConfigurations
{
    public class AdditionDocumentEntityTypeConfiguration : IEntityTypeConfiguration<AdditionDocument>
    {
        public void Configure(EntityTypeBuilder<AdditionDocument> builder)
        {
            builder.ToTable("additionDocuments");

            builder.HasKey(t => t.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("id");

            builder.Property(x => x.Other)
                .HasColumnName("other");

            builder.Property(x => x.DocumentType)
                .HasColumnName("document_type");

            builder.Property(x => x.CustomerId)
                .IsRequired()
                .HasColumnName("customer_id");

            builder.HasMany(x => x.Documents)
                .WithOne(x => x.AdditionDocument)
                .HasForeignKey(x => x.AdditionDocumentId)
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

}