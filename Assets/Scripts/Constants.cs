using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BlockSetting
{
    public Color color;
    public List<Vector2> blockItemPositions;
}

[CreateAssetMenu]
public class Constants : ScriptableObject
{
    public int sourceBoardWidth = 5;
    public int sourceBoardHeight = 5;
    public float scaleFactor = 2f;
    public float sourceCellSize = 40f;
    public float cellSpacing = 1f;
    public GameObject blockPrefab;
    public GameObject blockItemPrefab;
    public BlockSetting[] blockSettings;
    
}
