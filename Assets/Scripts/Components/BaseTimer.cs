using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An extensible implementation of a Timer component.
/// </summary>
public abstract class BaseTimer : MonoBehaviour
{
	public enum TimerMode { Scaled, Unscaled }

	[Tooltip("The timer duration.")]
	[SerializeField] private float duration = 15f;

	[Tooltip("Controls the time scale (speed) at which the timer counts.")]
	[SerializeField] private float timeScale = 1f;

	[Tooltip("Automatically starts the timer on scene start.")]
	[SerializeField] private bool playOnAwake = true;

	[Tooltip("The timer mode.")]
	[SerializeField] private TimerMode mode = TimerMode.Scaled;

	[Tooltip("The update event.")]
	[SerializeField] protected UnityEvent<BaseTimer> onUpdate = new UnityEvent<BaseTimer>();

	[Tooltip("The completion event.")]
	[SerializeField] protected UnityEvent onComplete = new UnityEvent();

	// The amount of elapsed time.
	private float _elapsed;

	// True if the timer is currently stopped.
	private bool _paused;

	/// <summary>
	/// Determines whether the timer has elapsed.
	/// </summary>
	public bool Complete
		=> Remaining <= 0;

	/// <summary>
	/// The timer progress.
	/// </summary>
	public float Duration
		=> duration;

	/// <summary>
    /// The amount of elapsed time.
    /// </summary>
	public float Elapsed
    {
		get => _elapsed;
		set
        {
			_elapsed = value;
			onUpdate.Invoke(this);
        }
    }

	/// <summary>
	/// Determines whether the timer is actively running.
	/// </summary>
	public bool IsPlaying
		=> Remaining > 0 && !_paused;

	/// <summary>
	/// Controls the way the time scale is set up.
	/// </summary>
	public TimerMode Mode
    {
		get => mode;
		set => mode = value;
	}

	/// <summary>
	/// The percentage value of elapsed time.
	/// </summary>
	public float Normalized => Mathf.Clamp01(Elapsed / Duration);

	/// <summary>
	/// The amount of remaining time.
	/// </summary>
	public float Remaining
		=> duration - _elapsed;

	/// <summary>
    /// The multiplier at which time is counted.
    /// </summary>
	public float TimeScale
    {
		get => timeScale;
		set => timeScale = value;
    }

	/// <summary>
    /// Setup and event invoking.
    /// </summary>
	protected virtual void Start()
    {
		_paused = !playOnAwake;
		Elapsed = 0;
    }

	/// <summary>
    /// Updates the timer value.
    /// </summary>
	protected abstract void Update();

	/// <summary>
    /// Pauses the timer.
    /// </summary>
	public void Pause()
		=> _paused = true;

	/// <summary>
    /// Plays the timer.
    /// </summary>
	public void Play()
		=> _paused = false;

	/// <summary>
    /// Sets the timer duration.
    /// <br/>
    /// This will cause the timer to stop and restart.
    /// </summary>
    /// <param name="newValue">The new value.</param>
	public void SetDuration(float newValue)
    {
		Stop();

		if (newValue < 0)
			throw new System.Exception("Timer value cannot be less than zero.");

		duration = newValue;
    }

	/// <summary>
    /// Resets the timer.
    /// </summary>
	public void Stop()
    {
		_paused = true;
		Elapsed = 0;
    }
}