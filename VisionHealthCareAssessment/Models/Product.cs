namespace VisionHealthCareAssessment.Models
{
    public class Product
    {
        public Guid? ProductId { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public Guid? ProductGroupId { get; set; }
    }
}
