using Coditech.Common.API.Model;
using Coditech.Common.Helper.Utilities;
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
                case CustomConstants.CompletionTime:
                    decimal completionTime = dBTMReportsList.Where(x => x.ParameterCode == CustomConstants.Time && x.CreatedDate == createdDate).Sum(x => x.ParameterValue);
                    newRow[calculationName] = $"{completionTime} {Unit(calculationCode)}";
                    break;
                case CustomConstants.AverageVelocity:
                    decimal totalDistance = dBTMReportsList.Where(x => x.ParameterCode == CustomConstants.Distance && x.CreatedDate == createdDate).Sum(x => x.ParameterValue);
                    decimal totalTime = dBTMReportsList.Where(x => x.ParameterCode == CustomConstants.Time && x.CreatedDate == createdDate).Sum(x => x.ParameterValue);
                    newRow[calculationName] = totalTime != 0 && totalDistance != 0 ? $"{Math.Round(totalDistance / totalTime, CustomConstants.GraphListRoundUpValue)} {Unit(calculationCode)}" : "Invalid Data";
                    break;
                case CustomConstants.TotalDistanceCovered:
                    decimal totalDistanceCovered = dBTMReportsList.Where(x => x.ParameterCode == CustomConstants.Distance && x.CreatedDate == createdDate).Sum(x => x.ParameterValue);
                    newRow[calculationName] = totalDistanceCovered != 0 ? $"{Math.Round(totalDistanceCovered, CustomConstants.GraphListRoundUpValue)} {Unit(calculationCode)}" : "Invalid Data";
                    break;
                case CustomConstants.MaxLap:
                    newRow[calculationName] = $"{dBTMReportsList.Where(x => x.ParameterCode == CustomConstants.Time && x.CreatedDate == createdDate).Max(x => x.ParameterValue)} {Unit(calculationCode)}";
                    break;
                case CustomConstants.MinLap:
                    newRow[calculationName] = $"{dBTMReportsList.Where(x => x.ParameterCode == CustomConstants.Time && x.CreatedDate == createdDate).Min(x => x.ParameterValue)} {Unit(calculationCode)}";
                    break;
                case CustomConstants.Power:
                    double jumpHeight = Convert.ToDouble(dBTMReportsList.FirstOrDefault(x => x.ParameterCode == CustomConstants.JumpHeight && x.CreatedDate == createdDate)?.ParameterValue);
                    newRow[calculationName] = weight == 0 ? "NA" : $"{Math.Round(weight * Math.Pow(9.81, 1.5) * Math.Sqrt(2 * jumpHeight) / 4, CustomConstants.GraphListRoundUpValue)} {Unit(calculationCode)}";
                    break;
                case CustomConstants.Force:
                    newRow[calculationName] = weight == 0 ? "NA" : $"{Math.Round(4 * weight * 9.81, CustomConstants.GraphListRoundUpValue)} {Unit(calculationCode)}";
                    break;
                default:
                    newRow[calculationName] = "NA";
                    break;
            }
        }

        public static string Calculation(string calculationCode, string calculationName, IGrouping<string, DBTMReportsModel> group, Int16 recurtion, bool isDisplayUnit = false, bool isGraph = false)
        {
            double weight = Convert.ToDouble(group.FirstOrDefault()?.Weight);
            calculationName = string.IsNullOrEmpty(calculationName) ? calculationCode : calculationName;
            string result = isGraph ? "0" : "NA";
            switch (calculationCode)
            {
                case CustomConstants.CompletionTime:
                    decimal completionTime = group.Where(x => x.ParameterCode == CustomConstants.Time).Sum(x => x.ParameterValue);
                    result = $"{Math.Round(completionTime / recurtion, CustomConstants.GraphListRoundUpValue)}";
                    break;
                case CustomConstants.AverageTotalCompletionTime:
                    completionTime = group.Where(x => x.ParameterCode == CustomConstants.Time).Sum(x => x.ParameterValue);
                    result = $"{Math.Round(completionTime / recurtion, CustomConstants.GraphListRoundUpValue)}";
                    break;
                case CustomConstants.AverageVelocity:
                    decimal totalDistance = group.Where(x => x.ParameterCode == CustomConstants.Distance).Sum(x => x.ParameterValue);
                    decimal totalTime = group.Where(x => x.ParameterCode == CustomConstants.Time).Sum(x => x.ParameterValue);
                    result = totalTime != 0 && totalDistance != 0 ? $"{Math.Round(totalDistance / totalTime, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : "Invalid Data";
                    break;
                case CustomConstants.AverageTotalVelocity:
                    totalDistance = group.Where(x => x.ParameterCode == CustomConstants.Distance).Sum(x => x.ParameterValue);
                    totalTime = group.Where(x => x.ParameterCode == CustomConstants.Time).Sum(x => x.ParameterValue);
                    totalTime = totalTime != 0 ? (totalTime / recurtion) : totalTime;
                    result = totalTime != 0 && totalDistance != 0 ? $"{Math.Round(totalDistance / totalTime, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : "Invalid Data";
                    break;
                case CustomConstants.TotalDistanceCovered:
                    decimal totalDistanceCovered = group.Where(x => (x.ParameterCode == CustomConstants.Distance || x.ParameterCode == CustomConstants.DistanceMultiplyByRow) && x.Row != 0).Sum(x => x.ParameterValue);
                    result = totalDistanceCovered != 0 ? $"{Math.Round(totalDistanceCovered, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : "Invalid Data";
                    break;
                case CustomConstants.DistanceMultiplyByRow:
                    decimal distance = group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Distance || x.ParameterCode == CustomConstants.DistanceMultiplyByRow && x.Row == recurtion).ParameterValue;
                    result = distance != 0 ? $"{Math.Round(distance * recurtion, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : "Invalid Data";
                    break;
                case CustomConstants.MaxLap:
                    result = $"{group.Where(x => x.ParameterCode == CustomConstants.Time).Max(x => x.ParameterValue)}";
                    break;
                case CustomConstants.MinLap:
                    result = $"{group.Where(x => x.ParameterCode == CustomConstants.Time).Min(x => x.ParameterValue)}";
                    break;
                case CustomConstants.Power:
                    double jumpHeight = Convert.ToDouble(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.JumpHeight)?.ParameterValue);
                    return weight == 0 ? "NA" : $"{Math.Round(weight * Math.Pow(9.81, 1.5) * Math.Sqrt(2 * jumpHeight) / 4, CustomConstants.GraphListRoundUpValue)}";
                case CustomConstants.Force:
                    result = weight == 0 ? "NA" : $"{Math.Round(4 * weight * 9.81, CustomConstants.GraphListRoundUpValue)}";
                    break;
                case CustomConstants.CumulativeTime:
                    decimal cumulativeTime = 0;
                    for (int i = 1; i <= recurtion; i++)
                    {
                        cumulativeTime += group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.Row == i).ParameterValue;
                    }
                    result = $"{cumulativeTime}";
                    break;
                case CustomConstants.Velocity:
                    distance = group.Where(x => x.ParameterCode == CustomConstants.Distance).Sum(x => x.ParameterValue);
                    decimal time = group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.Row == recurtion).ParameterValue;
                    result = time != 0 && distance != 0 ? $"{Math.Round(distance / time, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : "Invalid Data";
                    break;
                case CustomConstants.VelocityByRow:
                    result = VelocityByRow(group, recurtion, isGraph);
                    break;
                case CustomConstants.CumulativeVelocityByRow:
                    distance = group.FirstOrDefault(x => x.ParameterCode == CustomConstants.DistanceMultiplyByRow && x.Row == recurtion).ParameterValue;
                    time = group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.Row == recurtion).ParameterValue;
                    result = time != 0 && distance != 0 ? $"{Math.Round(distance * recurtion / time, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : "Invalid Data";
                    break;
                case CustomConstants.CumulativeVelocity:
                    distance = group.Where(x => x.ParameterCode == CustomConstants.Distance).Sum(x => x.ParameterValue);
                    cumulativeTime = 0;
                    for (int i = 1; i <= recurtion; i++)
                    {
                        cumulativeTime += group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.Row == i).ParameterValue;
                    }
                    result = cumulativeTime != 0 && distance != 0 ? $"{Math.Round(distance * recurtion / cumulativeTime, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : "Invalid Data";
                    break;
                case CustomConstants.AccelerationByRow:
                    result = AccelerationByRow(group, recurtion, isGraph);
                    break;
                case CustomConstants.ForceByRow:
                    result = ForceByRow(group, recurtion, weight, isGraph);
                    break;
                case CustomConstants.PowerByRow:
                    var velocityByRow = Convert.ToDecimal(VelocityByRow(group, recurtion, isGraph));
                    var forceByRow = Convert.ToDecimal(ForceByRow(group, recurtion, weight, isGraph));
                    result = weight == 0 ? "NA" : $"{Math.Round(forceByRow * velocityByRow, CustomConstants.GraphListRoundUpValue)}";
                    break;
                case CustomConstants.ChangeOfDirection:
                    decimal time2, time3;
                    result = ChangeOfDirection(group);
                    break;
                case CustomConstants.AgilityDeficitRatio:
                    var changeOfDirection = Convert.ToDecimal(ChangeOfDirection(group));
                    var time1 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "A-B")?.ParameterValue);
                    result = changeOfDirection > 0 && time1 > 0 ? $"{Math.Round(changeOfDirection / time1, CustomConstants.GraphListRoundUpValue)}" : "0";
                    break;
                case CustomConstants.ChangeOfDirectionDeficit:
                    changeOfDirection = Convert.ToDecimal(ChangeOfDirection(group));
                    time1 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "A-B")?.ParameterValue);
                    result = changeOfDirection > 0 && time1 > 0 ? $"{Math.Round(changeOfDirection - time1, CustomConstants.GraphListRoundUpValue)}" : "0";
                    break;
                case CustomConstants.ChangeOfDirectionRatio:
                    time2 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "B-C")?.ParameterValue);
                    time3 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "C-B")?.ParameterValue);
                    result = time2 > 0 & time3 > 0 ? $"{Math.Round(time3 / time2, CustomConstants.GraphListRoundUpValue)}" : "0";
                    break;
                case CustomConstants.JumpHeight:
                    decimal totalJumpHeight = group.Where(x => x.ParameterCode == CustomConstants.JumpHeight).Sum(x => x.ParameterValue);
                    result = totalJumpHeight > 0 ? $"{Math.Round(totalJumpHeight / recurtion, CustomConstants.GraphListRoundUpValue)}" : "0";
                    break;
                case CustomConstants.JumpLength:
                    decimal totalJumpLength = group.Where(x => x.ParameterCode == CustomConstants.JumpLength).Sum(x => x.ParameterValue);
                    result = totalJumpLength > 0 ? $"{Math.Round(totalJumpLength / recurtion, CustomConstants.GraphListRoundUpValue)}" : "0";
                    break;
                case CustomConstants.TotalCount:
                    decimal totalCount = group.Where(x => x.ParameterCode == CustomConstants.Count).Sum(x => x.ParameterValue);
                    result = $"{Convert.ToInt32(totalCount)}";
                    break;
                case CustomConstants.CountByTime:
                    decimal count = group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Count && x.Row == recurtion).ParameterValue;
                    time = group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.Row == recurtion).ParameterValue;
                    result = time != 0 && count != 0 ? $"{Math.Round(count / time, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : "Invalid Data";
                    break;
            }
            return result = isDisplayUnit ? $"{result} {Unit(calculationCode)}" : result;
        }

        private static string ChangeOfDirection(IGrouping<string, DBTMReportsModel> group)
        {
            string result;
            var time2 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "B-C")?.ParameterValue);
            var time3 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "C-B")?.ParameterValue);
            result = time2 > 0 & time3 > 0 ? $"{Math.Round(time2 + time3, CustomConstants.GraphListRoundUpValue)}" : "0";
            return result;
        }

        private static string ForceByRow(IGrouping<string, DBTMReportsModel> group, short recurtion, double weight, bool isGraph)
        {
            string result;
            var accelerationByRow = Convert.ToDecimal(AccelerationByRow(group, recurtion, isGraph));
            result = weight == 0 ? "NA" : $"{Math.Round(Convert.ToDecimal(weight) * accelerationByRow, CustomConstants.GraphListRoundUpValue)}";
            return result;
        }

        private static string AccelerationByRow(IGrouping<string, DBTMReportsModel> group, short recurtion, bool isGraph)
        {
            string result;
            var timeValue = group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.Row == recurtion)?.ParameterValue ?? 0;
            if (recurtion == 1)
            {
                var velocityValue = Convert.ToDecimal(VelocityByRow(group, 1, isGraph));
                result = timeValue != 0 ? $"{Math.Round(velocityValue / timeValue, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : "Invalid Data";
            }
            else
            {
                var velocityValueCurrent = Convert.ToDecimal(VelocityByRow(group, recurtion, isGraph));
                var velocityValueBefore = Convert.ToDecimal(VelocityByRow(group, (short)(recurtion - 1), isGraph));
                result = timeValue != 0 ? $"{Math.Round((velocityValueCurrent - velocityValueBefore) / timeValue, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : "Invalid Data";
            }

            return result;
        }

        private static string VelocityByRow(IGrouping<string, DBTMReportsModel> group, short recurtion, bool isGraph)
        {
            string result = string.Empty;
            decimal distance = group.FirstOrDefault(x => x.ParameterCode == CustomConstants.DistanceMultiplyByRow && x.Row == recurtion).ParameterValue;
            decimal time = group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.Row == recurtion).ParameterValue;
            result = time != 0 && distance != 0 ? $"{Math.Round(distance / time, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : "Invalid Data";
            return result;
        }

        public static string Unit(string parameterCode)
        {
            string data = string.Empty;
            switch (parameterCode)
            {
                case CustomConstants.CompletionTime:
                case CustomConstants.Time:
                case CustomConstants.MaxLap:
                case CustomConstants.MinLap:
                case CustomConstants.AirTime:
                case CustomConstants.CumulativeTime:
                case CustomConstants.ChangeOfDirection:
                case CustomConstants.ChangeOfDirectionDeficit:
                    data = "sec";
                    break;
                case CustomConstants.TotalDistanceCovered:
                case CustomConstants.PersonDetectionRange:
                case CustomConstants.DistanceMultiplyByRow:
                case CustomConstants.Distance:
                    data = "m";
                    break;
                case CustomConstants.CumulativeVelocity:
                case CustomConstants.AverageVelocity:
                case CustomConstants.VelocityByRow:
                case CustomConstants.CumulativeVelocityByRow:
                case CustomConstants.Velocity:
                    data = "m/s";
                    break;
                case CustomConstants.Power:
                case CustomConstants.PowerByRow:
                    data = "W";
                    break;
                case CustomConstants.Force:
                case CustomConstants.ForceByRow:
                    data = "N";
                    break;
                case CustomConstants.Weight:
                    data = "kg";
                    break;
                case CustomConstants.Height:
                    data = "cm";
                    break;
                case CustomConstants.JumpHeight:
                case CustomConstants.JumpLength:
                    data = "cm";
                    break;
                case CustomConstants.AccelerationByRow:
                    data = "m/s^2";
                    break;
                default:
                    data = "";
                    break;
            }
            return data;
        }
    }
}
