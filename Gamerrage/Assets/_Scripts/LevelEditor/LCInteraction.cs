using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class LCInteraction : MonoBehaviour
{
    public static event Action<Vector2Int, Vector2Int> OnInteractionCoordChanged;
    public static event Action<Vector2Int, BlockType> OnBlockPlaced;
    [field: SerializeField] private SpriteRenderer previewBorder;
    private SpriteRenderer previewBlock;
    private BlockType currentBlockType;
    private Vector2Int currentCoord = new Vector2Int(-1, -1);
    private bool IsMouseDown;
    private Camera mainCam;
    private void Start()
    {

    }
    private bool IsValidBlockPos()
    {
        LevelData levelData = LevelCreator.Instance.levelData;
        if (currentBlockType == BlockType.Empty)
            return false;
        if (levelData[currentCoord] == BlockType.StaticWalls)
            return false;
        if (currentBlockType == BlockType.Goal || currentBlockType == BlockType.Player)
        {
            // grounded check
            if (!levelData.IsGrounded(currentCoord))
                return false;
            // check if other block would be overwritten
            if (levelData[currentCoord] == BlockType.Goal || levelData[currentCoord] == BlockType.Player)
                return false;
        }

        return true;

    }
    private void OnMousePosInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector3 mousePos = context.ReadValue<Vector2>();
            mousePos.z = mainCam.nearClipPlane;
            Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);
            Vector2Int newCoord = LevelCreator.PosToCoord(worldPos);
            if (newCoord != currentCoord)
            {
                Vector2Int oldCoord = currentCoord;
                currentCoord = newCoord;
                OnInteractionCoordChanged?.Invoke(oldCoord, newCoord);
            }
        }
    }

    private void OnLMBInput(InputAction.CallbackContext context)
    {
        if (context.started)
            IsMouseDown = true;
        if (context.performed)
        {
            if (IsValidBlockPos())
            {

            }
        }
        if (context.canceled)
            IsMouseDown = false;
    }

    private void OnRMBInput(InputAction.CallbackContext context)
    {

    }

    private void OnScrollInput(InputAction.CallbackContext context)
    {

    }

    private void SubscribeInputs()
    {
        InputManager.OnMousePos += OnMousePosInput;
        InputManager.OnLMB += OnLMBInput;
        InputManager.OnRMB += OnRMBInput;
        InputManager.OnScroll += OnScrollInput;
    }

    private void UnSubscribeInputs()
    {
        InputManager.OnMousePos -= OnMousePosInput;
        InputManager.OnLMB -= OnLMBInput;
        InputManager.OnRMB -= OnRMBInput;
        InputManager.OnScroll -= OnScrollInput;
    }
}