using Coditech.Admin.Agents;
using Coditech.Admin.Utilities;
using Coditech.Admin.ViewModel;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc;
namespace Coditech.Admin.Controllers
{
    public class DBTMOrganisationCentrewiseJoiningCodeController : BaseController
    {
        private readonly IDBTMOrganisationCentrewiseJoiningCodeAgent _dBTMOrganisationCentrewiseJoiningCodeAgent;
        public DBTMOrganisationCentrewiseJoiningCodeController(IDBTMOrganisationCentrewiseJoiningCodeAgent dBTMOrganisationCentrewiseJoiningCodeAgent)
        {
            _dBTMOrganisationCentrewiseJoiningCodeAgent = dBTMOrganisationCentrewiseJoiningCodeAgent;
        }

        [HttpGet]
        public IActionResult DownloadTraineeJoiningCode(string centreCode)
        {
            if (string.IsNullOrEmpty(centreCode))
                return Content("Centre not selected.");
            DBTMOrganisationCentrewiseJoiningCodeViewModel result = _dBTMOrganisationCentrewiseJoiningCodeAgent.GetTraineeActiveJoiningCode(centreCode);
            if (result == null || string.IsNullOrEmpty(result.FilePath) || !System.IO.File.Exists(result.FilePath))
                return Content("File not found.");
            var bytes = System.IO.File.ReadAllBytes(result.FilePath);
            var fileName = result.FileName;
            _dBTMOrganisationCentrewiseJoiningCodeAgent.DeleteJoiningCodeFile(fileName);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        #region Protected
        #endregion
    }
}
