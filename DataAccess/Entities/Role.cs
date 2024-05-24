#nullable disable

// Way 1:
// namespace DataAccess.Entities;
// Way 2:
using DataAccess.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities // namespace DataAccess.Entities; can also be written
							  // therefore we don't need to use curly braces
{
	public class Role : RecordBase
	{


		[Required]
		[StringLength(5, MinimumLength = 4)]
		public string Name { get; set; } 
		public List<User> Users { get; set; }
	}
}