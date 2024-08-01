using System;
using System.Collections.Generic;

namespace BasedeDatos.PostgresqlModels;

public partial class Regione
{
    public int Region { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Cantone> Cantones { get; set; } = new List<Cantone>();

    public virtual ICollection<Parroquia> Parroquia { get; set; } = new List<Parroquia>();

    public virtual ICollection<Provincia> Provincia { get; set; } = new List<Provincia>();
}
