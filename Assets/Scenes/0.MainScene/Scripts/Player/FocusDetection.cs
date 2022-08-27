using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusDetection : MonoBehaviour
{
    [SerializeField] private int focusIndex = 0;
    [SerializeField] private string searchTag = "";


    public delegate void FocusAction(int focusIndex, string objectName, bool hasFocus);
    public static event FocusAction OnFocusChanged;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != searchTag)
            return;

        //Debug.Log("Enter: " + other.gameObject.transform.parent.name);
        OnFocusChanged?.Invoke(focusIndex, other.gameObject.transform.parent.name, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != searchTag)
            return;

        //Debug.Log("Exit:" + other.gameObject.transform.parent.name);
        OnFocusChanged?.Invoke(focusIndex, other.gameObject.transform.parent.name, false);
    }

}
