using System.ComponentModel.DataAnnotations;

namespace Coditech.Common.API.Model
{
    public class DBTMActivityCategoryModel : BaseModel
    {
        public short DBTMActivityCategoryId { get; set; }

        public int DBTMParentActivityCategoryId { get; set; }

        [MaxLength(50)]
        [Required]
        public string ActivityCategoryCode { get; set; }

        [MaxLength(200)]
        [Required]
        public string ActivityCategoryName { get; set; }

        public bool IsActive { get; set; }
    }
}