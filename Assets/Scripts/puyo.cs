using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puyo : MonoBehaviour
{
    GameObject[] puyos;
    GameObject Director;
    public int num = 0;    //num=0で落下し、終了時にfallcheckへ、num=1なら休止
    float[] puyox = new float[100];
    float[] puyoy = new float[100];

    public float previousTime;

    // mino回転
    public Vector3 rotationPoint;

    private void Start()
    {
        
    }



    void Update()
    {
        int i = 0;
        this.Director = GameObject.Find("Director");
        this.puyos = GameObject.FindGameObjectsWithTag("puyo");
        foreach (GameObject puyo in this.puyos)
        {
            //丸め誤差解消（フィールト中の全ぷよの位置）
            this.puyox[i] = Mathf.RoundToInt(puyo.transform.position.x * 10.0f) / 10.0f;
            this.puyoy[i] = Mathf.RoundToInt(puyo.transform.position.y * 10.0f) / 10.0f;
            i++;
        }

        MinoMovememt();

        i = 0;
        //丸め誤差解消（自分の今の位置）
        double nowx = Mathf.RoundToInt(transform.position.x * 10.0f) / 10.0f;
        double nowy = Mathf.RoundToInt(transform.position.y * 10.0f) / 10.0f;

        if (this.num == 1) return;  //落下完了済なので以下の処理不要
        if (transform.root.gameObject == gameObject)
        {
            //コンビ解散後の挙動を記述
            if (nowy == 1.5f)
            {
                this.num = 1;   //落下完了をお知らせ
                return;
            }
            i = 0;
            foreach (GameObject puyo in this.puyos)
            {
                if (nowx == this.puyox[i] && nowy == this.puyoy[i] + 1.0f)
                {
                    this.num = 1;   //落下完了をお知らせ
                    return;
                }
                i++;
            }
            //落下完了していないので引き続き落下
            transform.Translate(0, -1.0f, 0, Space.World);
        }

    }

    private void MinoMovememt()
    {
        if (transform.root.gameObject != gameObject)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // ぷよの回転
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);

                if (!ValidMovement())
                {
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), +90);
                    
                }


            }
        }
    }

    // minoの移動範囲の制御
    bool ValidMovement()
    {


        foreach (Transform children in transform)
        {
            double roundX = Mathf.RoundToInt(children.transform.position.x);
            double roundY = Mathf.RoundToInt(children.transform.position.y);

            // minoがステージよりはみ出さないように制御
            if (roundX <= 4.0 || roundX >= 11.0 || roundY <= 1.0)
            {
                return false;
            }

        }

        return true;
    }





}
