using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndMenuController : MonoBehaviour
{
    /// <summary>
    /// On Enable
    /// Set up the UI
    /// </summary>
    private void OnEnable()
    {
        VisualElement baseComponent = GetComponent<UIDocument>().rootVisualElement;

        Button mainMenu = baseComponent.Q<Button>("Return");
        Button buttonQuit = baseComponent.Q<Button>("ExitButton");

        Label winText = baseComponent.Q<Label>("WinText");
        SetWinText(winText);

        mainMenu.clicked += () => LoadMainMenu();
        buttonQuit.clicked += () => Application.Quit();
    }

    /// <summary>
    /// Load the main menu
    /// </summary>
    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Set the text of the winning condition.
    /// </summary>
    /// <param name="winText">The label that should receive text</param>

    void SetWinText(Label winText)
    {
        string text = "";
        if (TicTacToeController.Instance.Winner)
        {
            text = (TicTacToeController.Instance.IsXTurn ? "X" : "O");
            text += " Wins!";
        }
        else
        {
            text = "It's a tie!";
        }
        winText.text = text;
    }


}
