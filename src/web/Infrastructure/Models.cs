using System;
using System.Collections.Generic;

namespace Web.Infrastructure;

public interface IBatch<T>
{
    int Total { get; set; }
    string Search { get; set; }
    int Number { get; set; }
    IEnumerable<T> Elements { get; set; }
}

public class ContactBatch : IBatch<ContactModel>
{
    public int Total { get; set; }
    public string Search { get; set; }
    public int Number { get; set; } = 1;
    public IEnumerable<ContactModel> Elements { get; set; } = new List<ContactModel>();
}

public class ContactModel
{
    public Guid ContactId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public DateTime Created { get; set; }
    public string Phone { get; set; }
}