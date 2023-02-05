using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.controllers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreScreenManager : BaseCanvasManager
{
    /// <summary>
    /// The list of scores.
    /// </summary>
    private static List<ScoreEntry> scoreList = new List<ScoreEntry>();

    /// <summary>
    /// The last score entered.
    /// </summary>
    private static ScoreEntry lastScore = null;

    [Tooltip("The score text.")]
    [SerializeField] private TextMeshProUGUI text;

    [Tooltip("The letter input boxes.")]
    [SerializeField] private LetterInput[] letterInputs;

    [SerializeField] private ScoreListItem[] topScores;
    [SerializeField] private ScoreListItem lastRank;
    [SerializeField] private GameObject unqualifiedContainer;

    /// <summary>
    /// Flag to switch the screen on check.
    /// </summary>
    public bool returnToLobby { get; private set; } = false;
    
    /// <summary>
    /// Sets up the score text.
    /// </summary>
    private void Start()
    {
        int score = ScoreController.TotalScore();
        string points = score == 1 ? "pt" : "pts";
        text.SetText($"{score}<size=48>{points}</size>");
        
        BindControls();
    }

    /// <summary>
    /// Builds the score UI.
    /// </summary>
    private void BuildUI()
    {
        ScoreEntry[] top = scoreList.Take(4).ToArray();
        for (int i = 0; i < topScores.Length; i++)
        {
            if (i > top.Length - 1)
            {
                topScores[i].gameObject.SetActive(false);
                continue;
            }
            
            topScores[i].gameObject.SetActive(true);
            topScores[i].RegisterScore(top[i].Name, top[i].Value);
            if (lastScore == top[i]) topScores[i].Glow(); 
        }

        int index = scoreList.IndexOf(lastScore);
        unqualifiedContainer.SetActive(index > 3);
        
        if (index < 3) return;
        lastRank.RegisterScore(index + 1, lastScore.Name, lastScore.Value);
        lastRank.Glow();
    }

    /// <summary>
    /// Confirms input.
    /// </summary>
    public override void Check()
    {
        string teamName = string.Empty;
        foreach (LetterInput letter in letterInputs)
            teamName += letter.Value;

        ScoreEntry entry = new ScoreEntry(teamName, ScoreController.TotalScore());
        scoreList.Add(entry);
        scoreList.Sort(ScoreComparer);
        
        lastScore = entry;

        BuildUI();
        StartCoroutine(LobbyDelayRoutine());
    }

    /// <summary>
    /// Sorts the score entries.
    /// </summary>
    /// <param name="first">The first score.</param>
    /// <param name="second">The second score.</param>
    private int ScoreComparer(ScoreEntry first, ScoreEntry second)
    {
        return second.Value - first.Value;
    }

    private IEnumerator LobbyDelayRoutine()
    {
        yield return new WaitForSeconds(2f);
        returnToLobby = true;
    }

    /// <summary>
    /// Returns to the player lobby.
    /// </summary>
    public void ReturnToLobby()
    {
        SceneManager.LoadScene("PlayerLobby");
    }
}