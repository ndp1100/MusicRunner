using System;
using System.Collections;
using System.Collections.Generic;
using Amanotes.Data;
using UnityEngine;
using Random = System.Random;


public enum ActionEventType
{
    TURNLEFT,
    TURNRIGHT,
    FORWARD_JUMPOVER,
//    JUMPOVER,
//    FALLDOWN
}


public class GeneratePlatformManager : MonoBehaviour
{
    private List<ActionEvent> actions;

    public List<int> randomActionRate = new List<int>(){25, 50, 100};

    // Start is called before the first frame update
    void Start()
    {

    }

    public List<ActionEvent> GenerateActionData(List<NoteData> noteDatas)
    {
        actions = new List<ActionEvent>();
        Random random = new Random(DateTime.Now.Millisecond);

        ActionEventType lastTurnEvent = ActionEventType.FORWARD_JUMPOVER;
        for (int i = 0; i < noteDatas.Count; i++)
        {
            ActionEvent action = new ActionEvent();

            if (lastTurnEvent == ActionEventType.TURNLEFT || lastTurnEvent == ActionEventType.TURNRIGHT)
            {
                while (true)
                {
                    ActionEventType randomEvent = GetRandomActionEvent(random);
                    if (randomEvent != lastTurnEvent)
                    {
                        action.actionEventType = randomEvent;
                        break;
                    }
                }
            }
            else
            {
                action.actionEventType = GetRandomActionEvent(random);
            }

            action.index = i;
            actions.Add(action);

            if(action.actionEventType == ActionEventType.TURNLEFT || action.actionEventType == ActionEventType.TURNRIGHT)
                lastTurnEvent = action.actionEventType;
        }

        return actions;
    }

    //TODO : optimize this algorithm, should add exceptType list, only random in that list
    private ActionEventType GetRandomActionEvent(Random random)
    {
        ActionEventType randomActionEvent = ActionEventType.FORWARD_JUMPOVER;

        Array values = Enum.GetValues(typeof(ActionEventType));

        int randomValue = random.Next(0, 100);
        for (int i = 0; i < randomActionRate.Count; i++)
        {
            if (randomValue <= randomActionRate[i])
            {
                randomActionEvent = (ActionEventType)values.GetValue(i);
                break;
            }
        }

//        ActionEventType randomActionEvent = (ActionEventType)values.GetValue(random.Next(values.Length));
        return randomActionEvent;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public class ActionEvent
{
    public ActionEventType actionEventType;
    public int index;
}