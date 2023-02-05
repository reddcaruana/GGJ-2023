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
    /// Prints the timer component's remaining time as seconds.
    /// </summary>
    /// <param name="baseTimer">The timer component.</param>
    public void PrintSeconds(BaseTimer baseTimer)
    {
        int duration = Mathf.FloorToInt(baseTimer.Remaining);
        string text = duration > 0 ? $"{duration:0}" : "GO!";
        
        countdownTimer.SetText(text);
    }

    /// <summary>
    /// Prints the timer component's remaining time as minutes:seconds.
    /// </summary>
    /// <param name="baseTimer">The timer component.</param>
    public void PrintTime(BaseTimer baseTimer)
    {
        int duration = Mathf.FloorToInt(baseTimer.Remaining);
        
        int minutes = Mathf.FloorToInt(duration / 60f);
        int seconds = duration % 60;
        
        countdownTimer.SetText($"{minutes}:{seconds:00}");
    }
}
