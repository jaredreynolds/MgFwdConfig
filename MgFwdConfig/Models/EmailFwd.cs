using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace MgFwdConfig.Models
{
    public class EmailFwd : TableEntity
    {
        public string ForwardTo { get; set; }
        public bool IsActive { get; set; }
        public int Priority { get; set; }
        public RuleType RuleType { get; set; }

        [IgnoreProperty] public string Domain { get { return PartitionKey; } set { PartitionKey = value; } }
        [IgnoreProperty] public string LocalPart { get { return RowKey; } set { RowKey = value; } }
        public string SentTo => $"{LocalPart}@{Domain}";

        public EmailFwd() : base() { }
        public EmailFwd(string localPart, string domain) : base(localPart, domain) { }
        public EmailFwd(string sentTo) : base()
        {
            (LocalPart, Domain) = SplitEmail(sentTo);
        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            return Flatten(this, operationContext);
        }

        public override string ToString()
        {
            return SentTo;
        }

        private (string LocalPart, string Domain) SplitEmail(string email)
        {
            var parts = email.Split('@');
            return (parts[0], parts[1]);
        }
    }
}
