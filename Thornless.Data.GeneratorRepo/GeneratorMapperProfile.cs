using System.Collections.Generic;
using System.Text.Json;
using AutoMapper;
using Thornless.Data.GeneratorRepo.DataModels;
using Thornless.Domain.Models.CharacterNames;

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
        }

        private string[] ParseJsonArray(string s)
        {
            return JsonSerializer.Deserialize<string[]>(s, null);
        }

        private string[] GetNamePartGroups(string format)
        {
            List<string> items = new List<string>();

            List<char> chars = new List<char>();
            bool addChars = false;
            foreach (var c in format)
            {
                switch (c)
                {
                    case '{':
                        chars.Clear();
                        addChars = true;
                        break;
                    case '}':
                        var s = new string(chars.ToArray());
                        items.Add(s);
                        addChars = false;
                        break;
                    default:
                        if (addChars)
                            chars.Add(c);
                        break;
                }
            }

            return items.ToArray();
        }
    }
}
