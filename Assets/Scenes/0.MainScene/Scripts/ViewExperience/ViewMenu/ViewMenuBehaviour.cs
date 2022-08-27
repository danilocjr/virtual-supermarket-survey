using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewMenuBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject viewWelcome;
    [SerializeField] private GameObject viewNavInstructions;
    [SerializeField] private GameObject viewQuestInstructions;
    
    void Start()
    {
        SetViewWelcome();
    }

    public void SetViewWelcome()
    {
        viewWelcome.SetActive(true);
        viewNavInstructions.SetActive(false);
        viewQuestInstructions.SetActive(false);
    }

    public void SetViewNavInstructions()
    {
        viewWelcome.SetActive(false);
        viewNavInstructions.SetActive(true);
        viewQuestInstructions.SetActive(false);
    }

    public void SetViewQuestInstructions()
    {
        viewWelcome.SetActive(false);
        viewNavInstructions.SetActive(false);
        viewQuestInstructions.SetActive(true);
    }

}
