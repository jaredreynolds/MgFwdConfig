using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace MgFwdConfig.Models
{
    public class EmailFwd : TableEntity
    {
        public string ForwardTo { get; set; }
        public bool IsActive { get; set; }
        public int Priority { get; set; }
        public RuleType RuleType { get; set; }

        public string Domain => PartitionKey;
        public string LocalPart => RowKey;
        public string SentTo => $"{RowKey}@{PartitionKey}";

        public EmailFwd() : base() { }
        public EmailFwd(string localPart, string domain) : base(localPart, domain) { }
    }
}
