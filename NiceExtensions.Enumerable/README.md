# NiceExtensions.Enumerable

## About

This contains helpful extensions for IEnumerable and IAsyncEnumerable as well as IEnumerable&lt;Task&gt; extensions for easy parallelization


## NiceExtensions.Enumerable

### ForEach() and ForEachAsync()

Self explanatory. ForEach does not need a list any more.

### Reverse()

Reverses order of an Enumerable.

### IEnumerable<KeyValuePair<T1,T2>>.ToDictionary()

Ever been annoyed that you have to write a proper expression for that? Yeah me too.

### ByMin() ByMax()

You've wanted an item by max or min value but hate iterating twice over a list and needing two lines for var max = l.Max(i =&gt; i.value) and l.First(i => i.value == max) not anymore

### ToCachedEnumerable()

Needing to iterate multiple times over an Enumerable but a .ToList() sucks D this is for you. Caches values you've already Enumerated.

### ToCachedEagerLoadingEnumerable()

You're happily enumerating over an Enumerable but after the foreach has done its work the next value isnt ready and takes as long as the work you did already? Time is money!
Enumerates in the background while doing work so values are always ready.

## NiceExtensions.Enumerable.Tasks

### CreateTasks

An easy way to generate Tasks or Func&lt;Task&gt;s out of an Enumerable.

#### Example

<code>await Directory.EnumerateFiles(@"C:\Windows\System32")<br>
.CreateTasks(f => File.Delete(f))<br>
.RunAndWhenAll()</code>

### RunAndWhenAll

One of the most powerful extensions. Easy configurable parallel execution, with parameters like maxActionsToRunInParallel, timeoutInMilliseconds (allows you to start tasks beyond the threshhold if no task is completed in an amount of time) 
 
#### Example

Delete All files in System32 10 at a time and after 1 second block start a new delete regardless.

<code>await Directory.EnumerateFiles(@"C:\Windows\System32")<br>
.CreateTasks(f => File.Delete(f))<br>
.RunAndWhenAll(10, 1000, ct)</code>

