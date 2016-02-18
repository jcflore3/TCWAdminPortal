using System;

namespace TCWAdminPortalWeb.Models
{
    public class FeaturedProperty
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public string ThumbnailURL { get; set; }
        public string MLSListingURL { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int BedRooms { get; set; }
        public int FullBaths { get; set; }
        public int HalfBaths { get; set; }
        public int Garage { get; set; }
        public int BuildingSQFeet { get; set; }
        public int LotSQFeet { get; set; }
        public DateTime Created { get; set; }
        public Boolean Enabled { get; set; }
    }
}