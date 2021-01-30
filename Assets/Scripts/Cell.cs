using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private Color defaultColor;

    public Tile currentTile;
    public int xIndex;
    public int yIndex;
    public bool IsFilled => currentTile;

    private void Awake()
    {
        defaultColor = GetComponent<Image>().color;
    }

    public void SetDefaultColor()
    {
        gameObject.GetComponent<Image>().color = defaultColor;
    }

    public void SetShadowColor()
    {
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
    }
}