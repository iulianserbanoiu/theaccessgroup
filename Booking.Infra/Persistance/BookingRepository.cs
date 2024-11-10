using Booking.Core.Model;
using Booking.Core.Repositories;

namespace Booking.Infra.Persistance
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDataStore<Core.Model.Booking> _bookingsStore;

        public BookingRepository(IDataStore<Core.Model.Booking> dataStore)
        {
            _bookingsStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        }

        public int CountBookedRooms(string hotelId, RoomTypeEnum roomTypeCode, DateOnly startDate, DateOnly endDate)
        {
            return _bookingsStore.Data
                .Where(b => b.HotelId == hotelId
                        && b.RoomType == roomTypeCode
                        && b.Arrival < endDate
                        && b.Departure > startDate)
                .Count();
        }
    }
}
