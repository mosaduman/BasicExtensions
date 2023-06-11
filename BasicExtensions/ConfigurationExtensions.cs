// Decompiled with JetBrains decompiler
// Type: BasicExtensions.ConfigurationExtensions
// Assembly: BasicExtensions, Version=22.3.29.850, Culture=neutral, PublicKeyToken=null
// MVID: 79FB4BFA-4B02-474B-868E-39507878FD3D
// Assembly location: C:\Users\Musa Duman\.nuget\packages\basic.extensions\22.3.29.850\lib\net5.0\BasicExtensions.dll

using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace BasicExtensions
{
  public static class ConfigurationExtensions
  {
    public static bool IsRunningUnderIde() => Debugger.IsAttached;

    public static string GetAppPath() => Directory.GetCurrentDirectory();

    public static string GetMethodName(this MethodBase currentMethod) => currentMethod.Name;

    public static string GetMethodFullName(this MethodBase currentMethod) => currentMethod.DeclaringType.FullName + "." + currentMethod.Name;
  }
}
