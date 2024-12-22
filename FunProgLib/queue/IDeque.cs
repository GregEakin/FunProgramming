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
// Okasaki, Chris. "8.4 Double-Ended Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 106-107. Print.

namespace FunProgLib.queue;

public interface IDeque<T>
{
    IDeque<T> Empty { get; }

    bool IsEmpty(IDeque<T> queue);

    IDeque<T> Cons(T element, IDeque<T> queue);

    T Head(IDeque<T> queue);

    IDeque<T> Tail(IQueue<T> queue);

    IDeque<T> Snoc(IDeque<T> queue, T element);

    T Last(IDeque<T> queue);

    IDeque<T> Init(IQueue<T> queue);
}