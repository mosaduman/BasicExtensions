namespace BasicExtensions.Models.DataTable
{
    public class DataTableResponse
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public object Data { get; set; }
    }
}
