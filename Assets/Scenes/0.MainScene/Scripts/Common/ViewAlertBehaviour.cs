using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ViewAlertBehaviour : MonoBehaviour
{
    Color myColor;
    bool isFading = false;

    private void Start()
    {
        MouseCursorBehaviour.OnMouseOutOfReach += MouseCursorBehaviour_OnMouseOutOfReach;
        myColor = GetComponent<TMP_Text>().color;
        GetComponent<TMP_Text>().enabled = false;
    }

    private void MouseCursorBehaviour_OnMouseOutOfReach(string objectName)
    {
        isFading = true;
        GetComponent<TMP_Text>().enabled = true;

        StopCoroutine("FadeOut");
        StartCoroutine("FadeOut");
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(4);
        isFading = false;
        GetComponent<TMP_Text>().enabled = false;
    }

    //private void Update()
    //{
    //    if(isFading)
    //        GetComponent<TMP_Text>().color = Color.Lerp(new Color(myColor.r, myColor.g, myColor.b, 1), new Color(myColor.r, myColor.g, myColor.b, 0), Time.deltaTime * 10f);
    //}
}
