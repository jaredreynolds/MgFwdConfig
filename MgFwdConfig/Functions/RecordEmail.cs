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
using System.Linq;

namespace MgFwdConfig.Functions
{
    public static class RecordEmail
    {
        [FunctionName("RecordEmail")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]HttpRequestMessage req,
            [Table("EmailFwd")] CloudTable emailFwdTable,
            ILogger log)
        {
            string data = null;

            try
            {
                data = await req.Content.ReadAsStringAsync();
                log.LogTrace("Body: [[{data}]]", data);

                var recipient = GetRecipient(data);
                log.LogDebug("Recipient: [[{recipient}]]", recipient);

                var fwd = new EmailFwd(recipient);
                log.LogDebug("EmailFwd: [[{fwd}]]", fwd.ToString());

                var insert = TableOperation.InsertOrReplace(fwd);
                var result = await emailFwdTable.ExecuteAsync(insert);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                log.LogDebug("Body: [[{data}]]", data);
                return new StatusCodeResult(406);
            }

            return new OkObjectResult("");
        }

        private static string GetRecipient(string data)
        {
            return Uri.UnescapeDataString(
                data
                    .Split('&')
                    .First(kvp => kvp.StartsWith("recipient=", StringComparison.InvariantCultureIgnoreCase))
                    .Split('=')
                    .Skip(1)
                    .Last());
        }
    }
}
