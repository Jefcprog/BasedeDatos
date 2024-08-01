using System;
using System.Collections.Generic;

namespace BasedeDatos.OracleModels;

public partial class Provincia
{
    public decimal IdProv { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal? Region { get; set; }

    public virtual ICollection<Cantone> Cantones { get; set; } = new List<Cantone>();

    public virtual Regione? RegionNavigation { get; set; }

    public virtual ICollection<Parroquia> Parroquia { get; set; } = new List<Parroquia>();
}
