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
// Okasaki, Chris. "5.2 Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 42-45. Print.

namespace FunProgLib.queue;

public interface IQueue<T>
{
    IQueue<T> Empty { get; }

    bool IsEmpty(IQueue<T> queue);

    IQueue<T> Snoc(IQueue<T> queue, T element);

    T Head(IQueue<T> queue);

    IQueue<T> Tail(IQueue<T> queue);
}