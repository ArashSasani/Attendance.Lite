namespace CMS.Service.Dtos.RestrictedAccessTime
{
    public class RestrictedAccessTimeDto
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public long RowNumber { get; set; }
    }
}
