#nullable disable

using DataAccess.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Tag : RecordBase
    {
        [Required]
        public string Name { get; set; }

        public bool IsPopular { get; set; }

        public List<BlogTag> BlogTags { get; set; }

    }
}
