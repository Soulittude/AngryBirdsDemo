using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int maxShotCount = 3;
    private int usedNumberOfShots;
    private HealthIcons iconsObj;

    public void Awake()
    {
        if(instance==null)
            instance=this;

        iconsObj = GameObject.FindObjectOfType<HealthIcons>();
    }
    
    public void UseShot()
    {
        usedNumberOfShots++;
        iconsObj.UsedShot(usedNumberOfShots);
    }

    public bool HasEnoughShots()
    {
        if(usedNumberOfShots<maxShotCount)
            return true;
        else
            return false;
    }
}
