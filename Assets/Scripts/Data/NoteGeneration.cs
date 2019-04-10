// Decompiled with JetBrains decompiler
// Type: Amanotes.Data.NoteGeneration
// Assembly: ContentReader, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 31109648-5020-4775-A5CF-CD35FB59B4E1
// Assembly location: D:\WorkingTools\APKEasyTool\1-Decompiled APKs\Tiles Hop EDM Rush_v2.7.5_apkpure.com\assets\bin\Data\Managed\ContentReader.dll

using Amanotes.Content;
using System;
using System.Collections.Generic;
using CSharpSynth.Midi;

namespace Amanotes.Data
{
    public class NoteGeneration
    {
        public static MidiFile LoadFileContent(byte[] data, Difficulty difficulty, Action<List<NoteData>> callbackNoteData, Action<string> errorLog)
        {

            ContentNoteGenerator contentNoteGenerator = new ContentNoteGenerator();
            MidiFile midiFile = contentNoteGenerator.GenerateNotes(data/*(object) CryptoHelper.DeCryptContentFile(data)*/, difficulty, (Action<float, List<NoteData>>)((progress, notelist) => { }), (Action<List<NoteData>>)(noteList =>
            {
                if (callbackNoteData == null)
                    return;
                callbackNoteData(noteList);
            }), (Action<string>)(error =>
            {
                if (errorLog == null)
                    return;
                errorLog(error);
            }));

            return midiFile;

            //            new ContentNoteGenerator().GenerateNotes(data/*(object) CryptoHelper.DeCryptContentFile(data)*/, difficulty, (Action<float, List<NoteData>>) ((progress, notelist) => {}), (Action<List<NoteData>>) (noteList =>
            //      {
            //        if (callbackNoteData == null)
            //          return;
            //        callbackNoteData(noteList);
            //      }), (Action<string>) (error =>
            //      {
            //        if (errorLog == null)
            //          return;
            //        errorLog(error);
            //      }));
        }
    }
}
