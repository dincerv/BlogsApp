#nullable disable

using DataAccess.Records.Bases;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Model
{
	public class TagModel : RecordBase
	{
		#region Properties
		[Required(ErrorMessage = "Name field is required.")]
		[StringLength(50, ErrorMessage = "Name field cannot be longer than 50 characters.")]
		[MinLength(2, ErrorMessage = "Name field must be at least 2 characters long.")]
		public string Name { get; set; }

		public bool IsPopular { get; set; }

		public List<int> BlogIds { get; set; }
		#endregion

		#region Extra Properties
		[DisplayName("Associated Blogs")]
		public string BlogNamesOutput { get; set; }
		#endregion
	}
}
