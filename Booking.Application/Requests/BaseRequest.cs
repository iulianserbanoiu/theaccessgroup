namespace Booking.Application.Requests
{
    public abstract class BaseRequest
    {
        public string HotelId { get; }
        public DateOnly StartDate { get; }
        public DateOnly? EndDate { get; }

        public BaseRequest(string hotelId, DateOnly startDate, DateOnly? endDate)
        {
            HotelId = hotelId;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
