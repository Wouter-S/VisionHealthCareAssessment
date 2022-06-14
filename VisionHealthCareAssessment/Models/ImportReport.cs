namespace VisionHealthCareAssessment.Models
{
    public class ImportReport
    {
        public int ProductsReadCount { get; set; }
        public int ProductsInsertedCount { get; set; }
        public int ProductsInvalidCount
        {
            get
            {
                return ValidationReport.Count;
            }
        }
        public List<string> ValidationReport { get; set; } = new List<string>();
    }
}
