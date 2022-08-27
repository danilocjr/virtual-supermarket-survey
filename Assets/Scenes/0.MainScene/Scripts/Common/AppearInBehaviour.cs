using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearInBehaviour : MonoBehaviour
{
    Vector3 initSize;
    bool isAnim = false;


    private void Awake()
    {
        initSize = transform.localScale;
    }

    private void OnEnable()
    {
        isAnim = true;
    }

    private void OnDisable()
    {
        isAnim = false;
    }


    void Update()
    {
        if(isAnim)
            Vector3.Lerp(Vector3.zero, initSize, Time.deltaTime);
    }
}
