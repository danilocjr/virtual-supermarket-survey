using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AppController;

public class ViewQuestInstructionsBehaviour : MonoBehaviour
{
    [SerializeField] private Sprite questOne;
    [SerializeField] private Sprite questTwo;

    [SerializeField] private Image questInstructionsPlaceholder;
   
    public void SetQuestType(ActivityTypes questType)
    {
        if (questType == ActivityTypes.TimedSearch)
            questInstructionsPlaceholder.sprite = questOne;
        else
            questInstructionsPlaceholder.sprite = questTwo;
    }

   
}
