using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighScoresLadder : MonoBehaviour
{
    [SerializeField] private List<HighScoreMenuTemplate> highScoreTemplates;
    protected void OnEnable()
    {
        var highScores = GameController.Instance.GetHighScores();
        int templateIndex = 0;
        foreach (var entry in highScores.Take(highScoreTemplates.Count))
        {
            var template = highScoreTemplates[templateIndex];
            template.gameObject.SetActive(true);
            template.Init(++templateIndex, entry);
        }

    }

    protected void OnDisable()
    {
        foreach (var template in highScoreTemplates)
        {
            template.gameObject.SetActive(false);
        }
    }
}
