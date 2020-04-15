using System;
using System.Collections.Generic;

namespace DataCreationFramework.Tests.Types
{
    public class Person
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public Address HomeAddress { get; set; }

        public Address BusinessAddress { get; set; }

        public string EMail { get; set; }

        public List<int> Changes { get; set; }

        public bool IsMember { get; set; }
    }
}
