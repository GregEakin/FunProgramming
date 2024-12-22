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
// Okasaki, Chris. "3.1 Leftist Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 17-20. Print.

namespace FunProgLib.heap;

public interface IHeap<T> where T : IComparable<T>
{
    IHeap<T> Empty { get; }

    bool IsEmpty(IHeap<T> heap);

    IHeap<T> Insert(T value, IHeap<T> heap);

    IHeap<T> Merge(IHeap<T> a, IHeap<T> b);

    T FindMin(IHeap<T> heap);

    IHeap<T> DeleteMin(IHeap<T> heap);
}