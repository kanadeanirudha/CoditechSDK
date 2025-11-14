namespace Coditech.Common.API.Model
{
    public class GraphModelListModel : BaseListModel
    {
        public List<GraphModel> GraphModelList { get; set; }
        public GraphModelListModel()
        {
            GraphModelList = new List<GraphModel>();
        }
    }
}
