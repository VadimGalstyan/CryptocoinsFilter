namespace CryptocoinsFilter.Background_Tasks
{
    public class CoinsUpdate : BackgroundService
    {
        private readonly ProgramLogic program = new ProgramLogic();

        private Task? _executeTask;
        private CancellationTokenSource? _stoppingCts;

        /// <summary>
        /// Gets the Task that executes the background operation.
        /// </summary>
        /// <remarks>
        /// Will return <see langword="null"/> if the background operation hasn't started.
        /// </remarks>
        public override Task? ExecuteTask => _executeTask;

        /// <summary>
        /// This method is called when the <see cref="IHostedService"/> starts. The implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="IHostedService.StopAsync(CancellationToken)"/> is called.</param>
        /// <returns>A <see cref="Task"/> that represents the long running operations.</returns>
        /// <remarks>See <see href="https://docs.microsoft.com/dotnet/core/extensions/workers">Worker Services in .NET</see> for implementation guidelines.</remarks>
        protected override async Task  ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                UpdateStatus.IsUpdating = true;
                await program.runTask();
                UpdateStatus.IsUpdating = false;

                // wait 6 hours without blocking the server
                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            // Create linked token to allow cancelling executing task from provided token
            _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // Store the task we're executing
            _executeTask = ExecuteAsync(_stoppingCts.Token);

            // If the task is completed then return it, this will bubble cancellation and failure to the caller
            if (_executeTask.IsCompleted)
            {
                return _executeTask;
            }

            // Otherwise it's runnin
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executeTask == null)
            {
                return;
            }

            try
            {
                _stoppingCts!.Cancel();
            }
            finally
            {
                var tcs = new TaskCompletionSource<object>();
                using CancellationTokenRegistration registration = cancellationToken.Register(s => ((TaskCompletionSource<object>)s!).SetCanceled(), tcs);
                await Task.WhenAny(_executeTask, tcs.Task).ConfigureAwait(false);
            }

        }

        public override void Dispose()
        {
            _stoppingCts?.Cancel();
        }

    }
    
}