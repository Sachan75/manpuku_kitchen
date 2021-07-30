using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData pointerData)
    {
        SceneManager.LoadScene("Title");
    }

    // Update is called once per frame
    void Update()
    {

    }
}

