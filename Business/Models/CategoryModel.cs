#nullable disable

using DataAccess.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Model
{
    public class CategoryModel : RecordBase
    {
        #region
        [Required(ErrorMessage = "Name field is required.")]
        [StringLength(100, ErrorMessage = "Name field cannot be longer than 100 characters.")]
        [MinLength(3, ErrorMessage = "Name field must be at least 3 characters long.")]
        public string Name { get; set; }

        public List<int> BlogIds { get; set; }

        

        
        #endregion

        #region Extra Proporties
        [DisplayName("Category Count")]
        public int BlogCountOutput { get; set; }

        [DisplayName("Category Names")]
        public string BlogNamesOutput { get; set; }
        #endregion

    }
}
