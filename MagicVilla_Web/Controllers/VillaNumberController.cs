using AutoMapper;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using MagicVilla_Web.Models.VM;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        public VillaNumberController(IVillaNumberService villaNumberService,IVillaService villaService, IMapper mapper)
        {
            _mapper = mapper;
            _villaNumberService = villaNumberService;
            _villaService = villaService;

        }
        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();
            var response = await _villaNumberService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM villaNumberVM = new();
            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }) ; 
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }

            }

            return View(model);
        }


        //public async Task<IActionResult> UpdateVillaNumber(int villaId)
        //{
        //    var response = await _villaNumberService.GetAsync<APIResponse>(villaId);
        //    if (response != null && response.IsSuccess)
        //    {
        //        VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
        //        return View(_mapper.Map<VillaUpdateDTO>(model));
        //    }

        //    return NotFound();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateDTO model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var response = await _villaNumberService.UpdateAsync<APIResponse>(model);
        //        if (response != null && response.IsSuccess)
        //        {
        //            return RedirectToAction(nameof(IndexVillaNumber));
        //        }

        //    }

        //    return View(model);
        //}


        //public async Task<IActionResult> DeleteVillaNumber(int villaId)
        //{
        //    var response = await _villaNumberService.GetAsync<APIResponse>(villaId);
        //    if (response != null && response.IsSuccess)
        //    {
        //        VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
        //        return View(model);

        //    }

        //    return NotFound();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteVillaNumber(VillaNumberDTO model)
        //{

        //    var response = await _villaNumberService.DeleteAsync<APIResponse>(model.VillaNo);
        //    if (response != null && response.IsSuccess)
        //    {
        //        return RedirectToAction(nameof(IndexVillaNumber));
        //    }



        //    return View(model);
        //}
    }
}
