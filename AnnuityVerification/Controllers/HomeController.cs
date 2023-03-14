using AnnuityVerification.Models;
using AnnuityVerification.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.DataProtection;

namespace AnnuityVerification.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private readonly IWebHostEnvironment _enviroment;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IMemoryCache memoryCache, IWebHostEnvironment enviroment)
        {
            _logger = logger;
            _configuration = configuration;
            _memoryCache = memoryCache;
            _enviroment = enviroment;
        }

        public IActionResult Index()
        {
            var fetchbankData = FetchBankData();

            if (fetchbankData == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.BankDetails = fetchbankData.Result.result.table;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(VerificationModel model)
        {

            if(ViewBag.BankDetails == null)
            {
                var fetchbankData = FetchBankData();

                ViewBag.BankDetails = fetchbankData.Result.result.table;
            }
          
            string WebBaseUrl = _configuration.GetValue<string>("ApiSettings:WebBaseUrl");
            string ImagePath = _configuration.GetValue<string>("ApiSettings:ImagePath");
            string ApiKey = _configuration.GetValue<string>("ApiSettings:APIkey");
            string PhotoUrl = "";

            try
            {
            
                    var authenticate = AuthenticationAsync();
                    string token = authenticate.Result.result.message;
                    model.Token = token;
                    if (authenticate.IsCompletedSuccessfully)
                    {
                        VerificationResponse verifyresponse = new VerificationResponse();
                        string BaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");
                        string Verify = _configuration.GetValue<string>("ApiSettings:VerifyNuban");
                        string requestUri = BaseUrl + Verify;
                        var client = new HttpClient();
                        client.BaseAddress = new Uri(requestUri);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{model.Token}");
                        client.DefaultRequestHeaders.Add("X-ApiKey", $"{ApiKey}");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.Timeout = TimeSpan.FromMinutes(5);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync(requestUri, content);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var apiTask = response.Content.ReadAsStringAsync();
                            var responseString = apiTask.Result;
                            verifyresponse = JsonConvert.DeserializeObject<VerificationResponse>(responseString);

                            if(verifyresponse.result.data == false)
                            {
                                TempData["delete"] = "Verification failed";
                                return RedirectToAction("Index");
                            }


                         Message message = new Message();

                        message = JsonConvert.DeserializeObject<Message>(verifyresponse.result.message);


                        string ImageBase64 = message.bvn_nuban.photo; 

                            Random rand = new Random();
                            string ImageId = Convert.ToString((long)Math.Floor(rand.NextDouble() * 9_000_000_000L + 1_000_000_000L));
                            string ImageString = ImageBase64;
                            string ImageUrl = WebBaseUrl + ImagePath;
                            var path = Path.Combine(_enviroment.WebRootPath, ImagePath);

                            //Check if directory exist
                            if (!System.IO.Directory.Exists(path))
                            {
                                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                            }
                            string imageName = $"ID{ImageId}" + ".jpg";
                            //set the image path
                            string imgPath = Path.Combine(path, imageName);
                            byte[] imageBytes = Convert.FromBase64String(ImageString);
                            System.IO.File.WriteAllBytes(imgPath, imageBytes);
                            string imageData = WebBaseUrl  + ImagePath  + imageName;
                        PhotoUrl = imageData;
                        string hexString = "";
                        string secretKey = _configuration.GetValue<string>("ApiSettings:SecretKey");
                        using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
                        {
                            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(imageData));
                            hexString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                        }                  

                            _memoryCache.Set(2, hexString , new MemoryCacheEntryOptions()
                  .SetSlidingExpiration(TimeSpan.FromMinutes(1)));

                            _memoryCache.Set(3, PhotoUrl, new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1)));



                            TempData["save"] = "Verification completed successfully";
                            return RedirectToAction("FaceVerification");
                        }
                        else if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            TempData["delete"] = "Verification failed";
                            return RedirectToAction("Index");
                        }
                        else

                        TempData["delete"] = "Verification failed";
                        return RedirectToAction("Index");

                    }
                    
                
                
                return View();
            }
            catch(Exception ex)
            {
                TempData["delete"] = "Please try again later";
                return RedirectToAction("Index");
            }
          
        }

    

        public async Task<AuthenticationResponse> AuthenticationAsync()
        {
            AuthenticationResponse Authresponse =  new AuthenticationResponse();
            string BaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");
            string Authentication = _configuration.GetValue<string>("ApiSettings:Authentication");
            string ApiKey = _configuration.GetValue<string>("ApiSettings:APIkey");
            string requestUri = BaseUrl+ Authentication;
            var client = new HttpClient();
            client.BaseAddress = new Uri(requestUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
            client.DefaultRequestHeaders.Add("X-ApiKey", $"{ApiKey}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.Timeout = TimeSpan.FromMinutes(5);
            HttpResponseMessage response = await client.PostAsync(requestUri, null);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var apiTask = response.Content.ReadAsStringAsync();
                var responseString = apiTask.Result;
                Authresponse = JsonConvert.DeserializeObject<AuthenticationResponse>(responseString);
                string token = Authresponse.result.message;
                _memoryCache.Set(1, token, new MemoryCacheEntryOptions()
                  .SetSlidingExpiration(TimeSpan.FromHours(3)));

                return Authresponse;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var apiTask = response.Content.ReadAsStringAsync();
                var responseString = apiTask.Result;
                Authresponse = JsonConvert.DeserializeObject<AuthenticationResponse>(responseString);
                return Authresponse;
            }
            else

                return Authresponse;
        }

        public async Task<BankResponse> FetchBankData()
        {      

            BankResponse bankresponse = new BankResponse();
            string BaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");
            string BankDetails = _configuration.GetValue<string>("ApiSettings:FetchBank");
            string ApiKey = _configuration.GetValue<string>("ApiSettings:APIkey");
            string requestUri = BaseUrl + BankDetails;
            var client = new HttpClient();
            client.BaseAddress = new Uri(requestUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
            client.DefaultRequestHeaders.Add("X-ApiKey", $"{ApiKey}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromMinutes(5);
            HttpResponseMessage response = await client.GetAsync(requestUri);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var apiTask = response.Content.ReadAsStringAsync();
                var responseString = apiTask.Result;
                bankresponse = JsonConvert.DeserializeObject<BankResponse>(responseString);
                return bankresponse;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var apiTask = response.Content.ReadAsStringAsync();
                var responseString = apiTask.Result;
                bankresponse = JsonConvert.DeserializeObject<BankResponse>(responseString);
                return bankresponse;
            }
            else

                return bankresponse;
        }


        public IActionResult FaceVerification()
        {

            ViewBag.PhotoHash = _memoryCache.Get(2);
            ViewBag.PhotoUrl = _memoryCache.Get(3);
            return View();
        }



















    }
}