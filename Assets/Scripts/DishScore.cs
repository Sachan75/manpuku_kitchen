using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine;

public class DishScore : MonoBehaviour
{
    private TextMeshProUGUI dishScoreText;

    void Start()
    {
        dishScoreText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var score = GManager.instance.GetPlateCount();
        dishScoreText.text = score.ToString();
    }
}
