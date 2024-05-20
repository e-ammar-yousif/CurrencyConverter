# Currency Converter API

This is a RESTful API for currency conversion built with ASP.NET Core.

## Getting Started

To run the API locally, follow these steps:

### Prerequisites

- [.NET Core SDK 8.0](https://dotnet.microsoft.com/download)
- [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) (optional)

### Installation

1. Clone this repository to your local machine:

   ```bash
   git clone https://github.com/e-ammar-yousif/CurrencyConverter.git
2. Navigate to the project directory:
   ```bash
   cd currency-converter
3. Build the solution:
   ```bash
   dotnet build
4. Run the API:
   ```bash
   dotnet run --project CurrencyConverter
5. Open your web browser and navigate to http://localhost:5279/swagger to access the API documentation using Swagger.'
6. You can hit the api endpoints from command line using curl: 
   ```bash
   curl -X GET "http://localhost:5279/api/exchangerates/latest?baseCurrency=USD" -H "Accept: application/json"
### Usage
#### Endpoints
- GET **/api/exchangerates/latest**: Retrieve the latest exchange rates for a specific base currency.
- GET **/api/exchangerates/convert**: Convert amounts between different currencies.
- GET **/api/exchangerates/history**: Return a set of historical rates for a given period.
#### Query Parameters
- **baseCurrency**: The base currency for exchange rates or conversion.
- **quoteCurrency**: The currency to convert to.
- **amount**: The amount to convert.
- **from**: The start date for historical rates.
- **to**: The end date for historical rates.

### Contributing
Contributions are welcome! Please follow the GitHub Flow when making changes:

1. Fork the repository
2. Create a new branch (git checkout -b feature-name)
3. Commit your changes (git commit -am 'Add new feature')
4. Push to the branch (git push origin feature-name)
5. Create a new Pull Request

### Licence
This project is licensed under the MIT License - see the LICENSE file for details.