# SellApp
Unofficial asynchronous library written in .NET Standard 2.0 that interacts with https://Sell.App/

[![GitHub Latest Release](https://img.shields.io/github/v/release/biitez/SellApp.svg)](https://github.com/biitez/SellApp/releases)
![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)

### Features:
- Built using .NET Standard 2.0
- Broad compatibility with different versions of .NET
- Very easy to use, more secure and interactive library
- The methods are commented inside the library

### [SellApp](https://github.com/biitez/SellApp/blob/master/SellApp/SellApp.cs)

- `SellApp(string AuthorizationTokenBearer, HttpClient httpClient = null)`
- `SellAppBlackList GetBlackListHandler()`
- `SellAppCoupons GetCouponsHandler()`
- `SellAppListings GetListingsHandler()`
- `SellAppTickets GetTicketsHandler()`
- `SellAppFeedbacks GetFeedbackHandler()`
- `SellAppInvoices GetInvoicesHandler()`
- `SellAppSections GetSectionsHandler()`

### [Sample Console Application](https://github.com/biitez/SellApp/blob/master/SellApp.Example/Program.cs)

Small example code:

```cs

var sellApp = new SellApp(AuthorizationTokenBearer: "Sell App Authorization Key");
// var sellAppCustomHttpClient = new SellApp(AuthorizationTokenBearer: "Sell App Authorization Key", new HttpClient());

var TicketManager = sellApp.GetTicketsHandler();

await TicketManager.Messages.ReplyTicketById(1, "Message by the admin!");

```

### Contributions, reports or suggestions
If you find a problem or have a suggestion inside this library, please let me know by [clicking here](https://github.com/biitez/SellApp/issues), if you want to improve the code, make it cleaner or even more secure, create a [pull request](https://github.com/biitez/SellApp/pulls). 

In case you will contribute in the code, please follow the same code base.

### Credits

- `Telegram: https://t.me/biitez`
- `Bitcoin Addy: bc1qzz4rghmt6zg0wl6shzaekd59af5znqhr3nxmms`
