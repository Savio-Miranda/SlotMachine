using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rules : MonoBehaviour
{   
    public Web request;
    private List<List<int>> receivedMatrix = new List<List<int>>();

    // Update is called once per frame
    void Update()
    {
        int lineIndex = 0;
        int firstIndex = 0;
        receivedMatrix = request.GetResults();
        foreach (Transform child in transform)
        {
            // Changes line and resets the Index Line
            if (firstIndex > receivedMatrix[lineIndex].Count - 1)
            {
                lineIndex++;
                firstIndex = 0;
            }
            
            // Verify if the sequence is valid;
            child.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{receivedMatrix[lineIndex][firstIndex]}");

            firstIndex++;
        }
    }
}