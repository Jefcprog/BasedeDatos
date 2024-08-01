using System;
using System.Collections.Generic;

namespace BasedeDatos.PostgresqlModels;

public partial class Parroquia
{
    public int IdPar { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdCan { get; set; }

    public int? IdProv { get; set; }

    public int? Region { get; set; }

    public virtual Cantone? IdCanNavigation { get; set; }

    public virtual Provincia? IdProvNavigation { get; set; }

    public virtual Regione? RegionNavigation { get; set; }
}
