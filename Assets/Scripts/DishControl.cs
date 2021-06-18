using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using manpuku_kitchen.Utils;

public class DishControl : MonoBehaviour
{
    [Header("食事のゲームオブジェクト")]
    public GameObject dishObj;

    // お題（クリアする食事）
    private Dish dishTheme;
    // お題（クリアする食事）の画像
    private Image dishImage;

    private Sprite curry, egg, friedpotato, gratin, humberg, omulet, stew, tonkatsu;
    void Start()
    {
        // お題（食事）の画像を読み込み
        dishImage = dishObj.GetComponent<Image>();
        curry = Resources.Load<Sprite>("Sprites/DishTheme/curry");
        egg = Resources.Load<Sprite>("Sprites/DishTheme/egg");
        friedpotato = Resources.Load<Sprite>("Sprites/DishTheme/friedpotato");
        gratin = Resources.Load<Sprite>("Sprites/DishTheme/gratin");
        humberg = Resources.Load<Sprite>("Sprites/DishTheme/humberg");
        omulet = Resources.Load<Sprite>("Sprites/DishTheme/omulet");
        stew = Resources.Load<Sprite>("Sprites/DishTheme/stew");
        tonkatsu = Resources.Load<Sprite>("Sprites/DishTheme/tonkatsu");

        // 初回のお題を決定
        dishTheme = loadDishTheme();
        Debug.Log(dishTheme);
        dishImage.sprite = loadDishSprite();
        dishObj.SetActive(true);
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

    private Sprite loadDishSprite()
    {
        switch (dishTheme)
        {
            case Dish.CURRY:
                return curry;
            case Dish.EGG:
                return egg;
            case Dish.FRIED_POTATO:
                return friedpotato;
            case Dish.GRATIN:
                return gratin;
            case Dish.HUMBERG:
                return humberg;
            case Dish.OMULET:
                return omulet;
            case Dish.STEW:
                return stew;
            case Dish.TONKATSU:
                return tonkatsu;
            default:
                return curry;
        }
    }
}
