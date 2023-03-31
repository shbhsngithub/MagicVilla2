using MagicVilla_VillaAPI_2ndTime.Data;
using MagicVilla_VillaAPI_2ndTime.Model;
using MagicVilla_VillaAPI_2ndTime.Model.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using MagicVilla_VillaAPI_2ndTime.Repository.IRepository;
using System.Net;

namespace MagicVilla_VillaAPI_2ndTime.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]

    public class VillaAPIController : ControllerBase

    {
        protected APIResponse _response;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;

        //private object _mapper;

        public VillaAPIController(IVillaRepository dbVilla, IMapper mapper)
        {
            _dbVilla = dbVilla;
            _mapper = mapper;
            this._response = new();// APIResponse();
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;


        }



        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _dbVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;



        }
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO villaCreateDTO)
        {
            try
            {
                // if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}

                //if (await _dbVilla.
                //Get(u => u.Name.ToLower() == villaCreateDTO.Name.ToLower()) != null)
                //{
                //    ModelState.AddModelError("", "Villa Name already exist");
                //    return BadRequest(ModelState);
                //}
                //if (villaCreateDTO == null)
                //{
                //    return BadRequest(villaCreateDTO);
                //}
                //if (villaCreateDTO.Id > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError);
                //}

                //Villa model = new Villa()
                //{
                //    Amenity=villaCreateDTO.Amenity,
                //    Details= villaCreateDTO.Details,

                //    ImageUrl= villaCreateDTO.ImageUrl,
                //    Name= villaCreateDTO.Name,
                //    Occupancy= villaCreateDTO.Occupancy,
                //    Rate= villaCreateDTO.Rate,
                //    Sqft= villaCreateDTO.Sqft
                //};
                Villa villa = _mapper.Map<Villa>(villaCreateDTO);
                //return Ok();

                await _dbVilla.CreateAsync(villa);

                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.statusCode = HttpStatusCode.Created;
                //return Ok(_response);

                // return Ok();


                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }





        [HttpDelete("id")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                Villa villa = await _dbVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                await _dbVilla.RemoveAsync(villa);

                _response.statusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);

                // return Ok();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;

        }
        //[HttpPut("id")]
        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateVilla([FromBody]  VillaUpdateDTO villaUpdateDTO)
        {
            try
            {
                if (villaUpdateDTO == null)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                //var villa=_db.Villas.FirstOrDefault(u=>u.Id==id);
                //if(villa == null)
                //{
                //    return BadRequest();
                //}
                //villa.Name=villaDTO.Name;
                //villa.Sqft = villaDTO.Sqft;
                //villa.Occupancy = villaDTO.Occupancy;
                Villa model = _mapper.Map<Villa>(villaUpdateDTO);
                //Villa model = new Villa()
                //{
                //    Amenity = villaUpdateDTO.Amenity,
                //    Details = villaUpdateDTO.Details,
                //    Id = villaUpdateDTO.Id,
                //    ImageUrl = villaUpdateDTO.ImageUrl,
                //    Name = villaUpdateDTO.Name,
                //    Occupancy = villaUpdateDTO.Occupancy,
                //    Rate = villaUpdateDTO.Rate,
                //    Sqft = villaUpdateDTO.Sqft
                //};

                await _dbVilla.UpdateAsync(model);
                // await _db.SaveChangesAsync();
                // _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.statusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
                //return NoContent();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;

        }
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            Villa villa = await _dbVilla.GetAsync(u => u.Id == id, false);
            if (villa == null)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            VillaUpdateDTO model = _mapper.Map<VillaUpdateDTO>(villa);
            //VillaUpdateDTO villaDTO = new VillaUpdateDTO
            //{
            //    Amenity = villa.Amenity,
            //    Details = villa.Details,
            //    Id = villa.Id,
            //    ImageUrl = villa.ImageUrl,
            //    Name = villa.Name,
            //    Occupancy = villa.Occupancy,
            //    Rate = villa.Rate,
            //    Sqft = villa.Sqft
            //};
            patchDTO.ApplyTo(model, ModelState);
            if (!ModelState.IsValid)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            Villa villa2 = _mapper.Map<Villa>(model);
            //Villa model = new Villa
            //{
            //    Amenity = villaDTO.Amenity,
            //    Details = villaDTO.Details,
            //    Id =villaDTO.Id,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Name = villaDTO.Name,
            //    Occupancy = villaDTO.Occupancy,
            //    Rate = villaDTO.Rate,
            //    Sqft =villaDTO.Sqft
            //};
            await _dbVilla.UpdateAsync(villa2);
            // await _db.SaveChangesAsync();

            return NoContent();
        }


    }
}
