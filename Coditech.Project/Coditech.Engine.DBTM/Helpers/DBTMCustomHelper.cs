using Coditech.Common.API.Model;
using System.Data;

namespace Coditech.Engine.DBTM.Helpers
{
    public static class DBTMCustomHelper
    {
        public static void Calculation(string calculationCode, string calculationName, DataRow newRow, List<DBTMReportsModel> dBTMReportsList, DateTime createdDate)
        {
            double weight = Convert.ToDouble(dBTMReportsList.FirstOrDefault(x => x.CreatedDate == createdDate)?.Weight);
            switch (calculationCode)
            {
                case "CompletionTime":
                    decimal completionTime = dBTMReportsList.Where(x => x.ParameterCode == "Time" && x.CreatedDate == createdDate).Sum(x => x.ParameterValue);
                    newRow[calculationName] = $"{completionTime} {Unit(calculationCode)}";
                    break;
                case "AverageVelocity":
                    decimal totalDistance = dBTMReportsList.Where(x => x.ParameterCode == "Distance" && x.CreatedDate == createdDate).Sum(x => x.ParameterValue);
                    decimal totalTime = dBTMReportsList.Where(x => x.ParameterCode == "Time" && x.CreatedDate == createdDate).Sum(x => x.ParameterValue);
                    newRow[calculationName] = totalTime != 0 && totalDistance != 0 ? $"{Math.Round(totalDistance / totalTime, 3)} {Unit(calculationCode)}" : "Invalid Data";
                    break;
                case "TotalDistanceCovered":
                    decimal totalDistanceCovered = dBTMReportsList.Where(x => x.ParameterCode == "Distance" && x.CreatedDate == createdDate).Sum(x => x.ParameterValue);
                    newRow[calculationName] = totalDistanceCovered != 0 ? $"{Math.Round(totalDistanceCovered, 3)} {Unit(calculationCode)}" : "Invalid Data";
                    break;
                case "MaxLap":
                    newRow[calculationName] = $"{dBTMReportsList.Where(x => x.ParameterCode == "Time" && x.CreatedDate == createdDate).Max(x => x.ParameterValue)} {Unit(calculationCode)}";
                    break;
                case "MinLap":
                    newRow[calculationName] = $"{dBTMReportsList.Where(x => x.ParameterCode == "Time" && x.CreatedDate == createdDate).Min(x => x.ParameterValue)} {Unit(calculationCode)}";
                    break;
                case "Power":
                    double jumpHeight = Convert.ToDouble(dBTMReportsList.FirstOrDefault(x => x.ParameterCode == "JumpHeight" && x.CreatedDate == createdDate)?.ParameterValue);
                    newRow[calculationName] = weight == 0 ? "N/A" : $"{Math.Round(weight * Math.Pow(9.81, 1.5) * Math.Sqrt(2 * jumpHeight) / 4, 3)} {Unit(calculationCode)}";
                    break;
                case "Force":
                    newRow[calculationName] = weight == 0 ? "N/A" : $"{Math.Round(4 * weight * 9.81, 3)} {Unit(calculationCode)}";
                    break;
                default:
                    newRow[calculationName] = "N/A";
                    break;
            }
        }

        public static string Calculation(string calculationCode, string calculationName, DataRow newRow, IGrouping<string, DBTMReportsModel> group, int recurtion, bool isDisplayUnit = false)
        {
            double weight = Convert.ToDouble(group.FirstOrDefault()?.Weight);
            calculationName = string.IsNullOrEmpty(calculationName) ? calculationCode : calculationName;
            string result = "NA";
            switch (calculationCode)
            {
                case "CompletionTime":
                    decimal completionTime = group.Where(x => x.ParameterCode == "Time").Sum(x => x.ParameterValue);
                    result = $"{completionTime}";
                    break;
                case "AverageVelocity":
                    decimal totalDistance = group.Where(x => x.ParameterCode == "Distance").Sum(x => x.ParameterValue);
                    decimal totalTime = group.Where(x => x.ParameterCode == "Time").Sum(x => x.ParameterValue);
                    result = totalTime != 0 && totalDistance != 0 ? $"{Math.Round(totalDistance / totalTime, 3)}" : "Invalid Data";
                    break;
                case "TotalDistanceCovered":
                    decimal totalDistanceCovered = group.Where(x => x.ParameterCode == "Distance").Sum(x => x.ParameterValue);
                    result = totalDistanceCovered != 0 ? $"{Math.Round(totalDistanceCovered, 3)}" : "Invalid Data";
                    break;
                case "MaxLap":
                    result = $"{group.Where(x => x.ParameterCode == "Time").Max(x => x.ParameterValue)}";
                    break;
                case "MinLap":
                    result = $"{group.Where(x => x.ParameterCode == "Time").Min(x => x.ParameterValue)}";
                    break;
                case "Power":
                    double jumpHeight = Convert.ToDouble(group.FirstOrDefault(x => x.ParameterCode == "JumpHeight")?.ParameterValue);
                    return weight == 0 ? "N/A" : $"{Math.Round(weight * Math.Pow(9.81, 1.5) * Math.Sqrt(2 * jumpHeight) / 4, 3)}";
                case "Force":
                    result = weight == 0 ? "N/A" : $"{Math.Round(4 * weight * 9.81, 3)}";
                    break;
                case "CumulativeTime":
                    decimal cumulativeTime = 0;
                    for (int i = 1; i <= recurtion; i++)
                    {
                        cumulativeTime += group.FirstOrDefault(x => x.ParameterCode == "Time" && x.Row == i.ToString()).ParameterValue;
                    }
                    result = $"{cumulativeTime}";
                    break;
                case "Velocity":
                    decimal distance = group.Where(x => x.ParameterCode == "Distance").Sum(x => x.ParameterValue);
                    decimal time = group.FirstOrDefault(x => x.ParameterCode == "Time" && x.Row == recurtion.ToString()).ParameterValue;
                    result = time != 0 && distance != 0 ? $"{Math.Round(distance / time, 3)}" : "Invalid Data";
                    break;
                case "CumulativeVelocity":
                    distance = group.Where(x => x.ParameterCode == "Distance").Sum(x => x.ParameterValue);
                    cumulativeTime = 0;
                    for (int i = 1; i <= recurtion; i++)
                    {
                        cumulativeTime += group.FirstOrDefault(x => x.ParameterCode == "Time" && x.Row == i.ToString()).ParameterValue;
                    }
                    result = cumulativeTime != 0 && distance != 0 ? $"{Math.Round(distance * recurtion / cumulativeTime, 3)}" : "Invalid Data";
                    break;
            }
            return result = isDisplayUnit ? $"{result} {Unit(calculationCode)}" : result;
        }

        public static string Unit(string parameterCode)
        {
            string data = string.Empty;
            switch (parameterCode)
            {
                case "CompletionTime":
                case "Time":
                case "MaxLap":
                case "MinLap":
                case "AirTime":
                case "CumulativeTime":
                    data = "sec";
                    break;
                case "TotalDistanceCovered":
                case "PersonDetectionRange":
                case "Distance":
                    data = "m";
                    break;
                case "CumulativeVelocity":
                case "AverageVelocity":
                case "Velocity":
                    data = "m/s";
                    break;
                case "Power":
                    data = "watts";
                    break;
                case "Force":
                    data = "newtons";
                    break;
                case "Weight":
                    data = "kg";
                    break;
                case "Height":
                    data = "cm";
                    break;
                case "JumpHeight":
                    data = "cm";
                    break;
                default:
                    data = "";
                    break;
            }
            return data;
        }
    }
}
