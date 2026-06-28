using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[Controller]")]
public class GuitarController : ControllerBase
{
    private readonly GuitarContext _context;
    public GuitarController(GuitarContext guitarContext)
    {
        _context = guitarContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetGuitars()
    {
      var guitar = await _context.Guitars
      .Include(g => g.Type)
      .Select(g => new GuitarResponseDto
      {
          Id = g.Id,
          Name = g.Name,
          Brand = g.Brand,
          NumberOfStrings = g.NumberOfStrings,
          Price = g.Price,
          TypeName = g.Type.Name
      })
      .ToListAsync();

      return Ok(guitar);
    }

    [HttpPost] 
    public async Task<IActionResult> AddGuitar([FromBody] CreateGuitarDto dto)
    {
    try
    {
     var guitar = new Guitar()
      {
        Name = dto.Name,
        Brand = dto.Brand,
        NumberOfStrings = dto.NumberOfStrings,
        Price = dto.Price,
        TypeId = dto.TypeId
        };
        _context.Guitars.Add(guitar);
        _context.SaveChanges();
        return Ok("Guitar added sucessfuly!");
        }
    catch(Exception e)
    {
     return BadRequest(e.Message);
        }
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteGuitar(Guid Id)
    {
        var guitar = await _context.Guitars.FirstOrDefaultAsync(g => g.Id == Id);
        if (guitar == null)
        {
            return NotFound();
        }
        _context.Guitars.Remove(guitar);
        _context.SaveChanges();
        return Ok("Guitar deleted sucessfuly!");
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> ModifyGuitar(Guid Id, [FromBody] CreateGuitarDto dto)
    {
        var guitar = await _context.Guitars.FirstOrDefaultAsync(g => g.Id == Id);
        if (guitar == null)
        {
            return NotFound();
        }
        guitar.Name = dto.Name;
        guitar.Brand = dto.Brand;
        guitar.NumberOfStrings = dto.NumberOfStrings;
        guitar.Price = dto.Price;
        guitar.TypeId = dto.TypeId;
        _context.Update(guitar);
        _context.SaveChanges();
        return Ok("Guitar modified sucessfuly!");
    }

}



