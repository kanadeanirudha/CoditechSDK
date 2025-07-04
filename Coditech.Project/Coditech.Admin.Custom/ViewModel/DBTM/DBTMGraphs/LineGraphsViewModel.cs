namespace Coditech.Admin.ViewModel
{
    public class LineGraphsViewModel
    {
        public string LineChartId { get; set; }
        public string Title { get; set; }
        public string XAxisLabel { get; set; }
        public string YAxisLabel { get; set; }
        public string XValues { get; set; }
        public List<LineGraphsDatasetViewModel> Datasets { get; set; }
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
