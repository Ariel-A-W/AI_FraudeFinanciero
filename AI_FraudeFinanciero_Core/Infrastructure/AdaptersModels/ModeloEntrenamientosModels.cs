using AI_FraudeFinanciero_Core.Domain.ModelosEntrenamientos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI_FraudeFinanciero_Core.Infrastructure.AdaptersModels;

public class ModeloEntrenamientosModels : IEntityTypeConfiguration<ModeloEntrenamiento>
{
    public void Configure(EntityTypeBuilder<ModeloEntrenamiento> builder)
    {
        builder.ToTable("ModelosEntrenamientos"); 

        builder
            .HasKey(k => k.Modelo_Entrenamiento_Id)
            .HasName("modelo_entrenamiento_id");

        builder.Property(p => p.Nombre)
            .HasMaxLength(50);

        builder.Property(p => p.Creacion)
            .HasColumnName("creacion");

        builder.Property(p => p.Modelo)
            .HasColumnType("longblob")
            .HasColumnName("modelo");
    }
}
