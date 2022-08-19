#nullable enable
using UntoMeWorld.Domain.Common;

namespace UntoMeWorld.Domain.Model
{
    public class Address : ICloneable<Address>
    {
        public string Country { get; set; } = "";
        public string City { get; set; } = "";
        public string Department { get; set; } = "";
        public string AddressLine1 { get; set; } = "";
        public string? AddressLine2 { get; set; }
        public string ZipCode { get; set; } = "";
        public Address Clone()
        {
            return (Address) MemberwiseClone();
        }
    }
}