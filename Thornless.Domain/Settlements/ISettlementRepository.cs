using System.Threading.Tasks;
using Thornless.Domain.Settlements.Models;

namespace Thornless.Domain.Settlements
{
    public interface ISettlementRepository
    {
        Task<SettlementTypeModel[]> ListSettlementTypes();

        Task<SettlementTypeDetails> GetSettlement(string settlementCode);
    }
}
