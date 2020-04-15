using System;
using System.Collections.Generic;
using System.Text;

namespace DataCreationFramework.Components
{
    public class MutableTest<TStrategy, TResult>
        where TStrategy : new()
        where TResult : MutableTestResult, new()
    {

        protected TestEnvironment<TDataPackage> When<TDataPackage>()
            where TDataPackage : TStrategy, new()
        {
            return new TestEnvironment<TDataPackage>();
        }

        protected class TestEnvironment<TDataPackage> : TestEnvironment<TDataPackage>.IEvaluation, 
                                                                    TestEnvironment<TDataPackage>.IEvaluationWithResult
            where TDataPackage : TStrategy, new()
        {

            #region Interfaces

            public interface IEvaluationWithResult
            {
                void Then(Action<TDataPackage, TResult> action);
            }

            public interface IEvaluation
            {
                void Then(Action<TDataPackage> action);
            }

            #endregion

            private Func<TDataPackage, TResult> contextAction;
            private IDataPackage dataPackage = null;
            
            protected internal TestEnvironment()
            {

            }

            public TestEnvironment<TDataPackage>.IEvaluationWithResult IsUpdatedWith(Func<TDataPackage, TResult> execution)
            {
                contextAction = execution;

                return this;
            }

            public void Then(Action<TDataPackage> executionResult)
            {

            }

            void TestEnvironment<TDataPackage>.IEvaluationWithResult.Then(Action<TDataPackage, TResult> executionResult)
            {

                TDataPackage dataPackage = default(TDataPackage);
                TResult result = default(TResult);

                try
                {
                    // create the data package
                    dataPackage = new TDataPackage();

                    if (dataPackage is IDataPackage package)
                    {
                        package.Create();
                    }

                    // optional execute
                    result = contextAction?.Invoke(dataPackage);

                    // call the resulting method
                    executionResult(dataPackage, result);
                }
                catch (Exception exception)
                {
                    executionResult(default(TDataPackage), new TResult() { Exception = exception });
                }
                finally
                {
                    if (dataPackage is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }


    }

    public abstract class MutableTestResult
    {
        public Exception Exception { get; internal set; }
    }

    public interface IDataPackage : IDisposable
    {
        void Create();
    }
}
