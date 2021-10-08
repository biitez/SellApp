using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SellApp.Helpers;
using SellApp.Models;
using SellApp.Models.BlackLists;

namespace SellApp.Managers
{
    public class SellAppBlackList
    {
        private readonly string _blackListApiEndpoint;
        private readonly HttpRequestHelper _httpRequestManager;

        private PagesModel<ResponsePagedModel<DataBlackListModel>> _currentBlackListModel;

        public SellAppBlackList(HttpRequestHelper HttpRequestManager, string BlackListApiEndpoint)
        {
            _httpRequestManager = HttpRequestManager;
            _blackListApiEndpoint = BlackListApiEndpoint;
        }

        /// <summary>
        /// Get page 1 of all the blacklist in your store. (Max 15 items)
        /// </summary>
        /// <returns><see cref="PagesModel<ResponsePagedModel<DataBlackListModel>>"/></returns>
        public async Task<PagesModel<ResponsePagedModel<DataBlackListModel>>> GetAllBlackLists()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _blackListApiEndpoint);

            var httpRequestMessageJson = await _httpRequestManager
                .SendRequestJsonDeserializeAsync<ResponsePagedModel<DataBlackListModel>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentBlackListModel = new PagesModel<ResponsePagedModel<DataBlackListModel>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return _currentBlackListModel;
        }

        /// <summary>
        /// Get *next* page of all the blacklist in your store. (Max 15 items)
        /// </summary>
        /// <returns>Next page</returns>
        public async Task<(bool Success, PagesModel<ResponsePagedModel<DataBlackListModel>> Info)> NextPage()
        {
            if (Equals(_currentBlackListModel, null) || Equals(_currentBlackListModel.BL.links.next, null))
                return (false, null);

            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Get, _currentBlackListModel.BL.links.next.ToString());

            var httpRequestMessageJson = await _httpRequestManager
                .SendRequestJsonDeserializeAsync<ResponsePagedModel<DataBlackListModel>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentBlackListModel = new PagesModel<ResponsePagedModel<DataBlackListModel>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return (true, _currentBlackListModel);
        }

        /// <summary>
        /// Get *previous* page of all the blacklist in your store. (Max 15 items)
        /// </summary>
        /// <returns>previous page</returns>
        public async Task<(bool Success, PagesModel<ResponsePagedModel<DataBlackListModel>> Info)> PreviousPage()
        {
            if (Equals(_currentBlackListModel, null) || Equals(_currentBlackListModel.BL.links.prev, null))
                return (false, null);

            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Get, _currentBlackListModel.BL.links.prev.ToString());

            var httpRequestMessageJson = await _httpRequestManager
                .SendRequestJsonDeserializeAsync<ResponsePagedModel<DataBlackListModel>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentBlackListModel = new PagesModel<ResponsePagedModel<DataBlackListModel>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return (true, _currentBlackListModel);
        }

        /// <summary>
        /// Create a new blacklist rule
        /// </summary>
        /// <param name="createBlackListRuleModel"><see cref="CreateBlackList(CreateRuleModelBlackList)"/> Parameter</param>
        /// <returns>BlackList Id</returns>
        public async Task<int> CreateBlackList(CreateRuleModelBlackList createBlackListRuleModel)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _blackListApiEndpoint)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    type = createBlackListRuleModel.RuleType.ToString(),
                    data = createBlackListRuleModel.RuleValue ?? throw new Exception("Invalid Data Value"),
                    description = createBlackListRuleModel.RuleDescription
                }), Encoding.UTF8, "application/json")
            };

            var httpRequestMessageJson = await _httpRequestManager
                .SendRequestJsonDeserializeAsync<object>(httpRequestMessage)
                .ConfigureAwait(false);

            JToken createBlackListResponse = JObject.Parse(httpRequestMessageJson.ToString());

            return (int) (createBlackListResponse.SelectToken("id") ?? 0);
        }

        /// <summary>
        /// Get the information from a blacklist by ID.
        /// </summary>
        /// <param name="Id">BlackList Id</param>
        /// <returns><see cref="Datum"/></returns>
        public async Task<Datum> SearchInfoById(int Id)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_blackListApiEndpoint}/{Id}");

            var httpRequestMessageJson = await _httpRequestManager
                .SendRequestJsonDeserializeAsync<Datum>(httpRequestMessage)
                .ConfigureAwait(false);

            return httpRequestMessageJson;
        }

        /// <summary>
        /// Update a blacklist rule already created using their Id
        /// </summary>
        /// <param name="Id">BlackList Id</param>
        /// <param name="BlackListRuleModel"><see cref="CreateRuleModelBlackList"/></param>
        /// <returns><see cref="Boolean"/></returns>
        public async Task<bool> UpdateBlackList(int Id, CreateRuleModelBlackList BlackListRuleModel)
        {
            var httpRequestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), $"{_blackListApiEndpoint}/{Id}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    type = BlackListRuleModel.RuleType.ToString(),
                    data = BlackListRuleModel.RuleValue ?? throw new Exception("Invalid Data Value"),
                    description = BlackListRuleModel.RuleDescription
                }), Encoding.UTF8, "application/json")
            };

            var httpRequestMessageJson = await _httpRequestManager
                .SendRequestJsonDeserializeAsync<object>(httpRequestMessage)
                .ConfigureAwait(false);

            JToken createBlackListResponse = JObject.Parse(httpRequestMessageJson.ToString());

            return createBlackListResponse.SelectToken("id") != null;
        }

        /// <summary>
        /// Delete a rule with the ID.
        /// </summary>
        /// <param name="Id">Black List Rule Id</param>
        /// <returns><see cref="Boolean"/></returns>
        public async Task<bool> DeleteBlackListId(int Id)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{_blackListApiEndpoint}/{Id}");

            await _httpRequestManager.SendRequestJsonDeserializeAsync<object>(httpRequestMessage)
                .ConfigureAwait(false);

            // It will be validated if it is eliminated or not by the response status code,
            // if there is an error response code, it will be caught by the throw inside HttpRequestManager.

            // if it reaches this point, it means that it was eliminated correctly
            return true;
        }
    }
}