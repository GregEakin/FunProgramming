// Copyright 2016 Gregory Eakin <greg@eakin.dev>
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
// Okasaki, Chris. "!0.2.1 Lists With Efficient Catenation." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 153-8. Print.

namespace FunProgLib.lists;

public interface ICatenableList<T>
{
    ICatenableList<T> Empty { get; }
    bool IsEmpty(ICatenableList<T> list);

    ICatenableList<T> Cons(T element, ICatenableList<T> list);
    ICatenableList<T> Snoc(ICatenableList<T> list, T element);
    ICatenableList<T> Cat(ICatenableList<T> list1, ICatenableList<T> list2);

    T Head(ICatenableList<T> list);
    ICatenableList<T> Tail(ICatenableList<T> list);
}