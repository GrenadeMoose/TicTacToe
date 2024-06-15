using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class TicTacToeController : MonoBehaviour
{
    public bool multiplayer;

    public GameObject TicTacToeParent;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private int rowCount = 3;
    [SerializeField] private int colCount = 3;
    [SerializeField] private int minToWin = 3;
    [SerializeField] private Texture2D[] boardTextures;

    private List<GameObject> availableSpaces;


    /*************************************************************************
     *  Initialization 
     *************************************************************************/

    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        availableSpaces = new List<GameObject>();
        InitializeBoard();
    }

    /// <summary>
    /// Initialize the board. Create buttons from prefabs at specified locations
    /// </summary>
    void InitializeBoard()
    { 
        RectTransform parentTransform = TicTacToeParent.GetComponent<RectTransform>();

        Vector2 canvasSize = parentTransform.GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta;
        float minParentSize = Mathf.Min(canvasSize.x, canvasSize.y);
        Debug.Log(minParentSize);
        parentTransform.sizeDelta = new Vector2(minParentSize, minParentSize);

        float minWidth = parentTransform.rect.width / rowCount;
        float minHeight = parentTransform.rect.height / colCount;

        float buttonSize = Mathf.Min(minWidth, minHeight);

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                GameObject newButton  = Instantiate(buttonPrefab, parentTransform);

                RectTransform buttonTransform = newButton.GetComponent<RectTransform>();
                buttonTransform.anchorMin = new Vector2(0.0f, 1.0f);
                buttonTransform.anchorMax = new Vector2(0.0f, 1.0f);
                buttonTransform.sizeDelta = new Vector2(buttonSize, buttonSize);
                buttonTransform.anchoredPosition = new Vector2(buttonSize * col - (buttonSize * (float)rowCount)/2.0f, -buttonSize * row - buttonSize/2);

                newButton.name = "Button_" + row.ToString() + "_" + col.ToString();

                AssignBoardTextures(newButton, row, col);

                availableSpaces.Add(newButton);
            }
        }
    }

    /// <summary>
    /// Assign the appropriate texture for each space of the board using the textre array specified in the inspector.
    /// </summary>
    /// <param name="button"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    void AssignBoardTextures(GameObject button, int row, int col)
    {
        if (row == 0)
        {
            if (col == 0)
            {
                button.GetComponent<RawImage>().texture = boardTextures[0];
            }
            else if (col == colCount - 1)
            {
                button.GetComponent<RawImage>().texture = boardTextures[2];
            }
            else
            {
                button.GetComponent<RawImage>().texture = boardTextures[1];
            }
        } else if (row == rowCount - 1)
        {
            if (col == 0)
            {
                button.GetComponent<RawImage>().texture = boardTextures[6];
            }
            else if (col == colCount - 1)
            {
                button.GetComponent<RawImage>().texture = boardTextures[8];
            }
            else
            {
                button.GetComponent<RawImage>().texture = boardTextures[7];
            }
        } else
        {
            if (col == 0)
            {
                button.GetComponent<RawImage>().texture = boardTextures[3];
            }
            else if (col == colCount - 1)
            {
                button.GetComponent<RawImage>().texture = boardTextures[5];
            }
            else
            {
                button.GetComponent<RawImage>().texture = boardTextures[4];
            }
        }
    }

    /*************************************************************************
    *  Gameplay logic
    *************************************************************************/

    /// <summary>
    /// Claiming a space by removing the used space from the available spaces pool
    /// </summary>
    /// <param name="button"> The space to be claimed by a player </param>
    public void ClaimSpace(GameObject button)
    {
        Assert.IsFalse(availableSpaces.Contains(button), "This space has already been claimed!");
        availableSpaces.Remove(button);
    }

    /// <summary>
    /// Keeping track of who's turn it is. 
    /// </summary>
    public void TurnTracker()
    {

    }

    /// <summary>
    /// The game has been won. Finalize the game, and load the win screen
    /// </summary>
    public void WinConditionAchieved()
    {

    }

}
