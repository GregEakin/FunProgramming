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

using Xunit.Abstractions;

namespace FunProgTests.lamda;

public class ListOfFuncsTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ListOfFuncsTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    // return a list of funcs, where each one returns a loaded page
    static IEnumerable<Func<int>> GetEnumerable(int? page = null, int limit = 10)
    {
        var currentPage = page ?? 1;
        while (true)
        {
            for (var i = limit * (currentPage - 1); i < limit * currentPage; i++)
            {
                var i1 = i;
                yield return () =>
                {
                    Thread.Sleep(10);
                    return i1;
                };
            }
            currentPage++;
        }
    }

    [Fact]
    public void Test1()
    {
        foreach (var item in GetEnumerable().Skip(100).Take(10))
        {
            _testOutputHelper.WriteLine(item().ToString());
        }
    }
}