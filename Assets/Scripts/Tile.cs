using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 localPositionInBlock;
    public Vector3 startingLocalPositionInBlock;
    public Cell cell;

    private void Start()
    {
        localPositionInBlock = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        startingLocalPositionInBlock = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
    }

    public  void Reset(bool resetPosition)
    {
        transform.localPosition =  resetPosition ? startingLocalPositionInBlock : transform.localPosition;
        if (cell != null && cell.currentTile!=null)
            cell.currentTile = null;
        cell = null;
    }

}
