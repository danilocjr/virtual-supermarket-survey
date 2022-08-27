using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewMessageBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject successMsg;
    [SerializeField] private GameObject failMsg;



    void Start()
    {
        successMsg.SetActive(false);
        failMsg.SetActive(false);
    }

    public void SetResultMessage(bool resultState)
    {
        successMsg.SetActive(resultState);
        failMsg.SetActive(!resultState);
    }

 
}
