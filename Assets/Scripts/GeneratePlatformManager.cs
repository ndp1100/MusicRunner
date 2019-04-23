using System.Collections;
using System.Collections.Generic;
using Amanotes.Data;
using UnityEngine;


public enum ActionEventType
{
    TURNLEFT,
    TURNRIGHT,
    JUMPUP,
    JUMPOVER,
    FALLDOWN
}


public class GeneratePlatformManager : MonoBehaviour
{
    private List<ActionEvent> actions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void GenerateActionData(List<NoteData> noteDatas)
    {
        actions = new List<ActionEvent>();


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