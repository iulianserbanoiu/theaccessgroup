
using Booking.Application.Dtos;
using Booking.Application.Requests;
using Booking.Core.Model;
using Booking.Core.Repositories;

namespace Booking.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IHotelRepository hotelRepository, IBookingRepository bookingRepository)
        {
            _hotelRepository = hotelRepository ?? throw new ArgumentNullException(nameof(hotelRepository));
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        }

        public ResponseDto<int> GetNoOfRoomsAvailable(RoomAvailabilityRequest request)
        {
            Validate(request);

            try
            {
                var roomsAvailable = GetNoOfRoomsAvailableInternal(request);

                return ResponseDto<int>.Success(roomsAvailable);
            }
            catch (Exception ex)
            {
                // log error
                return ResponseDto<int>.Error(ex.Message);
            }
        }

        private int GetNoOfRoomsAvailableInternal(RoomAvailabilityRequest request)
        {
            int roomsCount = _hotelRepository.GetRoomNumbersBy(request.HotelId, request.RoomTypeCode);
            int bookedRooms = _bookingRepository.CountBookedRooms(request.HotelId, request.RoomTypeCode, request.StartDate, request.EndDate ?? request.StartDate);

            // can return negative for overbooking
            return roomsCount - bookedRooms;
        }

        private static void Validate(BaseRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.HotelId)) throw new ArgumentNullException(nameof(request.HotelId));
            // enable for real life scenario
            //if (startDate < DateOnly.FromDateTime(DateTime.UtcNow)) throw new ArgumentNullException(nameof(startDate)); 
            if (request.EndDate.HasValue && request.StartDate > request.EndDate) throw new ArgumentOutOfRangeException(nameof(request.EndDate));
        }

        public ResponseDto<RoomAlocationDto> GetRoomAllocation(RoomAllocationRequest request)
        {
            Validate(request);
            if (request.PersonsNumber <= 0) throw new ArgumentException(nameof(request.PersonsNumber));

            try
            {
                var hotel = _hotelRepository.GetHotel(request.HotelId);
                var roomTypesWithCountDictionary = GetAvailableRoomTypes(hotel, request);

                // check if request contains more people than what the available rooms can accomodate 
                if (request.PersonsNumber > roomTypesWithCountDictionary.Sum(r => r.Key.Size * r.Value))
                    throw new Exception("There are no available rooms for the request.");

                var result = new RoomAlocationDto
                {
                    HotelId = request.HotelId,
                    RoomTypes = AllocatePeopleInRooms(roomTypesWithCountDictionary, request.PersonsNumber)
                };

                return ResponseDto<RoomAlocationDto>.Success(result);
            }
            catch (Exception ex) 
            {
                // log error
                return ResponseDto<RoomAlocationDto>.Error(ex.Message);
            }
        }

        /// <summary>
        /// Get the available room types and their number 
        /// </summary>
        /// <param name="hotel"></param>
        /// <param name="request"></param>
        /// <returns>Dictionary RoomType as Key and no of rooms of that type available in the specified time interval as Value</returns>
        private Dictionary<RoomType, int> GetAvailableRoomTypes(Hotel hotel, RoomAllocationRequest request)
        {
            var availableRooms = hotel.Rooms.GroupBy(r => hotel.RoomTypes.First(rt => rt.Code == r.RoomType))
                .OrderByDescending(g => g.Key.Size)
                .ToDictionary(g => g.Key, g => GetNoOfRoomsAvailableInternal(new RoomAvailabilityRequest(hotel.Id, request.StartDate, request.EndDate, g.Key.Code)))
                .Where(i => i.Value > 0)
                .ToDictionary(i => i.Key, i => i.Value);

            return availableRooms;
        }

        private List<string> AllocatePeopleInRooms(Dictionary<RoomType, int> roomTypesWithCountDictionary, int personsLeftToAccomodate)
        {
            var roomTypeAllocations = new List<string>();

            // itterate through rooms and subtract the no of persons it can accomodate
            foreach (var roomType in roomTypesWithCountDictionary.Keys)
            {
                while (roomTypesWithCountDictionary[roomType] > 0 && personsLeftToAccomodate > 0)
                {
                    bool isPartialOccupy = personsLeftToAccomodate < roomType.Size;
                    if (isPartialOccupy)
                    {
                        // filter out this type of room since we try to avoid partial filling
                        var roomsExcludingCurrentOnesType = roomTypesWithCountDictionary.Where(r => r.Key.Code != roomType.Code)
                            .ToDictionary(r => r.Key, r => r.Value);

                        var allocationsExcudingCurrentRoomType = AllocatePeopleInRooms(roomsExcludingCurrentOnesType, personsLeftToAccomodate);
                        if (allocationsExcudingCurrentRoomType != null)
                        {
                            roomTypeAllocations.AddRange(allocationsExcudingCurrentRoomType);
                            return roomTypeAllocations;
                        }
                    }

                    roomTypeAllocations.Add($"{roomType.Code}{(isPartialOccupy ? "!" : string.Empty)}");
                    personsLeftToAccomodate -= roomType.Size;
                    roomTypesWithCountDictionary[roomType]--;
                }
            }

            if (personsLeftToAccomodate > 0)
            {
                // cannot allocate all persons
                return null;
            }

            return roomTypeAllocations;
        }
    }
}
