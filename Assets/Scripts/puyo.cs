using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puyo : MonoBehaviour
{
    GameObject[] puyos;
    GameObject Director;
    int num = 0;    //num=0�ŗ������A�I������fallcheck�ցAnum=1�Ȃ�x�~
    float[] puyox = new float[100];
    float[] puyoy = new float[100];

    public float previousTime;

    // mino��]
    public Vector3 rotationPoint;

    private void Start()
    {
        int i = 0;
        this.Director = GameObject.Find("Director");
        this.puyos = GameObject.FindGameObjectsWithTag("puyo");
        foreach (GameObject puyo in this.puyos)
        {
            //�ۂߌ덷�����i�t�B�[���g���̑S�Ղ�̈ʒu�j
            this.puyox[i] = Mathf.RoundToInt(puyo.transform.position.x * 10.0f) / 10.0f;
            this.puyoy[i] = Mathf.RoundToInt(puyo.transform.position.y * 10.0f) / 10.0f;
            i++;
        }
    }



    void Update()
    {
        MinoMovememt();

        int i = 0;
        //�ۂߌ덷�����i�����̍��̈ʒu�j
        double nowx = Mathf.RoundToInt(transform.position.x * 10.0f) / 10.0f;
        double nowy = Mathf.RoundToInt(transform.position.y * 10.0f) / 10.0f;

        if (this.num == 1) return;  //���������ςȂ̂ňȉ��̏����s�v
        if (transform.root.gameObject == gameObject)
        {
            //�R���r���U��̋������L�q
            if (nowy == 1.5f)
            {
                this.num = 1;   //�������������m�点
                return;
            }
            i = 0;
            foreach (GameObject puyo in this.puyos)
            {
                if (nowx == this.puyox[i] && nowy == this.puyoy[i] + 1.0f)
                {
                    this.num = 1;   //�������������m�点
                    return;
                }
                i++;
            }
            //�����������Ă��Ȃ��̂ň�����������
            transform.Translate(0, -1.0f, 0, Space.World);
        }

    }

    private void MinoMovememt()
    {
        if (transform.root.gameObject != gameObject)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // �u���b�N�̉�]
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);

                if (!ValidMovement())
                {
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), +90);
                }


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
            if (roundX <= 4.0 || roundX >= 11.0 || roundY <= 1.0)
            {
                return false;
            }

        }

        int i = 0;
        foreach (GameObject puyo in this.puyos)
        {
            double roundX = Mathf.RoundToInt(transform.position.x * 10.0f) / 10.0f;
            double roundY = Mathf.RoundToInt(transform.position.y * 10.0f) / 10.0f;
            //�����I������
            if (roundX == this.puyox[i] && roundY == this.puyoy[i] + 1.0f)
            {
                gameObject.transform.DetachChildren();    //�e�q�֌W�̉���
                Destroy(gameObject);     //�Ղ�Z�b�g�I�u�W�F�N�g�i�e�j���폜
                return false;
            }
            i++;
        }
        return true;
    }





}
