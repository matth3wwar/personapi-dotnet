using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace personapi_dotnet.Models.Entities;

// Las validaciones siguen los tipos y tamaños de la tabla telefono
public partial class Telefono
{
    [Required(ErrorMessage = "El número es obligatorio.")]
    [StringLength(15, ErrorMessage = "El número tiene máximo 15 dígitos.")]
    [RegularExpression("^[0-9]+$", ErrorMessage = "El número solo puede tener dígitos.")]
    public string Num { get; set; } = null!;

    [StringLength(45, ErrorMessage = "El operador tiene máximo 45 caracteres.")]
    public string? Oper { get; set; }

    [Required(ErrorMessage = "Seleccione la persona propietaria.")]
    public string Dueno { get; set; } = null!;

    public virtual Persona DuenoNavigation { get; set; } = null!;
}
