using System;
using System.Collections.Generic;

namespace BasedeDatos.SqlServerModels;

public partial class Cantone
{
    public int IdCan { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdProv { get; set; }

    public int? Region { get; set; }

    public virtual Provincia? IdProvNavigation { get; set; }

    public virtual Regione? RegionNavigation { get; set; }

    public virtual ICollection<Parroquia> Parroquia { get; set; } = new List<Parroquia>();
}
