using System;
using System.Collections.Generic;

namespace GeriDonus.Data.Entities;

public partial class Durum
{
    public int Id { get; set; }

    public string Ad { get; set; } = null!;

    public virtual ICollection<BasvuruDurumGecmisi> BasvuruDurumGecmisis { get; set; } = new List<BasvuruDurumGecmisi>();
}
