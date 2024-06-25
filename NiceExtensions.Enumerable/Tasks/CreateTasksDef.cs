namespace NiceExtensions.Enumerable.Tasks
{
    public static partial class Tasks
    {
        /// <summary>
        /// Creates a list of Task expressions that can be executed eg. tasks.ForEach(t =&gt; Task.Run(t))
        /// <para>
        /// CreateTasks(async j =&gt; await MyFunction(j))<br/>
        /// CreateTasks(j =&gt; new Task(() =&gt; {}))
        /// </para>
        /// <para>!!! WARNING: DO NOT USE THIS WITH ASYNC VOID TASKS CreateTasks(() =&gt; new Task(async () =&gt; {})) AS THEY EXECUTE WITH NO WAITING !!!</para>
        /// </summary>
        /// <param name="values">Values as task parameters</param>
        /// <param name="func">Task definition</param>
        public static IEnumerable<Func<Task<TResult>>> CreateTasks<TInput, TResult>(this IEnumerable<TInput> values, Func<TInput, Task<TResult>> func)
        {
            return values.Select(v => new Func<Task<TResult>>(() => func.Invoke(v)));
        }

        /// <summary>
        /// Creates a list of Task expressions that can be executed eg. tasks.ForEach(t =&gt; Task.Run(t))
        /// <para>
        /// CreateTasks(async j =&gt; await MyMethod(j))"<br/>
        /// CreateTasks(j =&gt; new Task(() =&gt; {}))"
        /// </para>
        /// <para>!!! WARNING: DO NOT USE THIS WITH ASYNC VOID TASKS CreateTasks(() =&gt; new Task(async () =&gt; {})) AS THEY EXECUTE WITH NO WAITING !!!</para>
        /// </summary>
        /// <param name="values">Values as task parameters</param>
        /// <param name="func">Task definition</param>
        public static IEnumerable<Func<Task>> CreateTasks<TInput>(this IEnumerable<TInput> values, Func<TInput, Task> func)
        {
            return values.Select(v => new Func<Task>(() => func.Invoke(v)));
        }

        /// <summary>
        /// Creates a list of Tasks that can be executed eg. tasks.ForEach(t =&gt; t.Start())
        /// <para>
        /// CreateTasks(j =&gt; MyFunction(j))"<br/>
        /// CreateTasks(j =&gt; { return j }))"
        /// </para>
        /// </summary>
        /// <param name="values">Values as task parameters</param>
        /// <param name="func">Task definition</param>
        public static IEnumerable<Task<TResult>> CreateTasks<TInput, TResult>(this IEnumerable<TInput> values, Func<TInput, TResult> func)
        {
            return values.Select(v => new Task<TResult>(() => func.Invoke(v)));
        }

        /// <summary>
        /// Creates a list of Tasks that can be executed
        /// <para>
        /// CreateTasks(j =&gt; MyMethod(j))"<br/>
        /// CreateTasks(j =&gt; { j++; }))"
        /// </para>
        /// </summary>
        /// <param name="values">Values as task parameters</param>
        /// <param name="expression">Task definition</param>
        public static IEnumerable<Task> CreateTasks<TInput>(this IEnumerable<TInput> values, Action<TInput> action)
        {
            return values.Select(v => new Task(() => action.Invoke(v)));
        }
    }
}
