using System;

namespace DataCreationFramework.TestStructure
{
    public class MutableTest<TStrategy, TResult>
        where TStrategy : new()
    {

        protected TestEnvironment When(Func<TStrategy, TResult> action)
        {
            return new TestEnvironment(action);
        }


        protected class TestEnvironment
        {
            private Func<TStrategy, TResult> action;

            protected internal TestEnvironment(Func<TStrategy, TResult> action)
            {
                this.action = action;
            }

            public TestExecution RunsWith(Action<TStrategy> configureStrategy)
            {
                // Hier wird die Instanz der Strategy konfiguriert
                TStrategy strategy = new TStrategy();

                configureStrategy(strategy);

                return new TestExecution(action, strategy);
            }

            public class TestExecution
            {
                private Func<TStrategy, TResult> _action;
                private TStrategy _strategy;

                internal TestExecution(Func<TStrategy, TResult> action, TStrategy strategy)
                {
                    _action = action;
                    _strategy = strategy;
                }

                public void Then(Action<TResult> checkResult)
                {
                    TResult result = _action(_strategy);

                    checkResult(result);
                }
            }

        }
    }
}
