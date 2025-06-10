using NUnit.Framework;
//following line will run the scenarios in parallel, but will execute feature file in sequence.
// [assembly: Parallelizable(ParallelScope.Children)]

//following will run the feature files and all their scenarios also parallelly
[assembly: Parallelizable(ParallelScope.All)]


