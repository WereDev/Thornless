using System.Threading.Tasks;
using Thornless.Domain.CharacterNames.Models;

namespace Thornless.Domain
{
    public interface IGeneratorRepo
    {
        Task<AncestryModel[]> ListAncestries();

        Task<AncestryDetailsModel> GetAncestry(string ancestryCode);
    }
}
