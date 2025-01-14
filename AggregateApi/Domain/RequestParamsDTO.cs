using System.ComponentModel.DataAnnotations;

namespace AggregateApi.Model
{
    public class RequestParamsDTO
    {
        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        public string? SortBy { get; set; }

        [Required(ErrorMessage = "Order is required.")]
        public string Order { get; set; } = "asc";  // Default to "asc" if not provided

        public string? Category { get; set; }
    }
}
