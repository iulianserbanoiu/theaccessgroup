using Booking.Core.Model;

namespace Booking.Application.Requests
{
    public class RoomAvailabilityRequest : BaseRequest
    {
        public RoomTypeEnum RoomTypeCode { get; }

        public RoomAvailabilityRequest(string hotelId, DateOnly startDate, DateOnly? endDate, RoomTypeEnum roomTypeCode)
            : base(hotelId, startDate, endDate)
        {
            RoomTypeCode = roomTypeCode;
        }
    }
}
