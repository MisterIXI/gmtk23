using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class BaseBlock : MonoBehaviour
{
    protected SpriteRenderer _spriteRenderer;
    protected Color _baseColor;
    protected bool IsPreview;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _baseColor = _spriteRenderer.color;
    }
    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    public virtual void SetPreviewState(bool state, bool isValid = false)
    {
        Color offset = new Color(
            isValid ? -0.3f : +0.3f,
            isValid ? +0.3f : -0.3f,
            0
        );
        if (state)
            _spriteRenderer.color = new Color(_baseColor.r, _baseColor.g, _baseColor.b, 0.5f) + offset;
        else
            _spriteRenderer.color = _baseColor;
        IsPreview = state;
        _spriteRenderer.sortingOrder = state ? 100 : 10;
    }
}