using System;
using System.Threading.Tasks;
using Thornless.Domain.Models.CharacterNames;

namespace Thornless.Domain.Interfaces
{
    public interface ICharacterNameGenerator
    {
        Task<AncestryModel[]> ListAncestries();
        
        Task<AncestryDetailsModel> ListAncestryOptions(string ancestryCode);

        Task<CharacterNameResultModel[]> GenerateNames(string ancestryCode, string ancestryOptionCode, int count);
    }
}
