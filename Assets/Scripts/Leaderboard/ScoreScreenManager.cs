using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts.controllers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreScreenManager : BaseCanvasManager
{
    /// <summary>
    /// The list of scores.
    /// </summary>
    private static readonly List<ScoreEntry> ScoreList = new List<ScoreEntry>();

    /// <summary>
    /// The score file.
    /// </summary>
    private readonly string _scoreFile = "/scores.egg";

    /// <summary>
    /// The score file path.
    /// </summary>
    private string FilePath => Application.persistentDataPath + _scoreFile;

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
        if (ScoreList.Count == 0)
            LoadScores();
        
        int score = ScoreController.TotalScore;
        string points = score == 1 ? "pt" : "pts";
        text.SetText($"{score}<size=48>{points}</size>");
        
        BindControls();
    }

    /// <summary>
    /// Builds the score UI.
    /// </summary>
    private void BuildUI()
    {
        ScoreEntry[] top = ScoreList.Take(4).ToArray();
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

        int index = ScoreList.IndexOf(lastScore);
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

        ScoreEntry entry = new ScoreEntry(teamName, ScoreController.TotalScore);
        ScoreList.Add(entry);
        ScoreList.Sort(ScoreComparer);
        
        lastScore = entry;
        SaveScores();

        BuildUI();
        StartCoroutine(LobbyDelayRoutine());
    }

    /// <summary>
    /// Loads the player scores.
    /// </summary>
    private void LoadScores()
    {
        try
        {
            if (!File.Exists(FilePath)) return;
        
            StreamReader reader = new StreamReader(FilePath);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line == string.Empty) break;
            
                ScoreList.Add(JsonUtility.FromJson<ScoreEntry>(line));
            }

            ScoreList.Sort(ScoreComparer);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Could not read from file.");
        }
    }

    /// <summary>
    /// Saves the player scores.
    /// </summary>
    private void SaveScores()
    {
        try
        {
            File.Delete(FilePath);

            StreamWriter writer = new StreamWriter(FilePath);
            foreach (var score in ScoreList)
                writer.WriteLine(JsonUtility.ToJson(score));
            writer.Close();
        }
        catch (Exception e)
        {
            Debug.LogWarning("Could not save to file.");
        }
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