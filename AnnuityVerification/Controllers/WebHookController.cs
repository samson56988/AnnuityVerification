using AnnuityVerification.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.IO;

using Method = RestSharp.Method;
using System;

namespace AnnuityVerification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHookController : ControllerBase
    {
        private readonly MetaDbContext _dbContext;
        private readonly IConfiguration configuration;



        public WebHookController(MetaDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            this.configuration = configuration;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json", "application/xml", "text/json")]
        [Consumes("application/json", "application/xml", "text/json")]
        public async Task<IActionResult> PayLoad(VerificationDetails user)
        {
            if (user.eventName == "verification_completed" || user.eventName == "verification_updated")
            {
                if (user.status == "verified")
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
                    var request = new RestRequest(RestSharp.Method.GET);
                    request.AddHeader("accept", "application/json");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("authorization", $"Bearer {Token}");
                    IRestResponse response = client.Execute(request);
                    verify = JsonConvert.DeserializeObject<AnnuityVerificationResponse>(response.Content);
                    PolicyNo = verify.customInputValues.fields[0].atomicFieldParams.value;
                    if (verify.steps[0].data.selfieUrl != null)
                    {
                        Image = verify.steps[0].data.selfieUrl;
                    }
                    else
                    {
                        Image = verify.steps[1].data.selfieUrl;
                    }

                    string imageAfter = Image.Replace("https://media.getmati.com/file?location=", "");

                    PolicyDto dto = new PolicyDto();
                    dto.Image = Convert.ToString(imageAfter.ToString());
                    dto.PolicyNo = "NCSP/IB/2017/077067";           
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
                    HttpResponseMessage Flexureresponse = await Flexureclient.PostAsync(requestUri, content);
                    PolicyUpdateResponse polres = new PolicyUpdateResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var apiTask = Flexureresponse.Content.ReadAsStringAsync();
                        var responseString = apiTask.Result;
                        polres = JsonConvert.DeserializeObject<PolicyUpdateResponse>(responseString);
                        return Ok();

                    }

                    VerificationPayLoad load = new VerificationPayLoad();
                    load.timestamp = DateTime.Now;
                    load.resource = Url;
                    load.PolicyNo = dto.PolicyNo;
                    load.eventName = user.eventName;
                    load.MetaPicture = Image;
                    _dbContext.tbl_Verification.Add(load);
                    await _dbContext.SaveChangesAsync();
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
