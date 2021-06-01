using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mino : MonoBehaviour
{

    public float previousTime;
    // mino�̗����鎞��
    public float fallTime = 1f;

    // �X�e�[�W�̑傫��
    private int width = 1;
    private int height = 1;

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
        }
        // �E���L�[�ŉE�ɓ���
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
        }
        // �����ŉ��Ɉړ������A�����L�[�ł��ړ�����
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousTime >= fallTime)
        {
            transform.position += new Vector3(0, -1, 0);
            previousTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // mino������L�[�������ĉ�]������
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        }
    }

    // mino�̈ړ��͈͂̐���
    bool ValidMovement()
    {

        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            // mino���X�e�[�W���͂ݏo���Ȃ��悤�ɐ���
            if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
            {
                return false;
            }
        }
        return true;
    }


}
