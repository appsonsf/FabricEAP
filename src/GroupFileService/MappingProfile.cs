using AutoMapper;
using GroupFile.Entities;
using GroupFile.Models;

namespace GroupFile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<FileItem, FileItemDto>();
                //.ForMember(dest=>dest.Size, opts => opts.MapFrom(src =>src.Store.Size));
        }
    }
}
