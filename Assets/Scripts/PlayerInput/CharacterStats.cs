using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    /// <summary>
    /// The character ID.
    /// </summary>
    public int ID { get; private set; }

    /// <summary>
    /// Sets the character ID.
    /// </summary>
    /// <param name="id">The new character ID.</param>
    public void SetID(int id)
    {
        ID = id;
    }
}