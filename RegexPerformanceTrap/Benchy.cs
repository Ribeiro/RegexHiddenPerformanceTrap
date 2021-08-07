using BenchmarkDotNet.Attributes;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexPerformanceTrap
{
    [MemoryDiagnoser]
    public class Benchy
    {
        private static readonly List<BenchmarkUser> FakeUsers = new Faker<BenchmarkUser>()
                                                                .UseSeed(420)
                                                                .RuleFor(u => u.Email, faker => faker.Person.Email)
                                                                .RuleFor(u => u.UserName, faker => faker.Person.UserName)
                                                                .Generate(5);

        private static readonly List<string> PotentialEmails = FakeUsers.Select(e => e.Email)
                                                                        .Concat(FakeUsers.Select(u => u.UserName))
                                                                        .ToList();

        private static readonly Regex _regexStatic = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
                                                     RegexOptions.IgnoreCase);

        private static readonly Regex _regexStaticCompiled = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
                                                             RegexOptions.IgnoreCase | RegexOptions.Compiled, 
                                                             TimeSpan.FromMilliseconds(250));

        [Benchmark]
        public void IsInternalMatch()
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);

            string potentialEmail = null;
            for (int i = 0; i < PotentialEmails.Count; i++)
            {
                potentialEmail = PotentialEmails[i];
                var isMatch = regex.Match(potentialEmail);
            }
        }

        [Benchmark]
        public void IsInternalMatchCompiled()
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string potentialEmail = null;
            for (int i = 0; i < PotentialEmails.Count; i++)
            {
                potentialEmail = PotentialEmails[i];
                var isMatch = regex.Match(potentialEmail);
            }
        }

        [Benchmark]
        public void IsMatchStatic()
        {
            string potentialEmail = null;
            for (int i = 0; i < PotentialEmails.Count; i++)
            {
                potentialEmail = PotentialEmails[i];
                var isMatch = _regexStatic.Match(potentialEmail);
            }
        }

        [Benchmark]
        public void IsMatchStaticCompiled()
        {
            string potentialEmail = null;
            for (int i = 0; i < PotentialEmails.Count; i++)
            {
                potentialEmail = PotentialEmails[i];
                var isMatch = _regexStaticCompiled.Match(potentialEmail);
            }
        }




    }

    internal class BenchmarkUser
    {
        public string Email { get; set; }
        public string UserName { get; set; }
    }

}
