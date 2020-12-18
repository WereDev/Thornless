using System.Threading.Tasks;
using Thornless.Domain.Settlements.Models;

namespace Thornless.Domain.Settlements
{
    public class SettlementGenerator : ISettlementGenerator
    {
        public Task<SettlementResultsModel> GenerateSettlement(string code)
        {
            throw new System.NotImplementedException();
        }

        public Task<SettlementTypeModel[]> ListSettlementTypes()
        {
            throw new System.NotImplementedException();
        }
    }
}
