using AutoMapper;
using BandwidthMonitorService.Domain.Models;

namespace BandwidthMonitorService.Mappers
{
    public class SamplesProfile : Profile
    {
        public SamplesProfile()
        {
            CreateMap<Sample, Dto.Response.Sample>();

            CreateMap<Dto.Response.Sample, Sample>();
        }
    }
}
