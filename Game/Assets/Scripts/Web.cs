using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Web : MonoBehaviour
{
    private static List<List<int>> matrix = new List<List<int>>();

    public static List<List<int>> GetResults()
    {
        return matrix;
    }

    public static IEnumerator GetElementRoutine()
    {
        yield return GetRequest("http://127.0.0.1:5000");
    }

    public static IEnumerator GetMatrixRoutine()
    {
        yield return GetRequest("http://127.0.0.1:5000/matrix");
    }

    public static IEnumerator GetRewardRoutine()
    {
        yield return GetRequest("http://127.0.0.1:5000/reward");
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