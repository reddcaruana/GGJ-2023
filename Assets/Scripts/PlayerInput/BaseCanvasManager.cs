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
    /// Component allocation.
    /// </summary>
    protected virtual void Awake()
    {
        Raycaster = canvas.GetComponent<GraphicRaycaster>();
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
}