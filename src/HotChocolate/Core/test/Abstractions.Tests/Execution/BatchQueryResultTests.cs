using System;
using System.Threading.Tasks;
using Xunit;

namespace HotChocolate.Execution
{
    public class BatchQueryResultTests
    {
        [Fact]
        public void RegisterDisposable_Disposable_Is_Null()
        {
            // arrange
            var result = new BatchQueryResult(() => default!, null);

            // act
            void Action() => result.RegisterDisposable(null!);

            // assert
            Assert.Throws<ArgumentNullException>(Action);
        }

        [Fact]
        public async Task RegisterDisposable()
        {
            // arrange
            var result = new BatchQueryResult(() => default!, null);
            var disposable = new TestDisposable();

            // act
            result.RegisterDisposable(disposable);

            // assert
            await result.DisposeAsync();
            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public async Task RegisterDisposable_When_Result_HasAsyncSession()
        {
            // arrange
            var session = new TestAsyncDisposable();
            var result = new BatchQueryResult(() => default!, null, session: session);
            var disposable = new TestDisposable();

            // act
            result.RegisterDisposable(disposable);

            // assert
            await result.DisposeAsync();
            Assert.True(session.IsDisposed);
            Assert.True(disposable.IsDisposed);
        }
        
        public class TestDisposable : IDisposable
        {
            public bool IsDisposed { get; private set; }

            public void Dispose()
            {
                IsDisposed = true;
            }
        }

        public class TestAsyncDisposable : IAsyncDisposable
        {
            public bool IsDisposed { get; private set; }

            public ValueTask DisposeAsync()
            {
                IsDisposed = true;
                return default;
            }
        }
    }
}
