using System;
using System.Collections.Generic;

namespace efcore_cosmosdb.Models
{
    public class Job
    {
        public Guid Id { get; set; }
        public Address Address { get; set; }

        public List<Contact> Contacts { get; set; }

        public Guid AssignedResourceId { get; set; }
        public Resource AssignedResource { get; set; }
    }

    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
    }

    public class Contact
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelephoneNumber { get; set; }
    }

    public class Resource
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelephoneNumber { get; set; }
    }
}