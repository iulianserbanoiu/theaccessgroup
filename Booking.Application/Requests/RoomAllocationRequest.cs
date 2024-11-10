namespace Booking.Application.Requests
{
    public class RoomAllocationRequest : BaseRequest
    {
        public int PersonsNumber { get; }

        public RoomAllocationRequest(string hotelId, DateOnly startDate, DateOnly? endDate, int personsNumber) : base(hotelId, startDate, endDate)
        {
            PersonsNumber = personsNumber;
        }
    }
}
