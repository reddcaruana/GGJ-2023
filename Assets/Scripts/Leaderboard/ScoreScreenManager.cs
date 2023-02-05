using Assets.Scripts.controllers;
using TMPro;
using UnityEngine;

public class ScoreScreenManager : BaseCanvasManager
{
    [Tooltip("The score text.")]
    [SerializeField] private TextMeshProUGUI text;

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

    public override void Check()
    {
        Debug.Log("Hi");
    }
}