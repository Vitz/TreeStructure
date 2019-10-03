using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreeItemApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TreeItemApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TreeItemApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TreeItem>>> GetTreeItem()
        {
            return await _context.TreeItem.ToListAsync();
        }

        // GET: api/TreeItemApi/5
        [HttpGet("item/{id}")]
        public async Task<ActionResult<TreeItem>> GetTreeItem(int id)
        {
            var treeItem = await _context.TreeItem.FindAsync(id);

            if (treeItem == null)
            {
                return NotFound();
            }

            return treeItem;
        }

        // PUT: api/TreeItemApi/5
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

        // POST: api/TreeItemApi
        [HttpPost]
        public async Task<ActionResult<TreeItem>> PostTreeItem(TreeItem treeItem)
        {
            _context.TreeItem.Add(treeItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTreeItem", new { id = treeItem.ID }, treeItem);
        }

        // DELETE: api/TreeItemApi/5
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTree(int id)
        {
            List<TreeItem> tree = new List<TreeItem>(); // one tree 

            //find 1st root            
            var root = await _context.TreeItem             //find 1st root            

                .FirstOrDefaultAsync(m => m.ID == id);
            if (root == null)
            {
                return NotFound(); //no any roots
            }

            tree = BuildTreeRec(root, tree);
            return CreatedAtAction("GetTree", tree);
        }


        private List<TreeItem> BuildTreeRec(TreeItem item, List<TreeItem> tree)
        {
            tree.Add(item);
            var elems = _context.TreeItem.Where(m => m.Parent == item.ID);
            if (elems != null)
            {
                foreach (TreeItem elem in elems)
                {
                    BuildTreeRec(elem, tree);
                }
            }
            return tree;
        }


    }
}
