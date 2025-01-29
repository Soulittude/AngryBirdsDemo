using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxShotCount = 3;

    private int usedNumberShots;
    
    private void UseShot()
    {
        usedNumberShots++;

    }

    public bool HasEnoughShots()
    {
        if(usedNumberShots<maxShotCount)
            return true;
        else
            return false;
    }
}
