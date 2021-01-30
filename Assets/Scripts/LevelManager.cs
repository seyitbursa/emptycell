using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelManager : ScriptableObject
{
    public List<Cell> CompletedCells { get; set; }
    public void Awake()
    {
        CompletedCells = new List<Cell>();
    }
}
