using UnityEngine;
using UnityEngine.EventSystems;

public class SourceBoard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 pointerDownPosition;
    public Block block;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDownPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (block != null && block.transform.parent == transform)
        {
            if (eventData.position.x == pointerDownPosition.x && eventData.position.y == pointerDownPosition.y)
            {
                block.transform.Rotate(0, 0, 90);
                int rotationMultiplier = (int)block.transform.rotation.eulerAngles.y > 0 ? 1 : -1;
                foreach (var item in block.tiles)
                {
                    item.localPositionInBlock = new Vector3(item.localPositionInBlock.y * rotationMultiplier, item.localPositionInBlock.x * -rotationMultiplier, 0);
                }
            }
            else
            {
                Cell cell = eventData.pointerPressRaycast.gameObject.GetComponent<Cell>();
                if (cell.yIndex <= 1)
                {
                    int rotationMultiplier = ((int)block.transform.rotation.eulerAngles.z / 90) % 2 == 0 ? -1 : 1;
                    block.transform.Rotate(0, 180, 0);
                    foreach (var item in block.tiles)
                    {
                        item.localPositionInBlock = new Vector3(item.localPositionInBlock.x * rotationMultiplier, item.localPositionInBlock.y * -rotationMultiplier, 0);
                    }
                }
            }
        }
    }
}
