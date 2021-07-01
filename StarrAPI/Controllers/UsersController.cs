using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarrAPI.Data.Interfaces;
using StarrAPI.DTOs;
using StarrAPI.Extensions;
using StarrAPI.InterfacesandClasses;
using StarrAPI.Models;

namespace StarrAPI.Controllers
{

    [Authorize]
    public class UsersController : BaseAPIController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _Mapper;
        private readonly IPhotoUpload _photoService; 

        public UsersController(IUserRepository userRepository, IMapper Mapper, IPhotoUpload PhotoService)
        {
            _photoService = PhotoService;
            _Mapper = Mapper;
            _userRepository = userRepository;


        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {

            return Ok(await _userRepository.GetMembersDTOAsync());
        }

        [HttpGet("{Username}", Name ="GetUser")]
        public async Task<ActionResult<MemberDTO>> GetUser(string Username)
        {
            return await _userRepository.GetMemberDTOAsync(Username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateInfo(MemberUpdateDTO MemberUpdate)
        {

            var _User = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            _Mapper.Map(MemberUpdate, _User);
            _userRepository.UpdateUser(_User);
            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user!");
        }

        [HttpPost("add-Photo")]
        public async Task<ActionResult<PhotosDTO>> AddPhotos(IFormFile file)
        {
            //Retrieving the User and eager Loading the Photos. The Parameter is an extension Method!
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            //Retrieving the Photo the user wants to upload!
            var results = await _photoService.AddPhotosAsync(file);
            //If there are any errors with variable "results", I want to display a message  
            if(results.Error != null) return BadRequest(results.Error.Message);
            //If there arent any errors, i want to upload them to the database.
            var Picture = new Photos
            {
                PhotoUrl = results.SecureUrl.AbsoluteUri,
                PublicId = results.PublicId
            };
            //If that was the first photo ever uploaded, it will be there main pic
            if(user.Photos.Count == 0)
            {
                Picture.MainPic = true;
            }
            //Using the dbContext to track the newly created pic.
            user.Photos.Add(Picture);
            //if when the "SaveAllAsync" is called, i want to map the info to my PhotosDTO
            if(await _userRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetUser",new {Username = user.Username},_Mapper.Map<PhotosDTO>(Picture));
            }
            
            //if everything failed i want to display this message.
            return BadRequest("Sorry! There was a problem Uploading your Photo!");       
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(p => p.ID == photoId);

            if(photo.MainPic) return BadRequest("This is your main picture!");
            var currentMain = user.Photos.FirstOrDefault(x =>x.MainPic);
            if(currentMain != null) currentMain.MainPic = false;
            photo.MainPic = true;
            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo!");

        }

        [HttpDelete("delete-Photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(p => p.ID == photoId);
            if(photo == null) return NotFound();
            if(photo.MainPic) return BadRequest("You cant delete main photo!");
            if(photo.PublicId != null)
            {
                var results = await _photoService.DeletePhotoAsync(photo.PublicId); 
                if(results.Error != null) return BadRequest(results.Error.Message);
            } 
            user.Photos.Remove(photo);
            if(await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Failed to delete photo!");
        }
    }
}