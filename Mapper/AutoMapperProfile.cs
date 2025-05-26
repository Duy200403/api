using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Models;
using AutoMapper;

namespace api.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Software -> SoftwareDto
            CreateMap<Software, SoftwareDto>();
            CreateMap<SoftwareDto, Software>();
            CreateMap<UpdateSoftwareDto, Software>();
            // DevelopmentTeam -> DevelopmentTeamDto
            CreateMap<DevelopmentTeam, DevelopmentTeamDto>();

            // Employee -> EmployeeDto
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<Employee, CreateEmployeeDto>();
            CreateMap<EmployeeUpdateDto, Employee>();
            // Shift -> ShiftDto
            CreateMap<Shift, ShiftDto>();
            // .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee))
            // .ReverseMap();
            CreateMap<Shift, CreateShiftDto>(); // Shift -> CreateShiftDto>
            CreateMap<CreateShiftDto, Shift>()
                .ForMember(dest => dest.EmployeeId, opt => opt.Ignore()); // Bỏ qua vì xử lý thủ công trong controller

            CreateMap<UpdateShiftDto, Shift>(); // UpdateShiftDto -> Shift>
            CreateMap<CreateDevelopmentTeamDto, DevelopmentTeam>(); // CreateDevelopmentTeamDto -> DevelopmentTeam>
            CreateMap<DevelopmentTeam, CreateDevelopmentTeamDto>();
            CreateMap<UpdateDevelopmentTeamDto, DevelopmentTeam>();
            // Report DTOs
            CreateMap<Shift, ShiftCountReportDto>();
            CreateMap<Shift, EmployeeShiftsReportDto>();
            CreateMap<Shift, EmployeeShiftTotalReportDto>();
            CreateMap<Shift, WorkEfficiencyReportDto>();

            // DevelopmentTeamMember
            CreateMap<DevelopmentTeamMember, DevelopmentTeamMemberDto>();
            CreateMap<CreateDevelopmentTeamMemberDto, DevelopmentTeamMember>();
            CreateMap<DevelopmentTeamMemberDto, DevelopmentTeamMember>();
            CreateMap<UpdateDevelopmentTeamMemberDto, DevelopmentTeamMember>();
        }
    }
}