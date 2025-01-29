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

    [Header("Objects")]
    [SerializeField] private SlingShotArea slingshotArea;

    [Header("Bird")]
    [SerializeField] private GameObject angryPrefab;
    private GameObject angryReal;

    private Vector2 slingShotLinesPosition;

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
        }
    }

    #region SlingShot Methods

    private void DrawSlingShot()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        slingShotLinesPosition = centerPos.position + Vector3.ClampMagnitude(touchPosition - centerPos.position, maxDistance);

        SetLines(slingShotLinesPosition);
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
        angryReal = Instantiate(angryPrefab, idlePos.position, Quaternion.identity);
    }

    private void PositionAndRotateAngry()
    {
        angryReal.transform.position = slingShotLinesPosition;
    }

    #endregion
}
