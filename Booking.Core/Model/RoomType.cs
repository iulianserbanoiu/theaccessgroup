namespace Booking.Core.Model
{
    public class RoomType
    {
        public RoomTypeEnum Code { get; set; }
        public int Size { get; set; }
        public string Description { get; set; }
        public List<string> Amenities { get; set; }
        public List<string> Features { get; set; }
    }
}
