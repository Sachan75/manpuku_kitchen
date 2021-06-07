using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mino : MonoBehaviour
{

    public float previousTime;
    // mino�̗����鎞��
    public float fallTime = 1f;

    // mino��]
    public Vector3 rotationPoint;

    
    void Update()
    {
        MinoMovememt();

    }

    private void MinoMovememt()
    {
        // �����L�[�ō��ɓ���
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            
            if (!ValidMovement())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }

        }
        // �E���L�[�ŉE�ɓ���
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            
            if (!ValidMovement())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }
        // �����ŉ��Ɉړ������A�����L�[�ł��ړ�����
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousTime >= fallTime)
        {
            transform.position += new Vector3(0, -1, 0);
            
            if (!ValidMovement())
            {
                transform.position -= new Vector3(0, -1, 0);
                this.enabled = false;
                FindObjectOfType<Spawn>().NewMino();

                if (transform.position.y == 1.5f)
                {
                    gameObject.transform.DetachChildren();
                    Destroy(gameObject);
                }

            }

            previousTime = Time.time;

            
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // �u���b�N�̉�]
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);

            if (!ValidMovement())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }


        }
    }

    // mino�̈ړ��͈͂̐���
    bool ValidMovement()
    {

        foreach (Transform children in transform)
        {
            double roundX = Mathf.RoundToInt(children.transform.position.x);
            double roundY = Mathf.RoundToInt(children.transform.position.y);

            // mino���X�e�[�W���͂ݏo���Ȃ��悤�ɐ���
            if (roundX < 1.0 || roundX >6.5  || roundY < 1.0)
            {
                return false;
            }

        }
        return true;
    }

    

}
