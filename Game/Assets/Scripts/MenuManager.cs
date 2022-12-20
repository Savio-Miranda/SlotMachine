using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    public UIDocument document;
    private Button beginMenuButton;

    // Start is called before the first frame update
    void Awake()
    {
        // Start button
        beginMenuButton = document.rootVisualElement.Q<Button>("begin-button");
        // Register function
        beginMenuButton.RegisterCallback<ClickEvent>(Slots);
    }

    // Update is called once per frame
    void Slots(ClickEvent evt)
    {
        SceneManager.LoadScene("SlotsScene");
    }
}
