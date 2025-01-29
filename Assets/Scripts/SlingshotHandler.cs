using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("Slingshot Values")]
    [SerializeField] private float maxDistance = 3.5f;
    [SerializeField] private float launchPower = 2f;

    [Header("Objects")]
    [SerializeField] private SlingShotArea slingshotArea;

    [Header("Bird")]
    [SerializeField] private Angry angryPrefab;
    [SerializeField] private float birdSitOffset = 0.3f;
    private Angry angryReal;

    private Vector2 slingShotLinesPosition;

    private Vector2 pullDir;
    private Vector2 dirNormalized;

    private bool clickedWithinArea;

    private void Awake()
    {
        leftLineRenderer.enabled = false;
        rightLineRenderer.enabled = false;

        SpawnAngry();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && slingshotArea.IsWithinSlingshotArea())
        {
            clickedWithinArea = true;
        }

        if (Mouse.current.leftButton.isPressed && clickedWithinArea)
        {
            DrawSlingShot();
            PositionAndRotateAngry();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            clickedWithinArea = false;

            angryReal.LaunchBirb(pullDir, launchPower);
        }
    }

    #region SlingShot Methods

    private void DrawSlingShot()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

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
        SetLines(idlePos.position);

        Vector2 dir = (centerPos.position - idlePos.position).normalized;
        Vector2 spawnPos = (Vector2)idlePos.position + dir * birdSitOffset;

        angryReal = Instantiate(angryPrefab, spawnPos, Quaternion.identity);
        angryReal.transform.right = dir;
    }

    private void PositionAndRotateAngry()
    {
        angryReal.transform.position = slingShotLinesPosition + dirNormalized * birdSitOffset;
        angryReal.transform.right = dirNormalized;
    }

    #endregion
}
