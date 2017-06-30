using AssetTracking.Models;
using AssetTracking.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AssetTracking.Managers
{
    public class HttpManager
    {
        private static HttpManager _instance;
        private HttpManager()
        {

        }
        public static HttpManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new HttpManager();
            }
            return _instance;
        }
        

        public enum LinkDeviceResponse { IdFailure, Success, UnAuthorize, NetworkException, ProcessError ,InternalError};

        public async Task<Dictionary<LinkDeviceResponse, string>> LinkDevice(string jsonData)
        {
            string responseString = null;
            Dictionary<LinkDeviceResponse, string> responseDic = new Dictionary<LinkDeviceResponse, string>();
            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Application.Current.Properties[Constants.APP_DOMAIN_URL_KEY].ToString());

                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Application.Current.Properties[Constants.APP_SETTINGS_ACCESS_TOKEN_KEY].ToString());
                    HttpResponseMessage response = await client.PostAsync("/api/Asset", content);
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        responseString = JsonConvert.DeserializeObject<ResponseMessageModel>(await response.Content.ReadAsStringAsync()).Message;
                        responseDic.Add(LinkDeviceResponse.ProcessError, responseString);
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        responseDic.Add(LinkDeviceResponse.IdFailure, responseString);
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        responseDic.Add(LinkDeviceResponse.UnAuthorize, responseString);
                    }
                    else if(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.NoContent)
                    {
						responseDic.Add(LinkDeviceResponse.Success, responseString);
                    }
                    else
                    {
                        responseDic.Add(LinkDeviceResponse.InternalError, responseString);
                    }
                    return responseDic;
                }

            }

            catch (Exception ex)
            {
                responseDic.Add(LinkDeviceResponse.NetworkException, responseString);
                return responseDic;
            }

        }

        public async Task<string> AuthenticateToken(string accessCode)
        {
            string requestUri = B2CConfigManager.GetInstance().GetB2CTokenUrl(accessCode);
            string strResponse = string.Empty;
            try
            {

                using (var client = new HttpClient())
                {
                    var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(requestUri, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        strResponse = await response.Content.ReadAsStringAsync();
                    }
                }
            }

            catch (Exception ex)
            {

            }
            return strResponse;
        }

        public async Task<Dictionary<LinkDeviceResponse, string>> GetSensorTypeFromID(string ID)
        {
            string responseString = null;
            Dictionary<LinkDeviceResponse, string> responseDic = new Dictionary<LinkDeviceResponse, string>();
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Application.Current.Properties[Constants.APP_DOMAIN_URL_KEY].ToString());
                    var content = new StringContent(ID, Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Application.Current.Properties[Constants.APP_SETTINGS_ACCESS_TOKEN_KEY].ToString());
                    HttpResponseMessage response = await client.PostAsync("/api/GetSensorType", content);
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        responseDic.Add(LinkDeviceResponse.ProcessError, responseString);
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        responseDic.Add(LinkDeviceResponse.IdFailure, responseString);
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        responseDic.Add(LinkDeviceResponse.UnAuthorize, responseString);
                    }
                    else //if(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                    {
                        responseString = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

                        responseDic.Add(LinkDeviceResponse.Success, responseString);

                    }
                    return responseDic;

                }
            }
            catch (Exception e)
            {
                responseDic.Add(LinkDeviceResponse.NetworkException, responseString);
                return responseDic;
            }
        }
        public async Task<Dictionary<LinkDeviceResponse, string>> GetAssetStatus(string assetId)
        {
            string responseString = null;
            Dictionary<LinkDeviceResponse, string> responseDic = new Dictionary<LinkDeviceResponse, string>();
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Application.Current.Properties[Constants.APP_DOMAIN_URL_KEY].ToString());
                    var content = new StringContent(assetId, Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Application.Current.Properties[Constants.APP_SETTINGS_ACCESS_TOKEN_KEY].ToString());
                    HttpResponseMessage response = await client.PostAsync("/api/AssetStatus", content);
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
						responseString = JsonConvert.DeserializeObject<ResponseMessageModel>(await response.Content.ReadAsStringAsync()).Message;
                        responseDic.Add(LinkDeviceResponse.ProcessError, responseString);
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        responseDic.Add(LinkDeviceResponse.IdFailure, responseString);
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        responseDic.Add(LinkDeviceResponse.UnAuthorize, responseString);
                    }
                    else //if(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                    {
                        responseString = await response.Content.ReadAsStringAsync();

                        responseDic.Add(LinkDeviceResponse.Success, responseString);

                    }
                    return responseDic;

                }
            }
            catch (Exception e)
            {
                responseDic.Add(LinkDeviceResponse.NetworkException, responseString);
                return responseDic;
            }
        }

        public async Task<string> GetB2CConfiguration(string baseAddress)
        {
            string responseStr = null; 
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseAddress);
                    HttpResponseMessage response = await client.GetAsync("/api/GetMobileConfiguration");
                    if (response.IsSuccessStatusCode)
                    {
                        responseStr = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch(Exception e)
            {
                
            }
            return responseStr;

            
        }

    }
}
