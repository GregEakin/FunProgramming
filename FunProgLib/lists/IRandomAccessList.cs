// Copyright 2014 Gregory Eakin <greg@eakin.dev>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Okasaki, Chris. "9.2.1 Binary Random-Access Lists." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 119-22. Print.

namespace FunProgLib.lists;

public interface IRandomAccessList<T>
{
    IRandomAccessList<T> Empty { get; }

    bool IsEmpty(IRandomAccessList<T> list);

    IRandomAccessList<T> Cons(T element, IRandomAccessList<T> list);

    T Head(IRandomAccessList<T> list);

    IRandomAccessList<T> Tail(IRandomAccessList<T> list);

    T Lookup(int index, IRandomAccessList<T> list);

    IRandomAccessList<T> Update(int index, T element, IRandomAccessList<T> list);
}