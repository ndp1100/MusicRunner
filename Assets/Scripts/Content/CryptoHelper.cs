// Decompiled with JetBrains decompiler
// Type: Amanotes.Content.CryptoHelper
// Assembly: ContentReader, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 31109648-5020-4775-A5CF-CD35FB59B4E1
// Assembly location: D:\WorkingTools\APKEasyTool\1-Decompiled APKs\Tiles Hop EDM Rush_v2.7.5_apkpure.com\assets\bin\Data\Managed\ContentReader.dll

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Amanotes.Content
{
  public static class CryptoHelper
  {
    public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
    {
      byte[] numArray = (byte[]) null;
      byte[] salt = new byte[8]
      {
        (byte) 1,
        (byte) 2,
        (byte) 3,
        (byte) 4,
        (byte) 5,
        (byte) 6,
        (byte) 7,
        (byte) 8
      };
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
        {
          rijndaelManaged.KeySize = 256;
          rijndaelManaged.BlockSize = 128;
          Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, salt, 1000);
          rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
          rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
          rijndaelManaged.Mode = CipherMode.CBC;
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
          {
            cryptoStream.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
            cryptoStream.Close();
          }
          numArray = memoryStream.ToArray();
        }
      }
      return numArray;
    }

    public static byte[] DeCryptContentFile(byte[] bytesToBeDecrypted)
    {
      byte[] hash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(GlobalKey.GetMidiKey()));
      return CryptoHelper.AES_Decrypt(bytesToBeDecrypted, hash);
    }
  }
}
