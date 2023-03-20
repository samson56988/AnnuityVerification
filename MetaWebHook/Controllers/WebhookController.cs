using MetaWebHook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http.Headers;
using System.Net;
using System.Text;

namespace MetaWebHook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {

        private readonly MetaDbContext _dbContext;
        private readonly IConfiguration configuration;

        public WebhookController(MetaDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            this.configuration = configuration;
        }


        [HttpPost("MetaMap")]
        public async Task<IActionResult> PayLoad(VerificationDetails user)
        {
            if (user.eventName == "verification_completed")
            {
                if(user.status == "verified")
                {
                    string Url = "";
                    Url = user.resource;
                    var auth = Authentication();
                    string Token = "";
                    string PolicyNo = "";
                    string Image = "";
                    Token = auth.Message;
                    string VerificationUrl = configuration.GetValue<string>("AnnuityVerification:VerificationUrl");
                    AnnuityVerificationResponse? verify = new AnnuityVerificationResponse();
                    var client = new RestClient($"{Url}");
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("accept", "application/json");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("authorization", $"Bearer {Token}");
                    IRestResponse response = client.Execute(request);
                    verify = JsonConvert.DeserializeObject<AnnuityVerificationResponse>(response.Content);
                    PolicyNo = verify.customInputValues.fields[0].atomicFieldParams.value;
                    //Image = verify.steps[0].data.selfieUrl;
                    PolicyDto dto = new PolicyDto();
                    dto.PolicyNo = PolicyNo;
                    dto.Image = user.metadata.userPhotoLink;  
                    string WebBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
                    string AnnuityUrl = configuration.GetValue<string>("ApiSettings:UpdateAnnuity");

                    string requestUri = WebBaseUrl + AnnuityUrl;
                    string ApiKey = configuration.GetValue<string>("ApiSettings:APIkey");
                    var Flexureclient = new HttpClient();
                    Flexureclient.BaseAddress = new Uri(requestUri);
                    Flexureclient.DefaultRequestHeaders.Accept.Clear();
                    Flexureclient.DefaultRequestHeaders.Add("X-ApiKey", $"{ApiKey}");
                    Flexureclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    Flexureclient.Timeout = TimeSpan.FromMinutes(5);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                    HttpResponseMessage Flexureresponse = await Flexureclient.PutAsync(requestUri, content);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        VerificationPayLoad load = new VerificationPayLoad();
                        load.timestamp = DateTime.Now;
                        load.resource = Url;
                        load.flowId = dto.PolicyNo;
                        load.eventName = user.status;
                        _dbContext.tbl_Verification.Add(load);
                        await _dbContext.SaveChangesAsync();
                        return Ok();

                    }
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
                
            }

            return BadRequest();
        }



        [HttpPost("Authenticate")]
        public ServiceResponse<bool> Authentication()
        {
            MatiAuthResponse? authResponse = new MatiAuthResponse();
            //string ClientId = configuration.GetValue<string>("AnnuityVerification:ClientID");
            //string ClientSecret = configuration.GetValue<string>("AnnuityVerification:ClientSeceret");
            string clientId = configuration.GetValue<string>("AnnuityVerification:ClientID");
            string clientSecret = configuration.GetValue<string>("AnnuityVerification:ClientSeceret");
            string credentials = clientId + ":" + clientSecret;
            byte[] credentialsBytes = Encoding.ASCII.GetBytes(credentials);
            string encodedCredentials = Convert.ToBase64String(credentialsBytes);
            string baseUrl = configuration.GetValue<string>("AnnuityVerification:baseUrl");
            var client = new RestClient($"{baseUrl}");
            var request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("authorization", $"Basic {encodedCredentials}");
            request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            authResponse = JsonConvert.DeserializeObject<MatiAuthResponse>(response.Content);
            return new ServiceResponse<bool>
            {
                Data = true,
                Success = true,
                Message = authResponse.access_token
            };

        }
    }
}
