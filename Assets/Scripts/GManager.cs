using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;

    // スコア
    private int score = 0;

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

    public int GetScore()
    {
        return this.score;
    }

    public void AddScore(int score)
    {
        //TODO: スコアの倍率などをここ（GManager）側で管理する場合は、計算をここに書く
        this.score += score;
    }

}