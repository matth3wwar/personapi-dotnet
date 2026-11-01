using System;
using System.Collections.Generic;

namespace personapi_dotnet.Models.Entities;

public partial class Telefono
{
    public string Num { get; set; } = null!;

    public string? Oper { get; set; }

    public string Dueno { get; set; } = null!;

    public virtual Persona DuenoNavigation { get; set; } = null!;
}
