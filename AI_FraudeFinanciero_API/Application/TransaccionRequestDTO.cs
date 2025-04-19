using System.ComponentModel.DataAnnotations;

namespace AI_FraudeFinanciero_API.Application;

public class TransaccionRequestDTO
{
    [Required(ErrorMessage = "El campo es obligatorio.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Debe tener entre 1 y 50 caracteres.")]
    public string? Origen { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Debe tener entre 1 y 50 caracteres.")]
    public string? Destino { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Debe tener entre 1 y 50 caracteres.")]
    public string? Canal { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Debe tener entre 1 y 50 caracteres.")]
    public string? Tipo { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio.")]
    [Range(0.01, float.MaxValue, ErrorMessage = "Se requiere valores mayores 0.01")]
    public float Frecuencia { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio.")]
    [Range(0.01, float.MaxValue, ErrorMessage = "Se requiere valores mayores 0.01")]
    public float TiempoTransaccion { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio.")]
    [Range(0.01, float.MaxValue, ErrorMessage = "Se requiere valores mayores $ 0.01")]
    public float Monto { get; set; }

    //[Required(ErrorMessage = "El campo es obligatorio.")]
    //[Range(1, 2, ErrorMessage = "El campo debe ser 1 o 2.")]
    //public int Algoritmo { get; set; } = 1; 
}
