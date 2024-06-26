using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Serialization.Json;
using UnityEngine;
using UnityEngine.Networking;

public class ApiHelper 
{
    public static IEnumerator Get(string url, Dictionary<string, string> paramenters, Action<string> onSuccess, Action<Exception> onFailure)
    {


        UnityWebRequest request = UnityWebRequest.Get(url);
        foreach(KeyValuePair<string,string> parameter in paramenters)
        {
            request.SetRequestHeader(parameter.Key, parameter.Value);
        }

        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            onFailure(new Exception(request.error));
            yield break;
        }

        string Text = request.downloadHandler.text;
        onSuccess(Text);
    }  


    public static IEnumerator Get<T>(string url,
        Dictionary<string, string> paramenters, 
        Action<T> onSuccess, 
        Action<Exception> onFailure)
    {
        return Get(url, paramenters, jsonText =>
        {
            try
            {
                T result = JsonSerialization.FromJson<T>(jsonText);
                //T result = JsonUtility.FromJson<T>(jsonText);
                onSuccess(result);
            }
            catch (Exception e)
            {
                onFailure(e);
            }


        }, onFailure);
    }

    public static IEnumerator GetTexture(string url, Action<Texture> onSuccess, Action<Exception> onFailure)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            onFailure(new Exception(request.error));
            yield break;
        }
        else
        {

            DownloadHandlerTexture handel = (DownloadHandlerTexture)request.downloadHandler;
            onSuccess(handel.texture);
        }
    }
}
