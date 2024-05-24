#nullable  disable

using DataAccess.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Category : RecordBase
    {
        [Required]
        public string Name { get; set; }

        public List<Blog> Blogs { get; set; }
    }
}
