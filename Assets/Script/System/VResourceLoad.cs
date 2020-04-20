using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class VResourceLoad
{
    static Dictionary<string, Object> resourceCache = new Dictionary<string, Object>();
    public static T Load<T>(string path) where T : Object
    {
        if (!resourceCache.ContainsKey(path))
            resourceCache[path] = Resources.Load<T>(path);
        return (T)resourceCache[path];
    }

    public static T DeepCopy<T>(T obj)
    {
        using (var stream = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Position = 0;

            return (T)formatter.Deserialize(stream);
        }
    }
}