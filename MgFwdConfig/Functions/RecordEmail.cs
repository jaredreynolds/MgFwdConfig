using System;
using System.Net.Http;
using System.Threading.Tasks;
using MgFwdConfig.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
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
                string data = await req.Content.ReadAsStringAsync();
                log.LogDebug(data);

                var fwd = JsonConvert.DeserializeObject<EmailFwd>(data);
                log.LogDebug(fwd.ToString());

                var insert = TableOperation.InsertOrReplace(fwd);
                var result = await emailFwdTable.ExecuteAsync(insert);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new UnprocessableEntityObjectResult("");
            }

            return new OkObjectResult("");
        }
    }
}
