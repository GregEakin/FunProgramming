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

namespace FunProgTests.ephemeral;

public class DictionaryTests
{
    protected const int Threads = 20;
    protected const int Count = 300;
    private readonly Random _random = new Random();

    protected string NextWord(int length)
    {
        var data = new byte[length];
        _random.NextBytes(data);
        return Convert.ToBase64String(data);
    }
}