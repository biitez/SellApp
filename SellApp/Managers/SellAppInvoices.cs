using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SellApp.Helpers;
using SellApp.Models;
using SellApp.Models.Invoices;
using SellApp.Models.Invoices.Response;
using SellApp.Models.Invoices.Session;

namespace SellApp.Managers
{
    public class SellAppInvoices
    {
        private readonly string _invoicesApiEndpoint;
        private readonly HttpRequestHelper _httpRequestManager;

        private PagesModel<ResponsePagedModel<DataInvoices>> _currentInvoicesListModel;

        public SellAppInvoices(HttpRequestHelper HttpRequestManager, string invoicesApiEndpoint)
        {
            _httpRequestManager = HttpRequestManager;
            _invoicesApiEndpoint = invoicesApiEndpoint;
        }

        /// <summary>
        /// Get page 1 of all the invoices in your store. (Max 15 items)
        /// </summary>
        /// <returns><see cref="PagesModel<ResponsePagedModel<DataInvoices>>"/></returns>
        public async Task<PagesModel<ResponsePagedModel<DataInvoices>>> GetAllCoupons()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _invoicesApiEndpoint);

            var httpRequestMessageJson = await _httpRequestManager
                .SendRequestJsonDeserializeAsync<ResponsePagedModel<DataInvoices>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentInvoicesListModel = new PagesModel<ResponsePagedModel<DataInvoices>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return _currentInvoicesListModel;
        }

        /// <summary>
        /// Get *next* page of all the invoices in your store. (Max 15 items)
        /// </summary>
        /// <returns>Next page</returns>
        public async Task<(bool Success, PagesModel<ResponsePagedModel<DataInvoices>> Info)> NextPage()
        {
            if (Equals(_currentInvoicesListModel?.BL.links.next, null))
                return (false, null);

            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Get, _currentInvoicesListModel.BL.links.next.ToString());

            var httpRequestMessageJson = await _httpRequestManager
                .SendRequestJsonDeserializeAsync<ResponsePagedModel<DataInvoices>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentInvoicesListModel = new PagesModel<ResponsePagedModel<DataInvoices>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return (true, _currentInvoicesListModel);
        }

        /// <summary>
        /// Get *previous* page of all the invoices in your store. (Max 15 items)
        /// </summary>
        /// <returns>previous page</returns>
        public async Task<(bool Success, PagesModel<ResponsePagedModel<DataInvoices>> Info)> PreviousPage()
        {
            if (Equals(_currentInvoicesListModel?.BL.links.prev, null))
                return (false, null);

            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Get, _currentInvoicesListModel.BL.links.prev.ToString());

            var httpRequestMessageJson = await _httpRequestManager
                .SendRequestJsonDeserializeAsync<ResponsePagedModel<DataInvoices>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentInvoicesListModel = new PagesModel<ResponsePagedModel<DataInvoices>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return (true, _currentInvoicesListModel);
        }

        /// <summary>
        /// Get the information from a invoice by ID.
        /// </summary>
        /// <param name="Id">invoice Id</param>
        /// <returns><see cref="DataInvoices"/></returns>
        public async Task<DataInvoices> SearchInfoById(int Id)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_invoicesApiEndpoint}/{Id}");

            var httpRequestMessageJson = await _httpRequestManager
                .SendRequestJsonDeserializeAsync<DataInvoices>(httpRequestMessage)
                .ConfigureAwait(false);

            return httpRequestMessageJson;
        }

        /// <summary>
        /// Issue a replacement for an order by the entered ID.
        /// </summary>
        /// <param name="Id">Invoice Id</param>
        /// <returns><see cref="ResponseDataInvoice"/></returns>
        public async Task<ResponseDataInvoice> IssueReplacementOrderById(int Id)
        {
            var httpRequestMessage = new HttpRequestMessage(new HttpMethod("PATCH"),
                $"{_invoicesApiEndpoint}/{Id}/issue-replacement");

            var httpRequestMessageJson = await _httpRequestManager
                .SendRequestJsonDeserializeAsync<ResponseDataInvoice>(httpRequestMessage)
                .ConfigureAwait(false);

            JToken issueReplacement = JObject.Parse(httpRequestMessageJson.ToString());

            if (Equals(issueReplacement.SelectToken("errors"), null))
            {
                return httpRequestMessageJson;
            }

            throw new Exception("Error processing the response.");
        }

        /// <summary>
        /// Generates a payment session for the given order.
        /// 
        /// POSTing to this endpoint will return a unique payment_url that you can pass 
        /// on to your customer. Once the customer has made the payment, our system will
        /// automatically process the relevant order and deliver the product to the customer, 
        /// you won't need to do anything else.
        /// </summary>
        /// <param name="Id">Invoice Id</param>
        /// <returns><see cref="SessionDataInvoices"/></returns>
        public async Task<SessionDataInvoices> GeneratePaymentSessionForOrder(int Id)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_invoicesApiEndpoint}/{Id}/checkout");

            var httpRequestMessageJson = await _httpRequestManager
                .SendRequestJsonDeserializeAsync<SessionDataInvoices>(httpRequestMessage)
                .ConfigureAwait(false);

            JToken issueReplacement = JObject.Parse(httpRequestMessageJson.ToString());

            if (Equals(issueReplacement.SelectToken("errors"), null))
            {
                return httpRequestMessageJson;
            }

            throw new Exception("Error processing the response.");
        }
    }
}