using System;
using System.Collections.Generic;

namespace GeriDonus.Data.Entities;

public partial class Basvuru
{
    public int Id { get; set; }

    public int KisiId { get; set; }

    public int MudurlukIlId { get; set; }

    public DateOnly Basvurutarihi { get; set; }

    public virtual ICollection<BasvuruDurumGecmisi> BasvuruDurumGecmisis { get; set; } = new List<BasvuruDurumGecmisi>();

    public virtual Kisi Kisi { get; set; } = null!;

    public virtual Il MudurlukIl { get; set; } = null!;
}
