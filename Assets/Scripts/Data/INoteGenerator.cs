// Decompiled with JetBrains decompiler
// Type: Amanotes.Data.INoteGenerator
// Assembly: ContentReader, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 31109648-5020-4775-A5CF-CD35FB59B4E1
// Assembly location: D:\WorkingTools\APKEasyTool\1-Decompiled APKs\Tiles Hop EDM Rush_v2.7.5_apkpure.com\assets\bin\Data\Managed\ContentReader.dll

using System;
using System.Collections.Generic;
using CSharpSynth.Midi;

namespace Amanotes.Data
{
    public interface INoteGenerator
    {
        MidiFile GenerateNotes(object source, Difficulty difficult, Action<float, List<NoteData>> OnProgress, Action<List<NoteData>> OnComplete, Action<string> OnError);
    }
}
