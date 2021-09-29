using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollEnvironment : MonoBehaviour
{
    [SerializeField] private float speed;
    void Update()
    {      

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            float translation = -Input.GetAxis("Mouse Y") * speed;
            Camera.main.transform.Translate(0, translation * Time.deltaTime, 0);      
        }
#endif

#if UNITY_ANDROID
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Moved)
            {
                float translation = -Input.GetAxis("Mouse Y") * speed;
                Camera.main.transform.Translate(0, translation * Time.deltaTime, 0);
            }
        }
#endif
    }
}
