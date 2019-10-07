using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TreeStruct.Controllers
{
    public class GenerateExampleTreeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GenerateExampleTreeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
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
        public IActionResult GenerateExampleTree()
        {
            var time = DateTime.Now;

            TreeItem treeItem = NewItem("Drive C", null);
            int DriveCID = treeItem.ID;
            treeItem = NewItem("Program Files", DriveCID);
            int ProgramFilesID = treeItem.ID;
            treeItem = NewItem("Program Files (x86)", DriveCID);
            int ProgramFilesID64 = treeItem.ID;
            treeItem = NewItem("Users", DriveCID);
            int UsersID = treeItem.ID;
            treeItem = NewItem("Windows", DriveCID);
            int WindowsID = treeItem.ID;
            treeItem = NewItem("Trash", DriveCID);
            int TrashID = treeItem.ID;

            treeItem = NewItem("WinRar", ProgramFilesID);
            int WinRarID = treeItem.ID;
            treeItem = NewItem("Internet Explorer", ProgramFilesID);
            int InternetExplorerID = treeItem.ID;
            treeItem = NewItem("Total Commander", ProgramFilesID);
            int TotalCommanderID = treeItem.ID;

            treeItem = NewItem("FireFox", ProgramFilesID64);
            int FireFoxID = treeItem.ID;

            treeItem = NewItem("Home", UsersID);
            int HomeID = treeItem.ID;
            treeItem = NewItem("Public", UsersID);
            int PublicID = treeItem.ID;

            treeItem = NewItem("Format.exe", WindowsID);

            treeItem = NewItem("Document", HomeID);
            treeItem = NewItem("Desktop", HomeID);
            treeItem = NewItem("Music", HomeID);
            int MusicID = treeItem.ID;
            treeItem = NewItem("Download", HomeID);
            int DownloadID = treeItem.ID;

            _ = NewItem("tmp", HomeID);

            _ = NewItem("a.zip", DownloadID);
            _ = NewItem("b.zip", DownloadID);
            _ = NewItem("c.zip", DownloadID);
            _ = NewItem("d.zip", DownloadID);
            _ = NewItem("z.zip", DownloadID);
            _ = NewItem("A.zip", DownloadID);
            _ = NewItem("B.zip", DownloadID);
            _ = NewItem("D.zip", DownloadID);
            _ = NewItem("1.zip", DownloadID);
            _ = NewItem("2.zip", DownloadID);
            _ = NewItem("3.zip", DownloadID);

            _ = NewItem("some good music.mp3", MusicID);
            _ = NewItem("music feat .NET Core .mp3", MusicID);

            var timeResultStr = (DateTime.Now - time).TotalMilliseconds.ToString();
            TempData["msg"] = "Operation time: " + timeResultStr;

            return Redirect("/TreeItems/GetRoots/");
        }
    }
}