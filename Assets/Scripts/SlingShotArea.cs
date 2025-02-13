using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingShotArea : MonoBehaviour
{
    [SerializeField] private LayerMask slingShotAreaMask;
    public bool IsWithinSlingshotArea()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(InputManager.touchPos);

        if (Physics2D.OverlapPoint(worldPos, slingShotAreaMask))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
