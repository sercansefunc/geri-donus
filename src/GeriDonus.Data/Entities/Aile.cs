using System;
using System.Collections.Generic;

namespace GeriDonus.Data.Entities;

public partial class Aile
{
    public int Id { get; set; }

    public virtual ICollection<Kisi> Kisis { get; set; } = new List<Kisi>();
}
