using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseCanvasManager : BasePlayerManager<PlayerCursor>
{
    [Tooltip("The scene canvas.")]
    [SerializeField] private Canvas canvas;
    /// <summary>
    /// The canvas scale factor.
    /// </summary>
    public float ScaleFactor => canvas.scaleFactor;
    
    /// <summary>
    /// The graphic raycaster.
    /// </summary>
    public GraphicRaycaster Raycaster { get; private set; }

    /// <summary>
    /// The rect transform component.
    /// </summary>
    private RectTransform _rectTransform;

    /// <summary>
    /// Component allocation.
    /// </summary>
    protected virtual void Awake()
    {
        Raycaster = canvas.GetComponent<GraphicRaycaster>();
        _rectTransform = (RectTransform)canvas.transform;
    }

    public abstract void Check();
    
    /// <summary>
    /// Restocks the lobby input.
    /// </summary>
    /// <param name="controls">The lobby input.</param>
    public void Deregister(PlayerCursor controls)
    {
        if (!PlayerCursor.All.Contains(controls)) return;
        controls.Deactivate();
    }

    public Vector2 ViewportToCanvasSpace(Vector2 point)
        => new Vector2(point.x * _rectTransform.sizeDelta.x, point.y * _rectTransform.sizeDelta.y);
}