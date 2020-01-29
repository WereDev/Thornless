using System;
using System.Threading.Tasks;
using Thornless.Domain.Interfaces;
using Thornless.Domain.Models.CharacterNames;

namespace Thornless.Domain.Services
{
    public class CharacterNameGenerator : ICharacterNameGenerator
    {
        private readonly IGeneratorRepo _generatorRepo;
        private readonly IRandomItemSelector _randomItemSelector;

        public CharacterNameGenerator(IGeneratorRepo generatorRepo, IRandomItemSelector randomItemSelector)
        {
            _generatorRepo = generatorRepo ?? throw new ArgumentNullException(nameof(generatorRepo));
            _randomItemSelector = randomItemSelector ?? throw new ArgumentNullException(nameof(randomItemSelector));
        }

        public Task<AncestryModel[]> ListAncestries()
        {
            var ancestries = _generatorRepo.ListAncestries();
            return ancestries;
        }

        public Task<AncestryDetailsModel> ListAncestryOptions(string ancestryCode)
        {
            var ancestry = _generatorRepo.GetAncestry(ancestryCode);
            return ancestry;            
        }
    }
}
