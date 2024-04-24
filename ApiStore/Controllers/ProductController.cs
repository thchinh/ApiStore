using ApiStore.Data;
using ApiStore.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductControllers(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _context.Products.ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var product = await _context.Products.FindAsync(id);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var getCategory = await _context.Categories.FindAsync(request.CategoryId);
        if (getCategory is null)
        {
            return BadRequest($"Can't find category with id: {request.CategoryId}");
        }
        var product = await _context.Products.AddAsync(new Models.Product
        {
            CategoryId = request.CategoryId,
            Name = request.Name,
            Description = request.Description
        });

        await _context.SaveChangesAsync();
        return Ok(product.Entity);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest($"Invalid request Id");
        }

        var product = await _context.Products.FindAsync(request.Id);

        if (product is null)
        {
            return BadRequest($"Can't find product with id: {request.CategoryId}");
        }

        product.Name = request.Name;
        product.Description = request.Description;
        product.CategoryId = request.CategoryId;

        await _context.SaveChangesAsync();
        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {

        var product = await _context.Products.FindAsync(id);
        if (product is null)
        {
            return BadRequest($"Can't find product with id: {id}");
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return Ok("Item removed successfully!");
    }
}
