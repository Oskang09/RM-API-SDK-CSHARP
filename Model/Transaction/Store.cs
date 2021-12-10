using System;

namespace RM_API_SDK_CSHARP.Model.Transaction
{
    public class Store
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public GeoLocation GeoLocation { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}

