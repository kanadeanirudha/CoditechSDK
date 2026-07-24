using Coditech.Common.API.Model;
using System.Data;
using QRCoder;
namespace Coditech.Common.Helper.Utilities
{
    public static class DBTMCustomHelper
    {
        public static void Calculation(string calculationCode, string calculationName, DataRow newRow, List<DBTMReportsModel> dBTMReportsList, DateTime createdDate)
        {
            double weight = Convert.ToDouble(dBTMReportsList.FirstOrDefault(x => x.CreatedDate == createdDate)?.Weight);
            switch (calculationCode)
            {
                case CustomConstants.CompletionTime:
                    decimal completionTime = dBTMReportsList.Where(x => x.ParameterCode == CustomConstants.Time && x.CreatedDate == createdDate).Sum(x => Convert.ToDecimal(x.ParameterValue));
                    newRow[calculationName] = $"{completionTime} {Unit(calculationCode)}";
                    break;
                case CustomConstants.AverageVelocity:
                    decimal totalDistance = dBTMReportsList.Where(x => x.ParameterCode == CustomConstants.Distance && x.CreatedDate == createdDate).Sum(x => Convert.ToDecimal(x.ParameterValue));
                    decimal totalTime = dBTMReportsList.Where(x => x.ParameterCode == CustomConstants.Time && x.CreatedDate == createdDate).Sum(x => Convert.ToDecimal(x.ParameterValue));
                    newRow[calculationName] = totalTime != 0 && totalDistance != 0 ? $"{Math.Round(totalDistance / totalTime, CustomConstants.GraphListRoundUpValue)} {Unit(calculationCode)}" : CustomConstants.InvalidData;
                    break;
                case CustomConstants.TotalDistanceCovered:
                    decimal totalDistanceCovered = dBTMReportsList.Where(x => x.ParameterCode == CustomConstants.Distance && x.CreatedDate == createdDate).Sum(x => Convert.ToDecimal(x.ParameterValue));
                    newRow[calculationName] = totalDistanceCovered != 0 ? $"{Math.Round(totalDistanceCovered, CustomConstants.GraphListRoundUpValue)} {Unit(calculationCode)}" : CustomConstants.InvalidData;
                    break;
                case CustomConstants.MaxLap:
                    newRow[calculationName] = $"{dBTMReportsList.Where(x => x.ParameterCode == CustomConstants.Time && x.CreatedDate == createdDate).Max(x => x.ParameterValue)} {Unit(calculationCode)}";
                    break;
                case CustomConstants.MinLap:
                    newRow[calculationName] = $"{dBTMReportsList.Where(x => x.ParameterCode == CustomConstants.Time && x.CreatedDate == createdDate).Min(x => x.ParameterValue)} {Unit(calculationCode)}";
                    break;
                case CustomConstants.Power:
                    double jumpHeight = Convert.ToDouble(dBTMReportsList.FirstOrDefault(x => x.ParameterCode == CustomConstants.JumpHeight && x.CreatedDate == createdDate)?.ParameterValue);
                    newRow[calculationName] = weight == 0 ? CustomConstants.NA : $"{Math.Round(weight * Math.Pow(9.81, 1.5) * Math.Sqrt(2 * jumpHeight) / 4, CustomConstants.GraphListRoundUpValue)} {Unit(calculationCode)}";
                    break;
                case CustomConstants.Force:
                    newRow[calculationName] = weight == 0 ? CustomConstants.NA : $"{Math.Round(4 * weight * 9.81, CustomConstants.GraphListRoundUpValue)} {Unit(calculationCode)}";
                    break;
                default:
                    newRow[calculationName] = CustomConstants.NA;
                    break;
            }
        }

