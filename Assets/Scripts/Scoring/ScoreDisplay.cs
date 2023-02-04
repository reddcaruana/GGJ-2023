using Assets.Scripts.controllers;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
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
    }
}