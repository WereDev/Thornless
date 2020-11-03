using System;
using System.Threading.Tasks;
using Thornless.Domain.CharacterNames.Models;

namespace Thornless.Domain.CharacterNames
{
    public interface ICharacterNameGenerator
    {
        Task<AncestryModel[]> ListAncestries();
        
        Task<AncestryDetailsModel> ListAncestryOptions(string ancestryCode);

        Task<CharacterNameResultModel[]> GenerateNames(string ancestryCode, string ancestryOptionCode, int count);
    }
}
