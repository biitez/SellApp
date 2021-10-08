using System.Net.Http;
using System.Threading.Tasks;
using SellApp.Helpers;
using SellApp.Models;
using SellApp.Models.Listings;
using SellApp.Models.Listings.Info;

namespace SellApp.Managers
{
    public class SellAppListings
    {
        private readonly string _listingsApiEndpoint;
        private readonly HttpRequestHelper _httpRequestManager;

        private PagesModel<ResponsePagedListingsModel> _currentListingsListModel;

        public SellAppListings(HttpRequestHelper HttpRequestManager, string listingsApiEndpoint)
        {
            _httpRequestManager = HttpRequestManager;
            _listingsApiEndpoint = listingsApiEndpoint;
        }

        /// <summary>
        /// Get page 1 of all the listings in your store. (Max 15 items)
        /// </summary>
        /// <returns><see cref="PagesModel<ResponsePagedModel<ResponsePagedListingsModel>>"/></returns>
        public async Task<PagesModel<ResponsePagedListingsModel>> GetAllListings()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _listingsApiEndpoint);

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<ResponsePagedListingsModel>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentListingsListModel = new PagesModel<ResponsePagedListingsModel>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return _currentListingsListModel;
        }

        /// <summary>
        /// Get *next* page of all the listings in your store. (Max 15 items)
        /// </summary>
        /// <returns>Next page</returns>
        public async Task<(bool Success, PagesModel<ResponsePagedListingsModel> Info)> NextPage()
        {
            if (Equals(_currentListingsListModel, null) || Equals(_currentListingsListModel.BL.links.next, null))
                return (false, null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _currentListingsListModel.BL.links.next.ToString());

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<ResponsePagedListingsModel>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentListingsListModel = new PagesModel<ResponsePagedListingsModel>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return (true, _currentListingsListModel);
        }

        /// <summary>
        /// Get *previous* page of all the listings in your store. (Max 15 items)
        /// </summary>
        /// <returns>previous page</returns>
        public async Task<(bool Success, PagesModel<ResponsePagedListingsModel> Info)> PreviousPage()
        {
            if (Equals(_currentListingsListModel?.BL.links.prev, null))
                return (false, null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _currentListingsListModel.BL.links.prev.ToString());

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<ResponsePagedListingsModel>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentListingsListModel = new PagesModel<ResponsePagedListingsModel>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return (true, _currentListingsListModel);
        }

        /// <summary>
        /// Get the information from a listing by ID.
        /// </summary>
        /// <param name="Id">listing Id</param>
        /// <returns><see cref="Models.Listings.Info.Data"/></returns>
        public async Task<Models.Listings.Info.Data> SearchInfoById(int Id)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_listingsApiEndpoint}/{Id}");

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<ProductInfoListingsModel>(httpRequestMessage)
                .ConfigureAwait(false);

            return httpRequestMessageJson.data;
        }

        /// <summary>
        /// Delete a listing with the ID.
        /// </summary>
        /// <param name="Id">listing Id</param>
        /// <returns><see cref="Boolean"/></returns>
        public async Task<bool> DeleteCouponById(int Id)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{_listingsApiEndpoint}/{Id}");

            await _httpRequestManager.SendRequestJsonDeserializeAsync<object>(httpRequestMessage)
                .ConfigureAwait(false);

            // It will be validated if it is eliminated or not by the response status code,
            // if there is an error response code, it will be caught by the throw inside HttpRequestManager.

            // if it reaches this point, it means that it was eliminated correctly
            return true;
        }
    }
}
