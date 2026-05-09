using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Coditech.Common.API.Model
{
    public class DBTMDeviceDataModelV2
    {
        public long DBTMDeviceDataId { get; set; }

        [JsonPropertyName("TOR")]
        public string TypeOfRecord { get; set; }

        [JsonPropertyName("TPCId")]
        public long TablePrimaryColumnId { get; set; }

        [Required, MaxLength(100)]
        [JsonPropertyName("DSC")]
        public string DeviceSerialCode { get; set; }

        [Required, MaxLength(200)]
        [JsonPropertyName("PC")]
        public string PersonCode { get; set; }

        [JsonPropertyName("W")]
        public decimal Weight { get; set; }

        [JsonPropertyName("H")]
        public decimal Height { get; set; }

        [Required, MaxLength(50)]
        [JsonPropertyName("TC")]
        public string TestCode { get; set; }

        [JsonPropertyName("CM")]
        public string Comments { get; set; }

        [JsonPropertyName("CB")]
        public long CreatedBy { get; set; }

        [JsonPropertyName("TFT")]
        public DateTime TestPerformedTime { get; set; }

        [Required]
        [JsonPropertyName("NT")]
        public byte NumberOfTurn { get; set; }

        [JsonPropertyName("EId")]
        public long EntityId { get; set; }

        [JsonPropertyName("VR")]
        public bool IsValidRecord { get; set; }

        [JsonPropertyName("DL")]
        public List<DBTMDeviceDataDetailModelV2> DataList { get; set; }
    }

    public class DBTMDeviceDataDetailModelV2
    {
        [JsonPropertyName("PC")]
        public string ParameterCode { get; set; }

        [JsonPropertyName("PV")]
        public decimal ParameterValue { get; set; }

        [JsonPropertyName("FT")]
        public string FromTo { get; set; }

        [JsonPropertyName("R")]
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