using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Elements : MonoBehaviour
{
    public float time;
    public GameObject PatternButton;
    private List<int> spriteList = new List<int>();
    private List<List<Transform>> columnsList = new List<List<Transform>>();
    private int[] numberOfElements = {1, 2, 3, 4, 5, 6, 7, 8, 9};
    public Web request;
    
    void Update()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{Random.Range (0, numberOfElements.Length)}");
        }
    }
    
    // Pre-established pattern
    public void Pattern()
    {
        StartCoroutine(PatternRoutine());
    }

    IEnumerator PatternRoutine()
    {        
        int counter = 0;
        // Receiving columns
        for (int i = 0; i < 5; i++)
        {
            columnsList.Add(ColumnBuilder(transform, counter));
            counter += 3;
        }
        int firstIndex = 0;
        
        yield return request.GetElementRoutine();
        
        foreach (List<Transform> column in columnsList)
        {

            if (firstIndex >= request.GetResults().Count)
            {
                yield break;
            }
            int secondIndex = 0;
            
            foreach (Transform image in column)
            {
                if (secondIndex >= request.GetResults()[firstIndex].Count)
                {
                    secondIndex = 0;
                }

                image.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{request.GetResults()[firstIndex][secondIndex]}"); //allSprites[request.GetResults()[firstIndex][secondIndex]];
                secondIndex++;
            }
            firstIndex++;
        }
        yield return null;
    }

    // Receives the matrix pattern from server
    private void Matrix()
    {
        StartCoroutine(MatrixRoutine());
    }

    IEnumerator MatrixRoutine()
    {
        int firstIndex = 0;
        int lineIndex = 0;
        
        yield return request.GetMatrixRoutine();

        List<List<int>> receivedMatrix = request.GetResults();

        foreach (Transform child in transform)
        {
            // Changes line and resets the Index Line
            if (firstIndex > receivedMatrix[lineIndex].Count - 1)
            {
                lineIndex++;
                firstIndex = 0;
            }
            
            // Adds the matrix sprite to the image;
            child.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{receivedMatrix[lineIndex][firstIndex]}");

            firstIndex++;
        }
    }

    // Creates the columns
    private List<Transform> ColumnBuilder(Transform element, int step)
    {
        List<Transform> column = new List<Transform>();
        int childCount = element.childCount;
        
        for (int i = step; i < childCount; i++)
        {
            column.Add(element.GetChild(i));
        }
        return column;
    }

    public void EnableOn()
    {
        enabled = true;
        spriteList.Clear();
        request.GetResults();
    }

    public void EnableOff()
    {
        bool isPatternActive = PatternButton.GetComponent<Button>().IsActive();
        if (isPatternActive)
        {
            Invoke("Pattern", time);
        }
        else
        {
            Invoke("Matrix", time);    
        }
    
        enabled = false;
    }
}
