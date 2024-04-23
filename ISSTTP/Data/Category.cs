using System;
using System.Collections.Generic;

namespace ISSTTP.Data;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; } 

    public string? Info { get; set; }

    public virtual ICollection<Detail> Details { get; set; } = new List<Detail>();
}
