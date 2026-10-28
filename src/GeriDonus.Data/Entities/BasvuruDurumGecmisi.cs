using System;
using System.Collections.Generic;

namespace GeriDonus.Data.Entities;

public partial class BasvuruDurumGecmisi
{
    public int Id { get; set; }

    public int BasvuruId { get; set; }

    public int DurumId { get; set; }

    public int PersonelId { get; set; }

    public DateTime Islemzamani { get; set; }

    public string? Gerekce { get; set; }

    public virtual Basvuru Basvuru { get; set; } = null!;

    public virtual Durum Durum { get; set; } = null!;

    public virtual Personel Personel { get; set; } = null!;
}
