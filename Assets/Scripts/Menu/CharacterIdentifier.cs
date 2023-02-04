using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterIdentifier : MonoBehaviour
{
    [Tooltip("The character ID.")]
    [SerializeField] private int referenceID;
    
    /// <summary>
    /// The character ID.
    /// </summary>
    public int ReferenceID => referenceID;

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
        Debug.Log($"{referenceID} {index}");
        
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = PlayerCursor.All.First(p => p.ID == index).Color;
    }
}