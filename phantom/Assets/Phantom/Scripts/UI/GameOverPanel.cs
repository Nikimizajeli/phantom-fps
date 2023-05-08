using Phantom.Scripts.Configuration;
using TMPro;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameOverHeader;
    [SerializeField] private TextMeshProUGUI gameOverScore;

    public void Refresh(bool victory, int score)
    {
        gameOverHeader.text = victory ? TextConstants.Victory : TextConstants.Defeated;
        gameOverScore.text = score.ToString();
    }
}
