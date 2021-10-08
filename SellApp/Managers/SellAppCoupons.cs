using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SellApp.Helpers;
using SellApp.Models;
using SellApp.Models.Coupons;

namespace SellApp.Managers
{
    public class SellAppCoupons
    {
        private readonly string _couponsApiEndpoint;        
        private readonly HttpRequestHelper _httpRequestManager;

        private PagesModel<ResponsePagedModel<DataCouponModel>> _currentCouponsListModel;

        public SellAppCoupons(HttpRequestHelper HttpRequestManager, string couponsApiEndpoint)
        {
            _httpRequestManager = HttpRequestManager;
            _couponsApiEndpoint = couponsApiEndpoint;
        }

        /// <summary>
        /// Get page 1 of all the coupons in your store. (Max 15 items)
        /// </summary>
        /// <returns><see cref="PagesModel<ResponsePagedModel<DataCouponModel>>"/></returns>
        public async Task<PagesModel<ResponsePagedModel<DataCouponModel>>> GetAllCoupons()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _couponsApiEndpoint);

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<ResponsePagedModel<DataCouponModel>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentCouponsListModel = new PagesModel<ResponsePagedModel<DataCouponModel>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return _currentCouponsListModel;
        }

        /// <summary>
        /// Get *next* page of all the coupons in your store. (Max 15 items)
        /// </summary>
        /// <returns>Next page</returns>
        public async Task<(bool Success, PagesModel<ResponsePagedModel<DataCouponModel>> Info)> NextPage()
        {
            if (Equals(_currentCouponsListModel, null) || Equals(_currentCouponsListModel.BL.links.next, null))
                return (false, null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _currentCouponsListModel.BL.links.next.ToString());

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<ResponsePagedModel<DataCouponModel>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentCouponsListModel = new PagesModel<ResponsePagedModel<DataCouponModel>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return (true, _currentCouponsListModel);
        }

        /// <summary>
        /// Get *previous* page of all the coupons in your store. (Max 15 items)
        /// </summary>
        /// <returns>previous page</returns>
        public async Task<(bool Success, PagesModel<ResponsePagedModel<DataCouponModel>> Info)> PreviousPage()
        {
            if (Equals(_currentCouponsListModel, null) || Equals(_currentCouponsListModel.BL.links.prev, null))
                return (false, null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _currentCouponsListModel.BL.links.prev.ToString());

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<ResponsePagedModel<DataCouponModel>>(httpRequestMessage)
                .ConfigureAwait(false);

            _currentCouponsListModel = new PagesModel<ResponsePagedModel<DataCouponModel>>
            {
                BL = httpRequestMessageJson,
                HasNextPage = !Equals(httpRequestMessageJson.links.next, null),
                HasPreviousPage = !Equals(httpRequestMessageJson.links.prev, null)
            };

            return (true, _currentCouponsListModel);
        }

        /// <summary>
        /// Create a new coupon
        /// </summary>
        /// <param name="NewCouponModel"><see cref="NewCouponModel"/> Parameter</param>
        /// <returns><see cref="DataCouponModel"/></returns>
        public async Task<DataCouponModel> CreateNewCoupon(NewCouponModel NewCouponSettings)
        {
            var bodyParams = new Dictionary<string, object>
            {
                { "code", NewCouponSettings.Code ?? throw new ArgumentNullException(nameof(NewCouponSettings.Code)) },
                { "type", NewCouponSettings.CouponType.ToString() },
                { "discount", NewCouponSettings.Discount.Length < 1 ? throw new ArgumentOutOfRangeException() : NewCouponSettings.Discount },
                { "store_wide", NewCouponSettings.StoreWide },
            };

            if (!Equals(NewCouponSettings.UsesLimit, null))
                bodyParams.Add("limit", NewCouponSettings.UsesLimit);

            if (!Equals(NewCouponSettings.ExpiresAt, null))
                bodyParams.Add("expires_at", NewCouponSettings.ExpiresAt);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _couponsApiEndpoint)
            {
                Content = new StringContent(JsonConvert.SerializeObject(bodyParams), Encoding.UTF8, "application/json")
            };

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<DataCouponModel>(httpRequestMessage)
                .ConfigureAwait(false);

            JToken createCouponResponse = JObject.Parse(httpRequestMessageJson.ToString());
            
            if (Equals(createCouponResponse.SelectToken("errors"), null))
            {
                return httpRequestMessageJson;
            }            

            throw new Exception(createCouponResponse.SelectToken("errors")?.SelectToken("code")?.ToString()
                ?? "Error when making the discount code.");
        }

        /// <summary>
        /// Get the information from a coupon by ID.
        /// </summary>
        /// <param name="Id">coupon Id</param>
        /// <returns><see cref="CouponInfoModel"/></returns>
        public async Task<CouponInfoModel> SearchInfoById(int Id)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_couponsApiEndpoint}/{Id}");

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<CouponInfoModel>(httpRequestMessage)
                .ConfigureAwait(false);

            return httpRequestMessageJson;
        }

        /// <summary>
        /// Update a coupon already created using their Id
        /// </summary>
        /// <param name="Id">coupon Id</param>
        /// <param name="NewCouponSettings"><see cref="NewCouponModel"/></param>
        /// <returns><see cref="DataCouponModel"/></returns>
        public async Task<DataCouponModel> UpdateCoupon(int Id, NewCouponModel NewCouponSettings)
        {
            var bodyParams = new Dictionary<string, object>
            {
                { "code", NewCouponSettings.Code ?? throw new ArgumentNullException(nameof(NewCouponSettings.Code)) },
                { "type", NewCouponSettings.CouponType.ToString() },
                { "discount", NewCouponSettings.Discount.Length < 1 ? throw new ArgumentOutOfRangeException() : NewCouponSettings.Discount },
                { "store_wide", NewCouponSettings.StoreWide },
            };

            if (!Equals(NewCouponSettings.UsesLimit, null))
                bodyParams.Add("limit", null);

            if (!Equals(NewCouponSettings.ExpiresAt, null))
                bodyParams.Add("expires_at", NewCouponSettings.ExpiresAt);

            var httpRequestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), $"{_couponsApiEndpoint}/{Id}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(bodyParams), Encoding.UTF8, "application/json")
            };

            var httpRequestMessageJson = await _httpRequestManager.SendRequestJsonDeserializeAsync<DataCouponModel>(httpRequestMessage)
                .ConfigureAwait(false);

            JToken createCouponResponse = JObject.Parse(httpRequestMessageJson.ToString());
           
            if (Equals(createCouponResponse.SelectToken("errors"), null))
            {
                return httpRequestMessageJson;
            }

            throw new Exception(createCouponResponse.SelectToken("errors")?.SelectToken("code")?.ToString()
                ?? "Error processing the response.");
        }

        /// <summary>
        /// Delete a coupon with the ID.
        /// </summary>
        /// <param name="Id">Coupon Id</param>
        /// <returns><see cref="Boolean"/></returns>
        public async Task<bool> DeleteCouponById(int Id)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{_couponsApiEndpoint}/{Id}");

            await _httpRequestManager.SendRequestJsonDeserializeAsync<object>(httpRequestMessage)
                .ConfigureAwait(false);

            // It will be validated if it is eliminated or not by the response status code,
            // if there is an error response code, it will be caught by the throw inside HttpRequestManager.

            // if it reaches this point, it means that it was eliminated correctly
            return true; 
        }
    }
}
