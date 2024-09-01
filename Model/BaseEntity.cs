namespace MediatrExample.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedDateTime { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
