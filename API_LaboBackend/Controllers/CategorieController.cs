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

        Categorie categorie = CategorieMapper.ToCategorie(c);

        categorie.Id = await _categorieService.CreateAsync(categorie);

        return CreatedAtAction(nameof(GetById), new { id = categorie.Id }, categorie);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategorieAllInfo>> GetById(int id)
    {
        Categorie? categorie = await _categorieService.GetByIdAsync(id);

        if (categorie != null)
        {
            CategorieAllInfo result = CategorieMapper.ToCategorieAllInfo(categorie);

            return Ok(result);
        }

        return NotFound();
    }

    [HttpGet]
    public async Task<ActionResult<List<CategorieShortInfo>>> GetAll()
    {
        List<Categorie> categories = await _categorieService.GetAllAsync();

        List<CategorieShortInfo> results = categories.Select(c => CategorieMapper.ToCategorieShortInfo(c)).ToList();

        return Ok(results);
    }
}
