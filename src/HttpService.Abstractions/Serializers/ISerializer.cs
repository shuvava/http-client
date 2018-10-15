using System;


namespace HttpService.Abstractions.Serializers
{
    public interface ISerializer
    {
        TContent Deserialize<TContent>(string content);
        object Deserialize(string content, Type type);
        string Serialize<TContent>(object content, bool pretty = false);
        string Serialize(object content, Type type, bool pretty = false);
    }
}
