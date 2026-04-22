using API_LaboBackend.DTO.Categorie;
using API_LaboBackend.Mappers;
using BLL.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API_LaboBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategorieController : ControllerBase
{
    private readonly ICategorieService _categorieService;

    public CategorieController(ICategorieService categorieService)
    {
        _categorieService = categorieService;
    }


    [HttpPost]
    public async Task<ActionResult> Create(CategorieCreate c)
    {
        if (c is null) 
        {
            return BadRequest();
        } 

        Categorie categorie = CategorieMapper.ToCategorie(c);

        int newId = await _categorieService.CreateAsync(categorie);

        categorie.Id = newId;

        CategorieAll result = CategorieMapper.ToCategorieAll(categorie);

        return CreatedAtAction(nameof(GetById), new { id = newId }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategorieAll>> GetById(int id)
    {
        Categorie? categorie = await _categorieService.GetByIdAsync(id);

        if (categorie is null) 
        {
            return NotFound();
        } 

        CategorieAll result = CategorieMapper.ToCategorieAll(categorie);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CategorieAll>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategorieAll>>> GetAll()
    {
        List<Categorie> categories = await _categorieService.GetAllAsync();

        if (categories == null || !categories.Any()) 
        {
            return Ok(new List<CategorieAll>());
        }

        List<CategorieAll> results = categories.Select(CategorieMapper.ToCategorieAll).ToList();

        return Ok(results);
    }
}
