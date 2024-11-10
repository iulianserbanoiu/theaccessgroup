using System.Text.Json.Serialization;

namespace Booking.Core.Model
{
    [JsonConverter(typeof(JsonStringEnumConverter<RoomTypeEnum>))]
    public enum RoomTypeEnum
    {
        SGL,
        DBL
    }
}
