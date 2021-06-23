using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set : MonoBehaviour
{

    public float previousTime;
    // minoの落ちる時間
    public float fallTime = 1f;
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



    void Update()
    {
        MinoMovememt();

    }

    private void MinoMovememt()
    {
        // 左矢印キーで左に動く
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            
            if (!ValidMovement())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }

        }
        // 右矢印キーで右に動く
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            
            if (!ValidMovement())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }
        // 自動で下に移動させつつ、下矢印キーでも移動する
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousTime >= fallTime)
        {
            transform.position += new Vector3(0, -1, 0);

            if (!ValidMovement())
            {
                transform.position -= new Vector3(0, -1, 0);
                fallCompFlg = 1;
                this.enabled = false;
                FindObjectOfType<Spawn>().NewMino();

                if (rotationCond != 2)
                {
                    if (transform.position.y == 1.5f)
                    {
                        gameObject.transform.DetachChildren();
                        Destroy(gameObject);
                    }
                }
                else if(rotationCond == 2)
                {
                    if (transform.position.y == 2.5f)
                    {
                        gameObject.transform.DetachChildren();
                        Destroy(gameObject);
                    }
                }

            }

            previousTime = Time.time;

            
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rotationFlg = 0;
            // ブロックの回転
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            rotationCond += 1;

            if (!ValidMovement())
            {
                rotationFlg = 1;
                rotationCond -= 1;
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }
            if(rotationCond == 4)
            {
                rotationCond = 0;
            }


        }
    }

    // minoの移動範囲の制御
    public bool ValidMovement()
    {

        foreach (Transform children in transform)
        {
            double roundX = Mathf.RoundToInt(children.transform.position.x);
            double roundY = Mathf.RoundToInt(children.transform.position.y);

            // minoがステージよりはみ出さないように制御
            if (roundX <= 4.0 || roundX >=11.0  || roundY <= 1.0)
            {
                return false;
            }

        }

        int i = 0;
        foreach (GameObject puyo in this.puyos)
        {

            double roundX = Mathf.RoundToInt(transform.position.x * 10.0f) / 10.0f;
            double roundY = Mathf.RoundToInt(transform.position.y * 10.0f) / 10.0f;
            if (rotationCond == 0) 
            {
                //落下終了条件
                if (roundX == this.puyox[i] && roundY == this.puyoy[i] + 1.0f)
                {
                    gameObject.transform.DetachChildren();    //親子関係の解除
                    Destroy(gameObject);     //ぷよセットオブジェクト（親）を削除
                    return false;
                }
            }
            else if (rotationCond == 1)
            {
                //落下終了条件
                if (roundX == this.puyox[i] && roundY == this.puyoy[i] + 1.0f || roundX == this.puyox[i] +1.0f&& roundY == this.puyoy[i] + 1.0f)
                {
                    gameObject.transform.DetachChildren();    //親子関係の解除
                    Destroy(gameObject);     //ぷよセットオブジェクト（親）を削除
                    return false;
                }
            }
            else if (rotationCond == 2)
            {
                //落下終了条件
                if (roundX == this.puyox[i] && roundY == this.puyoy[i] + 2.0f )
                {
                    gameObject.transform.DetachChildren();    //親子関係の解除
                    Destroy(gameObject);     //ぷよセットオブジェクト（親）を削除
                    return false;
                }
            }
            else if (rotationCond == 3)
            {
                //落下終了条件
                if (roundX == this.puyox[i] && roundY == this.puyoy[i] + 1.0f || roundX == this.puyox[i] - 1.0f && roundY == this.puyoy[i] + 1.0f)
                {
                    gameObject.transform.DetachChildren();    //親子関係の解除
                    Destroy(gameObject);     //ぷよセットオブジェクト（親）を削除
                    return false;
                }
            }
            i++;
        }
        return true;
    }

    

}
