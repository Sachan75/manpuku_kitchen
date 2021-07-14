using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using manpuku_kitchen.Utils;

public class Puyo : MonoBehaviour
{
    GameObject[] puyos;
    //GameObject Director;
    float[] puyox = new float[100];
    float[] puyoy = new float[100];

    public float previousTime;

    // mino回転
    public Vector3 rotationPoint;

    [Header("素材の種類")]
    public Ingredients ingredient;

    //落下完了フラグ
    public int fallCompFlg = 0;

    private void Start()
    {

    }

    void Update()
    {
        // 着地完了しているため読み飛ばし
        if (fallCompFlg > 0)
        {
            return;
        }

        MinoMovememt();
    }

    public void Restart()
    {
        // ペア分解後➜リスタート
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

            Observer();

            //下に食材があるorエリアの下端まで来た時
            if (!ValidMovement())
            {
                fallCompFlg++;
            }
            //まだ下に移動できるとき
            else if (ValidMovement())
            {
                transform.position += new Vector3(0, -1, 0);
            }
            previousTime = Time.time;
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
        int i = 0;
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

        this.puyox = new float[100];
        this.puyoy = new float[100];

        foreach (GameObject puyo in this.puyos)
        {
            //丸め誤差解消（フィールト中の全ぷよの位置）
            this.puyox[i] = Mathf.RoundToInt(puyo.transform.position.x * 10.0f) / 10.0f;
            this.puyoy[i] = Mathf.RoundToInt(puyo.transform.position.y * 10.0f) / 10.0f;
            i++;
        }
    }
}