namespace Coditech.Common.API.Model.Response;

public class CustomTrueFalseResponse
{
    public bool IsSuccess { get; set; }
    public bool HasError { get; set; }

    public string ErrorMessage { get; set; }

    public int? ErrorCode { get; set; }

    public string DUI { get; set; }
}