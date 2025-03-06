using System.ComponentModel.DataAnnotations;

namespace Coditech.Common.API.Model
{
    public class DBTMTestModel : BaseModel
    {
        public int DBTMTestMasterId { get; set; }
        public short DBTMActivityCategoryId { get; set; }

        [Required]
        [MaxLength(200)]
        public string TestName { get; set; }

        [Required]
        [MaxLength(50)]
        public string TestCode { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public List<string> DBTMSelectedTestParameter { get; set; }
        public int TopActivityCount { get; set; }
        public byte DBTMTestParameterId { get; set; }
        [Required]
        public List<string> DBTMSelectedTestCalculation { get; set; }
        public byte DBTMTestCalculationId { get; set; }

    }
}
