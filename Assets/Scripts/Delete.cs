using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using manpuku_kitchen.Utils;

public class Delete : MonoBehaviour
{

    GameObject[] puyos;
    List<int> samecolorset = new List<int>();
    float[] puyox = new float[100];
    float[] puyoy = new float[100];
    int[] checks = new int[100];

    // アニメーター
    private Animator animator;
    private Dictionary<Ingredients, List<int>> ingredientsCount;

    void Start()
    {

    }

    void Update()
    {

    }

    public void init()
    {
        this.puyos = GameObject.FindGameObjectsWithTag("puyo");

        int i = 0;
        foreach (GameObject puyo in this.puyos)
        {
            //checks[i]：i番ぷよの確認作業終了フラグ…0：未完了、1：完了
            this.checks[i] = 0;
            //puyox[i],puyoy[i]：i番ぷよの位置座標（丸め誤差対策済）
            this.puyox[i] = Mathf.RoundToInt(puyo.transform.position.x * 10.0f) / 10.0f;
            this.puyoy[i] = Mathf.RoundToInt(puyo.transform.position.y * 10.0f) / 10.0f;
            i++;
        }
    }

    async public Task<int> puyoDestroy(int chainCount)
    {

        int destroyCount = 0;
        List<int> samecolorset = new List<int>();
        int i = 0;
        foreach (GameObject puyo in this.puyos)
        {
            // カウンターリセット
            ResetIngredientsCount();

            Check(i);

            if (this.puyos[i] == null)
            {
                continue;
            }

            var myPuyo = this.puyos[i].GetComponent<Puyo>();
            var countList = this.ingredientsCount[myPuyo.ingredient];
            if (countList.Count >= 4)
            {
                foreach (int deleteIndex in countList)
                {
                    animator = this.puyos[deleteIndex].GetComponent<Animator>();
                    animator.SetBool("cutting", true);
                }

                await DelayMethod(300, () =>
                    {
                        foreach (int deleteIndex in countList)
                        {
                            Destroy(this.puyos[deleteIndex]);
                            destroyCount++;
                        }
                    });
                GManager.instance.CollectIngredients(myPuyo.ingredient);
            }

            // 得点計算
            var score = Scorer.CountScore(this.ingredientsCount, chainCount);
            GManager.instance.AddScore(score);

            i++;
        }
        return destroyCount;
    }

    public void Check(int i)
    {
        // this.samecolorset.Add(i);

        // チェック済みのぷよは二重カウントしないためにも読み飛ばし
        if (this.checks[i] == 1)
        {
            return;
        }

        // 自身のチェック完了（素材のカウンターをインクリメント）
        var myPuyo = this.puyos[i].GetComponent<Puyo>();
        this.ingredientsCount[myPuyo.ingredient].Add(i);
        this.checks[i] = 1;

        // 周りのぷよに同色がないかチェックしてある場合は再帰的にチェックを行う
        for (int j = 0; j < this.puyos.Length; j++)
        {
            // 未チェックのぷよのみを対象に
            if (this.checks[j] == 0)
            {

                // 上下左右のいずれかと同色の場合は Check メソッドをコール
                if (MatchUnder(i, j) || MatchOver(i, j) || MatchLeft(i, j) || MatchRight(i, j))
                {
                    Check(j);
                }
            }
        }
        return;
    }

    private bool MatchUnder(int index, int targetIndex)
    {
        return this.puyox[index] == this.puyox[targetIndex] &&
            this.puyoy[index] == this.puyoy[targetIndex] + 1.0f &&
            checkSameIngredients(index, targetIndex);
    }

    private bool MatchOver(int index, int targetIndex)
    {
        return this.puyox[index] == this.puyox[targetIndex] &&
            this.puyoy[index] == this.puyoy[targetIndex] - 1.0f &&
            checkSameIngredients(index, targetIndex);
    }

    private bool MatchLeft(int index, int targetIndex)
    {
        return this.puyox[index] == this.puyox[targetIndex] + 1.0f &&
            this.puyoy[index] == this.puyoy[targetIndex] &&
            checkSameIngredients(index, targetIndex);
    }

    private bool MatchRight(int index, int targetIndex)
    {
        return this.puyox[index] == this.puyox[targetIndex] - 1.0f &&
            this.puyoy[index] == this.puyoy[targetIndex] &&
            checkSameIngredients(index, targetIndex);
    }

    private bool checkSameIngredients(int index, int targetIndex)
    {
        var myPuyo = this.puyos[index].GetComponent<Puyo>();
        var targetPuyo = this.puyos[targetIndex].GetComponent<Puyo>();
        return myPuyo.ingredient == targetPuyo.ingredient;
    }

    private void ResetIngredientsCount()
    {
        if (this.ingredientsCount == null)
        {
            this.ingredientsCount = new Dictionary<Ingredients, List<int>>
            {
                {Ingredients.CARROT, new List<int>()},
                {Ingredients.EGG, new List<int>()},
                {Ingredients.MEET, new List<int>()},
                {Ingredients.ONION, new List<int>()},
                {Ingredients.POTATO, new List<int>()}
            };
        }
        else
        {
            this.ingredientsCount[Ingredients.CARROT] = new List<int>();
            this.ingredientsCount[Ingredients.EGG] = new List<int>();
            this.ingredientsCount[Ingredients.MEET] = new List<int>();
            this.ingredientsCount[Ingredients.ONION] = new List<int>();
            this.ingredientsCount[Ingredients.POTATO] = new List<int>();
        }

    }

    async Task<string> DelayMethod(int waitTime, Action action)
    {
        await Task.Delay(waitTime);
        action();
        return "finish";
    }


}