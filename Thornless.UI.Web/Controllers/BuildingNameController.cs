using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Thornless.Domain.BuildingNames;
using Thornless.UI.Web.ViewModels.BuildingName;

namespace Thornless.UI.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingNameController : BaseApiController
    {
        private readonly IBuildingNameGenerator _generator;
        private readonly IMapper _mapper;

        public BuildingNameController(IBuildingNameGenerator generator, IMapper mapper)
        {
            _generator = generator ?? throw new ArgumentNullException(nameof(generator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> ListBuildings()
        {
            var types = await _generator.ListBuildingTypes();
            var result = _mapper.Map<BuildingTypesViewModel[]>(types);
            
            return ApiResponse(result);
        }

        [HttpGet("{buildingTypeCode}")]
        public async Task<IActionResult> GenerateName(string buildingTypeCode)
        {
            var buildingName = await _generator.GenerateBuildingName(buildingTypeCode);

            var result = _mapper.Map<BuildingNameResultViewModel>(buildingName);

            return ApiResponse(result);
        }
    }
}
