using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Elements : MonoBehaviour
{
    public float time;
    public UIDocument document;
    private VisualElement screen;
    private Button enableButton;
    private Button disableButton;
    private Button playButton;
    private Button stopButton;
    public bool setBool = false;
    private List<VisualElement> spriteList = new List<VisualElement>();
    private List<List<VisualElement>> columnsList = new List<List<VisualElement>>();
    private int[] numberOfElements = {1, 2, 3, 4, 5, 6, 7, 8, 9};

    void Start()
    {
        screen = document.rootVisualElement.Q<VisualElement>("screen");
        enableButton = document.rootVisualElement.Q<Button>("enable-button");
        disableButton = document.rootVisualElement.Q<Button>("disable-button");
        playButton = document.rootVisualElement.Q<Button>("play-button");
        stopButton = document.rootVisualElement.Q<Button>("stop-button");
        
        // registered functions
        playButton.RegisterCallback<ClickEvent>(EnableOff);
        stopButton.RegisterCallback<ClickEvent>(EnableOn);

        //enableButton.RegisterCallback<ClickEvent>(ButtonClicked);
        //disableButton.RegisterCallback<ClickEvent>(ButtonClicked);
    }
    
    void Update()
    {
        foreach (VisualElement Image in screen.Children())
        {
            Image.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>($"Sprites/{Random.Range (0, numberOfElements.Length)}"));
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
            columnsList.Add(ColumnBuilder(counter));
            counter += 3;
        }
        int firstIndex = 0;
        
        yield return Web.GetElementRoutine();
        
        foreach (List<VisualElement> column in columnsList)
        {

            if (firstIndex >= Web.GetResults().Count)
            {
                yield break;
            }
            int secondIndex = 0;
            
            foreach (VisualElement image in column)
            {
                if (secondIndex >= Web.GetResults()[firstIndex].Count)
                {
                    secondIndex = 0;
                }

                //image.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{request.GetResults()[firstIndex][secondIndex]}"); //allSprites[request.GetResults()[firstIndex][secondIndex]];
                image.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>($"Sprites/{Web.GetResults()[firstIndex][secondIndex]}"));
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
        
        yield return Web.GetMatrixRoutine();

        List<List<int>> receivedMatrix = Web.GetResults();

        foreach (VisualElement child in screen.Children())
        {
            // Changes line and resets the Index Line
            if (firstIndex > receivedMatrix[lineIndex].Count - 1)
            {
                lineIndex++;
                firstIndex = 0;
            }
            
            // Adds the matrix sprite to the image;
            //child.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{receivedMatrix[lineIndex][firstIndex]}");
            child.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>($"Sprites/{receivedMatrix[lineIndex][firstIndex]}"));


            firstIndex++;
        }
    }

    public void Reward()
    {
        StartCoroutine(RewardRoutine());
    }

    IEnumerator RewardRoutine()
    {   
        yield return Web.GetRewardRoutine();

        List<List<int>> receivedWins = Web.GetResults();

        // Shows the player the reward of every play

    }

    // Creates the columns
    private List<VisualElement> ColumnBuilder(int step)
    {
        List<VisualElement> column = new List<VisualElement>();
        int childCount = screen.childCount;
        
        for (int i = step; i < childCount; i++)
        {
            column.Add(screen.ElementAt(i));
        }
        return column;
    }

     public void EnableOn(ClickEvent evt)
    {
        spriteList.Clear();
        Web.GetResults();

        // disableButton.style.display = DisplayStyle.None;
        // enableButton.style.display = DisplayStyle.Flex;

        stopButton.style.display = DisplayStyle.None;
        playButton.style.display = DisplayStyle.Flex;
        
        enabled = true;
    }

    public void EnableOff(ClickEvent evt)
    {
        ButtonClicked(disableButton, enableButton);
        if (setBool)
        {
            Invoke("Pattern", time);
        }
        else
        {
            Invoke("Matrix", time);    
        }
        
        // disableButton.style.display = DisplayStyle.None;
        // enableButton.style.display = DisplayStyle.Flex;

        stopButton.style.display = DisplayStyle.Flex;
        playButton.style.display = DisplayStyle.None;
        
        enabled = false;
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

}
