using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput playerInput;

    private InputAction touchPosAction;
    private InputAction touchAction;

    public static Vector2 touchPos;

    public static bool wasTouchPressed;
    public static bool wasTouchReleased;
    public static bool isTouchPressed;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    
        touchPosAction = playerInput.actions["TouchPosition"];
        touchAction = playerInput.actions["Touch"];
    }

    private void Update()
    {
        touchPos = touchPosAction.ReadValue<Vector2>();

        wasTouchPressed = touchAction.WasPressedThisFrame();
        wasTouchReleased = touchAction.WasReleasedThisFrame();
        isTouchPressed = touchAction.IsPressed();
    }
}
