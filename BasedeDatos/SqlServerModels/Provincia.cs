using System;
using System.Collections.Generic;

namespace BasedeDatos.SqlServerModels;

public partial class Provincia
{
    public int IdProv { get; set; }

    public string Nombre { get; set; } = null!;

    public int? Region { get; set; }

    public virtual ICollection<Cantone> Cantones { get; set; } = new List<Cantone>();

    public virtual Regione? RegionNavigation { get; set; }

    public virtual ICollection<Parroquia> Parroquia { get; set; } = new List<Parroquia>();
}
