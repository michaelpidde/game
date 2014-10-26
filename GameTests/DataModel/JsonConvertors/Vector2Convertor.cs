using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Xna.Framework;

namespace GameTests.DataModel {
    /// <summary>
    /// Gleaned from:
    /// http://stackoverflow.com/questions/10200047/deserialize-json-data-generated-by-different-json-net-lib
    /// </summary>
    class Vector2Convertor : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return (objectType == typeof(Vector2));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            JObject jsonObject = JObject.Load(reader);
            var properties = jsonObject.Properties().ToList();
            return new Vector2((float)properties[0].Value, (float)properties[1].Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            Vector2 v = (Vector2)value;
            writer.WriteStartObject();
            writer.WritePropertyName("X");
            serializer.Serialize(writer, v.X);
            writer.WritePropertyName("Y");
            serializer.Serialize(writer, v.Y);
            writer.WriteEndObject();
        }
    }
}
