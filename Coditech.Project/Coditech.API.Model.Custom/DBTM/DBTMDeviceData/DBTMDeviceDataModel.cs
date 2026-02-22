using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Coditech.Common.API.Model
{
    public class DBTMDeviceDataModel
    {
        public long DBTMDeviceDataId { get; set; }
        [JsonPropertyName("TOR")]
        public string TypeOfRecord { get; set; }
        [JsonPropertyName("TPCId")]
        public long TablePrimaryColumnId { get; set; }
        [MaxLength(100)]
        [Required]
        [JsonPropertyName("DSC")]
        public string DeviceSerialCode { get; set; }
        [MaxLength(200)]
        [Required]
        public string PersonCode { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }

        [MaxLength(50)]
        [Required]
        public string TestCode { get; set; }
        [MaxLength(200)]
        public string Comments { get; set; }
        public long CreatedBy { get; set; }
        [JsonPropertyName("TFT")]
        public DateTime TestPerformedTime { get; set; }
        [Required]
        public byte NumberOfTurn { get; set; }
        public long EntityId { get; set; }
        public List<DBTMDeviceDataDetailModel> DataList { get; set; }
    }

    public class DBTMDeviceDataDetailModel
    {
        [JsonPropertyName("PC")]
        public string ParameterCode { get; set; }
        [JsonPropertyName("PV")]
        public decimal ParameterValue { get; set; }
        public string FromTo { get; set; }
        public short Row { get; set; }
        [JsonPropertyName("U")]
        public string Unit { get; set; }
        [JsonPropertyName("C1")]
        public string Comment1 { get; set; }     
        [JsonPropertyName("C2")]
        public string Comment2 { get; set; }
        [JsonPropertyName("C3")]
        public string Comment3 { get; set; }
    }
}
