using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class TreeItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TreeItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TreeItem.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treeItem = await _context.TreeItem
                .FirstOrDefaultAsync(m => m.ID == id);
            if (treeItem == null)
            {
                return NotFound();
            }

            return View(treeItem);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Value,Parent")] TreeItem treeItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(treeItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(treeItem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSimple([Bind("ID,Value,Parent")] TreeItem treeItem, int rootId)
        {
            if (ModelState.IsValid)
            {
                TempData["msg"] = "New item added";
                _context.Add(treeItem);
                await _context.SaveChangesAsync();
            }
            else TempData["msg"] = "Can't add this item";

            return Redirect("/TreeItems/GetTree/" + rootId.ToString());
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treeItem = await _context.TreeItem.FindAsync(id);
            if (treeItem == null)
            {
                return NotFound();
            }
            return View(treeItem);
        }

        [HttpPost]
        public IActionResult Move(int id, [Bind("ID,Value,Parent")] TreeItem treeItem, int rootId)
        {
            
            if (id != treeItem.ID) 
            {
                TempData["msg"] = "Unknown error, sorry";
                return NotFound();
            }
            TreeItem treeItemOld = _context.TreeItem.Find(id);
            if (treeItemOld.Parent == null)
            {
                if (treeItem.Parent != treeItemOld.Parent)
                {
                    TempData["msg"] = "Can't move roots";
                    return Redirect("/TreeItems/GetTree/" + rootId.ToString());
                }
                else
                {
                    _context.Entry(treeItemOld).State = EntityState.Detached;
                    _context.Update(treeItem);
                    _context.SaveChanges();
                    TempData["msg"] = "Edited";
                    return Redirect("/TreeItems/GetTree/" + rootId.ToString());
                }
            }          
            else
            {
                _context.Entry(treeItemOld).State = EntityState.Detached;
            }

            bool isMovementCorret;
            if (treeItem.Parent == null) isMovementCorret = true;
            else if (treeItem.Parent == treeItem.ID) {
                isMovementCorret = false;
            }
            else { 
                List<String> stats = new List<String>();
                stats = IsNotIDInChildren(treeItem.ID, treeItem.Parent, stats);
                if (stats.Contains("error")) isMovementCorret = false;
                else isMovementCorret = true;
                }

            if (ModelState.IsValid)
            {                   
                try
                {
                    if (isMovementCorret)
                    {
                        TempData["msg"] = "Edited";
                        _context.Update(treeItem);
                        _context.SaveChanges();
                    }
                    else TempData["msg"] = "Can't move branch into itself";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreeItemExists(treeItem.ID))
                    {
                        TempData["msg"] = "Unknown error, sorry";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/TreeItems/GetTree/" + rootId.ToString());
            }
            return View(treeItem);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Value,Parent")] TreeItem treeItem)
        {
            if (id != treeItem.ID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {                
                try
                {
                    _context.Update(treeItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreeItemExists(treeItem.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(treeItem);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treeItem = await _context.TreeItem
                .FirstOrDefaultAsync(m => m.ID == id);
            if (treeItem == null)
            {
                return NotFound();
            }

            return View(treeItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treeItem = await _context.TreeItem.FindAsync(id);
            _context.TreeItem.Remove(treeItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreeItemExists(int id)
        {
            return _context.TreeItem.Any(e => e.ID == id);
        }

        public async Task<IActionResult> GetRoots()
        {
            var roots = await _context.TreeItem.Where(m => m.Parent == 0 || m.Parent == null).ToListAsync();
            if (roots == null)
            {
                return NotFound(); 
            }

            return View(roots);
        }

        public async Task<IActionResult> GetTreeItems(int id)
        {
            List<TreeItem> tree = new List<TreeItem>(); 

            var root = await _context.TreeItem                         

                .FirstOrDefaultAsync(m => m.ID == id);
            if (root == null)
            {
                return NotFound(); 
            }

            tree = BuildTreeRec(root, tree);
            return View(tree.ToList());
        }

        private List<TreeItem> BuildTreeRec(TreeItem root, List<TreeItem> tree) 
        {
            tree.Add(root);
            var elems = _context.TreeItem.Where(m => m.Parent == root.ID).ToList();
            if (elems != null)
            {
                foreach (TreeItem elem in elems)
                {
                    BuildTreeRec(elem, tree);
                }
            }
            return tree;
        }

        public IActionResult GetTree(int id)
        {
            return View(id);
        }

        private List<String> IsNotIDInChildren(int? ID, int? newParentID, List<String> stats)
        {

            var children = _context.TreeItem.Where(m => m.Parent == ID).ToList();
            if (children != null)
            {
                foreach (TreeItem elem in children)
                {
                    if (newParentID != elem.ID && newParentID != elem.Parent )
                    {
                        stats.Add("ok");
                        IsNotIDInChildren(elem.ID, newParentID, stats);
                    }
                    else
                    {
                        stats.Add("error");
                    }
                }
            }
            return stats;
        }


        [HttpPost]
        public IActionResult DeleteWithChildren(int id, int rootId)
        {
            try
            {
                DeleteWithChildrenRec(id);
                TempData["msg"] = "Deleted";
            }
            catch (Exception e)
            {
                TempData["msg"] = "Can't delete it";
            }
            return Redirect("/TreeItems/GetTree/" + rootId.ToString());
        }

        private void DeleteWithChildrenRec(int branchID)
        {
            var childrenToDelete = _context.TreeItem.Where(m => m.Parent == branchID).ToArray();
            if (childrenToDelete.Count() > 0)
            {
                foreach (TreeItem elem in childrenToDelete)
                {
                    DeleteWithChildrenRec(elem.ID);
                }
                var treeItem = _context.TreeItem.Find(branchID);
                _context.TreeItem.Remove(treeItem);
                _context.SaveChanges();
            }
            else
            {
                try
                {
                    var treeItem = _context.TreeItem.Find(branchID);
                    _context.TreeItem.Remove(treeItem);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
        }

        private TreeItem NewItem(string value, int? parent)
        {
            TreeItem treeItem = new TreeItem();
            treeItem.Value = value;
            treeItem.Parent = parent;
            _context.Add(treeItem);
            _context.SaveChanges();
            return treeItem;
        }

        [HttpPost]
        public IActionResult AddRoot()
        {
            TreeItem treeItem = NewItem("<new root>", null);
            return Redirect("/TreeItems/GetRoots/");
        }
    }
}
