using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Web : MonoBehaviour
{
    //private const string URL = "https://Server-Slot-Machine.saviomiranda.repl.co";
    private const string URL = "http://127.0.0.1:5000";

    private static List<string> betList = new List<string>();
    private static List<List<int>> matrix = new List<List<int>>();
    private static int roundPoints;

    public static List<string> GetBetList()
    {
        return betList;
    }

    public static List<List<int>> GetResults()
    {
        return matrix;
    }

    public static int GetRewards()
    {
        return roundPoints;
    }

    public static IEnumerator GetOrderedMatrixRoutine()
    {
        yield return GetRequest($"{URL}/ordered");
    }

    public static IEnumerator GetRandomMatrixRoutine()
    {
        yield return GetRequest($"{URL}/random");
    }

    public static IEnumerator GetRewardRoutine()
    {
        yield return GetRequest($"{URL}/rewards");
    }

    public static IEnumerator GetBetRoutine()
    {
        yield return GetRequest($"{URL}/betlist");
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
                    var jsonObject = webRequest.downloadHandler.text;

                    if (uri == $"{URL}/betlist")
                        betList = JsonConvert.DeserializeObject<List<string>>(jsonObject);
                    
                    else if (uri == $"{URL}/rewards" || uri == $"{URL}/menu")
                        roundPoints = JsonConvert.DeserializeObject<int>(jsonObject);  
                    
                    else
                    {
                        matrix = JsonConvert.DeserializeObject<List<List<int>>>(jsonObject);
                    }

                    break;                  
            }   
        }
    }
}