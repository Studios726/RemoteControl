using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class JsonMgr
{
    public static JsonSerializerSettings settings;
    static JsonMgr()
    {
        settings = new JsonSerializerSettings();
        settings.DefaultValueHandling = DefaultValueHandling.Include;
    }
    public static string Serialize<T>(T t)
    {
        return JsonConvert.SerializeObject(t,settings);
    }
 
    public static T DeSerialize <T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}
