using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : BasePlayerManager<LeaderBoardInput>
{
    /// <summary>
    /// The last added score.
    /// </summary>
    public ScoreEntry recentScore;
    
    /// <summary>
    /// The list of scores.
    /// </summary>
    public readonly List<ScoreEntry> Scores = new List<ScoreEntry>();

    private void Start()
    {
        StartCoroutine(Bind());
        IEnumerator Bind()
        {
            yield return new WaitForSeconds(5);
            BindControls();
        }
    }

    /// <summary>
    /// Adds a new entry to the leaderboard.
    /// </summary>
    /// <param name="entry">The new entry.</param>
    public void AddScore(ScoreEntry entry)
    {
        Scores.Add(entry);
        Scores.Sort(ScoreComparer);

        recentScore = entry;
    }

    /// <summary>
    /// The score comparer.
    /// </summary>
    /// <param name="first">The lower score.</param>
    /// <param name="second">The higher score.</param>
    private int ScoreComparer(ScoreEntry first, ScoreEntry second)
    {
        return second.Value - first.Value;
    }
}
