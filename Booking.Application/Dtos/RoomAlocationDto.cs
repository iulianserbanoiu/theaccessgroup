namespace Booking.Application.Dtos
{
    public class RoomAlocationDto
    {
        public string HotelId { get; set; }
        public List<string> RoomTypes { get; set; }
    }
}
