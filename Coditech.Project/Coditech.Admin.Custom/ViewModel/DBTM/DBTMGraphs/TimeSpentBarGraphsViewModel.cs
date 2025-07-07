using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class TimeSpentBarGraphsViewModel
    {
        public string BarChartId { get; set; }
        public string[] XValues { get; set; } 
        public int[] YValues { get; set; }    
        public string Title { get; set; } 
        public string XAxisLabel { get; set; } 
        public string YAxisLabel { get; set; } 
        public string[] BackgroundColor { get; set; }
        [Display(Name = "Date")]
        public string ToDate { get; set; }
    }
}






























//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

//namespace MVC5FullCalandarPlugin.Models
//{
//    public class PublicHoliday
//    {
//        public int Sr { get; set; }
//        public string Title { get; set; }
//        public string Desc { get; set; }
//        public string Start_Date { get; set; }
//        public string End_Date { get; set; }
//    }
//}
