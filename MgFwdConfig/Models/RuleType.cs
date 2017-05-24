using System;

namespace MgFwdConfig.Models
{
    [Flags]
    public enum RuleType
    {
        Undefined = 0,
        ForwardToEmail = 1 << 0,
        ForwardToUri = 1 << 1,
        Stop = 1 << 2
    }
}