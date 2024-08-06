﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user_management.core.Commands.Role;
using user_management.core.DataTransferObjects;
using user_management.core.Queries.Role;
using user_management.core.Shared;

namespace user_management.api.Controllers
{
    public class RolesController : ApiBaseController
    {
        private readonly IMapper _mapper;
        public RolesController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FetchRoleById([FromRoute]string id)
        {
            var response = await Mediator.Send(new HandleFetchRoleById.Query { RoleId = id});
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "TenantSuperAdmin,SuperAdmin")]
        [ProducesResponseType(typeof(GenericResponse<List<HandleFetchRoles.Result>>), 200)]
        public async Task<IActionResult> FetchRoles()
        {
            var response = await Mediator.Send(new HandleFetchRoles.Query());
            return Ok(response);
        }

        [HttpPatch("assign")]
        [Authorize(Roles = "TenantSuperAdmin,SuperAdmin")]
        [ProducesResponseType(typeof(GenericResponse<string>), 200)]
        public async Task<IActionResult> AssignUserRole([FromBody] AssignUserRoleDto request)
        {
            var response = await Mediator.Send(new HandleAssignUserRole.Command 
            { 
                Email = request.Email, 
                RoleId = request.RoleId
            });
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "TenantSuperAdmin,TenantAdmin,SuperAdmin,Admin")]
        [ProducesResponseType(typeof(GenericResponse<string>), 200)]
        public async Task<IActionResult> CreateRole([FromBody]CreateRoleDto request)
        {
            var response = await Mediator.Send(new HandleCreateRole.Command { Name = request.RoleName});
            return Ok(response);
        }
    }
}
