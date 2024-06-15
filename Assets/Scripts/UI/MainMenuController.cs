using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
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

    private void LoadSinglePlayerGame()
    {
        TicTacToeController.Instance.Multiplayer = false;
        SceneManager.LoadScene("TicTacToe");
    }

    private void LoadMultiplayerGame()
    {
        TicTacToeController.Instance.Multiplayer = true;
        SceneManager.LoadScene("TicTacToe");
    }
}
