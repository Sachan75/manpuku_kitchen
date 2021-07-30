using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] Minos;
    // Start is called before the first frame update
    void Start()
    {
        GManager.instance.Reset();
        SetNextIngredient();
        NewMino();
    }

    public void NewMino()
    {
        StartFalling();
        SetNextIngredient();
    }

    private void SetNextIngredient()
    {
        Instantiate(Minos[Random.Range(0, Minos.Length)], transform.position, Quaternion.identity);
    }

    private void StartFalling()
    {
        foreach (Set obj in UnityEngine.Object.FindObjectsOfType(typeof(Set)))
        {
            if (obj.isNext)
            {
                obj.movePuyo();
            }
        }
    }
}
