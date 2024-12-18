namespace BasicExtensions.Attribute
{
    public class HtmlTableColumnAttribute : System.Attribute
    {
        public HtmlTableColumnAttribute(string name) => this.PropertyName = name;
        public string PropertyName { get; set; }
    }
}