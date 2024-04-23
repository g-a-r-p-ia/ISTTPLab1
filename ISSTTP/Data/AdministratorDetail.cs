using System;
using System.Collections.Generic;

namespace ISSTTP.Data;

public partial class AdministratorDetail
{
    public int Id { get; set; }

    public int AdministratorId { get; set; }

    public int DetailId { get; set; }

    public virtual Administarator? Administrator { get; set; }

    public virtual Detail? Detail { get; set; } 
}
