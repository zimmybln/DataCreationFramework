using System.Collections.Generic;

namespace DataCreationFramework.Tests.Types
{
    public class PrimaryType
    {
        public int AnIntegerValue { get; set; }

        public bool AnBooleanValue { get; set; }

        public string AnStringValue { get; set; }

        public List<SubType> SubTypes { get; set; }
    }
}
