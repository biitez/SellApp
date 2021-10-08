using System.Net.Http;
using System.Threading.Tasks;
using SellApp.Helpers;
using SellApp.Models;
using SellApp.Models.Tickets;
using SellApp.Tickets.Manager;

namespace SellApp.Tickets
{
    public class SellAppTickets
    {
        private readonly string _ticketsApiEndpoint;
        private readonly HttpRequestHelper _httpRequestManager;

        private PagesModel<ResponsePagedModel<DataTicketHandlerModel>> _currentTicketListModel;

        /// <summary>
        /// Manage ticket messages
        /// </summary>
        public SellAppTicketsMessagesManager Messages;

        public SellAppTickets(HttpRequestHelper HttpRequestManager, string ticketsApiEndpoint)
        {
            _httpRequestManager = HttpRequestManager;
            _ticketsApiEndpoint = ticketsApiEndpoint;

            Messages = new SellAppTicketsMessagesManager(HttpRequestManager, ticketsApiEndpoint);
        }

        /// <summary>
        /// Get all the tickets in the system
        /// </summary>
        /// <returns><see cref="PagesModel<ResponsePagedModel<DataTicketHandlerModel>>"/></returns>
        public async Task<PagesModel<ResponsePagedModel<DataTicketHandlerModel>>> GetAllTickets()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _ticketsApiEndpoint);

            var httpRequestMessageJson = await _httpRequestManager
                .SendRequestJsonDeserializeAsync<ResponsePagedModel<DataTicketHandlerModel>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentTicketListModel = new PagesModel<ResponsePagedModel<DataTicketHandlerModel>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return _currentTicketListModel;
        }

        /// <summary>
        /// If you called the method to obtain all the items and there 
        /// are more than 15, with this method you will go to page 2.
        /// </summary>
        /// <returns>Page 2</returns>
        public async Task<(bool Success, PagesModel<ResponsePagedModel<DataTicketHandlerModel>> Info)> NextPage()
        {
            if (Equals(_currentTicketListModel?.BL.links.next, null))
                return (false, null);

            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Get, _currentTicketListModel.BL.links.next.ToString());

            var httpRequestMessageJson = await _httpRequestManager
                .SendRequestJsonDeserializeAsync<ResponsePagedModel<DataTicketHandlerModel>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentTicketListModel = new PagesModel<ResponsePagedModel<DataTicketHandlerModel>>
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
        public async Task<(bool Success, PagesModel<ResponsePagedModel<DataTicketHandlerModel>> Info)> PreviousPage()
        {
            if (Equals(_currentTicketListModel?.BL.links.prev, null))
                return (false, null);

            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Get, _currentTicketListModel.BL.links.prev.ToString());

            var httpRequestMessageJson = await _httpRequestManager
                .SendRequestJsonDeserializeAsync<ResponsePagedModel<DataTicketHandlerModel>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentTicketListModel = new PagesModel<ResponsePagedModel<DataTicketHandlerModel>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return (true, _currentTicketListModel);
        }

        /// <summary>
        /// Search the ticket information under an ID.
        /// </summary>
        /// <param name="Id">Ticket Id</param>
        /// <returns><see cref="DataTicketHandlerModel"/></returns>
        public async Task<DataTicketHandlerModel> SearchInfoById(int Id)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_ticketsApiEndpoint}/{Id}");

            var httpRequestMessageJson = await _httpRequestManager
                .SendRequestJsonDeserializeAsync<DataTicketHandlerModel>(httpRequestMessage)
                .ConfigureAwait(false);

            return httpRequestMessageJson;
        }
    }
}