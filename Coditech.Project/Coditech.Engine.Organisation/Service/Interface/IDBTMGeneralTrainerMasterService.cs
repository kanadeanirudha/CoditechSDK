using Coditech.Common.API.Model;
using System.Collections.Specialized;
namespace Coditech.API.Service
{
    public interface IDBTMGeneralTrainerMasterService
    {
        bool AssociateUnAssociateTrainer(GeneralTraineeAssociatedToTrainerModel model);
    }
}
