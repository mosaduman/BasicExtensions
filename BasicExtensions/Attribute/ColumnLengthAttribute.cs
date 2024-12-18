namespace BasicExtensions.Attribute
{
    public class ColumnLengthAttribute : System.Attribute
    {
        public int Length { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public ColumnLengthAttribute(int length) => this.Length = length;
        public ColumnLengthAttribute(int precision, int scale)
        {
            this.Precision = precision;
            this.Scale = scale;
        }
    }
}