using System;

namespace FunProgTests.ephemeral
{
    public class DictionaryTests
    {
        protected const int threads = 20;
        protected const int count = 300;
        private readonly Random _random = new Random();

        protected string NextWord(int length)
        {
            var data = new byte[length];
            _random.NextBytes(data);
            return Convert.ToBase64String(data);
        }
    }
}