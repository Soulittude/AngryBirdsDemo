using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIcons : MonoBehaviour
{
    [SerializeField] private Image[] healthIcons;
    [SerializeField] private Color usedColor;

    public void UsedShot(int shotNumber)
    {
        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (shotNumber == i + 1)
            {
                healthIcons[i].color = usedColor;
                return;
            }
        }
    }
}
