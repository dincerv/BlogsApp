#nullable disable

using DataAccess.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace Business.Model
{
	public class BlogTagModel : RecordBase
	{
		#region Properties
		[Required(ErrorMessage = "BlogId field is required.")]
		public int BlogId { get; set; }

		[Required(ErrorMessage = "TagId field is required.")]
		public int TagId { get; set; }

		public BlogModel Blog { get; set; }
		public TagModel Tag { get; set; }
		#endregion
	}
}
