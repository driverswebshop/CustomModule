/*
' Copyright (c) 2024 Daniel Damjan
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using DriversWebshop.Dnn.Dnn.DriversWebshop.HelloWorld.Components;
using DriversWebshop.Dnn.Dnn.DriversWebshop.HelloWorld.Models;
using DriversWebshop.Dnn.Dnn.DriversWebshop.HelloWorld.Data;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace DriversWebshop.Dnn.Dnn.DriversWebshop.HelloWorld.Controllers
{
    [DnnHandleError]
    public class ItemController : DnnController
    {

        public ActionResult Delete(int itemId)
        {
            ItemManager.Instance.DeleteItem(itemId, ModuleContext.ModuleId);
            return RedirectToDefaultRoute();
        }

        public ActionResult Edit(int itemId = -1)
        {
            DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.DnnPlugins);

            var userlist = UserController.GetUsers(PortalSettings.PortalId);
            var users = from user in userlist.Cast<UserInfo>().ToList()
                        select new SelectListItem { Text = user.DisplayName, Value = user.UserID.ToString() };

            ViewBag.Users = users;

            var item = (itemId == -1)
                 ? new Item { ModuleId = ModuleContext.ModuleId }
                 : ItemManager.Instance.GetItem(itemId, ModuleContext.ModuleId);

            return View(item);
        }

        [HttpPost]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (item.ItemId == -1)
            {
                item.CreatedByUserId = User.UserID;
                item.CreatedOnDate = DateTime.UtcNow;
                item.LastModifiedByUserId = User.UserID;
                item.LastModifiedOnDate = DateTime.UtcNow;

                ItemManager.Instance.CreateItem(item);
            }
            else
            {
                var existingItem = ItemManager.Instance.GetItem(item.ItemId, item.ModuleId);
                existingItem.LastModifiedByUserId = User.UserID;
                existingItem.LastModifiedOnDate = DateTime.UtcNow;
                existingItem.ItemName = item.ItemName;
                existingItem.ItemDescription = item.ItemDescription;
                existingItem.AssignedUserId = item.AssignedUserId;

                ItemManager.Instance.UpdateItem(existingItem);
            }

            return RedirectToDefaultRoute();
        }

        [ModuleAction(ControlKey = "Edit", TitleKey = "AddItem")]
        public ActionResult AnotherAction()
        {
            var items = ItemManager.Instance.GetItems(ModuleContext.ModuleId);
            return View(items);
        }

        private readonly CustomModuleRepository _repository;

        public ItemController(CustomModuleRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            var cpuBenchmarks = _repository.GetCpuBenchmarks();
            var gpuBenchmarks = _repository.GetGpuBenchmarks();
            var ramBenchmarks = _repository.GetRamBenchmarks();

            ViewBag.CpuBenchmarks = cpuBenchmarks;
            ViewBag.GpuBenchmarks = gpuBenchmarks;
            ViewBag.RamBenchmarks = ramBenchmarks;

            return View();
        }

        [HttpPost]
        public ActionResult CalculateScore(string selectedCpu, string selectedGpu, string selectedRam)
        {
            var cpuBenchmark = _repository.GetCpuBenchmarks().FirstOrDefault(c => c.CpuName == selectedCpu);
            var gpuBenchmark = _repository.GetGpuBenchmarks().FirstOrDefault(g => g.GpuName == selectedGpu);
            var ramBenchmark = _repository.GetRamBenchmarks().FirstOrDefault(r => r.RamName == selectedRam);

            if (cpuBenchmark == null || gpuBenchmark == null || ramBenchmark == null)
            {
                return RedirectToAction("Index");
            }

            double score = (gpuBenchmark.GpuPercentage * 0.6) + (cpuBenchmark.CpuPercentage * 0.3) + (ramBenchmark.RamPercentage * 0.1);

            ViewBag.Score = score;
            return View("Index");
        }
    }
}
