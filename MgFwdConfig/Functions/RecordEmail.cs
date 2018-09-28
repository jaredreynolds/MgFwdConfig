using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using MgFwdConfig.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;

namespace MgFwdConfig.Functions
{
    public class RecordEmail
    {
        [FunctionName("RecordEmail")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]HttpRequestMessage req,
            [Table("EmailFwd")] CloudTable emailFwdTable,
            ILogger log)
        {
            try
            {
                var data = await req.Content.ReadAsStringAsync();

                log.LogInformation(data);

                await emailFwdTable.CreateIfNotExistsAsync();

                var fwd = new EmailFwd("blah.com", "blah") {
                    IsActive = true,
                    ForwardTo = "bozot@clown.com",
                    Priority = 1,
                    RuleType = RuleType.ForwardToEmail
                };
                var insert = TableOperation.InsertOrReplace(fwd);
                var iresult = await emailFwdTable.ExecuteAsync(insert);
                log.LogInformation(iresult.Result.GetType().Name);

                var query = TableOperation.Retrieve<EmailFwd>("blah.com", "bozot");
                var result = await emailFwdTable.ExecuteAsync(query);
                log.LogInformation(result.Result.GetType().Name);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }

            return new OkObjectResult("");
        }
    }
}