        public static string Calculation(string calculationCode, string calculationName, IGrouping<string, DBTMReportsModel> group, Int16 recurtion, bool isDisplayUnit = false, bool isGraph = false, int DBTMTestMasterId = 0)
        {
            try
            {
                double weight = Convert.ToDouble(group.FirstOrDefault()?.Weight);
                calculationName = string.IsNullOrEmpty(calculationName) ? calculationCode : calculationName;
                string result = isGraph ? "0" : CustomConstants.NA;
                switch (calculationCode)
                {
                    case CustomConstants.TotalLaps:
                        int totalLaps = group.Max(x => x.Row);
                        result = $"{totalLaps}";
                        break;
                    case CustomConstants.CompletionTime:
                        decimal completionTime = group.Where(x => x.ParameterCode == CustomConstants.Time).Sum(x => Convert.ToDecimal(x.ParameterValue));
                        result = $"{Math.Round(completionTime / recurtion, CustomConstants.GraphListRoundUpValue)}";
                        break;
                    case CustomConstants.AverageTotalCompletionTime:
                        completionTime = group.Where(x => x.ParameterCode == CustomConstants.Time).Sum(x => Convert.ToDecimal(x.ParameterValue));
                        result = $"{Math.Round(completionTime / recurtion, CustomConstants.GraphListRoundUpValue)}";
                        break;
                    case CustomConstants.AverageVelocity:
                        decimal totalDistance = group.Where(x => x.ParameterCode == CustomConstants.Distance).Sum(x => Convert.ToDecimal(x.ParameterValue));
                        decimal totalTime = group.Where(x => x.ParameterCode == CustomConstants.Time).Sum(x => Convert.ToDecimal(x.ParameterValue));
                        result = totalTime != 0 && totalDistance != 0 ? $"{Math.Round(totalDistance / totalTime, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : CustomConstants.InvalidData;
                        break;
                    case CustomConstants.AverageTotalVelocity:
                        totalDistance = group.Where(x => x.ParameterCode == CustomConstants.Distance).Sum(x => Convert.ToDecimal(x.ParameterValue));
                        totalTime = group.Where(x => x.ParameterCode == CustomConstants.Time).Sum(x => Convert.ToDecimal(x.ParameterValue));
                        totalTime = totalTime != 0 ? (totalTime / recurtion) : totalTime;
                        result = totalTime != 0 && totalDistance != 0 ? $"{Math.Round(totalDistance / totalTime, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : CustomConstants.InvalidData;
                        break;
                    case CustomConstants.TotalDistanceCovered:
                        decimal totalDistanceCovered = group.Where(x => (x.ParameterCode == CustomConstants.Distance || x.ParameterCode == CustomConstants.DistanceMultiplyByRow) && x.Row != 0).Sum(x => Convert.ToDecimal(x.ParameterValue));
                        result = totalDistanceCovered != 0 ? $"{Math.Round(totalDistanceCovered, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : CustomConstants.InvalidData;
                        break;
                    case CustomConstants.DistanceMultiplyByRow:
                        decimal distance = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Distance || x.ParameterCode == CustomConstants.DistanceMultiplyByRow && x.Row == recurtion).ParameterValue);
                        result = distance != 0 ? $"{Math.Round(distance * recurtion, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : CustomConstants.InvalidData;
                        break;
                    case CustomConstants.MaxLap:
                        result = $"{group.Where(x => x.ParameterCode == CustomConstants.Time).Max(x => x.ParameterValue)}";
                        break;
                    case CustomConstants.MinLap:
                        result = $"{group.Where(x => x.ParameterCode == CustomConstants.Time).Min(x => x.ParameterValue)}";
                        break;
                    case CustomConstants.Power:
                        return Power(group, weight, isGraph);
                    case CustomConstants.Force:
                        result = weight == 0 ? CustomConstants.NA : $"{Math.Round(4 * weight * 9.81, CustomConstants.GraphListRoundUpValue)}";
                        break;
                    case CustomConstants.CumulativeTime:
                        decimal cumulativeTime = 0;
                        for (int i = 1; i <= recurtion; i++)
                        {
                            cumulativeTime += Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.Row == i)?.ParameterValue);
                        }
                        result = $"{Math.Round(cumulativeTime, CustomConstants.GraphListRoundUpValue)}";
                        break;
                    case CustomConstants.CumulativeDistance:
                        decimal cumulativeDistance = 0;
                        for (int i = 1; i <= recurtion; i++)
                        {
                            cumulativeDistance += Convert.ToDecimal(group.FirstOrDefault(x => (x.ParameterCode == CustomConstants.Distance || x.ParameterCode == CustomConstants.DistanceMultiplyByRow) && x.Row == i).ParameterValue);
                        }
                        result = $"{Math.Round(cumulativeDistance, CustomConstants.GraphListRoundUpValue)}";
                        break;
                    case CustomConstants.Velocity:
                        distance = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Distance || x.ParameterCode == CustomConstants.DistanceMultiplyByRow).ParameterValue);
                        decimal time = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.Row == recurtion).ParameterValue);
                        result = time != 0 && distance != 0 ? $"{Math.Round(distance / time, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : CustomConstants.InvalidData;
                        break;
                    case CustomConstants.VelocityByRow:
                        result = VelocityByRow(group, recurtion, isGraph);
                        break;
                    case CustomConstants.VelocityByRowWithFirstDistance:
                        result = VelocityByRowWithFirstDistance(group, recurtion, isGraph);
                        break;
                    case CustomConstants.CumulativeVelocityWithSameDistance:
                        result = CumulativeVelocityWithSameDistance(group, recurtion, isGraph);
                        break;
                    case CustomConstants.CumulativeVelocityWithChangeDistance:
                        result = CumulativeVelocityWithChangeDistance(group, recurtion, isGraph);
                        break;
                    case CustomConstants.AccelerationByRow:
                        result = AccelerationByRow(group, recurtion, isGraph);
                        break;
                    case CustomConstants.ForceByRow:
                        result = ForceByRow(group, recurtion, weight, isGraph);
                        break;
                    case CustomConstants.PowerByRow:
                        string velocityByRowValue = VelocityByRow(group, recurtion, isGraph);
                        if (velocityByRowValue != CustomConstants.InvalidData)
                        {
                            var velocityByRow = Convert.ToDecimal(velocityByRowValue);
                            string forceByRowValue = ForceByRow(group, recurtion, weight, isGraph);
                            if (forceByRowValue != CustomConstants.InvalidData && forceByRowValue != CustomConstants.NA)
                            {
                                var forceByRow = Convert.ToDecimal(forceByRowValue);
                                if (weight == 0 && isGraph)
                                {
                                    result = "0";
                                }
                                else
                                    result = weight == 0 ? CustomConstants.NA : $"{Math.Round(forceByRow * velocityByRow, CustomConstants.GraphListRoundUpValue)}";
                            }
                            else
                            {
                                return CustomConstants.InvalidData;
                            }
                        }
                        else
                        {
                            return CustomConstants.InvalidData;
                        }
                        break;
                    case CustomConstants.ChangeOfDirection:
                        result = ChangeOfDirection(group, DBTMTestMasterId);
                        break;
                    case CustomConstants.AgilityDeficitRatio:
                        result = AgilityDeficitRatio(group, DBTMTestMasterId);
                        break;
                    case CustomConstants.ChangeOfDirectionDeficit:
                        result = ChangeOfDirectionDeficit(group, DBTMTestMasterId);
                        break;
                    case CustomConstants.ChangeOfDirectionRatio:
                        result = ChangeOfDirectionRatio(group, DBTMTestMasterId);
                        break;
                    case CustomConstants.FrontToBackRunRation:
                        result = FrontToBackRunRation(group, DBTMTestMasterId);
                        break;
                    case CustomConstants.JumpHeight:
                        decimal totalJumpHeight = group.Where(x => x.ParameterCode == CustomConstants.JumpHeight).Sum(x => Convert.ToDecimal(x.ParameterValue));
                        result = totalJumpHeight > 0 ? $"{Math.Round(totalJumpHeight / recurtion, CustomConstants.GraphListRoundUpValue)}" : "0";
                        break;
                    case CustomConstants.JumpLength:
                        decimal totalJumpLength = group.Where(x => x.ParameterCode == CustomConstants.JumpLength).Sum(x => Convert.ToDecimal(x.ParameterValue));
                        result = totalJumpLength > 0 ? $"{Math.Round(totalJumpLength / recurtion, CustomConstants.GraphListRoundUpValue)}" : "0";
                        break;
                    case CustomConstants.TotalCount:
                        decimal totalCount = group.Where(x => x.ParameterCode == CustomConstants.Count).Sum(x => Convert.ToDecimal(x.ParameterValue));
                        result = totalCount > 0 ? totalCount.ToString() : "0";
                        break;
                    case CustomConstants.CountByTime:
                        decimal count = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Count && x.Row == recurtion).ParameterValue);
                        time = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.Row == recurtion).ParameterValue);
                        result = time != 0 && count != 0 ? $"{Math.Round(count / time, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : CustomConstants.InvalidData;
                        break;
                }
                return result = isDisplayUnit ? $"{result} {Unit(calculationCode)}" : result;
            }
            catch (Exception ex)
            {
                return CustomConstants.InvalidData;
            }
        }

        private static string Power(IGrouping<string, DBTMReportsModel> group, double weight, bool isGraph)
        {
            double jumpHeight = Convert.ToDouble(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.JumpHeight)?.ParameterValue);
            if (weight == 0 && isGraph)
                return "0";
            else
                return weight == 0 ? CustomConstants.NA : $"{Math.Round(weight * Math.Pow(9.81, 1.5) * Math.Sqrt(2 * jumpHeight) / 4, CustomConstants.GraphListRoundUpValue)}";
        }

        private static string CumulativeVelocityWithSameDistance(IGrouping<string, DBTMReportsModel> group, short recurtion, bool isGraph)
        {
            string result = string.Empty;
            decimal cumulativeTime = 0;
            for (short i = 1; i <= recurtion; i++)
            {
                string velocityByRowValue = CumulativeVelocityByRowWithFirstDistance(group, i, isGraph);
                if (velocityByRowValue != CustomConstants.InvalidData)
                {
                    cumulativeTime = Convert.ToDecimal(velocityByRowValue);
                }
                else
                {
                    return isGraph ? "0" : CustomConstants.InvalidData;
                }
            }
            result = cumulativeTime != 0 ? $"{Math.Round(cumulativeTime, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : CustomConstants.InvalidData;
            return result;
        }

        private static string CumulativeVelocityWithChangeDistance(IGrouping<string, DBTMReportsModel> group, short recurtion, bool isGraph)
        {
            string result = string.Empty;
            decimal cumulativeTime = 0;
            for (short i = 1; i <= recurtion; i++)
            {
                string velocityByRowValue = VelocityByRow(group, i, isGraph);
                if (velocityByRowValue != CustomConstants.InvalidData)
                {
                    cumulativeTime += Convert.ToDecimal(velocityByRowValue);
                }
                else
                {
                    return isGraph ? "0" : CustomConstants.InvalidData;
                }
            }
            result = cumulativeTime != 0 ? $"{Math.Round(cumulativeTime, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : CustomConstants.InvalidData;
            return result;
        }

        private static string ChangeOfDirectionRatio(IGrouping<string, DBTMReportsModel> group, int DBTMTestMasterId)
        {
            string result;
            if (DBTMTestMasterId == 5)
            {
                var time2 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "B-C")?.ParameterValue);
                var time3 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "C-D")?.ParameterValue);
                var time4 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "D-B")?.ParameterValue);
                result = time2 > 0 & time3 > 0 & time4 > 0 ? $"{Math.Round((time2 + time4) / time3, CustomConstants.GraphListRoundUpValue)}" : "0";
            }
            else
            {
                var time2 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "B-C")?.ParameterValue);
                var time3 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "C-B")?.ParameterValue);
                result = time2 > 0 & time3 > 0 ? $"{Math.Round(time3 / time2, CustomConstants.GraphListRoundUpValue)}" : "0";
            }
            return result;
        }

        private static string FrontToBackRunRation(IGrouping<string, DBTMReportsModel> group, int DBTMTestMasterId)
        {
            string result;
            if (DBTMTestMasterId == 5)
            {
                var time1 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "A-B")?.ParameterValue);
                var time2 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "B-C")?.ParameterValue);
                result = time1 > 0 & time2 > 0 ? $"{Math.Round(time1 + time2, CustomConstants.GraphListRoundUpValue)}" : "0";
            }
            else
            {
                result = "0";
            }
            return result;
        }

        private static string ChangeOfDirectionDeficit(IGrouping<string, DBTMReportsModel> group, int DBTMTestMasterId)
        {
            string result;
            decimal changeOfDirection = Convert.ToDecimal(ChangeOfDirection(group, DBTMTestMasterId));
            if (DBTMTestMasterId == 5)
            {
                var time1 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "A-B")?.ParameterValue);
                var time5 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "B-A")?.ParameterValue);
                result = changeOfDirection > 0 && time1 > 0 ? $"{Math.Round(changeOfDirection - (time1 + time5), CustomConstants.GraphListRoundUpValue)}" : "0";
            }
            else
            {
                var time1 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "A-B")?.ParameterValue);
                result = changeOfDirection > 0 && time1 > 0 ? $"{Math.Round(changeOfDirection - time1, CustomConstants.GraphListRoundUpValue)}" : "0";
            }
            return result;
        }

        private static string AgilityDeficitRatio(IGrouping<string, DBTMReportsModel> group, int DBTMTestMasterId)
        {
            string result;
            var changeOfDirection = Convert.ToDecimal(ChangeOfDirection(group, DBTMTestMasterId));
            if (DBTMTestMasterId == 5)
            {
                var time1 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "A-B")?.ParameterValue);
                var time5 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "B-A")?.ParameterValue);
                result = changeOfDirection > 0 && time1 > 0 ? $"{Math.Round(changeOfDirection / (time1 + time5), CustomConstants.GraphListRoundUpValue)}" : "0";
            }
            else
            {
                var time1 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "A-B")?.ParameterValue);
                result = changeOfDirection > 0 && time1 > 0 ? $"{Math.Round(changeOfDirection / time1, CustomConstants.GraphListRoundUpValue)}" : "0";
            }
            return result;
        }

        private static string ChangeOfDirection(IGrouping<string, DBTMReportsModel> group, int DBTMTestMasterId)
        {
            string result;
            if (DBTMTestMasterId == 5)
            {
                var time2 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "B-C")?.ParameterValue);
                var time3 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "C-D")?.ParameterValue);
                var time4 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "D-B")?.ParameterValue);
                result = time2 > 0 && time3 > 0 && time4 > 0 ? $"{Math.Round(time2 + time3 + time4, CustomConstants.GraphListRoundUpValue)}" : "0";
            }
            else
            {

                var time2 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "B-C")?.ParameterValue);
                var time3 = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.FromTo == "C-B")?.ParameterValue);
                result = time2 > 0 & time3 > 0 ? $"{Math.Round(time2 + time3, CustomConstants.GraphListRoundUpValue)}" : "0";
            }
            return result;
        }

        private static string ForceByRow(IGrouping<string, DBTMReportsModel> group, short recurtion, double weight, bool isGraph)
        {
            string result;
            string accelerationByRowValue = AccelerationByRow(group, recurtion, isGraph);
            if (accelerationByRowValue != CustomConstants.InvalidData)
            {
                var accelerationByRow = Convert.ToDecimal(accelerationByRowValue);
                if (weight == 0 && isGraph)
                {
                    result = "0";
                }
                else
                {
                    result = weight == 0 ? CustomConstants.NA : $"{Math.Round(Convert.ToDecimal(weight) * accelerationByRow, CustomConstants.GraphListRoundUpValue)}";
                }
            }
            else
            {
                result = CustomConstants.InvalidData;
            }
            return result;
        }

        private static string AccelerationByRow(IGrouping<string, DBTMReportsModel> group, short recurtion, bool isGraph)
        {
            string result;
            var timeValue = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.Row == recurtion)?.ParameterValue ?? "0");
            if (recurtion == 1)
            {
                string velocityByRow = VelocityByRow(group, 1, isGraph);
                if (velocityByRow != CustomConstants.InvalidData)
                {
                    var velocityValue = Convert.ToDecimal(velocityByRow);
                    result = timeValue != 0 ? $"{Math.Round(velocityValue / timeValue, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : CustomConstants.InvalidData;
                }
                else
                {
                    result = CustomConstants.InvalidData;
                }
            }
            else
            {
                string velocityByRow = VelocityByRow(group, recurtion, isGraph);
                if (velocityByRow != CustomConstants.InvalidData)
                {
                    var velocityValueCurrent = Convert.ToDecimal(velocityByRow);
                    string velocityByRowBefore = VelocityByRow(group, (short)(recurtion - 1), isGraph);
                    if (velocityByRowBefore != CustomConstants.InvalidData)
                    {
                        var velocityValueBefore = Convert.ToDecimal(velocityByRowBefore);
                        result = timeValue != 0 ? $"{Math.Round((velocityValueCurrent - velocityValueBefore) / timeValue, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : CustomConstants.InvalidData;
                    }
                    else
                    {
                        result = CustomConstants.InvalidData;
                    }
                }
                else
                {
                    result = CustomConstants.InvalidData;
                }
            }
            return result;
        }

        private static string VelocityByRow(IGrouping<string, DBTMReportsModel> group, short recurtion, bool isGraph)
        {
            string result = string.Empty;
            decimal distance = Convert.ToDecimal(group.FirstOrDefault(x => (x.ParameterCode == CustomConstants.DistanceMultiplyByRow || x.ParameterCode == CustomConstants.Distance) && x.Row == recurtion)?.ParameterValue);
            decimal time = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.Row == recurtion)?.ParameterValue);
            result = time != 0 && distance != 0 ? $"{Math.Round(distance / time, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : CustomConstants.InvalidData;
            return result;
        }

        private static string VelocityByRowWithFirstDistance(IGrouping<string, DBTMReportsModel> group, short recurtion, bool isGraph)
        {
            string result = string.Empty;
            decimal distance = Convert.ToDecimal(group.FirstOrDefault(x => (x.ParameterCode == CustomConstants.DistanceMultiplyByRow || x.ParameterCode == CustomConstants.Distance) && x.Row == 1)?.ParameterValue);
            decimal time = Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.Row == recurtion)?.ParameterValue);
            result = time != 0 && distance != 0 ? $"{Math.Round(distance / time, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : CustomConstants.InvalidData;
            return result;
        }

        private static string CumulativeVelocityByRowWithFirstDistance(IGrouping<string, DBTMReportsModel> group, short recurtion, bool isGraph)
        {
            string result = string.Empty;
            decimal distance = Convert.ToDecimal(group.FirstOrDefault(x => (x.ParameterCode == CustomConstants.DistanceMultiplyByRow || x.ParameterCode == CustomConstants.Distance) && x.Row == 1)?.ParameterValue);
            decimal cumulativeTime = 0;
            for (int i = 1; i <= recurtion; i++)
            {
                cumulativeTime += Convert.ToDecimal(group.FirstOrDefault(x => x.ParameterCode == CustomConstants.Time && x.Row == i)?.ParameterValue);
            }
            result = cumulativeTime != 0 && distance != 0 ? $"{Math.Round(distance * recurtion / cumulativeTime, CustomConstants.GraphListRoundUpValue)}" : isGraph ? "0" : CustomConstants.InvalidData;
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
                case CustomConstants.AverageTime:
                    data = "sec";
                    break;
                case CustomConstants.TotalDistanceCovered:
                case CustomConstants.PersonDetectionRange:
                case CustomConstants.DistanceMultiplyByRow:
                case CustomConstants.Distance:
                case CustomConstants.CumulativeDistance:
                    data = "m";
                    break;
                case CustomConstants.CumulativeVelocity:
                case CustomConstants.AverageVelocity:
                case CustomConstants.VelocityByRow:
                case CustomConstants.CumulativeVelocityWithSameDistance:
                case CustomConstants.CumulativeVelocityWithChangeDistance:
                case CustomConstants.Velocity:
                case CustomConstants.VelocityByRowWithFirstDistance:
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

        public static int GetAgeGroupEnumIdByDOB(DateTime? dob, List<(int EnumId, int EnumValue)> ageGroups)
        {
            if (!dob.HasValue)
                return 0;
            int age = DateTime.Today.Year - dob.Value.Year;
            if (dob.Value.Date > DateTime.Today.AddYears(-age))
                age--;
            foreach (var item in ageGroups.OrderBy(x => x.EnumValue))
            {
                if (age <= item.EnumValue)
                {
                    return item.EnumId;
                }
            }
            return ageGroups.LastOrDefault().EnumId;
        }

        public static string GenerateQRCode(string textdData,string imageType)
        {
            if (string.IsNullOrEmpty(textdData))
                return string.Empty;

            // Generate PNG bytes for the QR code using QRCoder and return a data URI.
            try
            {
                using (var qrGenerator = new QRCodeGenerator())
                using (var qrData = qrGenerator.CreateQrCode(textdData, QRCodeGenerator.ECCLevel.Q))
                {
                    var png = new PngByteQRCode(qrData).GetGraphic(20);
                    string base64 = Convert.ToBase64String(png);
                    return $"data:image/png;base64,{base64}";
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
