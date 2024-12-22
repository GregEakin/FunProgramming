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
// Okasaki, Chris. "6.4.3 Example: Bottom-Up Mergesort with Sharing." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 74-78. Print.

using FunProgLib.lists;

namespace FunProgLib.sort;

public interface ISortable<T> where T : IComparable<T>
{
    ISortable<T> Empty { get; }

    ISortable<T> Add(T element, ISortable<T> sortable);

    FunList<T>.Node Sort(ISortable<T> sortable);
}