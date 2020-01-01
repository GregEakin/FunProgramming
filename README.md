# Fun Programming
This project implements __C#__ versions of the data structures from Chris Okasaki’s book
[*Purely Functional Data Structures*](http://www.cambridge.org/catalogue/catalogue.asp?isbn=0521663504).
These are implemented as static functions, to closely resemble the book’s version.
These are meant to be used as a companion when reading the book. 
This allows one to fire up the samples, in a debugger, and see how they work. 
It also allows one to test the performance to see how they stack up with other data structures.

## From the book:
[![Purely Functional Data Structures](pictures/pfds-180x245.jpg)](https://en.wikipedia.org/wiki/Purely_functional_data_structure)
>Okasaki, Chris. *Purely Functional Data Structures*. 
>Cambridge, U.K.: Cambridge UP, 1998. Print.

## Features:
 * Abstract Data Types --&gt; templates
 * Immutable data structures --&gt; readonly data members
 * Lazy evaluation --&gt; `class Lazy<T>`
 * Polymorphic recursion --&gt; Static member functions
 * Structure dumping --&gt; Only implemented in test code
 * Recursive structures --&gt; Not supported (cut and paste for now)

## Links:
 * [Community Edition of Visual Studio (Free)](https://www.visualstudio.com/vs/community/)
 * [Git Extensions (Free)](http://gitextensions.github.io/)
 * [ReSharper, Extensions for .NET Developers](https://www.jetbrains.com/resharper/)
 * [A retrospective from Chris Okasaki](http://okasaki.blogspot.com/2008/02/ten-years-of-purely-functional-data.html)
 * [Amazon.com](https://www.amazon.com/Purely-Functional-Structures-Chris-Okasaki/dp/0521663504/)
 * [Google Books](https://books.google.com/books?id=SxPzSTcTalAC)
 * [Chris Okasaki’s original PhD dissertation](http://www.cs.cmu.edu/~rwh/theses/okasaki.pdf)

## Sample code
To update a shared [`set`](FunProgLib/heap/SplayHeap.cs) across multiple threads, 
we can use an [`Interlocked.CompareExchange()`](https://msdn.microsoft.com/en-us/library/system.threading.interlocked.exchange) 
to only update the pointer, when it hasn't changed.
```C#
var word = NextWord(10);
while (true)
{
    var workingSet = _set;
    Thread.MemoryBarrier();
    var newSet = SplayHeap<string>.Insert(word, workingSet);
    var oldSet = Interlocked.CompareExchange(ref _set, newSet, workingSet);
    if (ReferenceEquals(oldSet, workingSet))
        break;
}
```

## Author
:fire: [Greg Eakin](https://www.linkedin.com/in/gregeakin)
