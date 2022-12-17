using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SlotsManager : MonoBehaviour
{
    public UIDocument document;
    public GameObject menu;
    public Sprite[] sprites;
    private VisualElement screen;
    private IntegerField points;
    private IntegerField credits;
    private DropdownField bet;
    private Button backMenuButton;
    private Button playButton;
    private Button patternButton;
    private bool patternBool = false;
    private List<VisualElement> spriteList = new List<VisualElement>();
    private List<List<VisualElement>> columnsList = new List<List<VisualElement>>();

    //_______________________________________________________________________ UNITY PATTERN AREA
    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites");
        bet = document.rootVisualElement.Q<DropdownField>("bet");
        BetList();

        // Screen objects
        screen = document.rootVisualElement.Q<VisualElement>("screen");
        points = document.rootVisualElement.Q<IntegerField>("points");
        credits = document.rootVisualElement.Q<IntegerField>("credits");

        // Start buttons
        backMenuButton = document.rootVisualElement.Q<Button>("back-button");
        playButton = document.rootVisualElement.Q<Button>("play-button");
        patternButton = document.rootVisualElement.Q<Button>("pattern-button");
        
        // registered callbacks
        playButton.RegisterCallback<ClickEvent>(Enable);
        patternButton.RegisterCallback<ClickEvent>(Pattern);
        backMenuButton.RegisterCallback<ClickEvent>(Menu);
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

    public void BetList()
    {
        StartCoroutine(BetListRoutine());
    }

    IEnumerator BetListRoutine()
    {
        yield return Web.GetBetRoutine();
        bet.choices = Web.GetBetList();
        bet.value = bet.choices[0];
    }
    //_______________________________________________________________________ ROUTINE AREA

    private void RandomMatrix()
    {
        StartCoroutine(MatrixRoutines(Web.GetRandomMatrixRoutine()));
    }

    public void OrderedMatrix()
    {
        StartCoroutine(MatrixRoutines(Web.GetOrderedMatrixRoutine()));
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

    public void Rewards()
    {
        StartCoroutine(RewardRoutine());
    }

    IEnumerator RewardRoutine()
    {
        yield return Web.GetRewardRoutine();
        int roundPoints = Web.GetRewards();
        points.value = roundPoints;
        credits.value += roundPoints;
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
        credits.value = 0;
        menu.SetActive(true);
        gameObject.SetActive(false);
    }
    //_______________________________________________________________________ EVENT AREA

    public void Enable(ClickEvent evt)
    {

        if (enabled)
        {
            int currentBet = int.Parse(bet.value);
            credits.value -= currentBet;
            
            if (patternBool)
                OrderedMatrix();
            else
            {
                RandomMatrix();
            }
            Invoke("Rewards", 1f);
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