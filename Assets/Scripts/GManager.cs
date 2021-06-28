using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using manpuku_kitchen.Utils;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;

    // スコア
    private int score = 0;

    // 素材を揃えたかどうか
    private bool hasCarrot, hasEgg, hasMeet, hasOnion, hasPotato;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// スコアを取得する.
    /// </summary>
    /// <returns></returns>
    public int GetScore()
    {
        return this.score;
    }

    /// <summary>
    /// 引数で渡されたスコアを加算する.
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        //TODO: スコアの倍率などをここ（GManager）側で管理する場合は、計算をここに書く
        this.score += score;
    }

    /// <summary>
    /// 引数に渡された素材が、すでに集めている（ぷよを消している）かを返す.
    /// </summary>
    /// <param name="ingredients">集めているか確認する素材</param>
    /// <returns> true : 集めている / false : 集めていない</returns>
    public bool HasIngredients(Ingredients ingredients)
    {
        switch (ingredients)
        {
            case Ingredients.CARROT:
                return hasCarrot;
            case Ingredients.EGG:
                return hasEgg;
            case Ingredients.MEET:
                return hasMeet;
            case Ingredients.ONION:
                return hasOnion;
            case Ingredients.POTATO:
                return hasPotato;
            default:
                return false;
        }
    }

    /// <summary>
    /// 引数に消したぷよ（素材）を渡すことで、集めた素材の状態を true にする.
    /// </summary>
    /// <param name="ingredients">消した素材</param>
    public void CollectIngredients(Ingredients ingredients)
    {
        switch (ingredients)
        {
            case Ingredients.CARROT:
                hasCarrot = true;
                break;
            case Ingredients.EGG:
                hasEgg = true;
                break;
            case Ingredients.MEET:
                hasMeet = true;
                break;
            case Ingredients.ONION:
                hasOnion = true;
                break;
            case Ingredients.POTATO:
                hasPotato = true;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 集めた素材をリセット（すべて false に）する.
    /// </summary>
    public void resetIngredients()
    {
        hasCarrot = hasEgg = hasMeet = hasOnion = hasPotato = false;
    }

}