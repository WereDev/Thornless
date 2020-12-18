using System.Threading.Tasks;
using Thornless.Domain.Settlements.Models;

namespace Thornless.Domain.Settlements
{
    public interface ISettlementGenerator
    {
        Task<SettlementTypeModel[]> ListSettlementTypes();

        Task<SettlementResultsModel> GenerateSettlement(string code);
    }
}
