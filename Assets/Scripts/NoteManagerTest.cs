using System;
using System.Collections;
using System.Collections.Generic;
using Amanotes.Data;
using CSharpSynth.Midi;
using CSharpSynth.Sequencer;
using CSharpSynth.Synthesis;
using UnityEngine;

public class NoteManagerTest : MonoBehaviour
{
    public static NoteManagerTest instance;


    [HideInInspector]
    public List<NoteData> noteDatas;
    [HideInInspector]
    public int noteCount;

    public float speed = 1f;
    public float MaxY = 2f;
    public GameObject cubePrefab;
    public string bankFilePath = "FM Bank/fm";
    public GameObject Ball;
    public AudioSource audioSource;

    public MoveFollowParabol ballMoveParabol;

    public GeneratePlatformManager generatePlatformManager;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        Ball.transform.position = new Vector3(0, 1, 0);

        LoadMidiFile();
    }

    void LoadMidiFile()
    {
        //        string fileName = "Paris.mid";
        string fileName = "Unity.bin";

        try
        {
            string path = Application.dataPath + "/" + fileName;
            byte[] midiBytes = Utils.LoadFileBinary(path);
            //            byte[] midiBytes = Utils.LoadFileFromResources(fileName);


            MidiFile midiFile = NoteGeneration.LoadFileContent(midiBytes, Difficulty.Easy, (Action<List<NoteData>>)(resSucess =>
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

            PlayMidi(midiFile);
        }
        catch (Exception ex)
        {
            Debug.Log((object)ex);
        }

    }

    void PlayMidi(MidiFile midiFile)
    {
        if (midiFile == null)
        {
            Debug.Log("MidiFile null");
            return;
        }

        Play(midiFile);
    }

    private bool seted = false;

    private int currentIndex = 0;
    private float nextAppearTime = 0;
    private float durationTime = 0;

    private List<Vector3> notePos = new List<Vector3>();

    void GenerateNoteToTile()
    {
        if (this.noteCount > 0)
        {
            FilterNote();

            Ball.transform.position = Vector3.zero;

            var actionsList = generatePlatformManager.GenerateActionData(noteDatas);
            Vector3 currentDirection = Vector3.forward;
            Vector3 currentPos = Vector3.zero;
            float lastAppertime = 0;

            for (int i = 0; i < noteDatas.Count; i++)
            {
                var noteData = noteDatas[i];

                Vector3 nextPos = currentPos + currentDirection * speed * (noteData.timeAppear - lastAppertime);
                lastAppertime = noteData.timeAppear;
                currentPos = nextPos;

                if (i < noteDatas.Count)
                {
                    ActionEvent nextAction = actionsList[i];

                    currentDirection = GetDirection(currentDirection, nextAction.actionEventType);
                }

                InstantiateCube(nextPos);

                if (!seted)
                {
                    seted = true;

                    currentIndex = 0;
                    nextAppearTime = noteData.timeAppear;
                    durationTime = nextAppearTime;

                    ballMoveParabol.SetData(durationTime, MaxY, Vector3.zero, nextPos);
                }
            }
        }
    }

    public Vector3 GetDirection(Vector3 currentDirection, ActionEventType actionEvent)
    {
        Vector3 direction = Vector3.forward;

        Quaternion quaternion = Quaternion.identity;
        if (actionEvent == ActionEventType.TURNLEFT)
        {
            quaternion = Quaternion.Euler(0, -90, 0);
        }
        else if(actionEvent == ActionEventType.TURNRIGHT)
        {
            quaternion = Quaternion.Euler(0, 90, 0);
        }else if (actionEvent == ActionEventType.FORWARD_JUMPUP)
        {
            return currentDirection;
        }

        direction = quaternion * currentDirection;
        return direction;
    }


    public void InstantiateCube(Vector3 position)
    {
        GameObject cube = Instantiate(cubePrefab) as GameObject;
        cube.transform.position = position;
        notePos.Add(position);
    }

    void FilterNote()
    {
        float currentTime = 0.0f;
        float minimumTimebetween2Note = 0.5f;

        for (int index = 0; index < this.noteCount; ++index)
        {
            if ((double)currentTime > 0.0 && (double)this.noteDatas[index].timeAppear - (double)currentTime < (double)minimumTimebetween2Note)
            {
                this.noteDatas.RemoveAt(index);
                --index;
                --this.noteCount;
            }
            else
                currentTime = this.noteDatas[index].timeAppear;
        }
    }

    // Update is called once per frame
    private float currentTimeClock = 0;

    void Update()
    {
        UpdateBallPosition();
    }

    private float currentDurationTime = 0;
    private void UpdateBallPosition()
    {
        currentTimeClock += Time.deltaTime;

        if (currentTimeClock >= nextAppearTime)
        {
            currentIndex++;
            if (currentIndex >= noteDatas.Count) return;

            nextAppearTime = noteDatas[currentIndex].timeAppear;
            durationTime = (noteDatas[currentIndex].timeAppear - currentTimeClock);
            currentDurationTime = 0;

            ballMoveParabol.SetData(durationTime, MaxY, Ball.transform.position, notePos[currentIndex]);
        }

        currentDurationTime += Time.deltaTime;

        ballMoveParabol.Move();

    }

    public class Sound
    {
        public Sound(string bank, string midifile)
        {
            midiStreamSynthesizer = new StreamSynthesizer(StreamSynthesizer.SampleRateType.High);
            sampleBuffer = new float[midiStreamSynthesizer.BufferSize];
            midiStreamSynthesizer.LoadBank(bank);

            midiSequencer = new MidiSequencer(midiStreamSynthesizer);
            midiSequencer.LoadMidi(midifile, false);
            midiSequencer.Play();
        }

        public Sound(string bank, MidiFile midifile)
        {
            midiStreamSynthesizer = new StreamSynthesizer(StreamSynthesizer.SampleRateType.High);
            sampleBuffer = new float[midiStreamSynthesizer.BufferSize];
            midiStreamSynthesizer.LoadBank(bank);

            midiSequencer = new MidiSequencer(midiStreamSynthesizer);
            midiSequencer.LoadMidi(midifile, false);
            midiSequencer.Play();
        }


        public bool isPlaying
        {
            get
            {

                return midiSequencer.isPlaying;
            }
        }
        public float volume = 1f;
        public bool isLoop
        {
            get
            {
                return midiSequencer.Looping;
            }
            set
            {
                midiSequencer.Looping = isLoop;
            }
        }
        //Private 
        private float[] sampleBuffer = new float[4096];

        private MidiSequencer midiSequencer;
        private StreamSynthesizer midiStreamSynthesizer;
        public void Mix(float[] data, int channels)
        {
            int sample = data.Length / channels;
            //µ¥Í¨µÀ²ÉÑù¼´¿É
            midiStreamSynthesizer.GetNext(sampleBuffer, sample);

            for (int i = 0; i < sample; i++)
            {
                float b = sampleBuffer[i] * volume;
                for (int c = 0; c < channels; c++)
                {
                    float a = data[i * channels + c];
                    data[i * channels + c] = a + b - a * b;
                }
            }
            float[] realout = new float[data.Length];
            float[] imagout = new float[data.Length];

            Ernzo.DSP.FFT.Compute((uint)data.Length, data, null, realout, imagout, false);
        }
    }

    List<Sound> sounds = new List<Sound>();

    public void Play(string midi)
    {
        Sound s = new Sound(bankFilePath, midi);
        lock (_lock)
        {
            sounds.Add(s);
        }
    }

    public void Play(MidiFile midi)
    {
        Sound s = new Sound(bankFilePath, midi);
        lock (_lock)
        {
            sounds.Add(s);
        }
    }
    public void StopAll()
    {
        lock (_lock)
        {
            sounds.Clear();
        }
    }
    class LockObj
    {

    }
    LockObj _lock = new LockObj();

    private void OnAudioFilterRead(float[] data, int channels)
    {
        try
        {
            Debug.Log("Play Midi");
            Sound remove = null;
            lock (_lock)
            {
                foreach (var s in sounds)
                {
                    if (s.isPlaying == false)
                    {
                        //remove = s;
                        //continue;
                    }
                    s.Mix(data, channels);
                }
                sounds.Remove(remove);
            }
        }
        catch
        {
            Debug.LogError("Play Midi err.");
        }

    }
}
