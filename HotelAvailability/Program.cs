using Booking.Application.Requests;
using Booking.Application.Services;
using Booking.Core.Model;
using Booking.Core.Repositories;
using Booking.Infra.Persistance;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBookingApp
{
    internal class Program
    {
        private static ServiceCollection container;

        static void Main(string[] args)
        {
            if (args.Length != 4 || args[0] != "--hotels" || args[2] != "--bookings")
            {
                Console.WriteLine("Usage: myapp --hotels hotels.json --bookings bookings.json");
                return;
            }

            RegisterTypes(args);

            string input;
            while (!string.IsNullOrWhiteSpace(input = Console.ReadLine()))
            {
                try
                {
                    if (input.StartsWith("Availability"))
                        ProcessAvailabilityCommand(input);
                    else if (input.StartsWith("RoomTypes"))
                        ProcessRoomTypesCommand(input);
                    else
                        Console.WriteLine("Invalid command");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private static void RegisterTypes(string[] args)
        {
            container = new();

            container.AddSingleton<IDataStore<Hotel>>(new DataStore<Hotel>(args[1]));
            container.AddSingleton<IDataStore<Booking.Core.Model.Booking>>(new DataStore<Booking.Core.Model.Booking>(args[3]));

            container.AddTransient<IHotelRepository, HotelRepository>();
            container.AddTransient<IBookingRepository, BookingRepository>();

            container.AddTransient<IBookingService, BookingService>();
        }

        // Command Handlers
        private static void ProcessAvailabilityCommand(string command)
        {
            var sp = container.BuildServiceProvider();
            var bookingService = sp.GetService<IBookingService>();

            var result = bookingService.GetNoOfRoomsAvailable(GetRoomAvailabilityRequest(command));
            if (result.Succeeded)
            {
                Console.WriteLine(result.Result);
            }
            else {
                Console.WriteLine($"Error: {result.Message}");
            }
        }

        private static void ProcessRoomTypesCommand(string command)
        {
            var sp = container.BuildServiceProvider();
            var bookingService = sp.GetService<IBookingService>();

            var result = bookingService.GetRoomAllocation(GetRoomAllocationRequest(command));
            if (result.Succeeded)
            {
                Console.WriteLine($"{result.Result.HotelId}: {string.Join(", ", result.Result.RoomTypes)}");
            }
            else
            {
                Console.WriteLine($"Error: {result.Message}");
            }
        }

        private static RoomAvailabilityRequest GetRoomAvailabilityRequest(string command)
        {
            var args = command.Replace("Availability(", "").Replace(")", "").Split(',');
            string hotelId = args[0].Trim();
            string dateArg = args[1].Trim();
            var roomTypeCode = Enum.Parse<RoomTypeEnum>(args[2].Trim());

            var (startDate, endDate) = ExtractDateInterval(dateArg);

            return new RoomAvailabilityRequest(hotelId, startDate, endDate, roomTypeCode);
        }

        private static RoomAllocationRequest GetRoomAllocationRequest(string command)
        {
            var args = command.Replace("RoomTypes(", "").Replace(")", "").Split(',');
            string hotelId = args[0].Trim();
            string dateArg = args[1].Trim();
            int personsNumber = int.Parse(args[2].Trim());
            var (startDate, endDate) = ExtractDateInterval(dateArg);

            return new RoomAllocationRequest(hotelId, startDate, endDate, personsNumber);
        }

        private static Tuple<DateOnly, DateOnly?> ExtractDateInterval(string dateArg)
        {
            DateOnly startDate;
            DateOnly? endDate = null;

            if (dateArg.Contains('-'))
            {
                var dates = dateArg.Split('-');
                startDate = DateOnly.ParseExact(dates[0], "yyyyMMdd", null);
                endDate = DateOnly.ParseExact(dates[1], "yyyyMMdd", null);
            }
            else
            {
                startDate = DateOnly.ParseExact(dateArg, "yyyyMMdd", null);
            }

            return new(startDate, endDate);
        }
    }
}
