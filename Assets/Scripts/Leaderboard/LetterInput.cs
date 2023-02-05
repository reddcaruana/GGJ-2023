using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LetterInput : MonoBehaviour
{
    /// <summary>
    /// The available letters.
    /// </summary>
    private static string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// The text component.
    /// </summary>
    private TextMeshProUGUI _text;
    
    /// <summary>
    /// The chosen index.
    /// </summary>
    private int _index = 0;

    /// <summary>
    /// The letter value.
    /// </summary>
    public char Value => Letters[_index];

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Increments the index.
    /// </summary>
    public void Increment()
    {
        _index = (_index + 1) % Letters.Length;
        _text.SetText(Value.ToString());
    }

    /// <summary>
    /// Decrements the index.
    /// </summary>
    public void Decrement()
    {
        _index = (_index + Letters.Length - 1) % Letters.Length;
        _text.SetText(Value.ToString());
    }
}