using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SellApp.Helpers;
using SellApp.Models;
using SellApp.Models.FeedBacks;

namespace SellApp.Managers
{
    public class SellAppFeedbacks
    {
        private readonly string _feedbackApiEndpoint;        
        private readonly HttpRequestHelper _httpRequestManager;

        private PagesModel<ResponsePagedModel<DataFeedBacks>> _currentFeedbacksListModel;

        public SellAppFeedbacks(HttpRequestHelper HttpRequestManager, string feedbackApiEndpoint)
        {
            _httpRequestManager = HttpRequestManager;
            _feedbackApiEndpoint = feedbackApiEndpoint;
        }

        /// <summary>
        /// Get page 1 of all the feedbacks in your store. (Max 15 items)
        /// </summary>
        /// <returns><see cref="PagesModel<ResponsePagedModel<DataFeedBacks>>"/></returns>
        public async Task<PagesModel<ResponsePagedModel<DataFeedBacks>>> GetAllFeedbacks()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _feedbackApiEndpoint);

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<ResponsePagedModel<DataFeedBacks>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentFeedbacksListModel = new PagesModel<ResponsePagedModel<DataFeedBacks>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return _currentFeedbacksListModel;
        }

        /// <summary>
        /// Get *next* page of all the feedbacks in your store. (Max 15 items)
        /// </summary>
        /// <returns>Next page</returns>
        public async Task<(bool Success, PagesModel<ResponsePagedModel<DataFeedBacks>> Info)> NextPage()
        {
            if (Equals(_currentFeedbacksListModel?.BL.links.next, null))
                return (false, null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _currentFeedbacksListModel.BL.links.next.ToString());

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<ResponsePagedModel<DataFeedBacks>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentFeedbacksListModel = new PagesModel<ResponsePagedModel<DataFeedBacks>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return (true, _currentFeedbacksListModel);
        }

        /// <summary>
        /// Get *previous* page of all the feedbacks in your store. (Max 15 items)
        /// </summary>
        /// <returns>previous page</returns>
        public async Task<(bool Success, PagesModel<ResponsePagedModel<DataFeedBacks>> Info)> PreviousPage()
        {
            if (Equals(_currentFeedbacksListModel, null) || Equals(_currentFeedbacksListModel.BL.links.prev, null))
                return (false, null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _currentFeedbacksListModel.BL.links.prev.ToString());

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<ResponsePagedModel<DataFeedBacks>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentFeedbacksListModel = new PagesModel<ResponsePagedModel<DataFeedBacks>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return (true, _currentFeedbacksListModel);
        }

        /// <summary>
        /// Get the information from a feedback by ID.
        /// </summary>
        /// <param name="Id">Feedback Id</param>
        /// <returns><see cref="DataFeedBacks"/></returns>
        public async Task<DataFeedBacks> SearchInfoById(int Id)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_feedbackApiEndpoint}/{Id}");

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<DataFeedBacks>(httpRequestMessage)
                .ConfigureAwait(false);

            return httpRequestMessageJson;
        }

        /// <summary>
        /// Responds to a given feedback by the entered ID.
        /// </summary>
        /// <param name="Id">Feedback Id</param>
        /// <param name="Reply">A reply message that was left by the store owner who received this rating</param>
        /// <returns><see cref="DataFeedBacks"/></returns>
        public async Task<DataFeedBacks> ReplyFeedback(int Id, string Reply)
        {
            var bodyParams = new Dictionary<string, object>
            {
                { "reply", Reply ?? throw new ArgumentNullException(nameof(Reply)) }
            };

            var httpRequestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), $"{_feedbackApiEndpoint}/{Id}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(bodyParams), Encoding.UTF8, "application/json")
            };

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<DataFeedBacks>(httpRequestMessage)
                .ConfigureAwait(false);

            JToken createFeedbackResponse = JObject.Parse(httpRequestMessageJson.ToString());
           
            if (Equals(createFeedbackResponse.SelectToken("errors"), null))
            {
                return httpRequestMessageJson;
            }

            throw new Exception(createFeedbackResponse.SelectToken("errors")?.SelectToken("code")?.ToString()
                ?? "Error replying feedback.");
        }
    }
}
