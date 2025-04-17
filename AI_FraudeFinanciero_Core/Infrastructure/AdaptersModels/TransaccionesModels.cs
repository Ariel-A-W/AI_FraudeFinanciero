using AI_FraudeFinanciero_Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI_FraudeFinanciero_Core.Infrastructure.AdaptersModels;

public class TransaccionesModels : IEntityTypeConfiguration<Transaccion>
{
    public void Configure(EntityTypeBuilder<Transaccion> builder)
    {
        builder.ToTable("Transacciones");

        builder
            .HasKey(t => t.Transaccion_Id)
            .HasName("transaccion_id");

        builder.Property(p => p.Fecha)
            .HasColumnName("fecha");

        builder.Property(p => p.Origen)
            .HasColumnName("origen")
            .HasMaxLength(50);

        builder.Property(p => p.Destino)
            .HasColumnName("destino")
            .HasMaxLength(50);

        builder.Property(p => p.Canal)
            .HasColumnName("canal")
            .HasMaxLength(50);

        builder.Property(p => p.Tipo)
            .HasColumnName("tipo")
            .HasMaxLength(50);

        builder.Property(p => p.Monto)
            .HasColumnName("monto");

        builder.Property(p => p.Frecuencia)
            .HasColumnName("frecuencia");

        builder.Property(p => p.TiempoTransaccion)
            .HasColumnName("tiempo_transaccion");

        builder.Property(p => p.Score)
            .HasColumnName("score");

        builder.Property(p => p.IsSospechosa)
            .HasColumnName("is_sospechosa");
    }
}
