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

    // Start is called before the first frame update
    void Start()
    {
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

    void GenerateNoteToTile()
    {
        if (this.noteCount > 0)
        {
            FilterNote();

            foreach (NoteData noteData in noteDatas)
            {
                float positionZ = speed * noteData.timeAppear;
                GameObject cube = Instantiate(cubePrefab) as GameObject;
                cube.transform.position = new Vector3(0, 1, positionZ);
                if (!seted)
                {
                    Ball.transform.position = cube.transform.position;
                    seted = true;

                    currentIndex = 0;
                    nextAppearTime = noteData.timeAppear;
                    durationTime = nextAppearTime;
                }
            }
        }
    }

    void FilterNote()
    {
        float currentTime = 0.0f;
        float minimumTimebetween2Note = 0.7f;

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
        currentTimeClock += Time.deltaTime;
        Ball.transform.position += Vector3.forward * speed * Time.deltaTime;

        UpdateBallY();
    }

    private float currentDurationTime = 0;
    private void UpdateBallY()
    {
        if (currentTimeClock >= nextAppearTime)
        {
            currentIndex++;
            
            nextAppearTime = noteDatas[currentIndex].timeAppear;
            durationTime = (noteDatas[currentIndex].timeAppear - currentTimeClock);
            currentDurationTime = 0;
        }

        currentDurationTime += Time.deltaTime;

        Vector3 currentBallPosition = Ball.transform.position;
        float ballY;
        

        if (currentDurationTime <= durationTime / 2f)
        {
            float ratio = currentDurationTime / (durationTime / 2f);
            ratio = Mathf.Clamp01(ratio);
            Debug.Log("Ratio : " + ratio);
            ballY = Mathf.Lerp(0, MaxY, ratio);
        }
        else
        {
            float ratio = (nextAppearTime - currentDurationTime) / (durationTime / 2f);
            ratio = Mathf.Clamp01(ratio);
            Debug.Log("Ratio : " + ratio);
            ballY = Mathf.Lerp(0, MaxY, ratio);
        }

        Ball.transform.position = new Vector3(currentBallPosition.x, ballY, currentBallPosition.z);
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
