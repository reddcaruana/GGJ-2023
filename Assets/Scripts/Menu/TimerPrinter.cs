using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(OneShotTimer))]
public class TimerPrinter : MonoBehaviour
{
    [Tooltip("The countdown timer.")]
    [SerializeField] private TextMeshProUGUI countdownTimer;

    /// <summary>
    /// The one-shot timer component.
    /// </summary>
    private OneShotTimer _timer;

    /// <summary>
    /// Component allocation.
    /// </summary>
    private void Awake()
    {
        _timer = GetComponent<OneShotTimer>();
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    private void OnDisable()
    {
        _timer.Stop();
    }

    /// <summary>
    /// Plays the timer.
    /// </summary>
    private void OnEnable()
    {
        _timer.Play();
    }

    /// <summary>
    /// Prints the timer components remaining time.
    /// </summary>
    /// <param name="baseTimer">The timer component.</param>
    public void Print(BaseTimer baseTimer)
    {
        int duration = Mathf.FloorToInt(baseTimer.Remaining);
        string text = duration > 0 ? $"{duration:0}" : "Go!";
        
        countdownTimer.SetText(text);
    }
}
