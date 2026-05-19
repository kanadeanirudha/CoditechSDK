using System.ComponentModel.DataAnnotations;
namespace Coditech.Common.API.Model
{
    public class DBTMTestWisePerformanceStandardModel : BaseModel
    {
        public int DBTMTestWisePerformanceStandardId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public int AgeGroupEnumId { get; set; }
        public int GenderEnumId { get; set; }
        public short ExcellentValue { get; set; }
        public short GoodValue { get; set; }
        public short AverageValue { get; set; }
        public short PoorValue { get; set; }
    }
}
