using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{

    private TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var score = GManager.instance.GetScore();
        scoreText.text = score.ToString();
    }
}
