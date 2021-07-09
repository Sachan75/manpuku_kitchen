using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using manpuku_kitchen.Utils;

public class Puyo : MonoBehaviour
{
    GameObject[] puyos;
    //GameObject Director;
    public int num = 0;    //num=0で落下し、終了時にfallcheckへ、num=1なら休止
    float[] puyox = new float[100];
    float[] puyoy = new float[100];

    public float previousTime;

    // mino回転
    public Vector3 rotationPoint;

    [Header("素材の種類")]
    public Ingredients ingredient;

    // minoの落ちる時間
    public float fallTime = 5.0f;
    //落下完了フラグ
    public int fallCompFlg = 0;

    private void Start()
    {

    }



    void Update()
    {

        if (fallCompFlg > 0)
        {
            Debug.Log(ingredient + ": ペア分解後➜着地完了");
            return;
        }


        MinoMovememt();

        //i = 0;
        ////丸め誤差解消（自分の今の位置）
        //double nowx = Mathf.RoundToInt(transform.position.x * 10.0f) / 10.0f;
        //double nowy = Mathf.RoundToInt(transform.position.y * 10.0f) / 10.0f;

        //if (this.num == 1) return;  //落下完了済なので以下の処理不要
        //if (transform.root.gameObject == gameObject)
        //{
        //    //コンビ解散後の挙動を記述
        //    if (nowy == 1.5f)
        //    {
        //        this.num = 1;   //落下完了をお知らせ
        //        FindObjectOfType<Delete>().init();
        //        FindObjectOfType<Delete>().puyoDestroy();
        //        return;
        //    }
        //    i = 0;
        //    foreach (GameObject puyo in this.puyos)
        //    {
        //        if (nowx == this.puyox[i] && nowy == this.puyoy[i] + 1.0f)
        //        {
        //            this.num = 1;   //落下完了をお知らせ
        //            FindObjectOfType<Delete>().init();
        //            FindObjectOfType<Delete>().puyoDestroy();
        //            return;
        //        }
        //        i++;
        //    }
        //    //落下完了していないので引き続き落下
        //    transform.Translate(0, -1.0f, 0, Space.World);
        //}

    }

    public void Restart()
    {
        Debug.Log(ingredient + ": ペア分解後➜リスタート");
        fallCompFlg = 0;
    }

    private void MinoMovememt()
    {
        //着地前の単体食材の動き
        if (transform.root.gameObject != gameObject)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // ぷよの回転
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);

                if (FindObjectOfType<Set>().rotationFlg == 1)
                {
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), +90);

                }


            }
        }
        //着地後の動き
        else if (transform.root.gameObject == gameObject)
        {

            if (Time.time - previousTime >= fallTime)
            {
                Observer();

                Debug.Log(ingredient + ": ペア分解後");

                //下に食材があるorエリアの下端まで来た時
                if (!ValidMovement())
                {

                    Debug.Log(ingredient + ": ペア分解後➜着地");
                    fallCompFlg++;

                    // FindObjectOfType<Delete>().init();
                    // FindObjectOfType<Delete>().puyoDestroy();
                    //this.enabled = false;
                }
                //まだ下に移動できるとき
                else if (ValidMovement())
                {
                    Debug.Log(ingredient + ": ペア分解後➜落下中");
                    transform.position += new Vector3(0, -1, 0);
                }

                previousTime = Time.time;

            }
        }
    }

    // minoの移動範囲の制御
    bool ValidMovement()
    {

        double roundX = Mathf.RoundToInt(transform.position.x * 10.0f) / 10.0f;
        double roundY = Mathf.RoundToInt(transform.position.y * 10.0f) / 10.0f;

        // minoがステージよりはみ出さないように制御
        if (roundX <= 4.0 || roundX >= 11.0 || roundY <= 2.0)
        {
            return false;
        }

        //this.puyos = GameObject.FindGameObjectsWithTag("puyo");

        int i = 0;
        foreach (GameObject puyo in this.puyos)
        {
            //丸め誤差解消
            this.puyox[i] = Mathf.RoundToInt(puyo.transform.position.x * 10.0f) / 10.0f;
            this.puyoy[i] = Mathf.RoundToInt(puyo.transform.position.y * 10.0f) / 10.0f;
            i++;

        }
        i = 0;
        foreach (float x in this.puyox)
        {

            //落下終了条件
            if (roundX == this.puyox[i] && roundY == this.puyoy[i] + 1.0f)
            {
                return false;
            }
            i++;
        }

        return true;
    }

    private void Observer()
    {

        int i = 0;
        this.puyos = GameObject.FindGameObjectsWithTag("puyo");
        foreach (GameObject puyo in this.puyos)
        {
            //丸め誤差解消（フィールト中の全ぷよの位置）
            this.puyox[i] = Mathf.RoundToInt(puyo.transform.position.x * 10.0f) / 10.0f;
            this.puyoy[i] = Mathf.RoundToInt(puyo.transform.position.y * 10.0f) / 10.0f;
            i++;
        }

    }





}