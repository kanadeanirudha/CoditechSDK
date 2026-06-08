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

        [Required, MaxLength(100)]
        [JsonPropertyName("DSC")]
        public string DeviceSerialCode { get; set; }

        [Required, MaxLength(200)]
        public string PersonCode { get; set; }

        [Required, MaxLength(200)]
        [JsonPropertyName("PC")]
        public string PersonCodeAlias
        {
            get => PersonCode;
            set => PersonCode = value;
        }
        public decimal Weight { get; set; }

        [JsonPropertyName("W")]
        public decimal WeightAlias
        {
            get => Weight;
            set => Weight = value;
        }

        public decimal Height { get; set; }
        [JsonPropertyName("H")]
        public decimal HeightAlias
        {
            get => Height;
            set => Height = value;
        }

        [Required, MaxLength(50)]
        public string TestCode { get; set; }
        [JsonPropertyName("TC")]
        public string TestCodeAlice
        {
            get => TestCode;
            set => TestCode = value;
        }

        public string Comments { get; set; }
        [JsonPropertyName("CM")]
        public string CommentsAlice
        {
            get => Comments;
            set => Comments = value;
        }

        public long CreatedBy { get; set; }
        [JsonPropertyName("CB")]
        public long CreatedByAlice
        {
            get => CreatedBy;
            set => CreatedBy = value;
        }

        [JsonPropertyName("TFT")]
        public DateTime TestPerformedTime { get; set; }

        [Required]
        public byte NumberOfTurn { get; set; }

        [Required]
        [JsonPropertyName("NT")]
        public byte NumberOfTurnAlice
        {
            get => NumberOfTurn;
            set => NumberOfTurn = value;
        }

        public List<DBTMDeviceDataDetailModel> DataList { get; set; }
        [JsonPropertyName("DL")]
        public List<DBTMDeviceDataDetailModel> DataListAlice
        {
            get => DataList;
            set => DataList = value;
        }
        [JsonPropertyName("DUI")]
        public string DataUniqueId { get; set; }
    }

    public class DBTMDeviceDataDetailModel
    {
        [JsonPropertyName("PC")]
        public string ParameterCode { get; set; }

        [JsonPropertyName("PV")]
        public decimal ParameterValue { get; set; }

        [JsonPropertyName("FT")]
        public string FromToAlice
        {
            get => FromTo;
            set => FromTo = value;
        }
        public string FromTo { get; set; }

        [JsonPropertyName("R")]
        public short RowAlice
        {
            get => Row;
            set => Row = value;
        }
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