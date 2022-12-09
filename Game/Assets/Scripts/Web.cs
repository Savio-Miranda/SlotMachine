using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Web : MonoBehaviour
{
    private const string URL = "https://Server-Slot-Machine.saviomiranda.repl.co";
    //private const string URL = "http://127.0.0.1:5000";

    private static List<List<int>> matrix = new List<List<int>>();

    public static List<Dictionary<int, List<int>>> win = new List<Dictionary<int, List<int>>>();

    public static List<List<int>> GetResults()
    {
        return matrix;
    }

    public static List<Dictionary<int, List<int>>> GetRewards()
    {
        return win;
    }

    public static IEnumerator GetOrdenedMatrixRoutine()
    {
        yield return GetRequest($"{URL}/ordened");
    }

    public static IEnumerator GetRandomMatrixRoutine()
    {
        yield return GetRequest($"{URL}/random");
    }

    public static IEnumerator GetRewardRoutine()
    {
        yield return GetRequest($"{URL}/rewards");
    }

    public static IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    
                    if (uri == $"{URL}/rewards")
                    {
                        win = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<int, List<int>>>>(webRequest.downloadHandler.text);
                    }
                    else
                    {
                        matrix = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<int>>>(webRequest.downloadHandler.text);
                    }

                    break;
                    
            }
        }
    }    
}