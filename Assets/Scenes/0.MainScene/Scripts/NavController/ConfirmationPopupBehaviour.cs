using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationPopupBehaviour : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void QuitApplication()
    {
#if PLATFORM_STANDALONE
        Application.Quit();
#else
        gameObject.SetActive(false);
#endif
    }


}
