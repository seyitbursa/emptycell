using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour, IDragHandler,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] private Constants constants;
    
    private Vector3 sourceBoardLocalPosition;
    private GameObject sourceBoard;
    private CanvasGroup canvasGroup;
    private Tile centerTile;
    private List<Cell> selectedCells;

    public TargetBoard targetBoard;
    public GameObject draggingParent;
    public Tile[] tiles;
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        selectedCells = new List<Cell>();
        sourceBoard = transform.parent.gameObject;
        sourceBoardLocalPosition = transform.localPosition;
        tiles = transform.GetComponentsInChildren<Tile>();
        centerTile = tiles.FirstOrDefault(p => p.transform.localPosition.x == 0 && p.transform.localPosition.y == 0);
    }

    private void CalculateSelectedCells(Cell hitCell, Tile hitTile)
    {
        ClearSelectedCells();
        if (hitTile != null && hitCell != null && hitCell.transform.parent == targetBoard.transform)
        {
            int hitXInBlock = (int)(hitTile.localPositionInBlock.x / constants.sourceCellSize);
            int hitYInBlock = (int)(hitTile.localPositionInBlock.y / constants.sourceCellSize);
            foreach (var tile in tiles)
            {
                int tileXInBlock = (int)(tile.localPositionInBlock.x / constants.sourceCellSize);
                int tileYInBlock = (int)(tile.localPositionInBlock.y / constants.sourceCellSize);

                if (tileXInBlock == hitXInBlock && tileYInBlock == hitYInBlock)
                {
                    selectedCells.Add(hitCell);
                }
                else
                {
                    if (hitCell.yIndex + tileYInBlock - hitYInBlock >= 0 && hitCell.yIndex + tileYInBlock - hitYInBlock < targetBoard.Height &&
                        hitCell.xIndex + tileXInBlock - hitXInBlock >= 0 && hitCell.xIndex + tileXInBlock - hitXInBlock < targetBoard.Width)
                    {
                        Cell cell = targetBoard.cells[hitCell.xIndex + tileXInBlock - hitXInBlock, hitCell.yIndex + tileYInBlock - hitYInBlock];
                        selectedCells.Add(cell);
                    }
                }
                if (selectedCells.Any())
                {
                    if (selectedCells.Last().IsFilled)
                    {
                        ClearSelectedCells();
                        return;
                    }
                    else
                    {
                        selectedCells.Last().SetShadowColor();
                    }
                }
            }
        }
    }

    private void ClearSelectedCells()
    {
        foreach (var item in selectedCells)
        {
            item.SetDefaultColor();
        }
        selectedCells.Clear();
    }

    private void ResetTiles()
    {
        foreach (var tile in tiles)
        {
            tile.Reset(true);
        }
    }

    private void SetCanvasGroupProperties(bool blockRaycasts,float alpha)
    {
        canvasGroup.blocksRaycasts = blockRaycasts;
        canvasGroup.alpha = alpha;
    }

    private void OnDrop()
    {
        if (selectedCells.Count == tiles.Length)
        {
            transform.SetParent(targetBoard.transform);
            transform.localScale = new Vector3(constants.scaleFactor, constants.scaleFactor, 0);
            for (int i = 0; i < tiles.Length; i++)
            {
                selectedCells[i].currentTile = tiles[i];
                tiles[i].transform.position = selectedCells[i].transform.position;
                tiles[i].cell = selectedCells[i];
            }
            targetBoard.OnDrop();
        }
    }

    public void MoveToSourceBoard()
    {
        transform.SetParent(sourceBoard.transform);
        transform.SetAsLastSibling();
        transform.localScale = Vector3.one;
        transform.localPosition = sourceBoardLocalPosition;
        transform.Rotate(Vector3.zero);

        ResetTiles();
        ClearSelectedCells();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetCanvasGroupProperties(false, 0.8f);
        ResetTiles();
        ClearSelectedCells();
        transform.SetParent(draggingParent.transform);
        transform.SetAsLastSibling();
        transform.localScale = new Vector3(constants.scaleFactor, constants.scaleFactor, 0);
        Vector2 pressPosition = Camera.main.ScreenToWorldPoint(eventData.pressPosition);
        float xPos = pressPosition.x + Mathf.Abs(tiles.Min(p => p.localPositionInBlock.x));
        float yPos = pressPosition.y + Mathf.Abs(tiles.Min(p => p.localPositionInBlock.y)) + 3 * (constants.sourceCellSize * constants.scaleFactor);
        transform.position = new Vector3(xPos,yPos);
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnDrop();
        SetCanvasGroupProperties(true, 1f);
        if (transform.parent == draggingParent.transform)
        {
            MoveToSourceBoard();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += new Vector3(eventData.delta.x * constants.scaleFactor / transform.lossyScale.x, eventData.delta.y  * constants.scaleFactor / transform.lossyScale.y, 0);
        foreach (var cell in targetBoard.cells)
        {
            if (Vector2.Distance(cell.transform.position,transform.position ) <= (constants.sourceCellSize * constants.scaleFactor / 2))
            {
                CalculateSelectedCells(cell, centerTile);
                return;
            }
        }   
    }
}
