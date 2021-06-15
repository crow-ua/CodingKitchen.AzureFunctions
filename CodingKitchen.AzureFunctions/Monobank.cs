using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using CodingKitchen.AzureFunctions.Extensions;
using CodingKitchen.Homemoney.Clients;
using CodingKitchen.Homemoney.Models;
using CodingKitchen.Monobank.Client;
using CodingKitchen.Monobank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CodingKitchen.AzureFunctions
{
    public class Monobank
    {
        private readonly IMonobankApiClient _monobankApiClient;
        private readonly IHomemoneyApiClient _homemoneyApiClient;
        private readonly IMapper _mapper;
        private readonly string _monobankAccountId;
        private readonly string _monobankStatementElementKey;

        public Monobank(
            IMonobankApiClient monobankApiClient,
            IHomemoneyApiClient homemoneyApiClient,
            IMapper mapper,
            IConfiguration configuration)
        {
            //var apiKeySetting = Environment.GetEnvironmentVariable("ApiKey");
            _monobankApiClient = monobankApiClient;
            _homemoneyApiClient = homemoneyApiClient;
            _mapper = mapper;
            _monobankAccountId = configuration["MonobankAccountId"];
            _monobankStatementElementKey = configuration["MonobankStatementElementKey"];
        }
        
        [FunctionName("GetCurrency")]
        public async Task<IActionResult> GetCurrencyAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req, ILogger log)
        {
            var client = new HttpClient();
            var result = await client.GetStringAsync(new Uri("https://api.monobank.ua/bank/currency"));
            var currency = JsonSerializer.Deserialize<CurrencyInfo[]>(result);
            return new OkObjectResult(currency);
        }
        
        [FunctionName("GetExchangeRates")]
        public async Task<IActionResult> GetExchangeRatesAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req, ILogger log)
        {
            var rates = await _monobankApiClient.GetExchangeRates();
            return new OkObjectResult(rates);
        }
        
        [FunctionName("GetUserInfo")]
        public async Task<IActionResult> GetUserInfoAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req, ILogger log)
        {
            var myUserInfo = await _monobankApiClient.GetUserInfo();
            var uahAccount = myUserInfo.Accounts.FirstOrDefault(a => a.CurrencyCode == CurrencyCode.UAH);
            return new OkObjectResult(uahAccount);
        }
        
        [FunctionName("SyncTransactions")]
        public async Task<IActionResult> SyncTransactionsAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            var uahAccount = new Account { Id = _monobankAccountId };
            var from = new DateTime(2021, 6, 8, 0, 0, 1).ToUnixTime();
            var to = new DateTime(2021, 6, 9, 0, 0, 1).ToUnixTime();// DateTime.Now.ToUnixTime();
            var statement = await _monobankApiClient.GetPersonalStatement(
                uahAccount.Id,
                from,
                to);

            var result = new List<object>();
            foreach (var statementItem in statement)
            {
                var convertedTransaction = _mapper.Map<AccountingTransaction>(statementItem);
                var transactionSaveResult = await _homemoneyApiClient.TransactionSave(convertedTransaction);
                result.Add(new
                {
                    Transaction = convertedTransaction.Description,
                    Success = transactionSaveResult.IsSuccessStatusCode
                });
            }

            return new OkObjectResult(result);
        }
        
        [FunctionName("MonobankWebhook")]
        public async Task<IActionResult> MonobankWebhookAsync(
            [HttpTrigger(AuthorizationLevel.User, "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            var webhook = JsonSerializer.Deserialize<Webhook>(content);
            if (webhook.Type.Equals(_monobankStatementElementKey) && webhook.Data.Account.Equals(_monobankAccountId))
            {
                var convertedTransaction = _mapper.Map<AccountingTransaction>(webhook.Data.StatementItem);
                var transactionSaveResult = await _homemoneyApiClient.TransactionSave(convertedTransaction);
                return new OkObjectResult(new
                {
                    Transaction = convertedTransaction.Description,
                    Success = transactionSaveResult.IsSuccessStatusCode
                });
            }
            return new BadRequestResult();
        }
    }
}
