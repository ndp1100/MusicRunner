// Decompiled with JetBrains decompiler
// Type: Amanotes.Data.ContentNoteGenerator
// Assembly: ContentReader, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 31109648-5020-4775-A5CF-CD35FB59B4E1
// Assembly location: D:\WorkingTools\APKEasyTool\1-Decompiled APKs\Tiles Hop EDM Rush_v2.7.5_apkpure.com\assets\bin\Data\Managed\ContentReader.dll

using CSharpSynth;
using System;
using System.Collections.Generic;
using CSharpSynth.Midi;
using UnityEngine;

namespace Amanotes.Data
{
    public class ContentNoteGenerator : INoteGenerator
    {
        private static byte[] lowestNotes = new byte[5]
        {
      (byte) 60,
      (byte) 72,
      (byte) 84,
      (byte) 96,
      (byte) 120
        };
        private static byte[] highestNotes = new byte[5]
        {
      (byte) 64,
      (byte) 76,
      (byte) 88,
      (byte) 102,
      (byte) 124
        };
        public List<string> bundleAllow = new List<string>()
    {
      "com.amanotes.",
      "com.youmusic.",
      "com.rhythmbeat.",
      "com.innovation."
    };
        public const int MicrosecondsPerSecond = 1000000;

        public static Difficulty GetSuitableDifficulty(byte note)
        {
            for (int index = 0; index < ContentNoteGenerator.lowestNotes.Length; ++index)
            {
                if ((int)ContentNoteGenerator.lowestNotes[index] <= (int)note && (int)ContentNoteGenerator.highestNotes[index] >= (int)note)
                    return (Difficulty)index;
            }
            return Difficulty.Easy;
        }

        public MidiFile GenerateNotes(object source, Difficulty difficulty, Action<float, List<NoteData>> OnProgress, Action<List<NoteData>> OnComplete, Action<string> OnError)
        {
            byte[] byteArray = (byte[])source;
            if (byteArray == null || byteArray.Length <= 0)
            {
                Debug.LogWarning((object)"Trying to parse note data from empty MIDI content, stopping progress...");
                if (OnError == null)
                    return null;
                OnError("MIDI file is empty");
            }
            else
            {
                /*bool flag = false;
                for (int index = 0; index < this.bundleAllow.Count; ++index)
                {
                    if (Application.identifier.Contains(this.bundleAllow[index]))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    Debug.LogError((object)"Error 2x7c1");
                }
                else*/
                {
                    MidiFile MidiFile = new MidiFile(byteArray);
                    Debug.Log("BPM = " + MidiFile.BeatsPerMinute);
                    List<MidiEvent> midiEventsofType = MidiFile.getAllMidiEventsofType(MidiHelper.MidiChannelEvent.None, MidiHelper.MidiMetaEvent.Tempo);
                    if (midiEventsofType.Count > 0)
                        MidiFile.MicroSecondsPerQuarterNote = (uint)midiEventsofType[0].Parameters[0];
                    List<NoteData> noteDataList = new List<NoteData>();
                    float num1 = (float)MidiFile.MicroSecondsPerQuarterNote / 1000000f / (float)MidiFile.MidiHeader.deltaTicksPerQuarterNote;
                    MidiFile.CombineTracks();
                    MidiEvent[] midiEvents = MidiFile.CombinedTrack.MidiEvents;
                    Dictionary<byte, Stack<MidiEvent>> dictionary = new Dictionary<byte, Stack<MidiEvent>>();
                    int length = midiEvents.Length;
                    for (int index = 0; index < length; ++index)
                    {
                        if (midiEvents[index].isChannelEvent())
                        {
                            MidiEvent midiEvent = midiEvents[index];
                            byte parameter1 = midiEvent.parameter1;
                            if (midiEvent.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_On)
                            {
                                if (dictionary.ContainsKey(parameter1))
                                {
                                    dictionary[parameter1].Push(midiEvent);
                                }
                                else
                                {
                                    Stack<MidiEvent> binaryContentEventStack = new Stack<MidiEvent>();
                                    binaryContentEventStack.Push(midiEvent);
                                    dictionary.Add(parameter1, binaryContentEventStack);
                                }
                            }
                            else if (midiEvent.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_Off)
                            {
                                MidiEvent binaryContentEvent2 = dictionary[parameter1].Pop();
                                float num2 = (float)binaryContentEvent2.deltaTimeFromStart * num1;
                                float num3 = (float)(midiEvent.deltaTimeFromStart - binaryContentEvent2.deltaTimeFromStart);
                                float num4 = (double)num3 <= 86.0 ? 0.0f : num3 * num1;
                                int num5 = (int)parameter1 % 12;
                                if (ContentNoteGenerator.GetSuitableDifficulty(parameter1) == difficulty)
                                {
                                    NoteData noteData = new NoteData()
                                    {
                                        duration = num4
                                    };
                                    noteData.type = (double)noteData.duration <= 0.0 ? NoteDataType.Single : NoteDataType.Multi;
                                    noteData.nodeID = (int)parameter1;
                                    noteData.timeAppear = num2;
                                    noteData.stringIndex = num5;
                                    noteData.noteOrder = noteDataList.Count;
                                    noteDataList.Add(noteData);
                                }
                            }
                        }
                    }
                    if (OnProgress != null)
                        OnProgress(1f, noteDataList);


                    if (OnComplete != null)
                        OnComplete(noteDataList);

                    return MidiFile;
                }
            }

            return null;
        }
    }
}
