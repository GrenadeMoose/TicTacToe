using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// On Enable
    /// Set up the UI
    /// </summary>
    private void OnEnable()
    {
        VisualElement baseComponent = GetComponent<UIDocument>().rootVisualElement;

        Button singlePlayerStart = baseComponent.Q<Button>("Start1P");
        Button multiPlayerStart = baseComponent.Q<Button>("Start2P");

        Button buttonQuit = baseComponent.Q<Button>("ExitButton");

        singlePlayerStart.clicked += () => LoadSinglePlayerGame();
        multiPlayerStart.clicked += () => LoadMultiplayerGame();

        buttonQuit.clicked += () => Application.Quit();
    }

    /// <summary>
    /// Load a single player game
    /// </summary>
    private void LoadSinglePlayerGame()
    {
        TicTacToeController.Instance.Multiplayer = false;
        SceneManager.LoadScene("TicTacToe");
    }
    
    /// <summary>
    /// Load a multiplayer game
    /// </summary>
    private void LoadMultiplayerGame()
    {
        TicTacToeController.Instance.Multiplayer = true;
        SceneManager.LoadScene("TicTacToe");
    }
}
