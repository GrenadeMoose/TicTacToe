using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject buttonPrefab;
    // Start is called before the first frame update
    void Start()
    {
        TicTacToeController.Instance.InitializeBoard(this.gameObject);
    }
}
