using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardGame : MonoBehaviour
{
    public static BoardGame instance;
    public GameObject[,] Board = new GameObject[3, 3]; // Changed to public to allow access in ButtonBehave
    private bool isXTurn = true;
    [SerializeField] private GameObject GameButton;
    [SerializeField] private Transform GameButtonTransform;
    [SerializeField] private Sprite Xsprite;
    [SerializeField] private Sprite OSprite;
    [SerializeField] private Transform GameCanvas;
    private Vector2 StartPosition = new Vector2(-150, 150);
    private Vector2 ButtonSpacing = new Vector2(150, -150);
    [SerializeField] private GameObject WinTextPrefab;
    private GameObject InstantiatedText;
    [SerializeField] private AudioSource WinningClip;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SetBoardGame();
    }

    public void SetBoardGame()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                Board[row, col] = Instantiate(GameButton, GameCanvas);
                RectTransform rectTransform = Board[row, col].GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(StartPosition.x + (col * ButtonSpacing.x), StartPosition.y + (row * ButtonSpacing.y));
            }
        }
    }

    public void SetXAtMark(int row, int col)
    {
        if (isXTurn)
        {
            Board[row, col].GetComponent<Image>().sprite = Xsprite;
        }
        else
        {
            Board[row, col].GetComponent<Image>().sprite = OSprite;
        }
    }

    public void WinOrLoss()
    {
        // Check rows for a win
        for (int row = 0; row < 3; row++)
        {
            if (Board[row, 0].GetComponent<ButtonBehave>().GetButtonContains() != 'e' &&
                Board[row, 0].GetComponent<ButtonBehave>().GetButtonContains() == Board[row, 1].GetComponent<ButtonBehave>().GetButtonContains() &&
                Board[row, 0].GetComponent<ButtonBehave>().GetButtonContains() == Board[row, 2].GetComponent<ButtonBehave>().GetButtonContains())
            {
                char winningCharacter = Board[row, 0].GetComponent<ButtonBehave>().GetButtonContains();
                string winner = (winningCharacter == 'x') ? "X" : "O";
                DisplayWinner(winner);
                return;
            }
        }

        // Check columns for a win
        for (int col = 0; col < 3; col++)
        {
            if (Board[0, col].GetComponent<ButtonBehave>().GetButtonContains() != 'e' &&
                Board[0, col].GetComponent<ButtonBehave>().GetButtonContains() == Board[1, col].GetComponent<ButtonBehave>().GetButtonContains() &&
                Board[0, col].GetComponent<ButtonBehave>().GetButtonContains() == Board[2, col].GetComponent<ButtonBehave>().GetButtonContains())
            {
                char winningCharacter = Board[0, col].GetComponent<ButtonBehave>().GetButtonContains();
                string winner = (winningCharacter == 'x') ? "X" : "O";
                DisplayWinner(winner);
                return;
            }
        }

        // Check main diagonal for a win
        if (Board[0, 0].GetComponent<ButtonBehave>().GetButtonContains() != 'e' &&
            Board[0, 0].GetComponent<ButtonBehave>().GetButtonContains() == Board[1, 1].GetComponent<ButtonBehave>().GetButtonContains() &&
            Board[0, 0].GetComponent<ButtonBehave>().GetButtonContains() == Board[2, 2].GetComponent<ButtonBehave>().GetButtonContains())
        {
            char winningCharacter = Board[0, 0].GetComponent<ButtonBehave>().GetButtonContains();
            string winner = (winningCharacter == 'x') ? "X" : "O";
            DisplayWinner(winner);
            return;
        }

        // Check anti-diagonal for a win
        if (Board[0, 2].GetComponent<ButtonBehave>().GetButtonContains() != 'e' &&
            Board[0, 2].GetComponent<ButtonBehave>().GetButtonContains() == Board[1, 1].GetComponent<ButtonBehave>().GetButtonContains() &&
            Board[0, 2].GetComponent<ButtonBehave>().GetButtonContains() == Board[2, 0].GetComponent<ButtonBehave>().GetButtonContains())
        {
            char winningCharacter = Board[0, 2].GetComponent<ButtonBehave>().GetButtonContains();
            string winner = (winningCharacter == 'x') ? "X" : "O";
            DisplayWinner(winner);
            return;
        }

        // Check for draw
        bool isDraw = true;
        foreach (GameObject button in Board)
        {
            if (button.GetComponent<ButtonBehave>().GetButtonContains() == 'e')
            {
                isDraw = false;
                break;
            }
        }

        if (isDraw)
        {
            DisplayWinner("Draw");
        }
    }

    private void DisplayWinner(string winner)
    {
        if (WinTextPrefab == null)
        {
            Debug.Log("Win Text Prefab is not assigned in the Inspector.");
            return;
        }

        if (InstantiatedText != null)
        {
            Destroy(InstantiatedText);
        }

        InstantiatedText = Instantiate(WinTextPrefab, GameCanvas);

        TextMeshProUGUI textComponent = InstantiatedText.GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = winner == "Draw" ? "Ah jeez, it's a draw..." : $"You’re the w-w-winner, Morty! Wubba Lubba Dub Dub!";
        }
        else
        {
            Debug.Log("The instantiated text prefab does not have a TextMeshProUGUI component.");
        }

        if (WinningClip != null)
        {
            WinningClip.Play();
            Debug.Log("Playing Winning Clip");
        }
        else
        {
            Debug.Log("Winning Clip is not assigned.");
        }

    }


public void ChangeTurn()
    {
        isXTurn = !isXTurn;

        if (!isXTurn)
        {
            ComputerMove();
        }
    }

    private void ComputerMove()
    {
        List<GameObject> availableButtons = new List<GameObject>();

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (Board[row, col].GetComponent<ButtonBehave>().GetButtonContains() == 'e')
                {
                    availableButtons.Add(Board[row, col]);
                }
            }
        }

        if (availableButtons.Count > 0)
        {
            GameObject chosenButton = availableButtons[Random.Range(0, availableButtons.Count)];

            chosenButton.GetComponent<ButtonBehave>().OnButtonClick();
        }
    }

    public bool GetXTurn()
    {
        return isXTurn;
    }
}