using System.IO;
using UnityEngine;

public class Utils
{
    public static string LocalPath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    public static string LoadFile(string fileName)
    {
        string str = string.Empty;
        string path = Utils.LocalPath(fileName);
        if (File.Exists(path))
        {
            StreamReader streamReader = File.OpenText(path);
            str = streamReader.ReadToEnd();
            streamReader.Close();
        }
        return str;
    }

    public static byte[] LoadFileBinary(string path)
    {
        byte[] numArray = (byte[])null;
        if (File.Exists(path))
        {
            using (FileStream fileStream = File.OpenRead(path))
            {
                using (BinaryReader binaryReader = new BinaryReader((Stream)fileStream))
                    numArray = binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
        return numArray;
    }

    public static byte[] LoadFileFromResources(string path)
    {
        byte[] numArray = (byte[])null;
        TextAsset txtAsset = Resources.Load<TextAsset>(path);
        if(txtAsset != null)
            numArray = txtAsset.bytes;

        return numArray;
    }

}
