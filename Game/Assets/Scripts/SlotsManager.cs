using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SlotsManager : MonoBehaviour
{
    public UIDocument document;
    public Sprite[] sprites;
    private CoroutineStarter coroutineStarter;
    private VisualElement screen;
    private IntegerField scores;
    private IntegerField credits;
    private DropdownField betField;
    private Button backMenuButton;
    private Button playButton;
    private Button patternButton;
    private bool patternBool = false;
    private List<VisualElement> spriteList = new List<VisualElement>();
    private List<List<VisualElement>> columnsList = new List<List<VisualElement>>();

    //_______________________________________________________________________ UNITY PATTERN AREA
    void Start()
    {
        coroutineStarter = FindObjectOfType<CoroutineStarter>();
        sprites = Resources.LoadAll<Sprite>("Sprites");
        betField = document.rootVisualElement.Q<DropdownField>("bet");
        SetBetList();
        SetSlotData();

        // Screen objects
        screen = document.rootVisualElement.Q<VisualElement>("screen");
        scores = document.rootVisualElement.Q<IntegerField>("points");
        credits = document.rootVisualElement.Q<IntegerField>("credits");

        // Start buttons
        backMenuButton = document.rootVisualElement.Q<Button>("back-button");
        playButton = document.rootVisualElement.Q<Button>("play-button");
        patternButton = document.rootVisualElement.Q<Button>("pattern-button");
        
        // registered callbacks
        playButton.RegisterCallback<ClickEvent>(Enable);
        patternButton.RegisterCallback<ClickEvent>(Pattern);
        backMenuButton.RegisterCallback<ClickEvent>(Menu);

        GameOver.OnReturningToMenu += TestGameOver;
    }
    //_______________________________________________________________________ UNITY PATTERN AREA

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
    //_______________________________________________________________________ UNITY PATTERN AREA

    public void SetBetList()
    {
        coroutineStarter.StartCoroutine(BetListRoutine());
    }

    IEnumerator BetListRoutine()
    {
        yield return Web.GetBetRoutine();
        betField.choices = Web.GetBetList();
        betField.value = betField.choices[0];
    }

    public void SendCurrentBet(string bet)
    {
        coroutineStarter.StartCoroutine(Web.PutBetRoutine(bet));
    }
    //_______________________________________________________________________ ROUTINE AREA

    private void TestGameOver()
    {
        coroutineStarter.StartCoroutine(DataRoutine(Web.GetMenuRoutine()));
    }
    private void SetSlotData()
    {
        coroutineStarter.StartCoroutine(DataRoutine(Web.GetStartMatrixRoutine()));
    }

    private void RandomMatrix()
    {
        coroutineStarter.StartCoroutine(DataRoutine(Web.GetRandomMatrixRoutine()));
    }

    public void OrderedMatrix()
    {
        coroutineStarter.StartCoroutine(DataRoutine(Web.GetOrderedMatrixRoutine()));
    }

    IEnumerator DataRoutine(IEnumerator routine)
    {        
        List<List<int>> matrix = null;
        yield return routine;
        
        if (Web.data.credits <= 5) { SceneManager.LoadScene("GameOverScene"); }
        credits.value = Web.data.credits;
        scores.value = Web.data.scores;
        matrix = Web.data.matrix;

        // Making a list of columns with dinamic amount of images in every column.
        // Futurely, if you want to change the number of images per column, you won't have to worry
        // about updating values throughout the code.
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
                image.style.backgroundImage = new StyleBackground(sprites[matrix[columnIndex][imageIndex]]);//new StyleBackground(Resources.Load<Sprite>($"Sprites/Icons/{matrix[columnIndex][imageIndex]}"));
                imageIndex ++; // Changing image inside column
            }

            columnIndex ++; // Changing column inside screen
        }
    }
    
    private List<VisualElement> ColumnBuilder(int nImage) // Creates the columns
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
    //_______________________________________________________________________ ROUTINE AREA

    public void Pattern(ClickEvent evt)
    {
        if (patternBool)
        {
            patternBool = false;
            StyleSheetModifier(patternButton, "Enable", "enable-button-activated", "enable-button");
            return;
        }

        patternBool = true;
        StyleSheetModifier(patternButton, "Disable", "enable-button", "enable-button-activated");
        
    }
    //_______________________________________________________________________ EVENT AREA

    public void Menu(ClickEvent evt)
    {
        coroutineStarter.StartCoroutine(Web.GetMenuRoutine());
        SceneManager.LoadScene("MenuScene");
    }
    //_______________________________________________________________________ EVENT AREA

    public void Enable(ClickEvent evt)
    {
        if (enabled)
        {
            SendCurrentBet(betField.value);
            
            if (patternBool)
                Invoke("OrderedMatrix", 2f);
            else
            {
                Invoke("RandomMatrix", 2f);
            }

            spriteList.Clear();
            StyleSheetModifier(playButton, "Play", "play-button", "play-button-activated");
            enabled = false;
        }
        else
        {
            StyleSheetModifier(playButton, "Stop", "play-button-activated", "play-button");
            enabled = true;
        }
    }
    
    void StyleSheetModifier(Button button, string text, string removeClassList, string addClassList) // Changes style sheets
    {
        button.text = text;
        button.RemoveFromClassList(removeClassList);
        button.AddToClassList(addClassList);
    }
    //_______________________________________________________________________ EVENT AREA

}