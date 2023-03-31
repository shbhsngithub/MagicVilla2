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
    [Route("api/VillaNumberAPI")]
    [ApiController]

    public class VillaNumberAPIController : ControllerBase

    {
        protected APIResponse _response;
        private readonly IVillaNumberRepository _dbVillaNumber;

        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;

        //private object _mapper;

        public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber,IVillaRepository dbVilla, IMapper mapper)
        {
            _dbVillaNumber = dbVillaNumber;
            _dbVilla = dbVilla; 
            _mapper = mapper;
            this._response = new();// APIResponse();
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                IEnumerable<VillaNumber> villaNumberList = await _dbVillaNumber.GetAllAsync(includeProperties:"Villa");
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
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



        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
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
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaCreateDTO)
        {
            try
            {
                // if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}

                if (await _dbVillaNumber.
                GetAsync(u => u.VillaNo == villaCreateDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("", "Villa Number already exist");
                    return BadRequest(ModelState);
                }

                if(await _dbVilla.GetAsync(u=>u.Id== villaCreateDTO.VillaID) != null)
                {
                    ModelState.AddModelError("", "Villa ID is Invalid....");
                    return BadRequest(ModelState);
                }
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
                VillaNumber villaNumber = _mapper.Map<VillaNumber>(villaCreateDTO);
                //return Ok();

                await _dbVillaNumber.CreateAsync(villaNumber);

                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.statusCode = HttpStatusCode.Created;
                //return Ok(_response);

                // return Ok();


                return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }





        [HttpDelete("{id:int}",Name ="DeleteVillaNumber")]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                //if (id == 0)
                //{
                //    _response.statusCode = HttpStatusCode.BadRequest;
                //    return BadRequest(_response);
                //}
                VillaNumber villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                await _dbVillaNumber.RemoveAsync(villaNumber);

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
        [HttpPut("{id:int}",Name ="UpdateVillaNumber")]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaNumberUpdateDTO villaUpdateDTO)
        {
            try
            {
                if (villaUpdateDTO == null || id != villaUpdateDTO.VillaNo)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (await _dbVilla.GetAsync(u => u.Id ==villaUpdateDTO.VillaID) == null)
                {
                    ModelState.AddModelError("", "Villa ID is Invalid....");
                    return BadRequest(ModelState);
                }
                //var villa=_db.Villas.FirstOrDefault(u=>u.Id==id);
                //if(villa == null)
                //{
                //    return BadRequest();
                //}
                //villa.Name=villaDTO.Name;
                //villa.Sqft = villaDTO.Sqft;
                //villa.Occupancy = villaDTO.Occupancy;
                VillaNumber model = _mapper.Map<VillaNumber>(villaUpdateDTO);
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

                await _dbVillaNumber.UpdateAsync(model);
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
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            VillaNumber villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id, false);
            if (villaNumber == null)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            VillaNumberUpdateDTO model = _mapper.Map<VillaNumberUpdateDTO>(villaNumber);
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
            VillaNumber villa2 = _mapper.Map<VillaNumber>(model);
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
            await _dbVillaNumber.UpdateAsync(villa2);
            // await _db.SaveChangesAsync();

            return NoContent();
        }


    }
}
