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
// Okasaki, Chris. "10.3.2 Trees." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 166-9. Print.

namespace FunProgLib.tree;

public static class TrieOfTree<TKey, TValue> where TKey : IComparable<TKey>
{
    // assumes polymorphic recursion!
    public sealed class Tree
    {
        // alpha
        // albha tree
        // alpha tree
    }

    public sealed class Map
    {
        // alpha option
        // alpha map map M.map
    }

    public static Map Empty { get; } = new();

    public static TValue Lookup()
    {
        throw new NotImplementedException();
    }

    public static Map Bind()
    {
        throw new NotImplementedException();
    }
}