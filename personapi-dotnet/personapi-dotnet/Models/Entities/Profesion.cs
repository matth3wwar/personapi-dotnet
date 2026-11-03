using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace personapi_dotnet.Models.Entities;

// Las validaciones siguen los tipos y tamaños de la tabla profesion
public partial class Profesion
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(90, ErrorMessage = "El nombre tiene máximo 90 caracteres.")]
    public string Nom { get; set; } = null!;

    public string? Des { get; set; }

    public virtual ICollection<Estudio> Estudios { get; set; } = new List<Estudio>();
}
