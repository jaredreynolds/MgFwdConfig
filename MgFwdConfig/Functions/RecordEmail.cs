using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Threading.Tasks;
using MgFwdConfig.Models;

namespace MgFwdConfig.Functions
{
    public class RecordEmail
    {
        public static async Task<HttpResponseMessage> Run(
            HttpRequestMessage req,
            //IQueryable<EmailFwd> emailFwdTable,
            TraceWriter log)
        {
            try
            {
                var data = await req.Content.ReadAsStringAsync();

                log.Info(data);

            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
            }

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
