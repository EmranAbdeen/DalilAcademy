namespace DalilAcademy.Core.DTOs
{
    // Used to divide large amounts of data into smaller pages, instead of returning all records at once.
    // For example, 10 records per page (Page 1: records 1-10, Page 2: records 11-20, and so on).
    public record PagedResult<T>(List<T> Items, int Page, int PageSize, int TotalCount)
    {
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
