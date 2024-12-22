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
// Okasaki, Chris. "2.3 Binaryt Search Trees." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 11-15. Print.

namespace FunProgLib.tree;

public interface IFiniteMap<TKey, T>
{
    IFiniteMap<TKey, T> Empty { get; }

    IFiniteMap<TKey, T> Bind(TKey key, T value, IFiniteMap<TKey, T> map);

    T Lookup(TKey key, IFiniteMap<TKey, T> map);  // throw NotFound if key isn't found
}