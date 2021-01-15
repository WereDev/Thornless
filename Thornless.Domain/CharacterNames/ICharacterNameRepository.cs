using System.Threading.Tasks;
using Thornless.Domain.CharacterNames.Models;

namespace Thornless.Domain.CharacterNames
{
    public interface ICharacterNameRepository
    {
        Task<AncestryModel[]> ListAncestries();

        Task<AncestryDetailsModel?> GetAncestry(string ancestryCode);
    }
}
