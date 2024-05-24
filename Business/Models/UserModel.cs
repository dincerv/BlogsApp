#nullable disable

using DataAccess.Enums;
using DataAccess.Records.Bases;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Model
{
    public class UserModel : RecordBase
    {
        #region Properties
        [Required(ErrorMessage = "Username field is required.")]
        [StringLength(100, ErrorMessage = "Username field cannot be longer than 50 characters.")]
        [MinLength(5, ErrorMessage = "Username field must be at least 5 characters long.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password field is required.")]
        [StringLength(100, ErrorMessage = "Password field cannot be longer than 100 characters.")]
        [MinLength(8, ErrorMessage = "Password field must be at least 8 characters long.")]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public Gender Gender { get; set; }

        public List<int> BlogIds { get; set; }
        #endregion

        #region Extra Properties
        [DisplayName("Blogs Count")]
        public int BlogCountOutput { get; set; }

        [DisplayName("Blogs Titles")]
        public string BlogTitlesOutput { get; set; }

        [DisplayName("Active Status")]
        public string IsActiveOutput { get; set; }

		[Required(ErrorMessage = "Role is required!")]
		public int? RoleId { get; set; }

		[DisplayName("Role")]
		public string RoleNameOutput { get; set; }
		#endregion

	}
}
