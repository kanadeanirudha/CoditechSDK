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
                    data = "sec";
                    break;
                case "TotalDistanceCovered":
                case "Distance":
                    data = "m";
                    break;
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
