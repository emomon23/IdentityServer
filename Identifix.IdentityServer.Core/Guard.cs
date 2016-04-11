using System;
using System.Collections.Generic;
using System.Linq;

namespace Identifix.IdentityServer
{
    /// <summary>
    /// Provides methods for easily checking preconditions in method calls.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Verifies an argument is not null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="GuardException"></exception>
        public static void IsNotNull<T>(T argument, string argumentName) where T : class
        {
            if (Equals(argument, null))
            {
                throw new GuardException($"Argument '{argumentName ?? "[unknown]"}' cannot be null.");
            }
        }

        /// <summary>
        /// Verifies that a string argument is not an empty string.
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="GuardException"></exception>
        public static void IsNotEmpty(string argument, string argumentName)
        {
            if (argument.Length == 0)
            {
                throw new GuardException($"Argument '{argumentName ?? "[unknown]"}' cannot be empty.");
            }
        }

        /// <summary>
        /// Verifies that an IEnumerable of T  is not empty.
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="GuardException"></exception>
        public static void IsNotEmpty<T>(IEnumerable<T> argument, string argumentName) where T : class
        {
            if (!argument.Any())
            {
                throw new GuardException($"Argument '{argumentName ?? "[unknown]"}' cannot be empty.");
            }
        }

        /// <summary>
        /// Verifies that a string argument is not null or an empty string.
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="GuardException"></exception>
        public static void IsNotNullOrEmpty(string argument, string argumentName)
        {
            IsNotNull(argument, argumentName);
            IsNotEmpty(argument, argumentName);
        }

        /// <summary>
        /// Verifies that an IEnumerable of T   is not null or empty.
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="GuardException"></exception>
        public static void IsNotNullOrEmpty<T>(IEnumerable<T> argument, string argumentName) where T : class
        {
            IsNotNull(argument, argumentName);
            IsNotEmpty(argument, argumentName);
        }

        /// <summary>
        /// Verifies that a string is not null empty or white space only.
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="GuardException"></exception>
        public static void IsNotNullOrWhiteSpace(string argument, string argumentName)
        {
            IsNotNull(argument, argumentName);
            if (argument.Trim().Length == 0)
            {
                throw new GuardException($"Argument '{argumentName ?? "[unknown]"}' cannot be empty or whitespace only.");
            }
        }

        /// <summary>
        /// Verifies that an argument is greater than the expected value
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="expected"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="GuardException"></exception>
        public static void IsGreaterThan<T>(T argument, T expected, string argumentName) where T : struct, IComparable<T>
        {
            if (argument.CompareTo(expected) <= 0)
            {
                throw new GuardException($"Argument '{argumentName ?? "[unknown]"}' must be greater than '{expected}' but was '{argument}'.");
            }
        }

        /// <summary>
        /// Verifies that an argument is greater than or equal to the expected value
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="expected"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="GuardException"></exception>
        public static void IsGreaterThanOrEqualTo<T>(T argument, T expected, string argumentName) where T : struct, IComparable<T>
        {
            if (argument.CompareTo(expected) < 0)
            {
                throw new GuardException($"Argument '{argumentName ?? "[unknown]"}' must be greater than or equak to '{expected}' but was '{argument}'.");
            }
        }

        /// <summary>
        /// Verifies that an argument is less than the expected value
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="expected"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="GuardException"></exception>
        public static void IsLessThan<T>(T argument, T expected, string argumentName) where T : struct, IComparable<T>
        {
            if (argument.CompareTo(expected) >= 0)
            {
                throw new GuardException($"Argument '{argumentName ?? "[unknown]"}' must be less than '{expected}' but was '{argument}'.");
            }
        }

