using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mino : MonoBehaviour
{

    public float previousTime;
    // minoの落ちる時間
    public float fallTime = 1f;

    // mino回転
    public Vector3 rotationPoint;

    void Update()
    {
        MinoMovememt();
    }

    private void MinoMovememt()
    {
        // 左矢印キーで左に動く
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-2, 0, 0);
            // 今回追加
            if (!ValidMovement())
            {
                transform.position -= new Vector3(-2, 0, 0);
            }

        }
        // 右矢印キーで右に動く
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(2, 0, 0);
            // 今回追加
            if (!ValidMovement())
            {
                transform.position -= new Vector3(2, 0, 0);
            }
        }
        // 自動で下に移動させつつ、下矢印キーでも移動する
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousTime >= fallTime)
        {
            transform.position += new Vector3(0, -2, 0);
            // 今回追加
            if (!ValidMovement())
            {
                transform.position -= new Vector3(0, -2, 0);

                this.enabled = false;
                FindObjectOfType<Spawn>().NewMino();
            }

            previousTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // ブロックの回転
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);

            if (!ValidMovement())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }


        }
    }

    // 今回追加
    // minoの移動範囲の制御
    bool ValidMovement()
    {

        foreach (Transform children in transform)
        {
            double roundX = Mathf.RoundToInt(children.transform.position.x);
            double roundY = Mathf.RoundToInt(children.transform.position.y);

            // minoがステージよりはみ出さないように制御
            if (roundX < -21 || roundX > -9 || roundY < -12)
            {
                return false;
            }
        }
        return true;
    }

}
