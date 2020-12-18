using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using AutoMapper;
using Thornless.Data.GeneratorRepo.DataModels;
using Thornless.Domain.BuildingNames.Models;
using Thornless.Domain.CharacterNames.Models;
using Thornless.Domain.Settlements.Models;

namespace Thornless.Data.GeneratorRepo
{
    internal class GeneratorMapperProfile : Profile
    {
        public GeneratorMapperProfile()
        {
            CreateMap<CharacterAncestryDto, AncestryModel>();

            CreateMap<CharacterAncestryDto, AncestryDetailsModel>()
                .IncludeBase<CharacterAncestryDto, AncestryModel>();

            CreateMap<CharacterAncestryNamePartDto, AncestryDetailsModel.AncestryNamePartModel>()
                .ForMember(dest => dest.NameMeanings, opts => opts.MapFrom(src => ParseJsonArray(src.NameMeaningsJson)))
                .ForMember(dest => dest.NameParts, opts => opts.MapFrom(src => ParseJsonArray(src.NamePartsJson)));

            CreateMap<CharacterAncestryOptionDto, AncestryDetailsModel.AncestryOptionsModel>()
                .ForMember(dest => dest.NamePartSeperators, opts => opts.MapFrom(src => ParseJsonArray(src.NamePartSeperatorJson)));

            CreateMap<CharacterAncestrySegmentGroupDto, AncestryDetailsModel.AncestrySegmentGroupModel>()
                .ForMember(dest => dest.NameSegmentCodes, opts => opts.MapFrom(src => ParseJsonArray(src.NameSegmentCodesJson)));

            CreateMap<BuildingTypeDto, BuildingTypeModel>();

            CreateMap<BuildingTypeDto, BuildingTypeDetailsModel>()
                .IncludeBase<BuildingTypeDto, BuildingTypeModel>()
                .ForMember(dest => dest.NameFormats, src => src.MapFrom(x => x.NameFormats));

            CreateMap<BuildingNameFormatDto, BuildingTypeDetailsModel.BuildingNameFormatModel>();

            CreateMap<IEnumerable<BuildingNamePartDto>, BuildingNameGroups>()
                .ForMember(dest => dest.NameParts, src => src.MapFrom(x => ParseBuildingNameParts(x)));

            CreateMap<SettlementTypeDto, SettlementTypeModel>();

            CreateMap<SettlementBuildingDistributionDto, SettlementBuildingModel>();
        }

        private string[] ParseJsonArray(string? s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return new string[0];
            return JsonSerializer.Deserialize<string[]>(s, null);
        }

        private Dictionary<string, BuildingNameGroups.NamePartModel[]> ParseBuildingNameParts(IEnumerable<BuildingNamePartDto>? nameParts)
        {
            nameParts ??= new BuildingNamePartDto[0];
            var dict = nameParts.Where(x => !string.IsNullOrWhiteSpace(x.NamePart))
                                .GroupBy(x => x.GroupCode)
                                .ToDictionary(key => key.Key,
                                                value => value.Select(x => new BuildingNameGroups.NamePartModel
                                                                            {
                                                                                NamePart = x.NamePart!,
                                                                                RandomizationWeight = x.RandomizationWeight
                                                                            })
                                                                .ToArray());
            return dict;
        }
    }
}
