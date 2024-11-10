using Booking.Core.Model;

namespace Booking.Core.Repositories
{
    public interface IHotelRepository
    {
        public Hotel GetHotel(string id);

        public int GetRoomNumbersBy(string hotelId, RoomTypeEnum roomType);
    }
}
