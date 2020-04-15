using DataCreationFramework.Components;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataCreationFramework.Data
{
    public enum ViolationType
    {
        MaximumValue,
        MinimumValue,
        MaximumLength,
        MinimumLength,
        OppositeValue,
        Presence
    }

    public partial class DefinitionOfValid<T>
        where T : class
    {
        private DataCreationStrategy<T> _defaultCreationStrategy = null;
        private Dictionary<string, Validity> _validations = new Dictionary<string, Validity>();

        public DefinitionOfValid()
        {

        }

        public DefinitionOfValid(DataCreationStrategy<T> defaultStrategy)
        {
            _defaultCreationStrategy = defaultStrategy;
        }

        public DataCreationStrategy<T> Strategy => _defaultCreationStrategy;

        public IntegerValidity Add(Expression<Func<T, int>> method)
        {
            return Add<IntegerValidity>(ExpressionsExtensions.GetMemberInfoName(method));
        }

        public DoubleValidity Add(Expression<Func<T, double>> method)
        {
            return Add<DoubleValidity>(ExpressionsExtensions.GetMemberInfoName(method));
        }

        public BooleanValidity Add(Expression<Func<T, bool>> method)
        {
            return Add<BooleanValidity>(ExpressionsExtensions.GetMemberInfoName(method));
        }

        public StringValidity Add(Expression<Func<T, string>> method)
        {
            return Add<StringValidity>(ExpressionsExtensions.GetMemberInfoName(method));
        }

        private TValidation Add<TValidation>(string memberName)
            where TValidation : Validity, new()
        {
            TValidation validation = default(TValidation);

            if (_validations.ContainsKey(memberName))
            {
                throw new InvalidOperationException("already member");
            }

            validation = new TValidation();

            _validations.Add(memberName, validation);

            return validation;
        }

        /// <summary>
        /// Creates a violation
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="propertyAccessMethod"></param>
        /// <param name="violationType"></param>
        /// <returns></returns>
        public DataCreationStrategy<T> CreateViolation<TResult>(Expression<Func<T, TResult>> propertyAccessMethod, ViolationType violationType)
        {
            var memberName = ExpressionsExtensions.GetMemberInfoName(propertyAccessMethod);

            if (String.IsNullOrWhiteSpace(memberName))
                throw new ArgumentException(nameof(propertyAccessMethod));

            return CreateViolation(memberName, violationType);
        }


        /// <summary>
        /// Creates a violation based on the name of a property
        /// </summary>
        public DataCreationStrategy<T> CreateViolation(string propertyName, ViolationType violationType)
        {
            if (!_validations.ContainsKey(propertyName))
            {
                throw new Exception("There is no definition of valid for this property");
            }

            DataCreationStrategy<T> strategy = _defaultCreationStrategy ?? new DataCreationStrategy<T>();

            Validity validation = _validations[propertyName];

            validation.ApplyDataCreationStrategy(propertyName, strategy, violationType);

            return strategy;
        }
    }
}
