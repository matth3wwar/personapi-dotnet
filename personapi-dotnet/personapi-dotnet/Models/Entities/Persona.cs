using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace personapi_dotnet.Models.Entities;

// Las validaciones siguen los tipos y tamaños de la tabla persona
public partial class Persona
{
    [Required(ErrorMessage = "La identificación es obligatoria.")]
    [StringLength(15, ErrorMessage = "La identificación tiene máximo 15 dígitos.")]
    [RegularExpression("^[0-9]+$", ErrorMessage = "La identificación solo puede tener números.")]
    public string Cc { get; set; } = null!;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(45, ErrorMessage = "El nombre tiene máximo 45 caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(45, ErrorMessage = "El apellido tiene máximo 45 caracteres.")]
    public string Apellido { get; set; } = null!;

    [Required(ErrorMessage = "El género es obligatorio.")]
    [RegularExpression("^[MF]$", ErrorMessage = "El género debe ser M o F.")]
    public string Genero { get; set; } = null!;

    [Range(0, 150, ErrorMessage = "La edad debe estar entre 0 y 150.")]
    public int? Edad { get; set; }

    public virtual ICollection<Estudio> Estudios { get; set; } = new List<Estudio>();

    public virtual ICollection<Telefono> Telefonos { get; set; } = new List<Telefono>();
}
