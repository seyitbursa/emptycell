using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text moveCountText;
    public TMP_Text timerText;

    public void UpdateLevelText(int level)
    {
        levelText.text = "Level " + level;
    }

    public void UpdateMoveCountText(int moveCount)
    {
        moveCountText.text = moveCount.ToString(); // + " Move" + (moveCount > 1 ? "s" : "");
    }

    public void UpdateTimerText(float time)
    {
        timerText.text = time.ToString("F0") ;
    }
}
