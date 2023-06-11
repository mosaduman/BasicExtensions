// Decompiled with JetBrains decompiler
// Type: BasicExtensions.CollectionExtensions
// Assembly: BasicExtensions, Version=22.3.29.850, Culture=neutral, PublicKeyToken=null
// MVID: 79FB4BFA-4B02-474B-868E-39507878FD3D
// Assembly location: C:\Users\Musa Duman\.nuget\packages\basic.extensions\22.3.29.850\lib\net5.0\BasicExtensions.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace BasicExtensions
{
  public static class CollectionExtensions
  {
    //public static List<T> ToList<T>(this DataSet ds) => ds.Tables[typeof (T).Name].ToList<T>();

    //public static List<T> ToList<T>(this DataTable dt)
    //{
    //  List<T> list = new List<T>();
    //  foreach (DataRow row in (InternalDataCollectionBase) dt.Rows)
    //  {
    //    T instance = Activator.CreateInstance<T>();
    //    foreach (PropertyInfo property in typeof (T).GetProperties())
    //    {
    //      if (dt.Columns.Contains(property.Name) && row[property.Name] != DBNull.Value)
    //        property.SetValue((object) instance, row[property.Name], (object[]) null);
    //    }
    //    list.Add(instance);
    //  }
    //  return list;
    //}

    public static List<T> CopyList<T>(this List<T> entities)
    {
      try
      {
        T[] objArray = new T[entities.Count];
        entities.CopyTo(objArray);
        return ((IEnumerable<T>) objArray).ToList<T>();
      }
      catch (Exception ex)
      {
        return (List<T>) null;
      }
    }

    public static IEnumerable<IEnumerable<T>> Paging<T>(
      this List<T> src,
      int pageSize = 100)
    {
      List<T>.Enumerator enumerator = src.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          List<T> objList = new List<T>(pageSize)
          {
            enumerator.Current
          };
          while (objList.Count < pageSize && enumerator.MoveNext())
            objList.Add(enumerator.Current);
          yield return (IEnumerable<T>) objList;
        }
      }
      finally
      {
        enumerator.Dispose();
      }
      enumerator = new List<T>.Enumerator();
    }

    public static bool HasItems<T>(this IEnumerable<T> src) => src != null && src.Any<T>();

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source == null || !source.Any<T>();
  }
}
