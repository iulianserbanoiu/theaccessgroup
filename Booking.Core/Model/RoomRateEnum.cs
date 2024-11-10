using System.Text.Json.Serialization;

namespace Booking.Core.Model
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoomRateEnum
    {
        Prepaid,
        Standard
    }
}
