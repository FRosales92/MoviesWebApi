namespace MoviesWebApi.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        public int recordsPerPage = 10;
        public readonly int maxRecordsPerPage = 50;

        public int RecordPerPage
        {
            get => recordsPerPage;
            set
            {
                //Ff user sent value mayor that "maxRecordsPerPage" return "50" records per page if not, return value.
                recordsPerPage = (value > maxRecordsPerPage) ? maxRecordsPerPage : value;
            }
        }


    }
}
