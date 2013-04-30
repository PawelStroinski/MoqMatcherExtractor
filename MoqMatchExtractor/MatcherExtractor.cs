using System;
using System.Linq;
using Moq;
using System.Reflection;

namespace MoqMatcherExtractor
{
    /// <summary>
    /// !!! WARNING : DANGEROUS CODE !!!
    /// This code uses reflection to get to internals of Moq.
    /// So it will stop working when something needed will be renamed/moved/removed in Moq.
    /// What you can do then:
    /// - See what breaks, how it was renamed in Moq and adjust this code. Or
    /// - Revert to previous Moq version. Or
    /// - Stop using this code and directly pass matching functions to mock.
    /// This code allows to use existing It.Is<>() written for Moq to setup non-Moq
    /// test mocks. Example: Extract(() => It.Is<Foo>(f => f.Bar == 10))
    /// </summary>
    public static class MatcherExtractor
    {
        public static Func<T, bool> Extract<T>(Func<T> match)
        {
            CheckMatch(match);
            var type = typeof(Mock).Assembly.GetType("Moq.FluentMockContext", true);
            using (var context = Activator.CreateInstance(type, true) as IDisposable)
            {
                CheckContext(context);
                var lastMatchProperty = type.GetProperty("LastMatch");
                CheckLastMatchProperty(lastMatchProperty);
                match();
                var lastMatch = lastMatchProperty.GetValue(context, null);
                CheckLastMatch(lastMatch);
                var matches = lastMatch.GetType().GetMethod("Matches",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                CheckMatches(matches);
                return value => (bool)matches.Invoke(lastMatch, new object[] { value });
            }
        }

        private static void CheckMatch<T>(Func<T> match)
        {
            if (match == null)
                throw new ArgumentException("match");
        }

        private static void CheckContext(IDisposable context)
        {
            if (context == null)
                throw new NullReferenceException("context");
        }

        private static void CheckLastMatchProperty(PropertyInfo lastMatchProperty)
        {
            if (lastMatchProperty == null)
                throw new NullReferenceException("lastMatchProperty");
        }

        private static void CheckLastMatch(object lastMatch)
        {
            if (lastMatch == null)
                throw new NullReferenceException("lastMatch");
        }

        private static void CheckMatches(MethodInfo matches)
        {
            if (matches == null)
                throw new NullReferenceException("matches");
        }
    }
}
