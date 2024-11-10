using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Booking.Infra.Converters
{
    internal class CustomDateJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                DateOnly.ParseExact(reader.GetString()!,
                    "yyyyMMdd", CultureInfo.InvariantCulture);

        public override void Write(
            Utf8JsonWriter writer,
            DateOnly dateValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(dateValue.ToString(
                    "yyyyMMdd", CultureInfo.InvariantCulture));
    }
}
