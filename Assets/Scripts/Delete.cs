using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete : MonoBehaviour
{

    GameObject[] puyos;
    List<int> samecolorset = new List<int>();
    float[] puyox = new float[100];
    float[] puyoy = new float[100];
    int[] checks = new int[100];
    int[] samecolornums = new int[100];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void init()
    {
        this.puyos = GameObject.FindGameObjectsWithTag("puyo");
        int i = 0;
        foreach (GameObject puyo in this.puyos)
        {
            //checks[i]：i番ぷよの確認作業終了フラグ…0：未完了、1：完了
            this.checks[i] = 0;
            //samecolornums[i]：i番ぷよと隣り合っている同色ぷよの数、基本は1（自分自身）
            this.samecolornums[i] = 1;
            //puyox[i],puyoy[i]：i番ぷよの位置座標（丸め誤差対策済）
            this.puyox[i] = Mathf.RoundToInt(puyo.transform.position.x * 10.0f) / 10.0f;
            this.puyoy[i] = Mathf.RoundToInt(puyo.transform.position.y * 10.0f) / 10.0f;
            i++;
        }
    }

    public void puyoDestroy()
    {

        List<int> samecolorset = new List<int>();
        int i = 0;
        foreach (GameObject puyo in this.puyos)
        {
            this.samecolorset.Clear();
            Check(i);
            for (int k = 0; k < this.samecolorset.Count; k++)
            {
                //i番ぷよと隣接同色ぷよのsamecolornumsにカウント結果を代入
                this.samecolornums[this.samecolorset[k]] = this.samecolorset.Count;
            }
            i++;
        }

        i = 0;
        foreach (GameObject puyo in this.puyos)
        {
            if (this.samecolornums[i] >= 4)
            {
                Destroy(puyo);
            }
            i++;
        }
    }

    public void Check(int i)
    {
        this.samecolorset.Add(i);
        //元からchecks[i]=1ならi番ぷよは調査済なので確認しない
        if (this.checks[i] == 1) return;
        this.checks[i] = 1;    //これからi番ぷよを調査するので0→1に直しておく
        for (int j = 0; j < this.puyos.Length; j++)
        {
            if (this.puyox[i] == this.puyox[j] && this.puyoy[i] == this.puyoy[j] + 1.0f &&
            this.puyos[i].transform.name == this.puyos[j].transform.name && this.checks[j] == 0)
            {
                // <span class="crayon-c">下（j番ぷよ：未調査）と自分自身（i番ぷよ）が同色</span>
                Check(j);
            }
            if (this.puyox[i] == this.puyox[j] && this.puyoy[i] == this.puyoy[j] - 1.0f &&
            this.puyos[i].transform.name == this.puyos[j].transform.name && this.checks[j] == 0)
            {
                // <span class="crayon-c">上（j番ぷよ：未調査）と自分自身（i番ぷよ）が同色</span>
                Check(j);
            }
            if (this.puyox[i] == this.puyox[j] + 1.0f && this.puyoy[i] == this.puyoy[j] &&
            this.puyos[i].transform.name == this.puyos[j].transform.name && this.checks[j] == 0)
            {
                // <span class="crayon-c">左（j番ぷよ：未調査）と自分自身（i番ぷよ）が同色</span>
                Check(j);
            }
            if (this.puyox[i] == this.puyox[j] - 1.0f && this.puyoy[i] == this.puyoy[j] &&
            this.puyos[i].transform.name == this.puyos[j].transform.name && this.checks[j] == 0)
            {
                // <span class="crayon-c">右（j番ぷよ：未調査）と自分自身（i番ぷよ）が同色</span>
                Check(j);
            }
        }
        return;
    }

}