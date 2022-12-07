using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Web : MonoBehaviour
{
    private const string URL = "https://server-slot-machine.saviomiranda.repl.co/";
    private static List<List<int>> matrix = new List<List<int>>();

    public static List<List<int>> GetResults()
    {
        return matrix;
    }

    public static IEnumerator GetOrdenedMatrixRoutine()
    {
        yield return GetRequest(URL);
    }

    public static IEnumerator GetRandomMatrixRoutine()
    {
        yield return GetRequest($"{URL}/matrix");
    }

    public static IEnumerator GetRewardRoutine()
    {
        yield return GetRequest($"{URL}/reward");
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
                    var receivedList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<int>>>(webRequest.downloadHandler.text);
                    matrix = receivedList;
                    break;
            }
        }
    }
}