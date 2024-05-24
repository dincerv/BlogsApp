#nullable disable

using DataAccess.Enums;
using DataAccess.Records.Bases;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class User : RecordBase
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public Gender Gender { get; set; }

        public List<Blog> Blogs { get; set; } = new List<Blog>();

        [DisplayName("Active Status")]
        public string IsActiveOutput { get; set; }
    }
}
