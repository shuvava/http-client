using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using HttpService.Abstractions.Serializers;


namespace HttpService.Serializers
{
    public class JsonRestSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings;


        public JsonRestSerializer(JsonSerializerSettings settings = null)
        {
            _settings = settings;

            if (_settings == null)
            {
                _settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                _settings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
            }
        }


        public TContent Deserialize<TContent>(string content)
        {
            return JsonConvert.DeserializeObject<TContent>(content);
        }


        public object Deserialize(string content, Type type)
        {
            if (type == typeof(string))
            {
                return content;
            }
            return JsonConvert.DeserializeObject(content, type);
        }


        public string Serialize<TContent>(object content, bool pretty = false)
        {
            return Serialize(content, typeof(TContent), pretty);
        }


        public string Serialize(object content, Type type, bool pretty = false)
        {
            if (type == typeof(string))
            {
                return (string)content;
            }
            _settings.Formatting = pretty ? Formatting.Indented : Formatting.None;

            return JsonConvert.SerializeObject(content, type, _settings);
        }
    }
}
