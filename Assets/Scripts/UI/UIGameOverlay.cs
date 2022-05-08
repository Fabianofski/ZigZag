using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TubeMeshGeneration;

public class UIGameOverlay : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreText;


    public void UpdateScore(int score)
    {
        scoreText.text = score + "";
    }

}
