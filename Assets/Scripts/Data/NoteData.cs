// Decompiled with JetBrains decompiler
// Type: Amanotes.Data.NoteData
// Assembly: ContentReader, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 31109648-5020-4775-A5CF-CD35FB59B4E1
// Assembly location: D:\WorkingTools\APKEasyTool\1-Decompiled APKs\Tiles Hop EDM Rush_v2.7.5_apkpure.com\assets\bin\Data\Managed\ContentReader.dll

using System;

namespace Amanotes.Data
{
  [Serializable]
  public class NoteData
  {
    public float duration = 0.0f;
    public NoteDataType type = NoteDataType.Single;
    public bool isSlash = false;
    public bool isShake = false;
    public float timeAppear;
    public int nodeID;
    public int stringIndex;
    public int noteOrder;
    public int numberNoteID;
  }
}
