using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// The tic tac toe controller is responsible for managing all spaces of the board.
/// </summary>
public class TicTacToeController : MonoBehaviour
{
    public static TicTacToeController Instance { get; private set; }

    public bool Multiplayer;
    public bool IsXTurn { get; private set; }
    public bool Winner { get; private set; }
    public List<GameObject> AvailableSpaces { get; private set; }

    [field: SerializeField] public int MinToWin { get; private set; } = 3;

    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private int rowCount = 3;
    [SerializeField] private int colCount = 3;
    
    [SerializeField] private Texture2D[] boardTextures;



    /*************************************************************************
     *  Initialization 
     *************************************************************************/

    /// <summary>
    /// Determine if we are the only copy of the controller
    /// Destroy this instance if another already exists.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this; 
        }
    }

    /// <summary>
    /// Initialize the board. Create board spaces at specified locations
    /// </summary>
    public void InitializeBoard(GameObject TicTacToeParent)
    {
        Winner = false;
        AvailableSpaces = new List<GameObject>();
        IsXTurn = true;

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
                GameObject newSpace = CreateSpace(row, col, buttonSize, parentTransform);

                AvailableSpaces.Add(newSpace);
            }
        }
    }
    
    /// <summary>
    /// Create a new board space using the specified prefab at the location Row, Col.
    /// Then assign neighbors to that space.
    /// </summary>
    /// <param name="row">Current row</param>
    /// <param name="col">Current colum</param>
    /// <param name="buttonSize">Size of each space</param>
    /// <param name="parentTransform">Transform of the parent UI object</param>
    /// <returns>The newly created button Game Object</returns>
    private GameObject CreateSpace(int row, int col, float buttonSize, RectTransform parentTransform)
    {
        GameObject res = Instantiate(buttonPrefab, parentTransform);

        RectTransform buttonTransform = res.GetComponent<RectTransform>();
        buttonTransform.anchorMin = new Vector2(0.5f, 1.0f);
        buttonTransform.anchorMax = new Vector2(0.5f, 1.0f);
        buttonTransform.sizeDelta = new Vector2(buttonSize, buttonSize);
        buttonTransform.anchoredPosition = new Vector2(buttonSize * col - (buttonSize * (float)rowCount) / 2.0f + (0.5f * buttonSize), -buttonSize * row - buttonSize / 2);

        res.name = "Button_" + row.ToString() + "_" + col.ToString();

        AssignBoardTextures(res, row, col);

        // Assigning Left, Upper Left, Upper, and Upper Right neighbors should cover all neighboring spaces.
        if (col > 0)
        {
            res.GetComponent<BoardSpace>().AssignNeighbor(AvailableSpaces.Last<GameObject>().GetComponent<BoardSpace>(), NeighboringSpace.DirectionEnum.Left);
            Debug.Log(res.GetComponent<BoardSpace>().Neighbors.Count);

        }
        if (row > 0)
        {
            if (col > 0)
            {
                int upperLeftIndex = AvailableSpaces.Count - colCount - 1;
                res.GetComponent<BoardSpace>().AssignNeighbor(AvailableSpaces[upperLeftIndex].GetComponent<BoardSpace>(), NeighboringSpace.DirectionEnum.UpperLeft);
                Debug.Log(res.GetComponent<BoardSpace>().Neighbors.Count);
            }
            int topIndex = AvailableSpaces.Count - colCount;
            res.GetComponent<BoardSpace>().AssignNeighbor(AvailableSpaces[topIndex].GetComponent<BoardSpace>(), NeighboringSpace.DirectionEnum.Top);
            Debug.Log(res.GetComponent<BoardSpace>().Neighbors.Count);
            if (col < colCount - 1)
            {
                int upperRightIndex = AvailableSpaces.Count - colCount + 1;
                res.GetComponent<BoardSpace>().AssignNeighbor(AvailableSpaces[upperRightIndex].GetComponent<BoardSpace>(), NeighboringSpace.DirectionEnum.UpperRight);
                Debug.Log(res.GetComponent<BoardSpace>().Neighbors.Count);
            }
        }
        return res;
    }

    /// <summary>
    /// Assign the appropriate texture for each space of the board using the textre array specified in the inspector.
    /// </summary>
    /// <param name="button"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    private void AssignBoardTextures(GameObject button, int row, int col)
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
    /// <returns> True on success, false on failure </returns>
    public bool ClaimSpace(GameObject button)
    {
        if (Winner) return false;

        if (AvailableSpaces.Contains(button))
        {
            AvailableSpaces.Remove(button);
            return true;
        }
        return false;
    }

    /// <summary>
    /// End a turn and update the turn tracker 
    /// </summary>
    public void EndTurn()
    {
        IsXTurn = !IsXTurn;

        if (AvailableSpaces.Count == 0)
        {
            DrawGame();
            return;
        }

        if (!IsXTurn && !Multiplayer)
        {
            StartCoroutine(ComputerTurn());
        }
    }

    /// <summary>
    /// The game has been won. Finalize the game, and load the win screen
    /// </summary>
    public void WinConditionAchieved()
    {
        Winner = true;
        StartCoroutine(EndGame());
    }

    /// <summary>
    /// There are no winners, and there are no available spaces
    /// </summary>
    public void DrawGame()
    {
        Winner = false;
        Debug.Log("The game is a draw. There are no available spaces left");
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("EndGame");
    }

    /// <summary>
    /// The computer's turn logic
    /// </summary>
    IEnumerator ComputerTurn()
    {
        Debug.Log("Computer thinking");
        // Simulate computer thinking
        float seconds = Random.Range(0.5f, 2.0f);
        yield return new WaitForSeconds(seconds);
 
        // Claim computer space
        int space = Random.Range(0, AvailableSpaces.Count - 1);
        AvailableSpaces[space].GetComponent<BoardSpace>().TryClaimSpace();
    }
}
