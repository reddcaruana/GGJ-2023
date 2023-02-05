using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ScoreListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ranking;
    [SerializeField] private TextMeshProUGUI team;
    [SerializeField] private TextMeshProUGUI score;

    /// <summary>
    /// The background image.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Component loading.
    /// </summary>
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Registers the score.
    /// </summary>
    /// <param name="teamName">The team name.</param>
    /// <param name="points">The point value.</param>
    public void RegisterScore(string teamName, int points)
    {
        team.SetText(teamName);
        score.SetText($"{points}");
    }

    /// <summary>
    /// Registers the score.
    /// </summary>
    /// <param name="rank">The rank number.</param>
    /// <param name="teamName">The team name.</param>
    /// <param name="points">The point value.</param>
    public void RegisterScore(int rank, string teamName, int points)
    {
        ranking.SetText($"{rank}.");
        team.SetText(teamName);
        score.SetText($"{points}");
    }

    public void Glow()
    {
        _animator.SetTrigger("Glow");
    }
}