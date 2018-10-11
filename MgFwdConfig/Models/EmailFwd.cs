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

        public string Domain { get { return PartitionKey; } set { PartitionKey = value; } }
        public string LocalPart { get { return RowKey; } set { RowKey = value; } }
        public string SentTo => $"{LocalPart}@{Domain}";

        public EmailFwd() : base() { }
        public EmailFwd(string localPart, string domain) : base(localPart, domain) { }

        public override string ToString()
        {
            return SentTo;
        }
    }
}
