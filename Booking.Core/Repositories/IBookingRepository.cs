
using Booking.Core.Model;

namespace Booking.Core.Repositories
{
    public interface IBookingRepository
    {
        public int CountBookedRooms(string hotelId, RoomTypeEnum roomType, DateOnly startDate, DateOnly endDate);
    }
}