        /// <summary>
        /// Verifies that an argument is less than or equal to the expected value
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="expected"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="GuardException"></exception>
        public static void IsLessThanOrEqualTo<T>(T argument, T expected, string argumentName) where T : struct, IComparable<T>
        {
            if (argument.CompareTo(expected) > 0)
            {
                throw new GuardException($"Argument '{argumentName ?? "[unknown]"}' must be less than or equal to '{expected}' but was '{argument}'.");
            }
        }

        /// <summary>
        /// Verifies that an argument is equal to the expected value
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="expected"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="GuardException"></exception>
        public static void IsEqualTo<T>(T argument, T expected, string argumentName) where T : struct, IComparable<T>
        {
            if (argument.CompareTo(expected) != 0)
            {
                throw new GuardException($"Argument '{argumentName ?? "[unknown]"}' must be equal to '{expected}' but was '{argument}'.");
            }
        }

        /// <summary>
        /// Verifies that an argument is not equal to the expected value
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="expected"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="GuardException"></exception>
        public static void IsNotEqualTo<T>(T argument, T expected, string argumentName) where T : struct, IComparable<T>
        {
            if (argument.CompareTo(expected) == 0)
            {
                throw new GuardException($"Argument '{argumentName ?? "[unknown]"}' must not be equal to '{expected}' but was '{argument}'.");
            }
        }

        /// <summary>
        /// Verifies that an argument is in the given range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="argumentName"></param>
        public static void IsInRange<T>(T argument, T start, T end, string argumentName) where T : struct, IComparable<T>
        {
            IsGreaterThanOrEqualTo(argument, start, argumentName);
            IsLessThanOrEqualTo(argument, end, argumentName);
        }

        /// <summary>
        /// Verifies that an argument is in the given range (excluding the start and end).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="argumentName"></param>
        public static void IsInRangeExclusive<T>(T argument, T start, T end, string argumentName) where T : struct, IComparable<T>
        {
            IsGreaterThan(argument, start, argumentName);
            IsLessThan(argument, end, argumentName);
        }

        /// <summary>
        /// Verifies that an argument is not in the given range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="argumentName"></param>
        public static void IsNotInRange<T>(T argument, T start, T end, string argumentName) where T : struct, IComparable<T>
        {
            if (argument.CompareTo(start) >= 0 && argument.CompareTo(end) <= 0)
            {
                throw new GuardException($"Argument '{argumentName ?? "[unknown]"}' must not be in the range '{start}' - '{end}' but was '{argument}'.");
            }
        }

        /// <summary>
        /// Verifies that an argument is not in the given range (excluding the start and end).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="argumentName"></param>
        public static void IsNotInRangeExclusive<T>(T argument, T start, T end, string argumentName) where T : struct, IComparable<T>
        {
            if (argument.CompareTo(start) > 0 && argument.CompareTo(end) < 0)
            {
                throw new GuardException($"Argument '{argumentName ?? "[unknown]"}' must not be in the range '{start}' - '{end}' (exclusive) but was '{argument}'.");
            }
        }

        /// <summary>
        /// Verifies the given condition is true.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        public static void IsRequiredThat(bool condition, string message)
        {
            if (!condition)
            {
                throw new GuardException(message ?? "The required expectation was not met.");
            }
        }

        /// <summary>
        /// Verifies the given condition is true.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        public static void IsTrue(bool condition, string message)
        {
            IsRequiredThat(condition, message ?? "Condition must be true.");
        }

        /// <summary>
        /// Verifies the given condition is false.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        public static void IsFalse(bool condition, string message)
        {
            IsRequiredThat(!condition, message ?? "Condition must be true.");
        }

        /// <summary>
        /// Verifies that two strings have a equal value.
        /// </summary>
        /// <param name="string1"></param>
        /// <param name="string2"></param>
        public static void StringsMatch(string string1, string string2)
        {
            IsNotNullOrWhiteSpace(string1, "string1");
            IsNotNullOrWhiteSpace(string2, "string2");
            if(!string1.Equals(string2))
            {
                throw new GuardException("The provided strings do not match.");
            }
        }

    }
}