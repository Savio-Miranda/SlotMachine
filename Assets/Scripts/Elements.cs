using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Elements : MonoBehaviour
{
    public GameObject play;
    public GameObject pattern;
    private List<int> spriteList = new List<int>();
    public GameObject[] drops;
    public Sprite[] newSprites;
    int[] numbersRand = {1, 2, 3, 4, 5, 6, 7, 8, 9};
    
    // SlotNum se refere as caixinhas de texto do canvas;
    public float time;

    void Update()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Image>().sprite = newSprites[Random.Range (0, numbersRand.Length)];
        }
    }
    
    public void Pattern()
    {
        bool isPatternActive = pattern.GetComponent<Button>().IsActive();
        
        foreach (GameObject dropdown in drops)
        {
            int dropdownValue = dropdown.GetComponent<TMP_Dropdown>().value;
            if (dropdownValue == 0)
            {
                return;
            }
            else
            {
                spriteList.Add(dropdownValue);
            }
        }

        if (isPatternActive)
        {
            transform.GetChild(6).GetComponent<Image>().sprite = newSprites[spriteList[0] - 1];
            transform.GetChild(7).GetComponent<Image>().sprite = newSprites[spriteList[1] - 1];
            transform.GetChild(8).GetComponent<Image>().sprite = newSprites[spriteList[2] - 1];
        }
        else
        {
            return;
        }
    }

    public void EnableOn()
    {
        enabled = true;
        spriteList.Clear();
    }
    public void EnableOff()
    {
        Invoke ("Pattern", time);
        enabled = false;
    }
}
