using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOver : MonoBehaviour
{
    public static System.Action OnReturningToMenu;
    public UIDocument document;
    private Button menuButton;

    // Start is called before the first frame update
    void Awake()
    {
        // Start button
        menuButton = document.rootVisualElement.Q<Button>("menu-button");
        // Register function
        menuButton.RegisterCallback<ClickEvent>(Menu);
    }

    // Update is called once per frame
    void Menu(ClickEvent evt)
    {
        OnReturningToMenu?.Invoke();
        SceneManager.LoadScene("MenuScene");
    }
}
