using CampusEats.Api.Dtos;
using CampusEats.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMenuService _svc;

    public MenuController(IMenuService svc)
    {
        _svc = svc;
    }

    // Public - anyone can view the menu
    [HttpGet]
    public ActionResult<IEnumerable<MenuItemDto>> GetAll()
    {
        return Ok(_svc.GetAll());
    }

    // Public - anyone can view one menu item
    [HttpGet("{id}")]
    public ActionResult<MenuItemDto> GetById(int id)
    {
        var item = _svc.GetById(id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    // Admin only - create
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public ActionResult<MenuItemDto> Create(
        [FromBody] CreateMenuItemDto dto)
    {
        var created = _svc.Create(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created);
    }

    // Admin only - update
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Update(
        int id,
        [FromBody] CreateMenuItemDto dto)
    {
        var ok = _svc.Update(id, dto);

        return ok ? NoContent() : NotFound();
    }

    // Admin only - delete
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var ok = _svc.Delete(id);

        return ok ? NoContent() : NotFound();
    }
}