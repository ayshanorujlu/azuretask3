using Azure;
using Azure.Data.Tables;
using AzureStorageLibrary;
using AzureStorageLibrary.Models;
using AzureStorageLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using MVCWebApp.Models;
using System.Diagnostics;
using System.Net;

namespace MVCWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly INoSqlStorage<Product> _noSqlStorage;
        private readonly INoSqlStorage<Store> _storeNoSqlStorage;

        public HomeController(INoSqlStorage<Product> noSqlStorage, INoSqlStorage<Store> storeNoSqlStorage)
        {
            _noSqlStorage = noSqlStorage;
            _storeNoSqlStorage = storeNoSqlStorage;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                Products = (await _noSqlStorage.All()).ToList(),
                Stores = (await _storeNoSqlStorage.All()).ToList(),
            };
            ViewBag.IsUpdate = false;
            return View(model);
        }

       

        public async Task<IActionResult> Update(string rowKey, string partitionKey)
        {

            var product = await _noSqlStorage.Get(rowKey, partitionKey);
            var model = new HomeViewModel
            {
                Products = (await _noSqlStorage.All()).ToList(),
                Stores = (await _storeNoSqlStorage.All()).ToList(),
                Product = product
            };
            ViewBag.IsUpdate = true;

            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            if (string.IsNullOrEmpty(product.StoreKey))
            {
                ModelState.AddModelError("Product.StoreKey", "Store is required");
                var model = new HomeViewModel
                {
                    Products = (await _noSqlStorage.All()).ToList(),
                    Stores = (await _storeNoSqlStorage.All()).ToList(),
                    Product = product
                };
                ViewBag.IsUpdate = true;
                return View("Index", model);

            }

            try
            {
                await _noSqlStorage.Update(product);
                return RedirectToAction("Index");
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.PreconditionFailed)
            {
                throw;
            }

        }


        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (string.IsNullOrEmpty(product.StoreKey))
            {
                ModelState.AddModelError("Product.StoreKey", "Store is required");
                var viewModel = new HomeViewModel
                {
                    Products = (await _noSqlStorage.All()).ToList(),
                    Stores = (await _storeNoSqlStorage.All()).ToList(),
                    Product = product
                };
                ViewBag.IsUpdate = false;
                return View("Index", viewModel);
            }
            product.RowKey = Guid.NewGuid().ToString();
            product.PartitionKey = "Shoes";

            await _noSqlStorage.Add(product);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateStore(Store store)
        {
            store.RowKey = Guid.NewGuid().ToString();
            store.PartitionKey = "Internal";

            await _storeNoSqlStorage.Add(store);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string rowKey, string partitionKey)
        {

            await _noSqlStorage.Delete(partitionKey, rowKey);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Query(int filterPrice)
        {
            ViewBag.IsUpdate = false;
            var model = new HomeViewModel
            {
                Products = (await _noSqlStorage.Query(x => x.Price > filterPrice)).ToList(),
                Stores = (await _storeNoSqlStorage.All()).ToList(),
            };
            return View("Index", model);
        }
    }
}