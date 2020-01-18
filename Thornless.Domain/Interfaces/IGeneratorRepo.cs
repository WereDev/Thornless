using System.Threading.Tasks;
using Thornless.Domain.Models.CharacterNames;

namespace Thornless.Domain.Interfaces
{
    public interface IGeneratorRepo
    {
        Task<AncestryModel[]> ListAncestries();

        Task<AncestryDetailsModel> GetAncestry(string ancestryCode);
    }
}
