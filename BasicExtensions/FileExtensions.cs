// Decompiled with JetBrains decompiler
// Type: BasicExtensions.FileExtensions
// Assembly: BasicExtensions, Version=22.3.29.850, Culture=neutral, PublicKeyToken=null
// MVID: 79FB4BFA-4B02-474B-868E-39507878FD3D
// Assembly location: C:\Users\Musa Duman\.nuget\packages\basic.extensions\22.3.29.850\lib\net5.0\BasicExtensions.dll

using System;
using System.IO;

namespace BasicExtensions
{
  public static class FileExtensions
  {
    private const string _password = "Devlet-i Ebed-Müddet";

    private static string GetPath(this string startupPath, params string[] files)
    {
      string path = startupPath + "\\Settings";
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      if (files != null && files.Length != 0)
      {
        foreach (string file in files)
        {
          path = path + "\\" + file;
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        }
      }
      return path;
    }

    public static void SaveSettings<T>(this T setting, string startupPath, params string[] files)
    {
      string path = startupPath.GetPath(files) + "\\" + typeof (T).Name + ".dem";
      if (File.Exists(path))
        File.Delete(path);
      using (File.Create(path))
        ;
      using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
        {
          string json = setting.ToJson<T>();
          streamWriter.Write(json.EncryptText("Devlet-i Ebed-Müddet"));
          streamWriter.Flush();
        }
      }
    }

    public static T LoadSettings<T>(string startupPath, params string[] files)
    {
      string path = startupPath.GetPath(files) + "\\" + typeof (T).Name + ".dem";
      if (!File.Exists(path))
        return default (T);
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        using (StreamReader streamReader = new StreamReader((Stream) fileStream))
          return streamReader.ReadToEnd().DecryptText("Devlet-i Ebed-Müddet").ToModelFromJson<T>();
      }
    }

    public static DateTime? GetLastWriteDate<T>(string startupPath, params string[] files)
    {
      string path = startupPath.GetPath(files) + "\\" + typeof (T).Name + ".dem";
      return !File.Exists(path) ? new DateTime?() : new DateTime?(File.GetLastWriteTime(path));
    }
  }
}
