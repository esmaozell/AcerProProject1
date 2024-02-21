using AcerProProject1.Data;
using AcerProProject1.Models;
using AcerProProject1.Models.Dto;
using AcerProProject1.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AcerProProject1.Controllers
{
    [Route("api/TargetAPI")]
    [ApiController]
    public class TargetAPIController : ControllerBase
    {
        private readonly ITargetAPIRepository targetAPIRepository;
        private readonly IMapper mapper;
        protected APIResponse response;
        public TargetAPIController(ITargetAPIRepository targetAPIRepository, IMapper mapper)
        {
            this.targetAPIRepository = targetAPIRepository;
            this.mapper = mapper;
            this.response = new();
        }

        [HttpGet("GetTargetAPIs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetTargetAPIs()
        {
            try
            {
                IEnumerable<TargetAPI> targetAPIList = await targetAPIRepository.GetAllAsync();
                response.Result = mapper.Map<IEnumerable<TargetAPIDto>>(targetAPIList);
                response.StatusCode = HttpStatusCode.OK;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return response;
        }

        [HttpGet("{id:int}", Name = "GetTargetAPI")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetTargetAPI(int id)
        {
            try
            {
                if (id <= 0)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }

                var targetAPI = await targetAPIRepository.GetAsync(t => t.Id == id);

                if (targetAPI == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(response);
                }
                response.Result = mapper.Map<TargetAPIDto>(targetAPI);
                response.StatusCode = HttpStatusCode.OK;

                return Ok(response);
            }

            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateTargetAPI([FromBody] TargetAPICreateDto targetAPI)
        {
            try
            { 
            if (targetAPI == null)
            {
                return BadRequest("Request body cannot be null");
            }

            var existingTargetAPI = await targetAPIRepository
                .GetAsync(t => t.Name.ToLower() == targetAPI.Name.ToLower() || t.Url == targetAPI.Url);

            if (existingTargetAPI != null)
            {
                ModelState.AddModelError("CustomError", "Name or Url already exists");
                return BadRequest(ModelState);
            }

            TargetAPI model = mapper.Map<TargetAPI>(targetAPI);

            await targetAPIRepository.CreateAsync(model);

            response.Result = mapper.Map<TargetAPIDto>(model);
            response.StatusCode = HttpStatusCode.Created;

            return CreatedAtAction(nameof(GetTargetAPI), new { id = model.Id }, mapper.Map<TargetAPIDto>(model));
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return response;
        }

        [HttpDelete("{id:int}", Name = "DeleteTargetAPI")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteTargetAPI(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id parameter");
                }

                var targetAPI = await targetAPIRepository.GetAsync(t => t.Id == id);

                if (targetAPI == null)
                {
                    return NotFound();
                }

                await targetAPIRepository.RemoveAsync(targetAPI);
                response.StatusCode = HttpStatusCode.NoContent;
                response.IsSuccess = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return response;
        }

        [HttpPut("{id:int}", Name = "UpdateTargetAPI")]
        public async Task<ActionResult<APIResponse>> UpdateTargetAPI(int id, [FromBody] TargetAPIUpdateDto targetAPIDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id parameter");
                }

                var targetApi = await targetAPIRepository.GetAsync(t => t.Id == id);

                if (targetApi == null)
                {
                    return NotFound("API not found.");
                }

                mapper.Map(targetAPIDto, targetApi);

                await targetAPIRepository.UpdateAsync(targetApi);

                response.StatusCode = HttpStatusCode.NoContent;
                response.IsSuccess = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return response;
        }

    }
}
