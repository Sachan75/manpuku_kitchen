using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using manpuku_kitchen.Utils;

public class DishControl : MonoBehaviour
{
    // お題（クリアする食事）
    private Dish dishTheme;
    void Start()
    {
        dishTheme = loadDishTheme();
        Debug.Log(dishTheme);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private Dish loadDishTheme()
    {
        var dishArray = Enum.GetValues(typeof(Dish));
        var random = UnityEngine.Random.Range(0, dishArray.Length);
        return (Dish)dishArray.GetValue(random);
    }
}
