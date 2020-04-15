using System;

namespace DataCreationFramework.Data
{
    public partial class DefinitionOfValid<T>
    where T : class
    {
        public class DateTimeValidity : Validity
        {
            private enum DateTimeValidationTypes
            {
                Range
            }

            private DateTimeValidationTypes _validationType;

            private DateTime _datetimeFrom;
            private DateTime _datetimeTo;

            public DateTimeValidity Range(DateTime datetimeFrom, DateTime datetimeTo)
            {
                _validationType = DateTimeValidationTypes.Range;
                _datetimeFrom = datetimeFrom;
                _datetimeFrom = datetimeTo;
                return this;
            }



            public override void ApplyDataCreationStrategy(string propertyName, DataCreationStrategy<T> strategy, ViolationType violationType)
            {
                throw new NotImplementedException();
            }

        }
    }
}
