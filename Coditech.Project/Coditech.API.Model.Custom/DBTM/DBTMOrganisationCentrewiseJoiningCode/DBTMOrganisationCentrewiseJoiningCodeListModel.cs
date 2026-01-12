namespace Coditech.Common.API.Model
{
    public class DBTMOrganisationCentrewiseJoiningCodeListModel : BaseListModel
    {
        public List<DBTMOrganisationCentrewiseJoiningCodeModel> DBTMOrganisationCentrewiseJoiningCodeList { get; set; }
        public DBTMOrganisationCentrewiseJoiningCodeListModel()
        {
            DBTMOrganisationCentrewiseJoiningCodeList = new List<DBTMOrganisationCentrewiseJoiningCodeModel>();
        }
    }
}
