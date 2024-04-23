using System;
using System.Collections.Generic;

namespace ISSTTP.Data;

public partial class Status
{
    public int Id { get; set; }

    public string? Name { get; set; } 

    public virtual ICollection<CustomerDetail> CustomerDetails { get; set; } = new List<CustomerDetail>();
}
