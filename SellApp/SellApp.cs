using SellApp.Helpers;
using System;
using System.Net.Http;
using SellApp.Managers;
using SellApp.Tickets;

namespace SellApp
{
    public class SellApp
    {
        private const string SellAppUrl = "https://sell.app/api/v1";

        private readonly HttpRequestHelper _httpRequestHelper;

        /// <summary>
        /// Create an instance of SellApp using your account authorization key.
        /// </summary>
        /// <param name="AuthorizationTokenBearer">
        /// SellApp's API uses Bearer authentication with your API Key.
        /// You can generate an unlimited amount of API keys for your account here: https://sell.app/user/api-tokens
        /// </param>
        /// <param name="httpClient">(Optional) You can assign your own instance of <see cref="HttpClient"/> instance.</param>
        public SellApp(string AuthorizationTokenBearer, HttpClient httpClient = null)
        {
            if (string.IsNullOrWhiteSpace(AuthorizationTokenBearer))
            {
                throw new ArgumentNullException(nameof(AuthorizationTokenBearer));
            }

            _httpRequestHelper = new HttpRequestHelper(AuthorizationTokenBearer, httpClient ?? new HttpClient());
        }

        /// <summary>
        /// An instance of <see cref="SellAppBlackList"/> is created where you can handle everything related to it.
        /// </summary>
        /// <returns><see cref="SellAppBlackList"/></returns>
        public SellAppBlackList GetBlackListHandler()
        {
            return new SellAppBlackList(_httpRequestHelper, $"{SellAppUrl}/blacklists");
        }

        /// <summary>
        /// An instance of <see cref="SellAppCoupons"/> is created where you can handle everything related to it.
        /// </summary>
        /// <returns><see cref="SellAppCoupons"/></returns>
        public SellAppCoupons GetCouponsHandler()
        {
            return new SellAppCoupons(_httpRequestHelper, $"{SellAppUrl}/coupons");
        }

        /// <summary>
        /// An instance of <see cref="SellAppListings"/> is created where you can handle everything related to it.
        /// </summary>
        /// <returns><see cref="SellAppListings"/></returns>
        public SellAppListings GetListingsHandler()
        {
            return new SellAppListings(_httpRequestHelper, $"{SellAppUrl}/listings");
        }

        /// <summary>
        /// An instance of <see cref="SellAppTickets"/> is created where you can handle everything related to it.
        /// </summary>
        /// <returns><see cref="SellAppTickets"/></returns>
        public SellAppTickets GetTicketsHandler()
        {
            return new SellAppTickets(_httpRequestHelper, $"{SellAppUrl}/tickets");
        }

        /// <summary>
        /// An instance of <see cref="SellAppFeedbacks"/> is created where you can handle everything related to it.
        /// </summary>
        /// <returns><see cref="SellAppFeedbacks"/></returns>
        public SellAppFeedbacks GetFeedbackHandler()
        {
            return new SellAppFeedbacks(_httpRequestHelper, $"{SellAppUrl}/feedback");
        }

        /// <summary>
        /// An instance of <see cref="SellAppInvoices"/> is created where you can handle everything related to it.
        /// </summary>
        /// <returns><see cref="SellAppInvoices"/></returns>
        public SellAppInvoices GetInvoicesHandler()
        {
            return new SellAppInvoices(_httpRequestHelper, $"{SellAppUrl}/invoices");
        }

        /// <summary>
        /// An instance of <see cref="SellAppSections"/> is created where you can handle everything related to it.
        /// </summary>
        /// <returns><see cref="SellAppSections"/></returns>
        public SellAppSections GetSectionsHandler()
        {
            return new SellAppSections(_httpRequestHelper, $"{SellAppUrl}/sections");
        }
    }
}