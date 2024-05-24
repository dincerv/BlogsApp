#nullable disable

using DataAccess.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Model
{
	public class BlogModel : RecordBase
	{
		[Required(ErrorMessage = "Title is required.")]
		[StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
		[MinLength(5, ErrorMessage = "Title must be at least 5 characters.")]
		public string Title { get; set; }

		[Required(ErrorMessage = "Content is required.")]
		[MinLength(10, ErrorMessage = "Content must be at least 10 characters.")]
		public string Content { get; set; }

		[Required(ErrorMessage = "Rating is required.")]
		[Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
		public decimal Rating { get; set; }

		[Required(ErrorMessage = "Category ID is required.")]
		public int CategoryId { get; set; }

		public CategoryModel Category { get; set; }

		public List<int> TagIds { get; set; }

		public DateTime? PublishedDate { get; set; }

		[DisplayName("Tags")]
		public string TagNamesOutput { get; set; }


		[DisplayName("Publish Date")]
		public string PublishedDateOutput { get; set; }
	}
}
