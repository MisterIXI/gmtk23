using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class LCInteraction : MonoBehaviour
{

    public static event Action<Vector2Int?, Vector2Int?> OnInteractionCoordChanged;
    public static event Action<BlockType, BlockType> OnBlockTypeChanged;
    public static event Action<Vector2Int?, BlockType> OnBlockPlaced;
    [field: SerializeField] private SpriteRenderer previewBorder;
    private BaseBlock previewBlock;
    private BlockType currentBlockType;
    private Vector2Int? currentCoord = null;
    private BlockType[] inputTypes = { BlockType.Empty, BlockType.Normal, BlockType.SlantLeft, BlockType.SlantRight, BlockType.Player, BlockType.Goal };
    private int inputID;
    private bool IsLMBDown;
    private bool IsRMBDown;
    private Camera mainCam;
    private LevelData levelData => LevelCreator.LevelData;
    private void Awake()
    {
        mainCam = Camera.main;
    }
    private void Start()
    {
        currentBlockType = inputTypes[0];
        SubscribeEvents();
        SubscribeInputs();
    }

    private void UpdatePreviewType(BlockType oldType, BlockType newType)
    {
        BlockPool.Instance.ReturnBlock(oldType, previewBlock);
        if (currentCoord != null)
        {
            previewBlock = BlockPool.Instance.PlaceBlockAt(newType, LevelCreator.CoordToPos(currentCoord.Value));
            bool isValid = levelData.IsValidBlockPos(currentCoord.Value, currentBlockType);
            previewBlock?.SetPreviewState(true, isValid);
            previewBorder.gameObject.SetActive(true);
            previewBorder.color = isValid ? Color.green : Color.red;
        }
        else
            previewBlock = null;
    }
    private void UpdatePreviewCoord(Vector2Int? oldCoord, Vector2Int? newCoord)
    {
        BlockPool.Instance.ReturnBlock(currentBlockType, previewBlock);
        previewBlock = null;
        if (newCoord == null)
        {
            previewBorder.gameObject.SetActive(false);
            return;
        }
        if (oldCoord == null)
        {
            previewBlock?.gameObject.SetActive(true);
        }
        bool isValid = levelData.IsValidBlockPos(currentCoord.Value, currentBlockType);
        previewBlock = BlockPool.Instance.PlaceBlockAt(currentBlockType, LevelCreator.CoordToPos(newCoord.Value));
        previewBlock?.SetPreviewState(true, isValid);
        previewBorder.gameObject.SetActive(true);
        previewBorder.color = isValid ? Color.green : Color.red;
        previewBorder.transform.position = LevelCreator.CoordToPos(newCoord.Value);
    }
    private void CheckForInteractionOnPosChange()
    {
        if (currentCoord == null)
            return;
        if (IsLMBDown)
            TryPlacingBlock();
        if (IsRMBDown)
            TryRemovingBlock();
    }

    private void OnMousePosInput(InputAction.CallbackContext context)
    {
        if (!LevelCreator.IsReady)
            return;
        if (context.performed)
        {
            Vector3 mousePos = context.ReadValue<Vector2>();
            mousePos.z = mainCam.nearClipPlane;
            Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);
            Vector2Int? newCoord = LevelCreator.PosToCoord(worldPos);
            if (newCoord?.x <= 0 || newCoord?.x >= levelData.sizeX - 1 || newCoord?.y <= 0 || newCoord?.y >= levelData.sizeY - 1)
                newCoord = null;
            if (newCoord != currentCoord)
            {
                Vector2Int? oldCoord = currentCoord;
                currentCoord = newCoord;
                OnInteractionCoordChanged?.Invoke(oldCoord, newCoord);
                CheckForInteractionOnPosChange();
            }
        }
    }
    private void TryPlacingBlock()
    {
        if (currentCoord != null && levelData.IsValidBlockPos(currentCoord.Value, currentBlockType))
        {
            levelData.SetBlock(currentCoord.Value, currentBlockType);
            OnBlockPlaced?.Invoke(currentCoord, currentBlockType);
            Debug.Log($"Placed block {currentBlockType} at {currentCoord.Value}");
        }
    }

    private void OnLMBInput(InputAction.CallbackContext context)
    {
        if (!LevelCreator.IsReady)
            return;
        if (context.started && !IsRMBDown)
        {
            IsLMBDown = true;
            TryPlacingBlock();
        }
        if (context.canceled)
            IsLMBDown = false;
    }
    private void TryRemovingBlock()
    {
        if (currentCoord != null && BlockInfo.IsRemovableBlock(levelData[currentCoord.Value]))
        {
            levelData.SetBlock(currentCoord.Value, BlockType.Empty);
            OnBlockPlaced?.Invoke(currentCoord, BlockType.Empty);
        }
    }
    private void OnRMBInput(InputAction.CallbackContext context)
    {
        if (!LevelCreator.IsReady)
            return;
        if (context.started && !IsLMBDown)
        {
            IsRMBDown = true;
            TryRemovingBlock();
        }
        if (context.canceled)
            IsRMBDown = false;
    }

    private void OnScrollInput(InputAction.CallbackContext context)
    {
        if (!LevelCreator.IsReady)
            return;
        if (context.performed)
        {
            float input = context.ReadValue<float>();
            if (input > 0)
                inputID = (inputID + 1) % inputTypes.Length;
            else
            {
                inputID--;
                if (inputID < 0)
                    inputID = inputTypes.Length - 1;
            }
            BlockType oldType = currentBlockType;
            currentBlockType = inputTypes[inputID];
            Debug.Log($"Type changed: {oldType}->{currentBlockType}");
            OnBlockTypeChanged?.Invoke(oldType, currentBlockType);
        }
    }
    private void SubscribeEvents()
    {
        OnBlockTypeChanged += UpdatePreviewType;
        OnInteractionCoordChanged += UpdatePreviewCoord;
    }

    private void UnSubscribeEvents()
    {
        OnBlockTypeChanged -= UpdatePreviewType;
        OnInteractionCoordChanged -= UpdatePreviewCoord;
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

    private void OnDestroy()
    {
        UnSubscribeEvents();
        UnSubscribeInputs();
    }
}