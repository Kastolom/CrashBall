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
            float positionCamera = -Input.GetAxis("Mouse Y") * speed;
            positionCamera = Mathf.Clamp(positionCamera, -15f, 0f);
            Camera.main.transform.Translate(0, positionCamera * Time.deltaTime, 0);      
        }
#endif

#if UNITY_ANDROID
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Moved)
            {
                float positionCamera = -Input.GetAxis("Mouse Y") * speed;
                Camera.main.transform.Translate(0, positionCamera * Time.deltaTime, 0);
                if (Camera.main.transform.position.y > 0)
                {
                    Camera.main.transform.position = new Vector3(0, 0, 0);
                }
                if (Camera.main.transform.position.y < -15)
                {
                    Camera.main.transform.position = new Vector3(0, -15, 0);
                }

            }
        }
#endif
    }
}
