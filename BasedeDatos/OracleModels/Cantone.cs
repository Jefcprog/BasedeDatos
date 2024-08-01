using System;
using System.Collections.Generic;

namespace BasedeDatos.OracleModels;

public partial class Cantone
{
    public decimal IdCan { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal? IdProv { get; set; }

    public decimal? Region { get; set; }

    public virtual Provincia? IdProvNavigation { get; set; }

    public virtual Regione? RegionNavigation { get; set; }

    public virtual ICollection<Parroquia> Parroquia { get; set; } = new List<Parroquia>();
}
