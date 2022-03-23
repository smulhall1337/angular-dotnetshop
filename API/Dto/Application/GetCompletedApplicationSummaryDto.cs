namespace API.Dto.Application
{
    public class GetCompletedApplicationSummaryDto
    {
        public bool Submitted { get; set; }
        public int Attempts { get; set; }
    }
}
