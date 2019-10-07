using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreeItemsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TreeItemsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TreeItem>>> GetTreeItem()
        {
            return await _context.TreeItem.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TreeItem>> GetTreeItem(int id)
        {
            var treeItem = await _context.TreeItem.FindAsync(id);

            if (treeItem == null)
            {
                return NotFound();
            }

            return treeItem;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTreeItem(int id, TreeItem treeItem)
        {
            if (id != treeItem.ID)
            {
                return BadRequest();
            }
            _context.Entry(treeItem).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TreeItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TreeItem>> PostTreeItem(TreeItem treeItem)
        {
            _context.TreeItem.Add(treeItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTreeItem", new { id = treeItem.ID }, treeItem);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TreeItem>> DeleteTreeItem(int id)
        {
            var treeItem = await _context.TreeItem.FindAsync(id);
            if (treeItem == null)
            {
                return NotFound();
            }

            _context.TreeItem.Remove(treeItem);
            await _context.SaveChangesAsync();

            return treeItem;
        }

        private bool TreeItemExists(int id)
        {
            return _context.TreeItem.Any(e => e.ID == id);
        }
    }
}
