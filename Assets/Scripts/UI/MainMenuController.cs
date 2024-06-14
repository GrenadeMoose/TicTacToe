using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement baseComponent = GetComponent<UIDocument>().rootVisualElement;

        Button singlePlayerStart = baseComponent.Q<Button>("Start1P");

        Button buttonQuit = baseComponent.Q<Button>("ExitButton");

        singlePlayerStart.clicked += () => Debug.Log("Hey i have been clicked");

        buttonQuit.clicked += () => Application.Quit();
    }
}
