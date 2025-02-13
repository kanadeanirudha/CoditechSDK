using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class CustomDashboardViewModel : BaseViewModel
    {
        public CustomDashboardViewModel()
        {
        }       
        public string CustomDashboardFormEnumCode { get; set; }
        public Int16 NumberOfDaysRecord { get; set; }
        public int NumberOfTrainers { get; set; }
        public int NumberOfTrainees { get; set; }
        public int Assignments { get; set; }
        public int TotalNumberOfActivityPerformedDuringWeek { get; set; }
    }
}