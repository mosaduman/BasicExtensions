// Decompiled with JetBrains decompiler
// Type: BasicExtensions.Attribute.HtmlTableColumnAttribute
// Assembly: BasicExtensions, Version=22.3.29.850, Culture=neutral, PublicKeyToken=null
// MVID: 79FB4BFA-4B02-474B-868E-39507878FD3D
// Assembly location: C:\Users\Musa Duman\.nuget\packages\basic.extensions\22.3.29.850\lib\net5.0\BasicExtensions.dll

namespace BasicExtensions.Attribute
{
  public class HtmlTableColumnAttribute : System.Attribute
  {
    public HtmlTableColumnAttribute()
    {
    }

    public HtmlTableColumnAttribute(string name) => this.PropertyName = name;

    public string PropertyName { get; set; }
  }
}
