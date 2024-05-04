using System;
using System.Collections.Generic;

namespace ISSTTP.Data;

public partial class Detail
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Info { get; set; }

    public int? CategoryId { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<AdministratorDetail> AdministratorDetails { get; set; } = new List<AdministratorDetail>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<CustomerDetail> CustomerDetails { get; set; } = new List<CustomerDetail>();
}
