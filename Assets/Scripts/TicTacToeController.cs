using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class TicTacToeController : MonoBehaviour
{
    public bool multiplayer;

    public GameObject TicTacToeParent;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private int rowCount = 3;
    [SerializeField] private int colCount = 3;
    [SerializeField] private int minToWin = 3;

    
    void Start()
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
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
