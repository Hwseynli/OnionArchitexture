using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnionArchitecture.Domain.Entities.Documents;

namespace OnionArchitecture.Persistence.EntityConfiguration.DocumentEntityTypeConfigurations;
public class DocumentEntityTypeConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("documents");

        builder.HasKey(t => t.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id");

        builder.Property(x => x.Name)
              .IsRequired()
              .HasColumnName("name");

        builder.Property(x => x.Path)
             .IsRequired()
             .HasColumnName("path");

        builder.Property(x => x.AdditionDocumentId)
            .IsRequired()
            .HasColumnName("additionDocument_id");

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

