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
//    JUMPUP,
//    JUMPOVER,
//    FALLDOWN
}


public class GeneratePlatformManager : MonoBehaviour
{
    private List<ActionEvent> actions;


    // Start is called before the first frame update
    void Start()
    {

    }

    public List<ActionEvent> GenerateActionData(List<NoteData> noteDatas)
    {
        actions = new List<ActionEvent>();
        Random random = new Random(DateTime.Now.Millisecond);

        ActionEventType lastEvent = ActionEventType.TURNLEFT;
        for (int i = 0; i < noteDatas.Count; i++)
        {
            ActionEvent action = new ActionEvent();

            if (lastEvent == ActionEventType.TURNLEFT || lastEvent == ActionEventType.TURNRIGHT)
            {
                while (true)
                {
                    ActionEventType randomEvent = GetRandomActionEvent(random);
                    if (randomEvent != lastEvent)
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
            lastEvent = action.actionEventType;
        }

        return actions;
    }

    private ActionEventType GetRandomActionEvent(Random random)
    {
        Array values = Enum.GetValues(typeof(ActionEventType));
        ActionEventType randomActionEvent = (ActionEventType)values.GetValue(random.Next(values.Length));
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