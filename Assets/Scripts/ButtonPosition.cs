using UnityEngine;

public class ButtonPosition : MonoBehaviour
{
    [SerializeField] private int row;
    [SerializeField] private int column;

    public void SetPosition(int r, int c)
    {
        row = r;
        column = c;
    }

    public int Row => row;
    public int Column => column;
}