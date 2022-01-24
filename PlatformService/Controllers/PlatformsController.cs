using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;

        public PlatformsController(IPlatformRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("Getting platforms...");
            var platforms = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));    
        }

        [HttpGet("{id}", Name="GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platform = _repository.GetPlatformById(id);
            if (platform == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }

        [HttpPost]
        public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto createPlatformDto)
        {
            var platformModel = _mapper.Map<Platform>(createPlatformDto);
            _repository.CreatePlatform(platformModel);
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id}, platformReadDto);
        }
    }
}