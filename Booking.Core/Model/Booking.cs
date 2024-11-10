namespace Booking.Core.Model
{
    public class Booking
    {
        public string HotelId { get; set; }
        public DateOnly Arrival { get; set; }
        public DateOnly Departure { get; set; }
        public RoomTypeEnum RoomType { get; set; }
        public RoomRateEnum RoomRate { get; set; }
    }
}
