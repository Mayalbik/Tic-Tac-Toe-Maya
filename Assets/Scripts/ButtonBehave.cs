using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehave : MonoBehaviour
{
    [SerializeField]
    private Sprite Xsprite;
    [SerializeField]
    private Sprite OSprite;
    [SerializeField]
    private Sprite DefaultSprite;
    public GameObject BoardGameObject;
    private char buttonContains = 'e'; // Default to 'e' for empty

    public void OnButtonClick()
    {
        // Loop through the board to find this button's row and column
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (BoardGame.instance.Board[row, col] == gameObject)
                {
                    Debug.Log($"Button clicked at position: Row {row}, Column {col}");

                    // Set sprite and character based on the current turn
                    if (BoardGame.instance.GetXTurn())
                    {
                        GetComponent<Image>().sprite = Xsprite;
                        buttonContains = 'x';
                    }
                    else
                    {
                        GetComponent<Image>().sprite = OSprite;
                        buttonContains = 'o';
                    }

                    // Disable the button after it has been clicked
                    GetComponent<Button>().interactable = false;

                    // Change the turn
                    BoardGame.instance.ChangeTurn();

                    // Check for win or loss
                    BoardGame.instance.WinOrLoss();

                    return;
                }
            }
        }
    }

    public char GetButtonContains()
    {
        return buttonContains;
    }
}