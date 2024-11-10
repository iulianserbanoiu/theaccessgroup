using Booking.Core.Model;
using Booking.Core.Repositories;

namespace Booking.Infra.Persistance
{
    public class HotelRepository : IHotelRepository
    {
        private readonly IDataStore<Hotel> _hotelsStore;

        public HotelRepository(IDataStore<Hotel> dataStore)
        {
            _hotelsStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        }

        public Hotel GetHotel(string id)
        {
            var hotel = _hotelsStore.Data.SingleOrDefault(h => h.Id == id);

            if (hotel == null)
            {
                throw new Exception("Hotel not found");
            }

            return hotel;
        }

        public int GetRoomNumbersBy(string hotelId, RoomTypeEnum roomType)
        {
            var hotel = GetHotel(hotelId);
            return hotel.Rooms.Count(r => r.RoomType == roomType);
        }
    }
}
