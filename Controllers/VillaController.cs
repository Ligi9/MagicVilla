﻿using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelo;
using MagicVilla_API.Modelo.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace MagicVilla_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VillaController : ControllerBase
{
    private readonly ILogger<VillaController> _logger;
    private readonly IVillaRepositorio _villaRepo;
    private readonly IMapper _mapper;
    protected APIResponse _response;

    public VillaController(ILogger<VillaController> logger, IVillaRepositorio villaRepo, IMapper mapper)
    {  
        _logger = logger;
        _villaRepo = villaRepo;
        _mapper = mapper;
        _response = new ();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetVillas()
    {
        try
        {
            _logger.LogInformation("Obtener las Villas");
            IEnumerable<Villa> villaList = await _villaRepo.ObtenerTodos();

            _response.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villaList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {

            _response.IsExitoso = false;
            _response.ErrorsMessages = new List<string>() { ex.ToString()};
        }
        return _response;
       
    }

    [HttpGet("id:int", Name= "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _logger.LogError("Error al traer Villa con Id" + id);
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso=false;
                return BadRequest(_response);
            }
            // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _villaRepo.Obtener(x => x.Id == id);
            if (villa == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsExitoso=false;
                return NotFound(_response);
            }

            _response.Resultado = _mapper.Map<VillaDto>(villa);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {

            _response.IsExitoso = false;
            _response.ErrorsMessages = new List<string>() { ex.ToString() };
        }
        return _response;
       
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CrearVilla([FromBody] VillaCreateDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _villaRepo.Obtener(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La Villa con ese Nombre ya existe");
                return BadRequest(ModelState);
            }

            if (createDto == null)
            {
                return BadRequest(createDto);
            }

            Villa modelo = _mapper.Map<Villa>(createDto);

            modelo.FechaCreacion = DateTime.Now;
            modelo.FechaActualizacion = DateTime.Now;

            await _villaRepo.Crear(modelo);
            _response.Resultado = modelo;
            _response.StatusCode = HttpStatusCode.Created;

            // villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            // VillaStore.villaList.Add(villaDto);

            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);
        }
        catch (Exception ex)
        {

            _response.IsExitoso = false;
            _response.ErrorsMessages = new List<string>() { ex.ToString() };
        }
        return _response;
        
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.IsExitoso=false;
                _response.StatusCode=HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var villa = await _villaRepo.Obtener(v => v.Id == id);

            if (villa == null)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

           await _villaRepo.Remover(villa);
            _response.StatusCode = HttpStatusCode.NoContent;

            // VillaStore.villaList.Remove(villa);

            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsExitoso = false;
            _response.ErrorsMessages = new List<string>() { ex.ToString() };
        }
        return BadRequest(_response);
        
    }

    private void k(APIResponse response)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
    {
        if (updateDto==null || id!= updateDto.Id)
        {
            _response.IsExitoso=false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }
        // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
        // villa.Nombre = villaDto.Nombre;
        // villa.Ocupantes = villaDto.Ocupantes;
        //  villa.MetrosCuadrados = villaDto.MetrosCuadrados;
        Villa modelo = _mapper.Map<Villa>(updateDto);
        
        await _villaRepo.Actualizar(modelo);
        _response.StatusCode = HttpStatusCode.NoContent;
        
        return Ok(_response);
    }

    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
    {
        if (patchDto == null || id == 0)
        {
            return BadRequest();
        }
     // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
        var villa = await _villaRepo.Obtener(v => v.Id == id, tracked:false);
        VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

        if (villa == null) return BadRequest();

        patchDto.ApplyTo(villaDto, ModelState);

        if (!ModelState.IsValid)
        { 
            return BadRequest(ModelState);
        }
        
        Villa modelo = _mapper.Map<Villa>(villaDto);
        
        await _villaRepo.Actualizar(modelo);
        _response.StatusCode=HttpStatusCode.NoContent;
      
        return Ok(_response);
    }
}
