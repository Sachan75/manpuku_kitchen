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
			//checks[i]�Fi�ԂՂ�̊m�F��ƏI���t���O�c0�F�������A1�F����
			this.checks[i] = 0;
			//samecolornums[i]�Fi�ԂՂ�Ɨׂ荇���Ă��铯�F�Ղ�̐��A��{��1�i�������g�j
			this.samecolornums[i] = 1;
			//puyox[i],puyoy[i]�Fi�ԂՂ�̈ʒu���W�i�ۂߌ덷�΍�ρj
			this.puyox[i] = Mathf.RoundToInt(puyo.transform.position.x * 10.0f) / 10.0f;
			this.puyoy[i] = Mathf.RoundToInt(puyo.transform.position.y * 10.0f) / 10.0f;
			i++;
		}
	}

    public void puyoDestroy() {

		List<int> samecolorset = new List<int>();
		int i = 0;
		foreach (GameObject puyo in this.puyos)
		{
			this.samecolorset.Clear();
			Check(i);
			for (int k = 0; k < this.samecolorset.Count; k++)
			{
				//i�ԂՂ�Ɨאړ��F�Ղ��samecolornums�ɃJ�E���g���ʂ���
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
		//������checks[i]=1�Ȃ�i�ԂՂ�͒����ςȂ̂Ŋm�F���Ȃ�
		if (this.checks[i] == 1) return;
		this.checks[i] = 1;    //���ꂩ��i�ԂՂ�𒲍�����̂�0��1�ɒ����Ă���
		for (int j = 0; j < this.puyos.Length; j++)
		{
			if (this.puyox[i] == this.puyox[j] && this.puyoy[i] == this.puyoy[j] + 1.0f &&
			this.puyos[i].transform.name == this.puyos[j].transform.name && this.checks[j] == 0)
			{
				// <span class="crayon-c">���ij�ԂՂ�F�������j�Ǝ������g�ii�ԂՂ�j�����F</span>
				Check(j);
			}
			if (this.puyox[i] == this.puyox[j] && this.puyoy[i] == this.puyoy[j] - 1.0f &&
			this.puyos[i].transform.name == this.puyos[j].transform.name && this.checks[j] == 0)
			{
				// <span class="crayon-c">��ij�ԂՂ�F�������j�Ǝ������g�ii�ԂՂ�j�����F</span>
				Check(j);
			}
			if (this.puyox[i] == this.puyox[j] + 1.0f && this.puyoy[i] == this.puyoy[j] &&
			this.puyos[i].transform.name == this.puyos[j].transform.name && this.checks[j] == 0)
			{
				// <span class="crayon-c">���ij�ԂՂ�F�������j�Ǝ������g�ii�ԂՂ�j�����F</span>
				Check(j);
			}
			if (this.puyox[i] == this.puyox[j] - 1.0f && this.puyoy[i] == this.puyoy[j] &&
			this.puyos[i].transform.name == this.puyos[j].transform.name && this.checks[j] == 0)
			{
				// <span class="crayon-c">�E�ij�ԂՂ�F�������j�Ǝ������g�ii�ԂՂ�j�����F</span>
				Check(j);
			}
		}
		return;
	}

}
