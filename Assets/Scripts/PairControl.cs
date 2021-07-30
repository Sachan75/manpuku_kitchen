using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PairControl : MonoBehaviour
{
    GameObject[] puyos;
    float[] puyox = new float[100];
    float[] puyoy = new float[100];

    bool isChecking = false;

    int chainCount = 0;

    int gameOverFlg = 0;

    void Start()
    {

    }

    void Update()
    {

        // 素材のペア解除前のため何もしない
        if (transform.childCount != 0)
        {
            return;
        }

        // 前回 Update 時のチェックが終わってない場合は何もしない
        if (isChecking)
        {
            return;
        }

        if (gameOverFlg == 1)
        {
            return;
        }

        setGameoverFlg();

        // 各素材の位置情報を取得
        getIngredientsPosition();

        // 各素材の落下＆連鎖状況をチェック
        CheckChildren();

    }

    /// <summary>
    /// 各素材の落下＆連鎖状況をチェック.
    /// </summary>
    /// <returns></returns>
    async void CheckChildren()
    {
        isChecking = true;

        var finishOrAwait = await CheckFalled();

        if (finishOrAwait == "finish")
        {
            // 連鎖後など下のぷよが落下前の場合は次ループにすすめる
            if (IsFallable())
            {
                Restart();
                isChecking = false;
                return;
            }

            chainCount++;
            FindObjectOfType<Delete>().init();
            int destroyCount = await FindObjectOfType<Delete>().puyoDestroy(chainCount);

            if (destroyCount == 0)
            {
                //ぷよセットオブジェクト（自分自身）を削除
                Destroy(gameObject);

                // 次のぷよセットを生成
                FindObjectOfType<Spawn>().NewMino();

                isChecking = false;
            }
            else
            {
                Restart();
                isChecking = false;
                return;
            }
        }
        else
        {
            isChecking = false;
        }
    }

    /// <summary>
    /// 各素材の画面上の位置を取得してまとめる.
    /// </summary>
    private void getIngredientsPosition()
    {
        this.puyos = GameObject.FindGameObjectsWithTag("puyo");
        Array.Sort(this.puyos, (a, b) => (int)a.transform.position.y - (int)b.transform.position.y);

        this.puyox = new float[100];
        this.puyoy = new float[100];

        int i = 0;
        foreach (GameObject puyoGo in this.puyos)
        {
            var puyo = puyoGo.GetComponent<Puyo>();
            if (puyo.isNext)
            {
                continue;
            }
            //丸め誤差解消
            this.puyox[i] = Mathf.RoundToInt(puyoGo.transform.position.x * 10.0f) / 10.0f;
            this.puyoy[i] = Mathf.RoundToInt(puyoGo.transform.position.y * 10.0f) / 10.0f;
            i++;
        }
    }

    /// <summary>
    /// 素材を全てチェックして、落下済み（ fallCompFlg > 0 ）かどうかを示す文字列を非同期で返す.
    /// </summary>
    /// <returns> finish: 落下済み / await: 落下中 </returns>
    async Task<string> CheckFalled()
    {
        var isFinishFall = true;
        await Task.Delay(0);

        foreach (GameObject puyoGo in this.puyos)
        {
            if (puyoGo == null)
            {
                continue;
            }
            var puyo = puyoGo.GetComponent<Puyo>();
            if (puyo == null)
            {
                continue;
            }
            if (puyo.isNext)
            {
                continue;
            }
            isFinishFall = isFinishFall && puyo.fallCompFlg > 0;
        }
        if (isFinishFall)
        {
            return "finish";
        }
        return "await";
    }

    /// <summary>
    /// 素材の中に落下可能なものがあるかを評価する.
    /// </summary>
    /// <returns>True: 落下中 / Flase: 落下済み</returns>
    bool IsFallable()
    {
        // どこかの列で最下行まできていない列がある場合は落下途中
        bool isFalled = true;

        foreach (var dict in YsPerX())
        {
            int i = 0;
            float last = 0.5f;
            foreach (var y in dict.Value)
            {
                if (i == 0)
                {
                    if (y > 1.5)
                    {
                        // 一番下が最下行ではない場合
                        isFalled = false;
                    }
                }
                else if (y - last > 1.0)
                {
                    // ぷよとぷよの間に隙間ができている場合
                    isFalled = false;
                }
                i++;
                last = y;
            }
        }
        // 落下していないものが一つでもある場合は落下可能として返却
        return !isFalled;
    }

    /// <summary>
    /// X列ごとに素材の高さデータをマッピングしたものを返す.
    /// </summary>
    /// <returns>key: X軸の座標, value: Y軸の座標のリスト</returns>
    Dictionary<float, List<float>> YsPerX()
    {
        var ysPerX = new Dictionary<float, List<float>>();
        int i = 0;
        foreach (float x in this.puyox)
        {
            // 枠外のデータは除外
            if (x < 5.0 || 10.0 < x)
            {
                continue;
            }

            if (ysPerX.ContainsKey(x))
            {
                ysPerX[x].Add(this.puyoy[i]);
            }
            else
            {
                ysPerX[x] = new List<float>();
                ysPerX[x].Add(this.puyoy[i]);
            }
            i++;
        }

        foreach (var dict in ysPerX)
        {
            dict.Value.Sort();
        }
        return ysPerX;
    }

    /// <summary>
    /// 素材の fallCompFlg をリセットして、落下を再スタートする.
    /// </summary>
    void Restart()
    {
        foreach (GameObject puyoGo in this.puyos)
        {
            if (puyoGo == null)
            {
                continue;
            }
            var puyo = puyoGo.GetComponent<Puyo>();
            if (puyo == null || puyo.isNext)
            {
                continue;
            }
            puyo.Restart();
        }
    }

    //ゲームオーバー処理
    //ぷよ出現時にぷよ出現位置にぷよがあった場合はゲームオーバーフラグをたてる
    void setGameoverFlg()
    {

        //ひとつ前のぷよの座標を取得
        //そのぷよの座標がぷよ出現位置の座標と同じ場合はゲームオーバーフラグを立てる
        int i = 0;
        foreach (float puyox in this.puyox)
        {
            if (puyox == 7.0f && puyoy[i] == 14.5f)
            {
                //ゲームオーバーフラグを立てる
                gameOverFlg = 1;
                GManager.instance.SetGameOverFlg();
                SceneManager.LoadScene("GameOver");
            }
            i++;
        }
    }
}
