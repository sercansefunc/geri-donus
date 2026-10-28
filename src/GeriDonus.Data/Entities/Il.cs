using System;
using System.Collections.Generic;

namespace GeriDonus.Data.Entities;

public partial class Il
{
    public int Id { get; set; }

    public string Ad { get; set; } = null!;

    public virtual ICollection<Basvuru> Basvurus { get; set; } = new List<Basvuru>();

    public virtual ICollection<Kisi> Kisis { get; set; } = new List<Kisi>();

    public virtual ICollection<Personel> Personels { get; set; } = new List<Personel>();
}
