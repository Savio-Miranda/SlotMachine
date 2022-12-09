using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenSlotManager : MonoBehaviour
{
    public float time;
    public UIDocument document;
    private VisualElement screen;
    private Label rewards;
    private Button enableButton;
    private Button disableButton;
    private Button playButton;
    private Button stopButton;
    public bool setBool = false;
    private List<VisualElement> spriteList = new List<VisualElement>();
    private List<List<VisualElement>> columnsList = new List<List<VisualElement>>();
    public Sprite[] sprites;

    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites");

        screen = document.rootVisualElement.Q<VisualElement>("screen");
        rewards = document.rootVisualElement.Q<Label>("reward");
        enableButton = document.rootVisualElement.Q<Button>("enable-button");
        disableButton = document.rootVisualElement.Q<Button>("disable-button");
        playButton = document.rootVisualElement.Q<Button>("play-button");
        stopButton = document.rootVisualElement.Q<Button>("stop-button");
        
        // registered functions
        playButton.RegisterCallback<ClickEvent>(EnableOff);
        stopButton.RegisterCallback<ClickEvent>(EnableOn);
    }
    
    void Update()
    {
        foreach (VisualElement column in screen.Children())
        {        
            foreach (VisualElement image in column.Children())
            {
                image.style.backgroundImage = new StyleBackground(sprites[UnityEngine.Random.Range(0, sprites.Length)]);
            }
        }
    }
    
    private void RandomMatrix()
    {
        StartCoroutine(MatrixRoutines(Web.GetRandomMatrixRoutine()));
    }

    public void OrdenedMatrix()
    {
        StartCoroutine(MatrixRoutines(Web.GetOrdenedMatrixRoutine()));
    }

    public void Rewards()
    {
        StartCoroutine(RewardRoutine());
    }

    IEnumerator MatrixRoutines(IEnumerator routine)
    {        
        List<List<int>> matrix = null;
        yield return routine;
        matrix = Web.GetResults();

        // Making a list of columns with dinamic amount of images in every column.
        // Futurely, if you want to change the number of images per column, you won't have to worry
        // about updating values throut the code.
        int matrixColumns = matrix.Count;
        columnsList.Add(ColumnBuilder(matrix[matrixColumns - 1].Count));

        int columnIndex = 0;

        foreach (VisualElement column in screen.Children())
        {
            // Raising error (warning when backend not running)
            if (matrix.Count <= 0)
                throw new NullReferenceException("receivedMatrix is null. Server might be offline");

            int imageIndex = 0; // Whenever column changes to the next, the imageIndex resets

            foreach (VisualElement image in column.Children())
            {   
                image.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>($"Sprites/{matrix[columnIndex][imageIndex]}"));
                imageIndex ++; // Changing image inside column
            }

            columnIndex ++; // Changing column inside screen
        }
    }

    // Shows the player the reward of every play
    IEnumerator RewardRoutine()
    {   
        int points = 0;
        
        yield return Web.GetRewardRoutine();
        List<Dictionary<int, List<int>>> wins = Web.GetRewards();
    
        if (wins.Count == 0)
        {
            points += 0;
        }
        else
        {
            for (int i = 0; i < wins.Count; i++)
            {
                for (int j = 0; j < wins[i].Count; j++)
                {
                    points += wins[i].ElementAt(j).Value.Count;
                }
            }

        }
        
        rewards.text = $"Points: {points.ToString()}";
    }

    // Creates the columns
    private List<VisualElement> ColumnBuilder(int nImage)
    {
        List<VisualElement> column = new List<VisualElement>();
        foreach (VisualElement screenColumn in screen.Children())
        {
            int numberOfImages = screenColumn.childCount;
            for (int i = nImage; i < numberOfImages; i++)
            {
                column.Add(screenColumn.ElementAt(i));
            }
        }

        return column;
    }

    public void ButtonClicked(Button disable, Button enable)
    {
        if (disable != null)
        {
            disable.clickable.clicked += () => {
                disable.style.display = DisplayStyle.None;
                enable.style.display = DisplayStyle.Flex;
                setBool = true;
            };
        }

        if(enable != null)
        {
            enable.clickable.clicked += () => {
                disable.style.display = DisplayStyle.Flex;
                enable.style.display = DisplayStyle.None;
                setBool = false;
            };
        }
    }

     public void EnableOn(ClickEvent evt)
    {
        spriteList.Clear();
        Web.GetResults();

        stopButton.style.display = DisplayStyle.None;
        playButton.style.display = DisplayStyle.Flex;
        
        enabled = true;
    }

    public void EnableOff(ClickEvent evt)
    {
        ButtonClicked(disableButton, enableButton);
        if (setBool)
        {
            Invoke("OrdenedMatrix", time);
        }
        else
        {
            Invoke("RandomMatrix", time);
        }
        
        Invoke("Rewards", time + 0.5f);

        stopButton.style.display = DisplayStyle.Flex;
        playButton.style.display = DisplayStyle.None;
        
        enabled = false;
    }
}
