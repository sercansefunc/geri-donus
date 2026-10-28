using System;
using System.Collections.Generic;

namespace GeriDonus.Data.Entities;

public partial class Kisi
{
    public int Id { get; set; }

    public int AileId { get; set; }

    public int IlId { get; set; }

    public string? Kimlikno { get; set; }

    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public string? Adres { get; set; }

    public string? Telno { get; set; }

    public DateOnly Dogumtarihi { get; set; }

    public virtual Aile Aile { get; set; } = null!;

    public virtual ICollection<Basvuru> Basvurus { get; set; } = new List<Basvuru>();

    public virtual Il Il { get; set; } = null!;
}
