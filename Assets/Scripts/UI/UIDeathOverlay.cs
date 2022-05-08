using TMPro;
using TubeMeshGeneration;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDeathOverlay : MonoBehaviour
{
    [SerializeField] AtomBaseVariable[] atomList;
    [SerializeField] private BoolVariable showStartOverlay;
    [SerializeField] private IntVariable score;
    [SerializeField] private TextMeshProUGUI scoreDeadText;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private GameObject[] tweenIn;
    [SerializeField] private LeanTweenType tweenType;

    private void Awake()
    {
        scoreDeadText.text = score.Value + "";
        SetHighscore();
        TweenUIElements();
    }

    private void TweenUIElements()
    {
        foreach (var obj in tweenIn) obj.transform.localScale = Vector3.zero;
        foreach (var obj in tweenIn) LeanTween.scale(obj, Vector3.one, 1f).setEase(tweenType);
    }

    private void SetHighscore()
    {
        int oldHighscore = PlayerPrefs.GetInt("Highscore");
        if (oldHighscore < score.Value)
        {
            PlayerPrefs.SetInt("Highscore", score.Value);
            highscoreText.text = "New Highscore!";
        }
        else
        {
            highscoreText.text = "Highscore: " + oldHighscore;
        }
    }

    public void RestartScene()
    {
        foreach (var atomBaseVariable in atomList)
        {
            atomBaseVariable.Reset();
        }

        Tube.ResetSize();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ActivateStartingScreen()
    {
        showStartOverlay.Value = true;
    }
    
}
