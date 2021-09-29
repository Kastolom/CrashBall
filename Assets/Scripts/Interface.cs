using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour
{

    [SerializeField] private GameObject PanelBalls;
    public void InvertSetActive()
    {
        PanelBalls.SetActive(!PanelBalls.activeSelf);
    }
}
