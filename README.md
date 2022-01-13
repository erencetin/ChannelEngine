# ChannelEngine

The solution consists of five projects.

**ChannelEngine.Infrastructure**
This project responsible to retrieve data from api. Has two repository classes. 
OfferRepository updates the product stock. OrderRepository gets all orders by the status.

**ChannelEngine.Core**
This layer has business logic. It includes the product service, invokes repository methods, performs the validation. Also includes the model classes and interfaces.

**ChannelEngine.Console**
Gets top products via product service and also gives an option to update the stock of selected product to 25. Does not require any arguments.

**ChannelEngine.WebUI**
This is a Blazor application. Lists top products and contains buttons in every row to update selected product stock. 

**Prerequisites**
The solution written with Visual Studio 2022. The Sln and cproj files compatible with this version of VS. 
The framework version is .NET 6.0
