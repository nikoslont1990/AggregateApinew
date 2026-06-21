namespace AggregateApi.Domain
{
    public class AggregateRequest
    {
       public string? Date { get; set; }
       public string? SortBy { get; set; }
       public string? Company { get; set; }
       public string? Country { get; set; }
       public string? Category { get; set; }
       public string? Url { get; set; }
    }
}
