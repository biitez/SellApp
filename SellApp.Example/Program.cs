using System;
using System.Threading.Tasks;
using SellApp.Models;
using SellApp.Models.Coupons;

namespace SellApp.Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var sellApp = new SellApp(AuthorizationTokenBearer: "Sell App Authorization Key");

            var couponsHandler = sellApp.GetCouponsHandler();            

            PagesModel<ResponsePagedModel<DataCouponModel>> CouponsCollection = await couponsHandler.GetAllCoupons().ConfigureAwait(false);

            // * It will only iterate 15 items per page
            foreach (var i in CouponsCollection.BL.data)
            {
                Console.WriteLine(i.id);
                Console.WriteLine(i.created_at);
                Console.WriteLine(i.deleted_at);                
                Console.WriteLine(i.expires_at);                
                // (...)
            }

            bool HasPreviousPage = CouponsCollection.HasPreviousPage;
            bool HasNextPage = CouponsCollection.HasNextPage;

            if (HasNextPage)
            {
                (bool Success, PagesModel<ResponsePagedModel<DataCouponModel>> Info) NextPage = await couponsHandler.NextPage();

                if (NextPage.Success) // Page 2! (Max. 15 Items)
                { 
                    foreach (var i in NextPage.Info.BL.data)
                    {
                        Console.WriteLine(i.id);
                        // (...)
                    }
                }
            }

            // Ticket Usage Example

            var TicketManager = sellApp.GetTicketsHandler();

            var AllSystemTickets = await TicketManager.GetAllTickets(); // return page 1            

            var GetTicketMessagesById = await TicketManager.Messages.GetAllTicketMessagesById(1).ConfigureAwait(false);

            await TicketManager.Messages.ReplyTicketById(1, "Message by the admin!");


            // Feedback Usage Example

            var FeedbackManager = sellApp.GetFeedbackHandler();

            var FeedBacks = await FeedbackManager.GetAllFeedbacks().ConfigureAwait(false);

            await FeedbackManager.ReplyFeedback(10, "Thank You!");


            // Sections Usage Example

            var sectionsManager = sellApp.GetSectionsHandler();

            var Section = await sectionsManager.CreateNewSection("Section Title", Hidden: false);

            Console.WriteLine("{0}: {1}",
                Section.id, Section.title);

            // For the rest it is used in a similar way, the methods are commented explaining their use!
        }
    }
}
