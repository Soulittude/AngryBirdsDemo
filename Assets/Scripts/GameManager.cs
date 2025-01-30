using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int maxShotCount = 3;
    [SerializeField]
    private float secToWaitBeforeDeathCheck = 3f;
    private int usedNumberOfShots;
    private HealthIcons iconsObj;

    private List<Green> _greens = new List<Green>();

    public void Awake()
    {
        if (instance == null)
            instance = this;

        iconsObj = FindObjectOfType<HealthIcons>();

        Green[] greens = FindObjectsOfType<Green>();

        for (int i = 0; i < greens.Length; i++)
        {
            _greens.Add(greens[i]);
        }
    }

    public void UseShot()
    {
        usedNumberOfShots++;
        iconsObj.UsedShot(usedNumberOfShots);

        CheckForLastshot();
    }

    public bool HasEnoughShots()
    {
        if (usedNumberOfShots < maxShotCount)
            return true;
        else
            return false;
    }

    public void CheckForLastshot()
    {
        if (usedNumberOfShots == maxShotCount)
        {
            StartCoroutine(CheckAfterWaitTime());
        }
    }

    private IEnumerator CheckAfterWaitTime()
    {
        yield return new WaitForSeconds(secToWaitBeforeDeathCheck);

        if (_greens.Count == 0)
        {
            WinGame();
        }
        else
        {
            LoseGame();
        }
    }

    public void RemoveGreen(Green green)
    {
        _greens.Remove(green);
        CheckForAllGreensDied();
    }

    private void CheckForAllGreensDied()
    {
        if (_greens.Count == 0)
        {
            WinGame();
        }
    }

    #region WinLose

    private void WinGame()
    {
        Debug.Log("Win");
    }

    private void LoseGame()
    {
        Debug.Log("Lose");
    }

    #endregion
}
