using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TargetBoard : MonoBehaviour
{
    [SerializeField] private GameObject gridItemPrefab;
    [SerializeField] private GameObject targetImagePrefab;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Constants constants;
    [SerializeField] private HUD hud;

    private GridLayoutGroup gridLayoutGroup;
    private AudioSource audioSource;
    private Cell targetCell;
    private GameObject targetImage;
    private int moveCount = 0;
    private int level = 1;
    private float time = 0;


    public int Width { get; private set; }
    public int Height { get; private set; }
    public Cell[,] cells;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        Initialize();
        ChooseTargetCell();
    }

    private void Update()
    {
        time += Time.deltaTime;
        hud.UpdateTimerText(time);
    }

    private void Initialize()
    {
        Width = constants.sourceBoardWidth + 1;
        Height = constants.sourceBoardHeight + 1;
        gridLayoutGroup.cellSize = new Vector2(constants.sourceCellSize * constants.scaleFactor, constants.sourceCellSize * constants.scaleFactor);
        gridLayoutGroup.spacing = new Vector2(constants.cellSpacing * constants.scaleFactor, constants.cellSpacing * constants.scaleFactor);
        gridLayoutGroup.constraintCount = Width;

        cells = new Cell[Width, Height];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                GameObject gridItem = Instantiate(gridItemPrefab, transform);
                gridItem.name = "Target[" + j + "," + i + "]";
                RectTransform rectTransform = gridItem.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(constants.sourceCellSize * constants.scaleFactor, constants.sourceCellSize * constants.scaleFactor);
                Cell cell = gridItem.GetComponent<Cell>();
                cell.xIndex = j;
                cell.yIndex = i;
                cells[j, i] = cell;
            }
        }
    }

    private void ChooseTargetCell()
    {
        while (targetCell == null || levelManager.CompletedCells.Contains(targetCell))
        {
            int targetX = Random.Range(0, Width);
            int targetY = Random.Range(0, Height);
            targetCell = cells[targetX, targetY];
        }
        if(targetImage == null)
            targetImage = Instantiate(targetImagePrefab, targetCell.transform.position, Quaternion.identity, targetCell.transform);
        targetImage.transform.SetParent(targetCell.transform);
        targetImage.transform.localPosition = Vector3.zero;
    }

    private bool IsLevelCompleted()
    {

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (cells[i, j] != targetCell && !cells[i, j].IsFilled)
                    return false;
            }
        }
        return true;
    }

    private IEnumerator LoadNextLevel(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        foreach (var block in GetComponentsInChildren<Block>())
        {
            block.MoveToSourceBoard();
        }
        ChooseTargetCell();
        moveCount = 0;
        time = 0;
        hud.UpdateMoveCountText(moveCount);
        hud.UpdateLevelText(++level);
    }

    public void CheckLevelCompleted()
    {
        if (IsLevelCompleted())
        {
            levelManager.CompletedCells.Add(targetCell);
            if (levelManager.CompletedCells.Count != Width * Height)
                StartCoroutine(LoadNextLevel(1));
            else
                Debug.Log("Game Over");
        }
    }

    public void OnDrop()
    {
        hud.UpdateMoveCountText(++moveCount);
        CheckLevelCompleted();
        audioSource.Play();
    }
}
