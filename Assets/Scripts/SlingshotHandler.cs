using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class SlingshotHandler : MonoBehaviour
{
    [Header("Line Renderers")]
    [SerializeField] private LineRenderer leftLineRenderer;
    [SerializeField] private LineRenderer rightLineRenderer;

    [Header("Transform References")]
    [SerializeField] private Transform leftStartPos;
    [SerializeField] private Transform rightStartPos;
    [SerializeField] private Transform centerPos;
    [SerializeField] private Transform idlePos;
    [SerializeField] private Transform elasticTransform;

    [Header("Slingshot Values")]
    [SerializeField] private float maxDistance = 3.5f;
    [SerializeField] private float launchPower = 8f;
    [SerializeField] private float timeBetweenBirbs = 2f;
    [SerializeField] private float elasticDivider = 1.2f;
    [SerializeField] private AnimationCurve elasticCurve;
    [SerializeField] private float maxAnimationTime = 1f;

    [Header("Objects")]
    [SerializeField] private SlingShotArea slingshotArea;
    [SerializeField] private CameraManager camManager;

    [Header("Bird")]
    [SerializeField] private Angry angryPrefab;
    [SerializeField] private float birdSitOffset = 0.3f;

    [Header("Sounds")]
    [SerializeField] private AudioClip elasticPulledClip;
    [SerializeField] private AudioClip[] elasticReleasedClips;

    private AudioSource audioSource;
    private Angry angryReal;

    private Vector2 slingShotLinesPosition;

    private Vector2 pullDir;
    private Vector2 dirNormalized;

    private bool clickedWithinArea = false;
    private bool birbOnSlingshot;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        leftLineRenderer.enabled = false;
        rightLineRenderer.enabled = false;

        SpawnAngry();
    }

    private void Update()
    {
        if (InputManager.wasTouchPressed && slingshotArea.IsWithinSlingshotArea())
        {
            clickedWithinArea = true;

            if (birbOnSlingshot)
            {
                SoundManager.instance.PlayClip(elasticPulledClip, audioSource);
                camManager.SwitchToFollowCam(angryReal.transform);
            }
        }

        if (InputManager.isTouchPressed && clickedWithinArea && birbOnSlingshot)
        {
            DrawSlingShot();
            PositionAndRotateAngry();
        }

        if (InputManager.wasTouchReleased && clickedWithinArea && birbOnSlingshot)
        {
            if (GameManager.instance.HasEnoughShots())
            {
                clickedWithinArea = false;
                birbOnSlingshot = false;

                angryReal.LaunchBirb(pullDir, launchPower);

                SoundManager.instance.PlayRandomClip(elasticReleasedClips, audioSource);
                GameManager.instance.UseShot();
                AnimatedSlingshot();

                if (GameManager.instance.HasEnoughShots())
                    StartCoroutine(SpawningBirbAfterTime());
            }
        }
    }

    #region SlingShot Methods

    private void DrawSlingShot()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.touchPos);

        slingShotLinesPosition = centerPos.position + Vector3.ClampMagnitude(touchPosition - centerPos.position, maxDistance);

        SetLines(slingShotLinesPosition);

        pullDir = (Vector2)centerPos.position - slingShotLinesPosition;
        dirNormalized = pullDir.normalized;
    }

    private void SetLines(Vector2 pos)
    {
        if (!leftLineRenderer.enabled && !rightLineRenderer.enabled)
        {
            leftLineRenderer.enabled = true;
            rightLineRenderer.enabled = true;
        }

        leftLineRenderer.SetPosition(0, pos);
        leftLineRenderer.SetPosition(1, leftStartPos.position);

        rightLineRenderer.SetPosition(0, pos);
        rightLineRenderer.SetPosition(1, rightStartPos.position);

    }

    #endregion

    #region Angry Methods

    private void SpawnAngry()
    {
        elasticTransform.DOComplete();
        SetLines(idlePos.position);

        Vector2 dir = (centerPos.position - idlePos.position).normalized;
        Vector2 spawnPos = (Vector2)idlePos.position + dir * birdSitOffset;

        angryReal = Instantiate(angryPrefab, spawnPos, Quaternion.identity);
        angryReal.transform.right = dir;

        birbOnSlingshot = true;
    }

    private void PositionAndRotateAngry()
    {
        angryReal.transform.position = slingShotLinesPosition + dirNormalized * birdSitOffset;
        angryReal.transform.right = dirNormalized;
    }

    private IEnumerator SpawningBirbAfterTime()
    {
        yield return new WaitForSeconds(timeBetweenBirbs);

        SpawnAngry();
        camManager.SwitchToIdleCam();
    }

    #endregion

    #region Animate Slingshot

    private void AnimatedSlingshot()
    {
        elasticTransform.position = leftLineRenderer.GetPosition(0);

        float dist = Vector2.Distance(elasticTransform.position, centerPos.position);

        float time = dist / elasticDivider;

        elasticTransform.DOMove(centerPos.position, time).SetEase(elasticCurve);
        StartCoroutine(AnimatedSlinghshotLines(elasticTransform, time));
    }

    private IEnumerator AnimatedSlinghshotLines(Transform trans, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time && elapsedTime < maxAnimationTime)
        {
            elapsedTime += Time.deltaTime;

            SetLines(trans.position);

            yield return null;
        }
    }

    #endregion
}
