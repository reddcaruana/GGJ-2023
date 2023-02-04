using UnityEngine;

/// <summary>
/// A timer implementation that runs once.
/// </summary>
public class OneShotTimer : BaseTimer
{
    /// <summary>
    /// Updates the timer state
    /// </summary>
    protected override void Update()
    {
        if (!IsPlaying) return;

        Elapsed += TimeScale * (Mode.Equals(TimerMode.Scaled) ?
            Time.deltaTime : Time.unscaledDeltaTime);

        if (Elapsed < Duration) return;
        
        Elapsed = Duration;
        Pause();
        onComplete.Invoke();
    }
}