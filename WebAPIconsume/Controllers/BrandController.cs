using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;
using WebAPIconsume.Models;

namespace WebAPIconsume.Controllers
{
    public class BrandController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7047/api");
        private readonly HttpClient _httpClient;

        public BrandController() 
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;  
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<BrandViewModel> brandList = new List<BrandViewModel>();
            HttpResponseMessage response =await _httpClient.GetAsync(_httpClient.BaseAddress + "/Brand/GetBrands");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();

                brandList = JsonConvert.DeserializeObject<List<BrandViewModel>>(data);

            }
            return View(brandList);
        }

        [HttpGet]
        public  IActionResult Create()
        { 
            return View();
        }

        [HttpPost]
        public IActionResult Create(BrandViewModel model) 
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Brand/PostBrand", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Brand Created.";
                    return RedirectToAction("Index");
                }
                else
                {
                    // If the request was not successful, display an error message
                    TempData["errorMessage"] = $"Failed to create brand. Status code: {response.StatusCode}";

                    // Return the view to allow the user to correct any issues
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

            return View();
           
            
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                BrandViewModel brand = new BrandViewModel();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Brand/GetBrand/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    brand = JsonConvert.DeserializeObject<BrandViewModel>(data);
                }
                return View(brand);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
           
          }
        [HttpPost]
        public IActionResult Edit(BrandViewModel model) 
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string requestData = content.ReadAsStringAsync().Result;
                Console.WriteLine(requestData);
                HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/Brand/PutBrand?id="+model.ID , content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Brand details updated";
                    return RedirectToAction("Index");
                }
                else
                {
                    // If the request was not successful, display an error message
                    TempData["errorMessage"] = $"Failed to create brand. Status code: {response.StatusCode}";

                    // Return the view to allow the user to correct any issues
                    return View(model);
                }
                
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }
    }

}
