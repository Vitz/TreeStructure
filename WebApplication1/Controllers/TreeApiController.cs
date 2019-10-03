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
    public class TreeApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TreeApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*   // GET: api/TreeApi
           [HttpGet]
           public async Task<ActionResult<IEnumerable<TreeItem>>> GetTreeItem()
           {
               return await _context.TreeItem.ToListAsync();
           }*/

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<TreeItem>>> GetTree(int id)
        {

            List<TreeItem> tree = new List<TreeItem>(); 
            var root = await _context.TreeItem.FirstOrDefaultAsync(m => m.ID == id);
            if (root == null)
            {
                return NotFound(); //no any roots
            }

            tree = BuildTreeRec(root, tree);
            return tree.ToList();
            // ;
        }


        private List<TreeItem> BuildTreeRec(TreeItem item, List<TreeItem> tree)
        {
            tree.Add(item);
            var elems = _context.TreeItem.Where(m => m.Parent == item.ID).ToList();
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
