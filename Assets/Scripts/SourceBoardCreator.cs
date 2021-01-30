using UnityEngine;
using UnityEngine.UI;

public class SourceBoardCreator : MonoBehaviour
{
    [SerializeField] private GameObject gridItemPrefab;
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private GameObject draggingParent;
    [SerializeField] private TargetBoard targetBoard;
    [SerializeField] private Constants constants;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        CreateSourceBoards();
    }

    private void CreateSourceBoards()
    {
        int sourceBoardCountInRow = (int)rectTransform.rect.width / (int)((constants.sourceCellSize + constants.cellSpacing) * constants.sourceBoardWidth);
        int horizontalLayoutCount = constants.blockSettings.Length / sourceBoardCountInRow + 1;
        GameObject rowObject = null;
        float pivotY = 0f;
        int index = 0;
        for (int k = 0; k < constants.blockSettings.Length; k++)
        {
            /*
                if (k % sourceBoardCountInRow == 0)
                {
                    rowObject = new GameObject("RowObject", typeof(RectTransform));
                    rowObject.transform.SetParent(transform);

                    RectTransform rectTransformRow = rowObject.GetComponent<RectTransform>();
                    rectTransformRow.sizeDelta = new Vector2(rectTransform.rect.width, rectTransform.rect.height / horizontalLayoutCount);
                    rectTransformRow.pivot = new Vector2(0.5f, pivotY);
                    pivotY += 1;

                    HorizontalLayoutGroup horizontalLayoutGroup = rowObject.AddComponent<HorizontalLayoutGroup>();
                    horizontalLayoutGroup.spacing = 5;
                    horizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
                    horizontalLayoutGroup.childForceExpandHeight = false;
                    horizontalLayoutGroup.childForceExpandWidth = false;
                }
            */
            if (k > 0 && k % sourceBoardCountInRow == 0) index++;
            CreateSourceBoardWithBlock(k, transform.GetChild(index));
        }
    }

    private void CreateSourceBoardWithBlock(int index, Transform parent)
    {
        GameObject grid = Instantiate(gridPrefab, parent);
        grid.name = "Grid-" + index;
    
        for (int i = 0; i < constants.sourceBoardWidth; i++)
        {
            for (int j = 0; j < constants.sourceBoardHeight; j++)
            {
                GameObject gridItem = Instantiate(gridItemPrefab, grid.transform);
                gridItem.name = "GridItem[" + j + "," + i + "]";
                RectTransform rectTransform = gridItem.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(constants.sourceCellSize, constants.sourceCellSize);
                Cell cell = gridItem.GetComponent<Cell>();
                cell.xIndex = j;
                cell.yIndex = i;
            }
        }

        SourceBoard sourceBoard = grid.GetComponent<SourceBoard>();
        GridLayoutGroup gridLayoutGroup = grid.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = new Vector2(constants.sourceCellSize, constants.sourceCellSize);
        gridLayoutGroup.spacing = new Vector2(constants.cellSpacing, constants.cellSpacing);

        GameObject blockGameObject = Instantiate(constants.blockPrefab, grid.transform);
        Block block = blockGameObject.GetComponent<Block>();
        BuildBlock(block, constants.blockSettings[index]);
        blockGameObject.name = "Block-" + index;
        block.transform.parent = grid.transform;

        sourceBoard.block = block;
    }

    private void BuildBlock(Block block,BlockSetting blockSetting)
    {
        foreach (var item in blockSetting.blockItemPositions)
        {
            GameObject blockItem = Instantiate(constants.blockItemPrefab, block.transform);
            RectTransform rectTransform = blockItem.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(constants.sourceCellSize, constants.sourceCellSize);
            Image image = blockItem.GetComponent<Image>();
            image.color = blockSetting.color;
            blockItem.transform.localPosition = new Vector3(item.x * constants.sourceCellSize + item.x / 2, item.y * constants.sourceCellSize + item.y / 2, 0);
        }

        block.transform.localPosition = Vector2.zero;
        block.draggingParent = draggingParent;
        block.targetBoard = targetBoard;
    }

}
