using System;
using System.Collections.Generic;

namespace ISSTTP.Data;

public partial class CustomerDetail
{
    public int Id { get; set; }

    public int DetailId { get; set; }

    public int CustomerId { get; set; }

    public DateOnly BuyingDate { get; set; }

    public int StatusId { get; set; }

    public string? Address { get; set; }

    public virtual Сustomer? Customer { get; set; }

    public virtual Detail? Detail { get; set; }

    public virtual Status? Status { get; set; } 
}
