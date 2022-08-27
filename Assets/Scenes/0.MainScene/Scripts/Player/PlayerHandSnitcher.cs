using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandSnitcher : MonoBehaviour
{
    [SerializeField] private string searchTag = "";

    public delegate void HandAction(bool isReachable);
    public static event HandAction OnHandReachable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != searchTag)
            return;

        //OnHandReachable?.Invoke(true);
        Debug.Log("Na Hands");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != searchTag)
            return;

        //OnHandReachable?.Invoke(false);
    }

}
