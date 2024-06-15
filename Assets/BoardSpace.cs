using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BoardSpace : MonoBehaviour
{
    public bool? X { get; private set; } = null;

    public List<NeighboringSpace> Neighbors;

    private Text mark;

    /*************************************************************************
    *  Initialization
    *************************************************************************/

    private void Awake()
    {
        Neighbors = new List<NeighboringSpace>();
    }

    void Start()
    {
        mark = GetComponentInChildren<Text>();
        mark.text = "";
        mark.color = Color.black;
    }

    /*************************************************************************
    *  Gameplay logic
    *************************************************************************/

    /// <summary>
    /// Assign X or O to this object on clicked, if the space is available, and it is the player's turn
    /// </summary>
    public void ClickedEvent()
    {
        if (!TicTacToeController.Instance.Multiplayer && !TicTacToeController.Instance.IsXTurn)
        {
            Debug.Log("Not the player's turn");
            return;
        } else
        {
            TryClaimSpace();
        }
    }


    /// <summary>
    /// Try to claim a space from the controller.
    /// If the space is available, check for a win condition.
    /// </summary>
    public void TryClaimSpace() {
        if (TicTacToeController.Instance.AvailableSpaces.Contains(this.gameObject))
        {
            bool success = TicTacToeController.Instance.ClaimSpace(this.gameObject);
            if (success)
            {
                X = TicTacToeController.Instance.IsXTurn;

                mark.text = (X == true ? "X" : "O");

                bool win = CheckForWinCondition();

                if (!win)
                {
                    TicTacToeController.Instance.EndTurn();
                }
                else
                {
                    TicTacToeController.Instance.WinConditionAchieved();
                }

            }
        }
    }

    /// <summary>
    /// Check nearby spaces to see if a win condition has been achieved.
    /// Keep in mind that it may be possible to win in more than one direction.
    /// </summary>
    /// <returns>True if game has been won, false if it has not</returns>
    private bool CheckForWinCondition()
    {
        bool leftRight = BiDirectionalCheck(NeighboringSpace.DirectionEnum.Left, NeighboringSpace.DirectionEnum.Right);
        bool topBottom = BiDirectionalCheck(NeighboringSpace.DirectionEnum.Top, NeighboringSpace.DirectionEnum.Bottom);
        bool upperLeftLowerRight = BiDirectionalCheck(NeighboringSpace.DirectionEnum.UpperLeft, NeighboringSpace.DirectionEnum.LowerRight);
        bool lowerLeftUpperRight = BiDirectionalCheck(NeighboringSpace.DirectionEnum.LowerLeft, NeighboringSpace.DirectionEnum.UpperRight);

        return leftRight || topBottom || upperLeftLowerRight || lowerLeftUpperRight;
    }

    /// <summary>
    /// Check for a winning condition in two directions from our currnent space.
    /// If the winning condition has been achieved, set the colors for these spaces to red
    /// </summary>
    /// <param name="direction1">First direction (i.e. left or up)</param>
    /// <param name="direction2">Second opposite direction (i.e. right or down)</param>
    /// <returns> true if the winning condition has been achieved across these directions </returns>
    private bool BiDirectionalCheck(NeighboringSpace.DirectionEnum direction1, NeighboringSpace.DirectionEnum direction2)
    {
        int count = 1;

        // Check Left and Right
        count += CheckNeighbor(direction1);
        count += CheckNeighbor(direction2);

        if (count >= TicTacToeController.Instance.MinToWin)
        {
            mark.color = Color.red;
            SetNeighborColors(direction1);
            SetNeighborColors(direction2);
            return true;
        }
        return false;
    }


    /*************************************************************************
    *  Neighboring Spaces
    *  
    *  Assigning each neighboring space to reference later when determining
    *  a win condition. Using this method, we should be able to make this
    *  game extendable to more than a 3x3 grid, with a variable total win 
    *  count. Tic Tac Toe can become "Connect Four" with a few minor 
    *  adjustments to the board and win condition.
    *************************************************************************/

    /// <summary>
    /// Assign a space as a neighbor to this space. Then do the same in reverse
    /// for the neighboring space.
    /// </summary>
    /// <param name="neighbor"> Neighboring space to be assigned </param>
    /// <param name="direction"> Neighbor's direction relative to this current space. </param>

    public void AssignNeighbor(BoardSpace neighbor, NeighboringSpace.DirectionEnum direction)
    {
        if (neighbor == null) return;
        if (Neighbors.Count > 0)
        {
            foreach (NeighboringSpace n in Neighbors)
            {
                if (n.Direction == direction && n.Space == neighbor)
                {

                    // Already assigned
                    Debug.Log("Neighbor already assigned for " + this.gameObject.name + " > " + direction.ToString() + " > " + neighbor.gameObject.name);
                    return;
                }
            }
        }
        
        Debug.Log("Adding Neighbor " + this.gameObject.name + " > " + direction.ToString() + " > " + neighbor.gameObject.name);
        NeighboringSpace newNeighbor = new NeighboringSpace(neighbor, direction);
        Neighbors.Add(newNeighbor);

        // Complete the space assignment handshake
        switch (direction)
        {
            case NeighboringSpace.DirectionEnum.Left:
                neighbor.AssignNeighbor(this, NeighboringSpace.DirectionEnum.Right);
                break;
            case NeighboringSpace.DirectionEnum.Right:
                neighbor.AssignNeighbor(this, NeighboringSpace.DirectionEnum.Left);
                break;
            case NeighboringSpace.DirectionEnum.Top:
                neighbor.AssignNeighbor(this, NeighboringSpace.DirectionEnum.Bottom);
                break;
            case NeighboringSpace.DirectionEnum.Bottom:
                neighbor.AssignNeighbor(this, NeighboringSpace.DirectionEnum.Top);
                break;
            case NeighboringSpace.DirectionEnum.UpperLeft:
                neighbor.AssignNeighbor(this, NeighboringSpace.DirectionEnum.LowerRight);
                break;
            case NeighboringSpace.DirectionEnum.UpperRight:
                neighbor.AssignNeighbor(this, NeighboringSpace.DirectionEnum.LowerLeft);
                break;
            case NeighboringSpace.DirectionEnum.LowerLeft:
                neighbor.AssignNeighbor(this, NeighboringSpace.DirectionEnum.UpperRight);
                break;
            case NeighboringSpace.DirectionEnum.LowerRight:
                neighbor.AssignNeighbor(this, NeighboringSpace.DirectionEnum.UpperLeft);
                break;
            case NeighboringSpace.DirectionEnum.Undefined:
                Debug.Log("There's an undefinded neighboring space");
                break;
            default:
                // This should never be reached, but here we are
                Debug.Log("We have reached default in the neighboring space assginment. This is bad.");
                break;
        }
    }

    public int CheckNeighbor(NeighboringSpace.DirectionEnum direction)
    {
        int count = 0;
        foreach (NeighboringSpace n in Neighbors)
        {
            if (n.Direction == direction)
            {
                if (n.Space.X != null && n.Space.X.Value == X.Value)
                {
                    count += 1;

                    return count + n.Space.CheckNeighbor(direction);
                }
            }
        }
        // The continuity has been broken. Return this result;
        return count;
    }

    private void SetNeighborColors(NeighboringSpace.DirectionEnum direction)
    {
        foreach(NeighboringSpace n in Neighbors)
        {
            if (n.Direction == direction)
            {
                if (n.Space.X != null && n.Space.X.Value == X.Value)
                {
                    n.Space.mark.color = Color.red;
                    n.Space.SetNeighborColors(direction);
                }
            }
        }
    }

}

