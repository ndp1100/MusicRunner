using System;
using System.Collections;
using System.Collections.Generic;
using Amanotes.Data;
using UnityEngine;

public class NoteManagerTest : MonoBehaviour
{
    [HideInInspector]
    public List<NoteData> noteDatas;
    [HideInInspector]
    public int noteCount;

    public float speed = 1f;
    public GameObject cubePrefab;

    // Start is called before the first frame update
    void Start()
    {
        LoadMidiFile();
    }

    void LoadMidiFile()
    {
        string fileName = "Paris.mid";

        try
        {
            NoteGeneration.LoadFileContent(Utils.LoadFileBinary(fileName), Difficulty.Easy, (Action<List<NoteData>>)(resSucess =>
            {
                this.noteDatas = resSucess;
                this.noteCount = this.noteDatas.Count;

                GenerateNoteToTile();
//                this.FilterNote(rawNote);
//                if (rawNote)
//                    return;
//                this.AddNecessaryNotes();
//                this.AddDuration();
            }), (Action<string>)(resError => Debug.LogError((object)("Error:" + resError))));
        }
        catch (Exception ex)
        {
            Debug.Log((object)ex);
        }

    }



    void GenerateNoteToTile()
    {
        if (this.noteCount > 0)
        {
            foreach (NoteData noteData in noteDatas)
            {
                float positionZ = speed * noteData.timeAppear;
                GameObject cube = Instantiate(cubePrefab) as GameObject;
                cube.transform.position = new Vector3(0, 1, positionZ);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
