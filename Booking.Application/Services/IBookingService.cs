using Booking.Application.Dtos;
using Booking.Application.Requests;

namespace Booking.Application.Services
{
    public interface IBookingService
    {
        ResponseDto<int> GetNoOfRoomsAvailable(RoomAvailabilityRequest request);

        ResponseDto<RoomAlocationDto> GetRoomAllocation(RoomAllocationRequest request);
    }
}
