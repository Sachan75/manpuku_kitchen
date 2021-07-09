using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class Set : MonoBehaviour
{

    public float previousTime;
    // minoの落ちる時間
    public float fallTime = 1f;
    //落下完了フラグ
    public int fallCompFlg = 0;

    // mino回転
    public Vector3 rotationPoint;
    public int rotationFlg = 0; //1の時ぷよ（単体）の回転をしないようにする
    public int rotationCond = 0;//0:初期状態,1:1回転,2:2回転,3:3回転

    public GameObject[] puyos;

    float[] puyox = new float[100];
    float[] puyoy = new float[100];

    private void Start()
    {
        getFoodsPosition();
    }

    void Update()
    {
        MinoMovememt();
    }

    //フィールド上の食材（単体）の場所を取得
    private void getFoodsPosition()
    {
        this.puyos = GameObject.FindGameObjectsWithTag("puyo");

        int i = 0;
        foreach (GameObject puyo in this.puyos)
        {
            //丸め誤差解消
            this.puyox[i] = Mathf.RoundToInt(puyo.transform.position.x * 10.0f) / 10.0f;
            this.puyoy[i] = Mathf.RoundToInt(puyo.transform.position.y * 10.0f) / 10.0f;
            i++;
        }
    }

    private void MinoMovememt()
    {
        // 左矢印キーで左に動く
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);

            //エリア外に出てしまう場合は、元の位置に戻す
            if (!ValidMovement())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }
        // 右矢印キーで右に動く
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);

            //エリア外に出てしまう場合は元の位置に戻す
            if (!ValidMovement())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }
        // 自動で下に移動させつつ、下矢印キーでも移動する
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousTime >= fallTime)
        {
            //エリア外にでるorほかのぷよの上に着地した場合はSTOP
            if (!ValidMovement())
            {
                fallCompFlg = 1;
                this.enabled = false;
            }
            else
            {
                transform.position += new Vector3(0, -1, 0);
            }
            previousTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // ブロックの回転
            rotationFlg = 0;
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            rotationCond += 1;
            if (!ValidMovement())
            {
                rotationCond -= 1;
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                rotationFlg = 1;
            }
            if (rotationCond == 4)
            {
                rotationCond = 0;
            }
        }
    }

    // ぷよの移動範囲の制御
    public bool ValidMovement()
    {
        //親オブジェクトの座標
        double roundX = Mathf.RoundToInt(gameObject.transform.position.x * 10.0f) / 10.0f;
        double roundY = Mathf.RoundToInt(gameObject.transform.position.y * 10.0f) / 10.0f;

        foreach (Transform children in transform)
        {
            // ぷよがステージよりはみ出さないように制御
            if (rotationCond == 2)
            {
                // 反転（90°）の場合
                if (roundY <= 2.5)
                {
                    DivideIngredient();
                    return false;
                }
            }
            else
            {
                // 反転意外の場合
                if (roundY <= 1.5)
                {
                    DivideIngredient();
                    return false;
                }
            }
            //子オブジェクトの座標
            double childX = Mathf.RoundToInt(children.transform.position.x * 10.0f) / 10.0f;

            if (childX < 5.0 || 10.0 < childX)
            {
                return false;
            }
        }

        //ぷよの上に落下するときの判定
        int i = 0;
        foreach (GameObject puyo in this.puyos)
        {
            // そのまま（0°）
            if (rotationCond == 0)
            {
                //落下終了条件
                if (roundX == this.puyox[i] && roundY == this.puyoy[i] + 1.0f)
                {
                    DivideIngredient();
                    return false;
                }
            }
            // 90°
            else if (rotationCond == 1)
            {
                if (roundX == this.puyox[i] && roundY == this.puyoy[i] + 1.0f || roundX == this.puyox[i] + 1.0f && roundY == this.puyoy[i] + 1.0f)
                {
                    DivideIngredient();
                    return false;
                }
            }
            //180°
            else if (rotationCond == 2)
            {
                if (roundX == this.puyox[i] && roundY == this.puyoy[i] + 2.0f)
                {
                    DivideIngredient();
                    return false;
                }
            }
            //270°
            else if (rotationCond == 3)
            {
                if (roundX == this.puyox[i] && roundY == this.puyoy[i] + 1.0f || roundX == this.puyox[i] - 1.0f && roundY == this.puyoy[i] + 1.0f)
                {
                    DivideIngredient();
                    return false;
                }
            }
            i++;
        }
        return true;
    }

    async private void DivideIngredient()
    {

        //親子関係の解除
        gameObject.transform.DetachChildren();

        //ぷよセットオブジェクト（自分自身）を削除
        Destroy(gameObject);

        for (; ; )
        {
            var finishOrAwait = await CheckFall();
            Debug.Log(finishOrAwait);

            if (finishOrAwait == "finish")
            {
                // 連鎖後など下のぷよが落下前の場合は次ループにすすめる
                if (!CheckFalled())
                {
                    Debug.Log("落ちきってないので再ループ");
                    Restart();
                    continue;
                }

                FindObjectOfType<Delete>().init();
                int destroyCount = await FindObjectOfType<Delete>().puyoDestroy();

                Debug.Log("Set:削除件数➜" + destroyCount);
                if (destroyCount == 0)
                {
                    FindObjectOfType<Spawn>().NewMino();
                }
                else
                {
                    // 再処理
                    Restart();
                    continue;
                }
                break;
            }
        }
    }

    bool CheckFalled()
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
                    // 一番下が最下行ではない場合
                    Debug.Log("一番下まで落ちてない！");
                    if (y > 1.5)
                    {
                        isFalled = false;
                    }
                }
                else if (y - last > 1.0)
                {
                    // ぷよとぷよの間に隙間ができている場合
                    Debug.Log("すきまができている！");
                    isFalled = false;
                }
                i++;
                last = y;
            }
        }
        // 全部落下済み
        return isFalled;
    }

    void Restart()
    {
        var puyos = GameObject.FindGameObjectsWithTag("puyo");
        foreach (GameObject puyoGo in puyos)
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
            puyo.Restart();
        }
    }

    async Task<string> CheckFall()
    {
        var puyos = GameObject.FindGameObjectsWithTag("puyo");
        var isFinishFall = true;
        await Task.Delay(100);
        foreach (GameObject puyoGo in puyos)
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
            isFinishFall = isFinishFall && puyo.fallCompFlg > 0;
        }
        if (isFinishFall)
        {
            return "finish";
        }
        return "await";
    }

    Dictionary<float, List<float>> YsPerX()
    {
        // 座標の再取得
        getFoodsPosition();

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

}
