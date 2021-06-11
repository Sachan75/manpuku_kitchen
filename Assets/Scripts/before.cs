using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class before : MonoBehaviour
{

    public float previousTime;

    // mino回転
    public Vector3 rotationPoint;

    private void Start()
    {
     
    }



    void Update()
    {
        MinoMovememt();

    }

    private void MinoMovememt()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // ブロックの回転
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);

            if (!ValidMovement())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), +90);
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
