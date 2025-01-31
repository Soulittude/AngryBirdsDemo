using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int maxShotCount = 3;
    [SerializeField]
    private float secToWaitBeforeDeathCheck = 3f;
    private int usedNumberOfShots;
    private HealthIcons iconsObj;

    private List<Green> _greens = new List<Green>();

    [SerializeField] private GameObject restartMenu;
    [SerializeField] private SlingshotHandler _slingShot;
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

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #region WinLose

    private void WinGame()
    {
        restartMenu.SetActive(true);
        _slingShot.enabled = false;
    }

    private void LoseGame()
    {
        DOTween.Clear(true);
        RestartGame();
    }

    #endregion
}
