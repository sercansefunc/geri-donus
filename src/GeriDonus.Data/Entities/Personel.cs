using System;
using System.Collections.Generic;

namespace GeriDonus.Data.Entities;

public partial class Personel
{
    public int Id { get; set; }

    public int IlId { get; set; }

    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public virtual ICollection<BasvuruDurumGecmisi> BasvuruDurumGecmisis { get; set; } = new List<BasvuruDurumGecmisi>();

    public virtual Il Il { get; set; } = null!;
}
