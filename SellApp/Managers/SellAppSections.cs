using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SellApp.Helpers;
using SellApp.Models;
using SellApp.Models.Sections;

namespace SellApp.Managers
{
    public class SellAppSections
    {
        private readonly string _sectionApiEndpoint;        
        private readonly HttpRequestHelper _httpRequestManager;

        private PagesModel<ResponsePagedModel<DataSectionsModel>> _currentCouponsListModel;

        public SellAppSections(HttpRequestHelper HttpRequestManager, string sectionApiEndpoint)
        {
            _httpRequestManager = HttpRequestManager;
            _sectionApiEndpoint = sectionApiEndpoint;
        }

        /// <summary>
        /// Get page 1 of all the coupons in your store. (Max 15 items)
        /// </summary>
        /// <returns><see cref="PagesModel<ResponsePagedModel<DataSectionsModel>>"/></returns>
        public async Task<PagesModel<ResponsePagedModel<DataSectionsModel>>> GetAllCoupons()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _sectionApiEndpoint);

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<ResponsePagedModel<DataSectionsModel>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentCouponsListModel = new PagesModel<ResponsePagedModel<DataSectionsModel>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return _currentCouponsListModel;
        }

        /// <summary>
        /// Get *next* page of all the sections in your store. (Max 15 items)
        /// </summary>
        /// <returns>Next page</returns>
        public async Task<(bool Success, PagesModel<ResponsePagedModel<DataSectionsModel>> Info)> NextPage()
        {
            if (Equals(_currentCouponsListModel?.BL.links.next, null))
                return (false, null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _currentCouponsListModel.BL.links.next.ToString());

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<ResponsePagedModel<DataSectionsModel>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentCouponsListModel = new PagesModel<ResponsePagedModel<DataSectionsModel>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return (true, _currentCouponsListModel);
        }

        /// <summary>
        /// Get *previous* page of all the sections in your store. (Max 15 items)
        /// </summary>
        /// <returns>previous page</returns>
        public async Task<(bool Success, PagesModel<ResponsePagedModel<DataSectionsModel>> Info)> PreviousPage()
        {
            if (Equals(_currentCouponsListModel?.BL.links.prev, null))
                return (false, null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _currentCouponsListModel.BL.links.prev.ToString());

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<ResponsePagedModel<DataSectionsModel>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentCouponsListModel = new PagesModel<ResponsePagedModel<DataSectionsModel>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return (true, _currentCouponsListModel);
        }

        /// <summary>
        /// Creates a section and returns the created section.
        /// </summary>
        /// <param name="Title">The section's title.</param>
        /// <param name="Hidden">Whether this section is hidden from public view (can only be accessed by direct URL)</param>
        /// <param name="Order">[Optional] The order of the section in which it is displayed on your storefront.</param>
        /// <returns><see cref="DataSectionsModel"/></returns>
        public async Task<DataSectionsModel> CreateNewSection(string Title, bool Hidden, int? Order = null)
        {
            var bodyParams = new Dictionary<string, object>
            {
                { "title", Title },
                { "hidden", Hidden }
            };

            if (!Equals(Order, null))
                bodyParams.Add("order", Order);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _sectionApiEndpoint)
            {
                Content = new StringContent(JsonConvert.SerializeObject(bodyParams), Encoding.UTF8, "application/json")
            };

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<DataSectionsModel>(httpRequestMessage)
                .ConfigureAwait(false);

            JToken createSectionResponse = JObject.Parse(httpRequestMessageJson.ToString());
            
            if (Equals(createSectionResponse.SelectToken("errors"), null))
            {
                return httpRequestMessageJson;
            }            

            throw new Exception(createSectionResponse.SelectToken("errors")?.SelectToken("section")?.ToString()
                ?? "Error processing the response.");
        }

        /// <summary>
        /// Get the information from a section by ID.
        /// </summary>
        /// <param name="Id">Section Id</param>
        /// <returns><see cref="DataSectionsModel"/></returns>
        public async Task<DataSectionsModel> SearchInfoById(int Id)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_sectionApiEndpoint}/{Id}");

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<DataSectionsModel>(httpRequestMessage)
                .ConfigureAwait(false);

            return httpRequestMessageJson;
        }

        /// <summary>
        /// Update a section already created using its ID and sending new parameters.
        /// </summary>
        /// <param name="Id">Section Id</param>
        /// <param name="Title">Title</param>
        /// <param name="Hidden">Hidden</param>
        /// <param name="Order">Order</param>
        /// <returns><see cref="DataSectionsModel"/></returns>
        public async Task<DataSectionsModel> UpdateSection(int Id, string Title, bool Hidden, int? Order = null)
        {
            var bodyParams = new Dictionary<string, object>
            {
                { "title", Title },
                { "hidden", Hidden }
            };

            if (!Equals(Order, null))
                bodyParams.Add("order", Order);

            var httpRequestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), $"{_sectionApiEndpoint}/{Id}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(bodyParams), Encoding.UTF8, "application/json")
            };

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<DataSectionsModel>(httpRequestMessage)
                .ConfigureAwait(false);

            JToken updateSectionResponse = JObject.Parse(httpRequestMessageJson.ToString());
           
            if (Equals(updateSectionResponse.SelectToken("errors"), null))
            {
                return httpRequestMessageJson;
            }

            throw new Exception(updateSectionResponse.SelectToken("errors")?.SelectToken("section")?.ToString()
                ?? "Error processing the response.");
        }

        /// <summary>
        /// Delete a section with the ID.
        /// </summary>
        /// <param name="Id">Section Id</param>
        /// <returns><see cref="Boolean"/></returns>
        public async Task<bool> DeleteSectionById(int Id)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{_sectionApiEndpoint}/{Id}");

            await _httpRequestManager.SendRequestJsonDeserializeAsync<object>(httpRequestMessage)
                .ConfigureAwait(false);

            // It will be validated if it is eliminated or not by the response status code,
            // if there is an error response code, it will be caught by the throw inside HttpRequestManager.

            // if it reaches this point, it means that it was eliminated correctly
            return true; 
        }
    }
}
