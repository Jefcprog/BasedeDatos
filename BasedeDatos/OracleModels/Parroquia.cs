using System;
using System.Collections.Generic;

namespace BasedeDatos.OracleModels;

public partial class Parroquia
{
    public decimal IdPar { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal? IdCan { get; set; }

    public decimal? IdProv { get; set; }

    public decimal? Region { get; set; }

    public virtual Cantone? IdCanNavigation { get; set; }

    public virtual Provincia? IdProvNavigation { get; set; }

    public virtual Regione? RegionNavigation { get; set; }
}
