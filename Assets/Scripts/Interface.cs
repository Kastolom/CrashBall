using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour
{

    [SerializeField] private GameObject[] PanelElements;
    public void InvertSetActive(int index)
    {
        for (int i = 0; i < PanelElements.Length; i++)
        {
            if (i == index)
            {
                PanelElements[index].SetActive(!PanelElements[index].activeSelf);
            }
            else
            {
                PanelElements[i].SetActive(false);
            }
        }
        //PanelBalls.SetActive(!PanelBalls.activeSelf);
    }
}
