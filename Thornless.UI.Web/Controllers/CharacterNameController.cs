using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Thornless.Domain.Interfaces;
using Thornless.UI.Web.ViewModels.CharacterName;

namespace Thornless.UI.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterNameController : BaseApiController
    {
        private readonly ICharacterNameGenerator _characterNameGenerator;
        private readonly IMapper _mapper;

        public CharacterNameController(
            ICharacterNameGenerator characterNameGenerator,
            IMapper mapper)
        {
            _characterNameGenerator = characterNameGenerator ?? throw new ArgumentNullException(nameof(characterNameGenerator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> ListAncestries()
        {
            var ancestries = await _characterNameGenerator.ListAncestries();
            if (ancestries?.Any() != true)
                return NotFound();

            var mapped = _mapper.Map<AncestryViewModel[]>(ancestries);
            return ApiResponse(mapped);
        }

        [HttpGet("{ancestryCode}")]
        public async Task<IActionResult> GetAncestryOptions(string ancestryCode)
        {
            var ancestry = await _characterNameGenerator.ListAncestryOptions(ancestryCode);
            if (ancestry == null)
                return NotFound("Invalid ancestry code.");

            var mapped = _mapper.Map<AncestryDetailViewModel>(ancestry);
            return ApiResponse(mapped);
        }

        [HttpGet("{ancestryCode}/{ancestryOptionCode}")]
        public async Task<IActionResult> GenerateNames(string ancestryCode, string ancestryOptionCode, [FromQuery] int count = 1)
        {
            var names = await _characterNameGenerator.GenerateNames(ancestryCode, ancestryOptionCode, count);

            if (names == null)
                return NotFound($"No combination found for {ancestryCode} and {ancestryOptionCode}.");

            var mapped = _mapper.Map<CharacterNameViewModel[]>(names);
            return ApiResponse(mapped);
        }
    }
}
