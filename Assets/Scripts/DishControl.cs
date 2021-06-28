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
    [Header("素材のゲームオブジェクト１")]
    public GameObject ingredientsObj1;
    [Header("素材のゲームオブジェクト２")]
    public GameObject ingredientsObj2;
    [Header("素材のゲームオブジェクト３")]
    public GameObject ingredientsObj3;
    [Header("素材のゲームオブジェクト４")]
    public GameObject ingredientsObj4;
    [Header("丸（素材のチェック用）のゲームオブジェクト１")]
    public GameObject maruObj1;
    [Header("丸（素材のチェック用）のゲームオブジェクト２")]
    public GameObject maruObj2;
    [Header("丸（素材のチェック用）のゲームオブジェクト３")]
    public GameObject maruObj3;
    [Header("丸（素材のチェック用）のゲームオブジェクト４")]
    public GameObject maruObj4;

    // お題（クリアする食事）
    private Dish dishTheme;
    // お題（クリアする食事）の画像
    private Image dishImage;
    // お題（クリアする食事）のスプライト
    private Sprite curry, boiledEgg, friedpotato, gratin, humberg, omulet, stew, tonkatsu;
    // 素材の画像（４枚分）
    private Image ingredientsImage1, ingredientsImage2, ingredientsImage3, ingredientsImage4;
    // 素材のスプライト
    private Sprite carrot, egg, meet, onion, potato;
    // 素材のゲームオブジェクト１～４にどの素材があてられてるかを管理するマップ
    private Dictionary<int, Ingredients> dispIngredientsMap;
    void Start()
    {
        // 画像の読み込み 
        InitDishImage();
        InitIngredientsImage();

        // 初回のお題を決定
        ResetDishTheme();
    }

    // Update is called once per frame
    void Update()
    {
        if (GManager.instance.HasIngredients(dispIngredientsMap[1]))
        {
            maruObj1.SetActive(true);
            ingredientsImage1.color = Color.gray;
        }
        if (GManager.instance.HasIngredients(dispIngredientsMap[2]))
        {
            maruObj2.SetActive(true);
            ingredientsImage2.color = Color.gray;
        }
        if (GManager.instance.HasIngredients(dispIngredientsMap[3]))
        {
            maruObj3.SetActive(true);
            ingredientsImage3.color = Color.gray;
        }
        if (GManager.instance.HasIngredients(dispIngredientsMap[4]))
        {
            maruObj4.SetActive(true);
            ingredientsImage4.color = Color.gray;
        }

        // すべての素材が揃って料理完成！
        if (checkIngredients(1) && checkIngredients(2) && checkIngredients(3) && checkIngredients(4))
        {
            // TODO 料理ができたアニメーションとか？

            // 次のお題を表示
            ResetDishTheme();
        }
    }

    /// <summary>
    /// 素材が集まっているかチェックする.
    /// Ingredients.NONE（素材なし）の場合も true を返す.
    /// </summary>
    /// <param name="i">表示されている素材の番号（１～４）</param>
    /// <returns>true : 素材が集まっている / false : 集まっていない</returns>
    private bool checkIngredients(int i)
    {
        return GManager.instance.HasIngredients(dispIngredientsMap[i]) ||
            dispIngredientsMap[i] == Ingredients.NONE;
    }

    private void ResetDishTheme()
    {
        // クリア
        ClearDishTheme();

        // お題を決定
        dishTheme = LoadDishTheme();

        // お題の画像を表示
        LoadDishSprite();
        dishObj.SetActive(true);

        // 素材の画像を表示
        LoadIngredientsSprite();
        ingredientsObj1.SetActive(true);
        ingredientsObj2.SetActive(true);
        ingredientsObj3.SetActive(true);
        ingredientsObj4.SetActive(true);
    }

    private void ClearDishTheme()
    {
        // お題（料理）のクリア
        dishObj.SetActive(true);
        // 素材のクリア
        ingredientsObj1.SetActive(false);
        ingredientsObj2.SetActive(false);
        ingredientsObj3.SetActive(false);
        ingredientsObj4.SetActive(false);
        ingredientsImage1.color = Color.white;
        ingredientsImage2.color = Color.white;
        ingredientsImage3.color = Color.white;
        ingredientsImage4.color = Color.white;
        // 素材チェックのクリア
        maruObj1.SetActive(false);
        maruObj2.SetActive(false);
        maruObj3.SetActive(false);
        maruObj4.SetActive(false);
        // GManager側もリセット
        GManager.instance.ResetIngredients();
    }

    /// <summary>
    /// お題（食事）の画像を読み込む
    /// </summary>
    private void InitDishImage()
    {
        dishImage = dishObj.GetComponent<Image>();

        curry = Resources.Load<Sprite>("Sprites/DishTheme/curry");
        boiledEgg = Resources.Load<Sprite>("Sprites/DishTheme/boiledegg");
        friedpotato = Resources.Load<Sprite>("Sprites/DishTheme/friedpotato");
        gratin = Resources.Load<Sprite>("Sprites/DishTheme/gratin");
        humberg = Resources.Load<Sprite>("Sprites/DishTheme/humberg");
        omulet = Resources.Load<Sprite>("Sprites/DishTheme/omulet");
        stew = Resources.Load<Sprite>("Sprites/DishTheme/stew");
        tonkatsu = Resources.Load<Sprite>("Sprites/DishTheme/tonkatsu");
    }

    /// <summary>
    /// 素材の画像を読み込む
    /// </summary>
    private void InitIngredientsImage()
    {
        ingredientsImage1 = ingredientsObj1.GetComponent<Image>();
        ingredientsImage2 = ingredientsObj2.GetComponent<Image>();
        ingredientsImage3 = ingredientsObj3.GetComponent<Image>();
        ingredientsImage4 = ingredientsObj4.GetComponent<Image>();

        carrot = Resources.Load<Sprite>("Sprites/Ingredients/carrot");
        egg = Resources.Load<Sprite>("Sprites/Ingredients/egg");
        meet = Resources.Load<Sprite>("Sprites/Ingredients/meet");
        onion = Resources.Load<Sprite>("Sprites/Ingredients/onion");
        potato = Resources.Load<Sprite>("Sprites/Ingredients/potato");
    }

    private Dish LoadDishTheme()
    {
        var dishArray = Enum.GetValues(typeof(Dish));
        var random = UnityEngine.Random.Range(0, dishArray.Length);

        return (Dish)dishArray.GetValue(random);
    }

    private void LoadDishSprite()
    {
        switch (dishTheme)
        {
            case Dish.CURRY:
                dishImage.sprite = curry;
                break;
            case Dish.BOILED_EGG:
                dishImage.sprite = boiledEgg;
                break;
            case Dish.FRIED_POTATO:
                dishImage.sprite = friedpotato;
                break;
            case Dish.GRATIN:
                dishImage.sprite = gratin;
                break;
            case Dish.HUMBERG:
                dishImage.sprite = humberg;
                break;
            case Dish.OMULET:
                dishImage.sprite = omulet;
                break;
            case Dish.STEW:
                dishImage.sprite = stew;
                break;
            case Dish.TONKATSU:
                dishImage.sprite = tonkatsu;
                break;
            default:
                dishImage.sprite = curry;
                break;
        }
    }

    private void LoadIngredientsSprite()
    {
        // 素材のゲームオブジェクト１～４にどの素材があてられてるかをリセット
        ResetDispIngredientsMap();

        switch (dishTheme)
        {
            case Dish.CURRY:
                ingredientsImage1.sprite = meet;
                ingredientsImage2.sprite = potato;
                ingredientsImage3.sprite = carrot;
                ingredientsImage4.sprite = onion;
                dispIngredientsMap[1] = Ingredients.MEET;
                dispIngredientsMap[2] = Ingredients.POTATO;
                dispIngredientsMap[3] = Ingredients.CARROT;
                dispIngredientsMap[4] = Ingredients.ONION;
                break;
            case Dish.BOILED_EGG:
                ingredientsImage1.sprite = egg;
                ingredientsImage2.sprite = null;
                ingredientsImage3.sprite = null;
                ingredientsImage4.sprite = null;
                dispIngredientsMap[1] = Ingredients.EGG;
                dispIngredientsMap[2] = Ingredients.NONE;
                dispIngredientsMap[3] = Ingredients.NONE;
                dispIngredientsMap[4] = Ingredients.NONE;
                break;
            case Dish.FRIED_POTATO:
                ingredientsImage1.sprite = potato;
                ingredientsImage2.sprite = null;
                ingredientsImage3.sprite = null;
                ingredientsImage4.sprite = null;
                dispIngredientsMap[1] = Ingredients.POTATO;
                dispIngredientsMap[2] = Ingredients.NONE;
                dispIngredientsMap[3] = Ingredients.NONE;
                dispIngredientsMap[4] = Ingredients.NONE;
                break;
            case Dish.GRATIN:
                ingredientsImage1.sprite = meet;
                ingredientsImage2.sprite = potato;
                ingredientsImage3.sprite = onion;
                ingredientsImage4.sprite = null;
                dispIngredientsMap[1] = Ingredients.MEET;
                dispIngredientsMap[2] = Ingredients.POTATO;
                dispIngredientsMap[3] = Ingredients.ONION;
                dispIngredientsMap[4] = Ingredients.NONE;
                break;
            case Dish.HUMBERG:
                ingredientsImage1.sprite = meet;
                ingredientsImage2.sprite = egg;
                ingredientsImage3.sprite = onion;
                ingredientsImage4.sprite = null;
                dispIngredientsMap[1] = Ingredients.MEET;
                dispIngredientsMap[2] = Ingredients.EGG;
                dispIngredientsMap[3] = Ingredients.ONION;
                dispIngredientsMap[4] = Ingredients.NONE;
                break;
            case Dish.OMULET:
                ingredientsImage1.sprite = egg;
                ingredientsImage2.sprite = onion;
                ingredientsImage3.sprite = carrot;
                ingredientsImage4.sprite = null;
                dispIngredientsMap[1] = Ingredients.EGG;
                dispIngredientsMap[2] = Ingredients.ONION;
                dispIngredientsMap[3] = Ingredients.CARROT;
                dispIngredientsMap[4] = Ingredients.NONE;
                break;
            case Dish.STEW:
                ingredientsImage1.sprite = meet;
                ingredientsImage2.sprite = potato;
                ingredientsImage3.sprite = carrot;
                ingredientsImage4.sprite = onion;
                dispIngredientsMap[1] = Ingredients.MEET;
                dispIngredientsMap[2] = Ingredients.POTATO;
                dispIngredientsMap[3] = Ingredients.CARROT;
                dispIngredientsMap[4] = Ingredients.ONION;
                break;
            case Dish.TONKATSU:
                ingredientsImage1.sprite = meet;
                ingredientsImage2.sprite = egg;
                ingredientsImage3.sprite = null;
                ingredientsImage4.sprite = null;
                dispIngredientsMap[1] = Ingredients.MEET;
                dispIngredientsMap[2] = Ingredients.EGG;
                dispIngredientsMap[3] = Ingredients.NONE;
                dispIngredientsMap[4] = Ingredients.NONE;
                break;
            default:
                ingredientsImage1.sprite = meet;
                ingredientsImage2.sprite = potato;
                ingredientsImage3.sprite = carrot;
                ingredientsImage4.sprite = onion;
                dispIngredientsMap[1] = Ingredients.MEET;
                dispIngredientsMap[2] = Ingredients.POTATO;
                dispIngredientsMap[3] = Ingredients.CARROT;
                dispIngredientsMap[4] = Ingredients.ONION;
                break;
        }
    }

    private void ResetDispIngredientsMap()
    {
        if (dispIngredientsMap == null)
        {
            dispIngredientsMap = new Dictionary<int, Ingredients>
            {
                {1, Ingredients.NONE},
                {2, Ingredients.NONE},
                {3, Ingredients.NONE},
                {4, Ingredients.NONE}
            };
        }
        else
        {
            dispIngredientsMap[1] = Ingredients.NONE;
            dispIngredientsMap[2] = Ingredients.NONE;
            dispIngredientsMap[3] = Ingredients.NONE;
            dispIngredientsMap[4] = Ingredients.NONE;
        }
    }
}
