using System.Collections.Generic;

namespace OutsuranceTask1
{
    public class Customer
    {
        public List<CustomerName> CustomerNames { get; set; }
        public CustomerAddress CustomerAddress { get; set; }
    }

    public class CustomerName
    {
        public string Name { get; set; }
    }

    public class CustomerAddress
    {
        public string Number { get; set; } //we will use string in case of alpha characters in home number e.g. 9a Smith Str, 9b Smith Str etc.
        public string Name { get; set; }
    }

}
