using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace personapi_dotnet.Models.Entities;

// Las validaciones siguen los tipos y tamaños de la tabla estudios
public partial class Estudio
{
    [Required(ErrorMessage = "Seleccione una profesión.")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione una profesión.")]
    public int IdProf { get; set; }

    [Required(ErrorMessage = "Seleccione una persona.")]
    public string CcPer { get; set; } = null!;

    public DateOnly? Fecha { get; set; }

    [StringLength(50, ErrorMessage = "La universidad tiene máximo 50 caracteres.")]
    public string? Univer { get; set; }

    public virtual Persona CcPerNavigation { get; set; } = null!;

    public virtual Profesion IdProfNavigation { get; set; } = null!;
}
