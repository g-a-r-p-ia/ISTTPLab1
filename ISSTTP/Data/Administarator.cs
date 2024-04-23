using System;
using System.Collections.Generic;

namespace ISSTTP.Data;

public partial class Administarator
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<AdministratorDetail> AdministratorDetails { get; set; } = new List<AdministratorDetail>();
}
