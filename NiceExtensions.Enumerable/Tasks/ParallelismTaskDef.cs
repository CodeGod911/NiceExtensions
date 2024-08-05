namespace NiceExtensions.Enumerable.Tasks
{
	public static partial class Tasks
	{
		/// <summary>
		/// Starts the given tasks and waits for them to complete. This will run the specified number of tasks in parallel.
		/// <para>WARNING: DO NOT USE THIS WITH ASYNC VOID TASKS "new Task(async () =&gt; {})" AS THEY EXECUTE WITH NO WAITING<br/>
		///       USE OVERLOAD WITH IEnumerable&lt;Func&lt;Task&gt;&gt; Instead</para>
		/// <para>NOTE: If a timeout is reached before the Task completes, another Task may be started, potentially running more than the specified maximum allowed.</para>
		/// <para>NOTE: If one of the given tasks has already been started, an exception will be thrown.</para>
		/// </summary>
		/// <param name="tasksToRun">The tasks to run. </param>
		/// <param name="maxActionsToRunInParallel">The maximum number of tasks to run in parallel.</param>
		/// <param name="timeoutInMilliseconds">The maximum milliseconds we should allow the max tasks to run in parallel before allowing another task to start. Specify -1 to wait indefinitely.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		public static async Task RunAndWhenAll(this IEnumerable<Task> tasksToRun, int maxActionsToRunInParallel = int.MaxValue, int timeoutInMilliseconds = -1, CancellationToken cancellationToken = new CancellationToken())
		{
			// Convert to a list of tasks so that we don't enumerate over it multiple times needlessly.
			using var throttler = new SemaphoreSlim(maxActionsToRunInParallel, maxActionsToRunInParallel);

			// Have each task notify the throttler when it completes so that it decrements the number of tasks currently running.
			var postTasks = new List<Task>();


			// Start running each task.
			foreach (var taskToRun in tasksToRun)
			{
				// Increment the number of tasks currently running and wait if too many are running.
				if (cancellationToken.IsCancellationRequested) break;
				if (!await throttler.WaitAsync(timeoutInMilliseconds, CancellationToken.None)) throw new Exception("Semaphore Slim");
				if (cancellationToken.IsCancellationRequested) break;

				postTasks.Add(taskToRun.ContinueWith(tsk => throttler.Release()));
				taskToRun.Start();
			}

			// Wait for all of the provided tasks to complete.
			// We wait on the list of "post" tasks instead of the original tasks, otherwise there is a potential race condition where the throttler&amp;amp;#39;s using block is exited before some Tasks have had their "post" action completed, which references the throttler, resulting in an exception due to accessing a disposed object.
			await Task.WhenAll(postTasks);
		}

		/// <summary>
		/// Starts the given task expressions and waits for them to complete. This will run the specified number of tasks in parallel.
		/// <para>NOTE: If a timeout is reached before the Task completes, another Task may be started, potentially running more than the specified maximum allowed.</para>
		/// </summary>
		/// <param name="tasksToRun">The tasks to run. Async methods be shaped for input like "new Func&lt;Task&gt;(async () => await {your method with params})) or use CreateTasks"</param>
		/// <param name="maxActionsToRunInParallel">The maximum number of tasks to run in parallel.</param>
		/// <param name="timeoutInMilliseconds">The maximum milliseconds we should allow the max tasks to run in parallel before allowing another task to start. Specify -1 to wait indefinitely.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		public static async Task<List<Task>> RunAndWhenAll(this IEnumerable<Func<Task>> tasksToRun, int maxActionsToRunInParallel = int.MaxValue, int timeoutInMilliseconds = -1, CancellationToken cancellationToken = new CancellationToken())
		{
			// Convert to a list of tasks so that we don't enumerate over it multiple times needlessly.
			using var throttler = new SemaphoreSlim(maxActionsToRunInParallel, maxActionsToRunInParallel);

			// Have each task notify the throttler when it completes so that it decrements the number of tasks currently running.
			var postTasks = new List<Task>();
			var tasks = new List<Task>();

			// Start running each task.
			foreach (var taskToRun in tasksToRun)
			{
				// Increment the number of tasks currently running and wait if too many are running.
				if (cancellationToken.IsCancellationRequested) break;
				if (!await throttler.WaitAsync(timeoutInMilliseconds, CancellationToken.None)) throw new Exception("Semaphore Slim");
				if (cancellationToken.IsCancellationRequested) break;

				var task = Task.Run(taskToRun, cancellationToken);
				tasks.Add(task);
				postTasks.Add(task.ContinueWith(tsk => throttler.Release()));
			}

			// Wait for all of the provided tasks to complete.
			// We wait on the list of "post" tasks instead of the original tasks, otherwise there is a potential race condition where the throttler&amp;amp;#39;s using block is exited before some Tasks have had their "post" action completed, which references the throttler, resulting in an exception due to accessing a disposed object.
			await Task.WhenAll(postTasks);
			return tasks;
		}

		/// <summary>
		/// Starts the given task expressions and waits for them to complete. This will run the specified number of tasks in parallel.
		/// <para>NOTE: If a timeout is reached before the Task completes, another Task may be started, potentially running more than the specified maximum allowed.</para>
		/// </summary>
		/// <param name="tasksToRun">The tasks to run. Async methods be shaped for input like "new Func&lt;Task&gt;(async () => await {your method with params})) or use CreateTasks"</param>
		/// <param name="maxActionsToRunInParallel">The maximum number of tasks to run in parallel.</param>
		/// <param name="timeoutInMilliseconds">The maximum milliseconds we should allow the max tasks to run in parallel before allowing another task to start. Specify -1 to wait indefinitely.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		public static async Task<List<Task<TResult>>> RunAndWhenAll<TResult>(this IEnumerable<Func<Task<TResult>>> tasksToRun, int maxActionsToRunInParallel = int.MaxValue, int timeoutInMilliseconds = -1, CancellationToken cancellationToken = new CancellationToken())
		{
			// Convert to a list of tasks so that we don't enumerate over it multiple times needlessly.
			using var throttler = new SemaphoreSlim(maxActionsToRunInParallel, maxActionsToRunInParallel);

			// Have each task notify the throttler when it completes so that it decrements the number of tasks currently running.
			var postTasks = new List<Task>();
			var tasks = new List<Task<TResult>>();

			// Start running each task.
			foreach (var taskToRun in tasksToRun)
			{
				// Increment the number of tasks currently running and wait if too many are running.
				if (cancellationToken.IsCancellationRequested) break;
				if (!await throttler.WaitAsync(timeoutInMilliseconds, CancellationToken.None)) throw new Exception("Semaphore Slim");
				if (cancellationToken.IsCancellationRequested) break;

				var task = Task.Run(taskToRun, cancellationToken);
				tasks.Add(task);
				postTasks.Add(task.ContinueWith(tsk => throttler.Release()));
			}

			// Wait for all of the provided tasks to complete.
			// We wait on the list of "post" tasks instead of the original tasks, otherwise there is a potential race condition where the throttler&amp;amp;#39;s using block is exited before some Tasks have had their "post" action completed, which references the throttler, resulting in an exception due to accessing a disposed object.
			await Task.WhenAll(postTasks);
			return tasks;
		}
	}
}
