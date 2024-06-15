using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class BoardSpace : MonoBehaviour
{
    public bool claimed { get; private set; }
    public bool? X { get; private set; } = null;

    /// <summary>
    /// Assign X or O to this object on clicked, if the space is available
    /// </summary>
    public void ClickedEvent()
    {
        Debug.Log("Hello I have been clicked " + gameObject.name);
        if (TicTacToeController.Instance.AvailableSpaces.Contains(this.gameObject))
        {
            bool success = TicTacToeController.Instance.ClaimSpace(this.gameObject);
            if (success)
            {
                X = TicTacToeController.Instance.IsXTurn;
                claimed = true;

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
    /// </summary>
    /// <returns>True if game has been won, false if it has not</returns>
    private bool CheckForWinCondition()
    {
        bool res = false;
        foreach (BoardSpace space in gameObject.transform.parent.GetComponentsInChildren<BoardSpace>())
        {
            if (space.X == X)
            {

            }
        }
        return res;
    }
}

