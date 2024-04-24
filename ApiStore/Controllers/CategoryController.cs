using ApiStore.Data;
using ApiStore.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CategoryControllers(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _context.Categories.ToListAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var category = await _context.Categories.FindAsync(id);
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
    {
        var category = await _context.Categories.AddAsync(new Models.Category
        {
            Name = request.Name,
            Description = request.Description
        });

        await _context.SaveChangesAsync();
        return Ok(category.Entity);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest($"Invalid request Id");
        }

        var category = await _context.Categories.FindAsync(request.Id);

        if (category is null)
        {
            return BadRequest($"Can't find category with id: {request.Id}");
        }

        category.Name = request.Name;
        category.Description = request.Description;

        await _context.SaveChangesAsync();
        return Ok(category);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {

        var category = await _context.Categories.FindAsync(id);
        if (category is null)
        {
            return BadRequest($"Can't find category with id: {id}");
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return Ok("Item removed successfully!");
    }
}
