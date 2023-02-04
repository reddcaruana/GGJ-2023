using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseControls<T> : MonoBehaviour, IBindable
    where T : BaseControls<T>
{
    /// <summary>
    /// A resizable array of all available players.
    /// </summary>
    private static readonly List<T> _ActiveInputs = new List<T>();

    /// <summary>
    /// A collection of all players.
    /// </summary>
    public static T[] All => _ActiveInputs.ToArray();

    /// <summary>
    /// The player Id.
    /// </summary>
    public int ID => Input ? Input.playerIndex : -1;
    
    /// <summary>
    /// The player input component.
    /// </summary>
    protected PlayerInput Input { get; private set; }

    /// <summary>
    /// Flag the player as disconnected.
    /// </summary>
    protected void OnDisable()
    {
        if (_ActiveInputs.Contains(this as T))
            _ActiveInputs.Remove(this as T);
    }

    /// <summary>
    /// Flag the player as connected.
    /// </summary>
    protected void OnEnable()
    {
        if (!_ActiveInputs.Contains(this as T))
            _ActiveInputs.Add(this as T);
    }

    /// <summary>
    /// Binds the player controls.
    /// </summary>
    public virtual void Bind(PlayerInput playerInput)
    {
        Input = playerInput;
        Input.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
    }

    /// <summary>
    /// Release the player controls.
    /// </summary>
    public virtual void Release()
    {
        Input = null;
    }
}