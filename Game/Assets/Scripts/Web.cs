using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Web : MonoBehaviour
{
    private const string URL = "https://Server-Slot-Machine.saviomiranda.repl.co";
    //private const string URL = "http://127.0.0.1:5000";
    public static SlotData data = new SlotData();
    
    public static bool menu = false;
    private static List<string> betList = new List<string>();
    public static List<string> GetBetList() { return betList; }
    public static IEnumerator GetBetRoutine() { yield return GetRequest($"{URL}/betlist"); }
    public static IEnumerator PutBetRoutine(string bet) { yield return PutRequest($"{URL}/bet", bet); }
    public static IEnumerator GetStartMatrixRoutine() { yield return GetRequest($"{URL}/start"); }
    public static IEnumerator GetOrderedMatrixRoutine() { yield return GetRequest($"{URL}/ordered"); }
    public static IEnumerator GetRandomMatrixRoutine() { yield return GetRequest($"{URL}/random"); }
    public static IEnumerator GetMenuRoutine() { yield return GetRequest($"{URL}/gameover"); }

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
                    
                    else
                    {
                        data = JsonConvert.DeserializeObject<SlotData>(jsonObject);
                    }

                    break;                  
            }   
        }
    }

    public static IEnumerator PutRequest(string uri, string bet)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(uri, bet))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                Debug.Log("Upload complete!");
            }
        }
    }
}