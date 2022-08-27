using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewExperienceBehaviour : MonoBehaviour
{
    [Header("View Components")]
    [SerializeField] private GameObject navPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject messagePanel;

    public enum ViewExperienceLayouts
    {
        Start,
        Finish,
        Activity,
        Message,
        None
    }

    void Start()
    {
        SetViewBehaviour(ViewExperienceLayouts.Start);
    }

    public void SetViewBehaviour(ViewExperienceLayouts layout)
    {
        switch (layout)
        {
            case ViewExperienceLayouts.Start:
                navPanel.SetActive(false);
                menuPanel.SetActive(true);
                messagePanel.SetActive(false);
                break;
            case ViewExperienceLayouts.Activity: 
                navPanel.SetActive(true);
                menuPanel.SetActive(false);
                messagePanel.SetActive(false);
                break;
            case ViewExperienceLayouts.Finish: 
                navPanel.SetActive(false);
                menuPanel.SetActive(false);
                messagePanel.SetActive(false);
                break;
            case ViewExperienceLayouts.Message:
                navPanel.SetActive(false);
                menuPanel.SetActive(false);
                messagePanel.SetActive(true);
                break;
            case ViewExperienceLayouts.None:
                navPanel.SetActive(false);
                menuPanel.SetActive(false);
                messagePanel.SetActive(false);
                break;

        }
    }
}
