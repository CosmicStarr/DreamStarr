using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarrAPI.Data;
using StarrAPI.Data.Interfaces;
using StarrAPI.DTOs;
using StarrAPI.Models;


namespace StarrAPI.Controllers
{

    [Authorize]
    public class UsersController : BaseAPIController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _Mapper;

        public UsersController(IUserRepository userRepository, IMapper Mapper)
        {
            _Mapper = Mapper;
            _userRepository = userRepository;


        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
       
            return Ok(await _userRepository.GetMembersDTOAsync());
        }

        [HttpGet("{Username}")]

        public async Task<ActionResult<MemberDTO>> GetUser(string Username)
        {            
            return await _userRepository.GetMemberDTOAsync(Username);
        }
    }
}