namespace DataAccess.Records.Bases
{
    public abstract class RecordBase
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
