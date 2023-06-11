// Decompiled with JetBrains decompiler
// Type: BasicExtensions.Models.DatetimeFormat
// Assembly: BasicExtensions, Version=22.3.29.850, Culture=neutral, PublicKeyToken=null
// MVID: 79FB4BFA-4B02-474B-868E-39507878FD3D
// Assembly location: C:\Users\Musa Duman\.nuget\packages\basic.extensions\22.3.29.850\lib\net5.0\BasicExtensions.dll

namespace BasicExtensions.Models
{
  public static class DatetimeFormat
  {
    public static class LongDateTimeFormat
    {
      public const string TrFormat1 = "dd.MM.yyyy HH:mm:ss";
      public const string TrFormat2 = "dd/MM/yyyy HH:mm:ss";
      public const string DbFormat = "yyyy-MM-dd HH:mm:ss";
    }

    public static class ShortDateFormat
    {
      public const string TrFormat1 = "dd.MM.yyyy";
      public const string TrFormat2 = "dd/MM/yyyy";
      public const string DbFormat = "yyyy-MM-dd";
    }

    public static class TimeFormat
    {
      public const string Format1 = "HH:mm";
      public const string Format2 = "HH:mm:ss";
      public const string Format3 = "HH:mm:ss.fff";
    }
  }
}
