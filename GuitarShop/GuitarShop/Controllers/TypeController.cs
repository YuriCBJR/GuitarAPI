using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class TypeController : ControllerBase
{
    private readonly GuitarContext _context;
    public TypeController(GuitarContext guitarContext)
    {
        _context = guitarContext;
    }

[HttpPost]
public async Task<IActionResult> AddType([FromBody] CreateTypeDto dto)
{
    try
    {
    var typeExists = await _context.Types.FirstOrDefaultAsync(t => t.Id == dto.Id);
     if (typeExists != null)
     return BadRequest("Type already Exists with this id");

      var type = new Type()
      {
        Name = dto.Name,
        Id = dto.Id
      };
      await _context.Types.AddAsync(type);
      await _context.SaveChangesAsync();
      return Ok("Type added sucessfuly!");   
    }
    catch(Exception e)
    {
            return BadRequest(e.Message);
        }
    }
[HttpGet]
public async Task<IActionResult> GetTypes()
    {
        var types = await _context.Types
        .Select(t => new TypeResponseDto
        {
            Id = t.Id,
            Name = t.Name
            })
            .ToListAsync();
            return Ok(types);
        }
        
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteType(int Id)
        {
            var guitarUses = await _context.Guitars.AnyAsync(g => g.TypeId == Id);
            if (guitarUses)
            return BadRequest("Cant delete this type because it's being used by a guitar");

            var type = await _context.Types.FirstOrDefaultAsync( t => t.Id == Id);
            if (type == null)
            return NotFound("Type not found");
            _context.Types.Remove(type);
            _context.SaveChanges();
            return Ok("Type deleted sucessfuly!");
         }
[HttpPut("{Id}")]
public async Task<IActionResult> ModifyType(int Id, [FromBody] CreateTypeDto dto)
{
    var type = await _context.Types.FirstOrDefaultAsync(t => t.Id == Id);
    if (type == null)
    {
        return NotFound();
    }
    type.Name = dto.Name;
    _context.Update(type);
    _context.SaveChanges();
    return Ok("Type modified sucessfuly!"); 
    }

}
