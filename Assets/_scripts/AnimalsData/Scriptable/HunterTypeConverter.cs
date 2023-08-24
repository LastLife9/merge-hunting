//using UnityEngine;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

//public class HunterTypeConverter : JsonConverter
//{
//    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//    {
//        if (value is HunterType)
//        {
//            HunterType hunterType = (HunterType)value;
//            writer.WriteValue(hunterType.ToString());
//        }
//    }

//    public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.String)
//        {
//            string value = (string)reader.Value;
//            if (System.Enum.TryParse(value, out HunterType hunterType))
//            {
//                return hunterType;
//            }
//        }
//        return null;
//    }

//    public override bool CanConvert(System.Type objectType)
//    {
//        return objectType == typeof(HunterType);
//    }
//}
