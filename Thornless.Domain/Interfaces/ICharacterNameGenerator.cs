using System.Threading.Tasks;
using Thornless.Domain.Models.CharacterNames;

namespace Thornless.Domain.Interfaces
{
    public interface ICharacterNameGenerator
    {
        Task<AncestryModel[]> ListAncestries();
        
        Task ListAncestryOptions(string ancestryCodeName);
    }
}
