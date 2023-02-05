using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterReference : MonoBehaviour
{
    [Tooltip("The character ID.")]
    [SerializeField] private int referenceID;

    [Tooltip("The cursor point.")]
    [SerializeField] private Transform cursorPoint;
    
    /// <summary>
    /// The character ID.
    /// </summary>
    public int ReferenceID => referenceID;

    /// <summary>
    /// The cursor point position.
    /// </summary>
    public Vector3 CursorPoint => cursorPoint.position;

    /// <summary>
    /// The player ID.
    /// </summary>
    public int PlayerID { get; private set; } = -1;

    /// <summary>
    /// The status of this character.
    /// </summary>
    public bool IsTaken => PlayerID > -1;

    /// <summary>
    /// Clears the player ID.
    /// </summary>
    public void Clear()
    {
        PlayerID = -1;
        
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;
    }

    /// <summary>
    /// Sets the player ID.
    /// </summary>
    /// <param name="index">The player index.</param>
    public void SetPlayer(int index)
    {
        PlayerID = index;
        
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = PlayerCursor.All.First(p => p.ID == index).Color;
    }
}