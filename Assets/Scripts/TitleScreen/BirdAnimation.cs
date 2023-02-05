using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class BirdAnimation : MonoBehaviour
{
    [Tooltip("The start delay.")]
    [SerializeField] private float startDelay = 0.5f;
    
    [Tooltip("The wait range.")]
    [SerializeField] private float delay = 0.5f;

    [Tooltip("The move radius.")]
    [SerializeField] private float radius = 800;

    /// <summary>
    /// The animator component.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// The component rect transform.
    /// </summary>
    private RectTransform _rectTransform;
    
    /// <summary>
    /// The wait coroutine reference.
    /// </summary>
    private Coroutine _waitRoutine;

    /// <summary>
    /// Component loading.
    /// </summary>
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rectTransform = (RectTransform)transform;
        StartCoroutine(WaitRoutine(startDelay));
    }

    /// <summary>
    /// Stops coroutines on disable.
    /// </summary>
    private void OnDisable()
    {
        if (_waitRoutine != null)
            StopCoroutine(_waitRoutine);
    }
    
    /// <summary>
    /// Delays playback.
    /// </summary>
    public void Wait()
    {
        _waitRoutine = StartCoroutine(WaitRoutine(delay));
    }

    /// <summary>
    /// The wait coroutine.
    /// </summary>
    private IEnumerator WaitRoutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Vector2 pos = _rectTransform.anchoredPosition;
        pos.x = Random.Range(-radius, radius);
        _rectTransform.anchoredPosition = pos;
        
        _animator.SetTrigger("Go");
    }
}