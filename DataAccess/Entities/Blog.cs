#nullable disable

using DataAccess.Records.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace DataAccess.Entities
{
	public class Blog : RecordBase
	{
		[Required]
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }

		[Required]
		public decimal Rating { get; set; }

		[Required]
		public int CategoryId { get; set; }

		[ForeignKey(nameof(CategoryId))]
		public Category Category { get; set; }

		public List<BlogTag> BlogTags { get; set; }
        public DateTime? PublishedDate { get; set; }

        [NotMapped] // Ensures EF Core does not try to map this property to a database column
        [Display(Name = "Publish Date")]
        public string PublishedDateOutput
        {
            get { return PublishedDate.HasValue ? PublishedDate.Value.ToString("yyyy-MM-dd") : "N/A"; }
        }
    }
}
