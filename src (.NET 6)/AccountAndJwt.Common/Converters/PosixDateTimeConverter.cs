using AccountAndJwt.Common.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AccountAndJwt.Common.Converters
{
    public class PosixDateTimeConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToPosixTimeMs());
        }
        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value;
            var posixTimeStamp = Int64.Parse(value.ToString());
            var time = posixTimeStamp.FromPosixTimeMs();
            return time;
        }
    }
}