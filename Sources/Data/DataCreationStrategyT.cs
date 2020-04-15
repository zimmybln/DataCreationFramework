using DataCreationFramework.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DataCreationFramework.Data
{
    public interface IStrategy
    {

    }


    public interface IConditionalStrategy<T>
    {
        TStrategy Then<TResult, TStrategy>(Expression<Func<T, TResult>> method, TStrategy strategy)
            where TStrategy : DataCreationStrategy<TResult>;

        TStrategy Add<TResult, TStrategy>(Expression<Func<T, TResult>> method)
                    where TStrategy : DataCreationStrategy<TResult>, new();
    }

    /// <summary>
    /// Definiert eine Vorgehen zur Erzeugung von Testdaten. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataCreationStrategy<T>
    {
        internal protected class StrategyInfo
        {
            internal string Name { get; set; }

            internal Func<T, bool> Condition { get; set; }

            internal object Strategy { get; set; }
        }

        public class ConditionalStrategy : IConditionalStrategy<T>
        {
            private StrategyInfo info;
            private Dictionary<string, StrategyInfo> dictionary;

            internal ConditionalStrategy(StrategyInfo strategyInfo, Dictionary<string, StrategyInfo> dictionary)
            {
                this.info = strategyInfo;
                this.dictionary = dictionary;
            }

            public TStrategy Then<TResult, TStrategy>(Expression<Func<T, TResult>> method, TStrategy strategy) where TStrategy : DataCreationStrategy<TResult>
            {
                string name = ExpressionsExtensions.GetMemberInfoName(method);

                var propertyInfo = typeof(T).GetProperty(name);

                if (propertyInfo.PropertyType.Equals(typeof(TResult)))
                {
                    if (strategy != null)
                    {
                        strategy.Initialize(propertyInfo);

                        info.Name = name;
                        info.Strategy = strategy;

                        dictionary.Add("_" + name, info);
                        return strategy;
                    }
                    else if (dictionary.ContainsKey(name))
                    {
                        dictionary.Remove(name);
                    }
                }
                return null;
            }

            public TStrategy Add<TResult, TStrategy>(Expression<Func<T, TResult>> method)
                    where TStrategy : DataCreationStrategy<TResult>, new()
            {
                string name = ExpressionsExtensions.GetMemberInfoName(method);

                var propertyInfo = typeof(T).GetProperty(name);

                if (propertyInfo.PropertyType.Equals(typeof(TResult)))
                {
                    TStrategy strategy = new TStrategy();
                    strategy.Initialize(propertyInfo);

                    info.Name = name;
                    info.Strategy = strategy;

                    dictionary.Add("_" + name, info);
                    return strategy;
                }
                return null;
            }
        }

        internal Dictionary<string, StrategyInfo> Strategies { get; } = new Dictionary<string, StrategyInfo>();


        public DataCreationStrategy()
        {
        }


        public virtual void Initialize(PropertyInfo property)
        {

        }

        public object this[string name]
        {
            get
            {
                if (Strategies.ContainsKey(name))
                {
                    return Strategies[name].Strategy;
                }

                return null;
            }
        }

        protected internal virtual void Reset()
        {
            foreach (var item in Strategies)
            {
                try
                {
                    var method = item.Value.Strategy.GetType().GetMethod("Reset", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (method != null)
                    {
                        method.Invoke(item.Value.Strategy, null);
                    }
                }
                catch
                {
                    //TEST environment no exception to be processed
                }
            }
        }

        /// <summary>
        /// Liefert den Wert für den generischen Typ
        /// </summary>
        public virtual T GetValue()
        {
            T item = CreateInstance();

            if (item != null)
            {
                var properties = GetProperties(item).ToList();

                // alle Elemente werden durchlaufen, die keine Bedingung haben
                foreach (PropertyInfo property in properties)
                {
                    if (Strategies.ContainsKey(property.Name))
                    {
                        var strategyInfo = Strategies[property.Name];

                        if (strategyInfo.Condition == null)
                        {
                            ApplyPropertyValueFromStrategy(item, property, strategyInfo);
                        }
                    }
                }

                // alle Strategien durchlaufen, die eine Bedingung haben
                foreach (StrategyInfo info in Strategies.Values.Where(s => s.Condition != null && s.Condition(item) == true))
                {
                    var propertyInfo = properties.FirstOrDefault(p => p.Name.Equals(info.Name));
                    ApplyPropertyValueFromStrategy(item, propertyInfo, info);
                }

                return item;

            }

            return default(T);
        }

        /// <summary>
        /// Ordnet einer Instanz des Typs einen Eigenschaftswert zu
        /// </summary>
        protected bool ApplyPropertyValueFromStrategy(T item, PropertyInfo property, StrategyInfo strategyInfo)
        {
            var method = strategyInfo?.Strategy.GetType().GetMethod("GetValue");

            if (method != null)
            {
                var value = method.Invoke(strategyInfo.Strategy, null);

                property.SetValue(item, value);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Erstellt eine neue Instanz des gesuchten Typs
        /// </summary>
        protected virtual T CreateInstance()
        {
            T item = default(T);

            try
            {
                item = (T)Activator.CreateInstance(typeof(T));
            }
            catch
            {
                throw;
            }

            return item;
        }

        /// <summary>
        /// Liefert eine Liste der Eigenschaften für die Eigenschaftswerte ermittelt werden sollen
        /// </summary>
        protected virtual IEnumerable<PropertyInfo> GetProperties(T item)
        {
            return item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite);
        }

        public ConditionalStrategy When(Func<T, bool> conditionFunc)
        {
            return new ConditionalStrategy(new StrategyInfo() { Condition = conditionFunc }, this.Strategies);
        }

        /// <summary>
        /// Fügt eine neue Strategie für die spezielle Eigenschaft hinzu.
        /// </summary>
        public TStrategy Add<TResult, TStrategy>(Expression<Func<T, TResult>> method, TStrategy strategy)
            where TStrategy : DataCreationStrategy<TResult>
        {
            if (strategy == null)
            {
                throw new ArgumentNullException(nameof(strategy));
            }

            string name = ExpressionsExtensions.GetMemberInfoName(method);

            var propertyInfo = typeof(T).GetProperty(name);

            if (propertyInfo.PropertyType.Equals(typeof(TResult)))
            {
                if (Strategies.ContainsKey(name))
                {
                    Strategies.Remove(name);
                }

                strategy.Initialize(propertyInfo);
                Strategies.Add(name, new StrategyInfo { Strategy = strategy, Name = name });
                return strategy;

            }
            return null;
        }

        /// <summary>
        /// Fügt eine neue Strategie für die spezielle Eigenschaft hinzu.
        /// </summary>
        public TStrategy Add<TResult, TStrategy>(string propertyName, TStrategy strategy)
            where TStrategy : DataCreationStrategy<TResult>
        {
            if (strategy == null)
            {
                throw new ArgumentNullException(nameof(strategy));
            }

            var propertyInfo = typeof(T).GetProperty(propertyName);

            if (propertyInfo != null && propertyInfo.PropertyType.Equals(typeof(TResult)))
            {
                if (Strategies.ContainsKey(propertyName))
                {
                    Strategies.Remove(propertyName);
                }

                strategy.Initialize(propertyInfo);
                Strategies.Add(propertyName, new StrategyInfo { Strategy = strategy, Name = propertyName });
                return strategy;

            }
            return null;
        }

        /// <summary>
        /// Fügt eine neue Strategie für die spezielle Eigenschaft hinzu.
        /// </summary>
        public TStrategy Add<TResult, TStrategy>(Expression<Func<T, TResult>> method)
            where TStrategy : DataCreationStrategy<TResult>, new()
        {
            string name = ExpressionsExtensions.GetMemberInfoName(method);

            var propertyInfo = typeof(T).GetProperty(name);

            if (propertyInfo.PropertyType.Equals(typeof(TResult)))
            {
                if (Strategies.ContainsKey(name))
                {
                    Strategies.Remove(name);
                }

                TStrategy strategy = new TStrategy();
                strategy.Initialize(propertyInfo);
                Strategies.Add(name, new StrategyInfo { Strategy = strategy, Name = name });
                return strategy;
            }
            return null;
        }


    }

}
