using AutoMapper;
using SimpleServicesDashboard.Application.DTOs;
using SimpleServicesDashboard.Application.Models;

namespace SimpleServicesDashboard.Application.Mapping
{
    /// <summary>
    /// Mapping profile for AutoMapper with configuration for DTO models (V1).
    /// </summary>
    public sealed class DtoV1MappingProfile : Profile
    {
        public DtoV1MappingProfile()
        {
            ConfigureResponseMappings();

            ConfigureSharedMappings();
        }

        /// <summary>
        /// Configure mappings for responses and output models.
        /// </summary>
        private void ConfigureResponseMappings()
        {
            CreateMap<ServiceStatusResponse, ServiceStatusResponseDto>();
            CreateMap<ServicesStatusResponse, ServicesStatusResponseDto>();

            CreateMap<ServiceDescription, ServiceDescriptionDto>();
            CreateMap<ServicesDescriptionResponse, ServicesDescriptionResponseDto>();
        }

        /// <summary>
        /// Configuration for shared models mappings that will be use for all the Api Versions.
        /// </summary>
        private void ConfigureSharedMappings()
        {
            CreateMap<StatusResponse, StatusResponseDto>();
            CreateMap<AppInfo, AppInfoDto>();
        }
    }
}