using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ColorsAjaxApp.Data;
using ColorsAjaxApp.Models;

namespace ColorsAjaxApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColorsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ColorsController(AppDbContext db) { _db = db; }

    [HttpGet]
    public async Task<IActionResult> Get() =>
        Ok(await _db.Colors.OrderBy(c => c.DisplayOrder).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(ColorItem item)
    {
        if (item.DisplayOrder == 0)
            item.DisplayOrder = (await _db.Colors.MaxAsync(c => (int?)c.DisplayOrder)) is int m ? m + 1 : 1;

        _db.Colors.Add(item);
        await _db.SaveChangesAsync();
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ColorItem item)
    {
        if (id != item.Id) return BadRequest();
        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await _db.Colors.FindAsync(id);
        if (e is null) return NotFound();
        _db.Colors.Remove(e);
        await _db.SaveChangesAsync();
        return Ok();
    }

    // בונוס: שמירת סדר אחרי גרירה
    public record ReorderDto(int Id, int DisplayOrder);

    [HttpPost("reorder")]
    public async Task<IActionResult> Reorder(List<ReorderDto> orders)
    {
        var ids = orders.Select(o => o.Id).ToHashSet();
        var rows = await _db.Colors.Where(c => ids.Contains(c.Id)).ToListAsync();
        foreach (var r in rows)
            r.DisplayOrder = orders.First(o => o.Id == r.Id).DisplayOrder;

        await _db.SaveChangesAsync();
        return Ok();
    }
}
