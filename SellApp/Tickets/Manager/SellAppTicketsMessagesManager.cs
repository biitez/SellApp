using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SellApp.Helpers;
using SellApp.Models;
using SellApp.Models.Tickets;
using SellApp.Models.Tickets.Messages;

namespace SellApp.Tickets.Manager
{
    public class SellAppTicketsMessagesManager
    {
        private readonly string _ticketsApiEndpoint;
        private readonly HttpRequestHelper _httpRequestManager;

        private PagesModel<ResponsePagedModel<DataTicketMessagesManagerModel>> _currentTicketListModel;

        public SellAppTicketsMessagesManager(HttpRequestHelper HttpRequestManager, string ticketsApiEndpoint)
        {
            _httpRequestManager = HttpRequestManager;
            _ticketsApiEndpoint = ticketsApiEndpoint;
        }

        /// <summary>
        /// Get all the messages of a ticket under the ID. (Max. 15 Items)
        /// </summary>
        /// <param name="Id">Ticket Id</param>
        /// <returns><see cref="PagesModel<ResponsePagedModel<DataTicketMessagesManagerModel>>"/></returns>
        public async Task<PagesModel<ResponsePagedModel<DataTicketMessagesManagerModel>>> GetAllTicketMessagesById(int Id)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_ticketsApiEndpoint}/{Id}/messages");

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<ResponsePagedModel<DataTicketMessagesManagerModel>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentTicketListModel = new PagesModel<ResponsePagedModel<DataTicketMessagesManagerModel>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return _currentTicketListModel;
        }

        /// <summary>
        /// If you called the method to obtain all the items and there 
        /// are more than 15, with this method you will go to next page.
        /// </summary>
        /// <returns>Next page</returns>
        public async Task<(bool Success, PagesModel<ResponsePagedModel<DataTicketMessagesManagerModel>> Info)> NextPage()
        {
            if (Equals(_currentTicketListModel?.BL.links.next, null))
                return (false, null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _currentTicketListModel.BL.links.next.ToString());

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<ResponsePagedModel<DataTicketMessagesManagerModel>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentTicketListModel = new PagesModel<ResponsePagedModel<DataTicketMessagesManagerModel>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return (true, _currentTicketListModel);
        }

        /// <summary>
        /// If you went to the next page, with this you can go to the previous one
        /// </summary>
        /// <returns>Previous page</returns>
        public async Task<(bool Success, PagesModel<ResponsePagedModel<DataTicketMessagesManagerModel>> Info)> PreviousPage()
        {
            if (Equals(_currentTicketListModel?.BL.links.prev, null))
                return (false, null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _currentTicketListModel.BL.links.prev.ToString());

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<ResponsePagedModel<DataTicketMessagesManagerModel>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentTicketListModel = new PagesModel<ResponsePagedModel<DataTicketMessagesManagerModel>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return (true, _currentTicketListModel);
        }

        /// <summary>
        /// Creates a message to a ticket by its entered ID and returns the created message.
        /// </summary>
        /// <param name="Id">Ticket Id</param>
        /// <param name="Message">The message that was left.</param>
        /// <returns><see cref="DataTicketMessages"/></returns>
        public async Task<DataTicketMessages> ReplyTicketById(int Id, string Message)
        {
            var contentBody = new Dictionary<string, string>
            {
                { "content", Message }
            };

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_ticketsApiEndpoint}/{Id}/messages")
            {
                Content = new StringContent(JsonConvert.SerializeObject(contentBody), Encoding.UTF8, "application/json")
            };

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<DataTicketMessages>(httpRequestMessage)
                .ConfigureAwait(false);

            return httpRequestMessageJson;
        }
    }
}
