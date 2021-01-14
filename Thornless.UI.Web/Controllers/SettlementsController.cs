using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Thornless.Domain.BuildingNames;
using Thornless.Domain.Settlements;
using Thornless.UI.Web.ViewModels.Settlement;

namespace Thornless.UI.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettlementsController : BaseApiController
    {
        private readonly ISettlementGenerator _settlementGenerator;
        private readonly IBuildingNameGenerator _buildingNameGenerator;
        private readonly IMapper _mapper;

        public SettlementsController(
          ISettlementGenerator settlementGenerator,
          IBuildingNameGenerator buildingNameGenerator,
          IMapper mapper)
        {
            _settlementGenerator = settlementGenerator;
            _buildingNameGenerator = buildingNameGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ListSettlementTypes()
        {
            var settlements = await _settlementGenerator.ListSettlementTypes();
            var response = new ListSettlementTypeResponse
            {
                SettlementTypes = _mapper.Map<ListSettlementTypeResponse.SettlementType[]>(settlements),
            };
            return ApiResponse(response);
        }

        [HttpGet("{settlementCode}")]
        public async Task<IActionResult> GenerateSettlement(string settlementCode)
        {
            var settlement = await _settlementGenerator.GenerateSettlement(settlementCode, _buildingNameGenerator);
            var response = _mapper.Map<GenerateSettlementResponse>(settlement);
            return ApiResponse(response);
        }
    }
}
