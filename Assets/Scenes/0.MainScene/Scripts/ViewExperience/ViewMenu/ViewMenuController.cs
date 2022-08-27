using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AppController;

public class ViewMenuController : MonoBehaviour
{
    [SerializeField] private ViewQuestInstructionsBehaviour behaviour;

    public void SetQuestType(ActivityTypes questType)
    {
        behaviour.SetQuestType(questType);
    }
}
