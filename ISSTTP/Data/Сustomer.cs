using System;
using System.Collections.Generic;

namespace ISSTTP.Data;

public partial class Сustomer
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; } 

    public int Card { get; set; }

    public int Phone { get; set; }

    public virtual ICollection<CustomerDetail> CustomerDetails { get; set; } = new List<CustomerDetail>();
}
