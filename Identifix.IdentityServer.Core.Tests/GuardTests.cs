using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Identifix.IdentityServer.Tests
{
    [TestClass]
    public class GuardTests
    {
        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestCleanup]
        public void CleanUp()
        {
            
        }

        [TestMethod]
        public void IsNull_WhenargumentIsNull_ThrowsWithExpectedMessage()
        {
            Exception result = AssertThrows<GuardException>(() => Guard.IsNotNull<string>(null, "argumentName"));
            Assert.AreEqual("Argument 'argumentName' cannot be null.", result.Message);
        }

        [TestMethod]
        public void IsNull_WhenargumentIsNotNull_DoesNotThrowException()
        {
            AssertDoesNotThrow(() => Guard.IsNotNull("Valid", "argumentName"));
        }

        [TestMethod]
        public void IsNotEmpty_WhenStringIsEmpty_ThrowsWithExpectedMessage()
        {
            Exception result = AssertThrows<GuardException>(() => Guard.IsNotEmpty(string.Empty, "argumentName"));
            Assert.AreEqual("Argument 'argumentName' cannot be empty.", result.Message);
        }

        [TestMethod]
        public void IsNotEmpty_WhenStringIsNotEmpty_DoesNotThrowException()
        {
            AssertDoesNotThrow(() => Guard.IsNotEmpty("Argument", "argumentName"));
        }

        [TestMethod]
        public void IsNotEmpty_WhenEnumerableIsEmpty_ThrowsWithExpectedMessage()
        {
            Exception result = AssertThrows<GuardException>(() => Guard.IsNotEmpty(new string[0], "argumentName"));
            Assert.AreEqual("Argument 'argumentName' cannot be empty.", result.Message);
        }

        [TestMethod]
        public void IsNotEmpty_WhenEnumerableIsNotEmpty_DoesNotThrow()
        {
            AssertDoesNotThrow(() => Guard.IsNotEmpty(new[] { "data" }, "argumentName"));
        }

        [TestMethod]
        public void IsNotNullOrEmpty_WhenStringIsNull_ThrowsWithExpectedMessage()
        {
            Exception result = AssertThrows<GuardException>(() => Guard.IsNotNullOrEmpty(null, "argumentName"));
            Assert.AreEqual("Argument 'argumentName' cannot be null.", result.Message);
        }

        [TestMethod]
        public void IsNotNullOrEmpty_WhenStringIsEmpty_ThrowsWithExpectedMessage()
        {
            Exception result = AssertThrows<GuardException>(() => Guard.IsNotNullOrEmpty(string.Empty, "argumentName"));
            Assert.AreEqual("Argument 'argumentName' cannot be empty.", result.Message);
        }

        [TestMethod]
        public void IsNotNullOrEmpty_WhenStringIsNotNullOrEmpty_DoesNotThrow()
        {
            AssertDoesNotThrow(() => Guard.IsNotNullOrEmpty("Argument", "argumentName"));
        }

        [TestMethod]
        public void IsNotNullOrEmpty_WhenEnumerableIsNull_ThrowsWithExpectedMessage()
        {
            Exception result = AssertThrows<GuardException>(() => Guard.IsNotNullOrEmpty<string[]>(null, "argumentName"));
            Assert.AreEqual("Argument 'argumentName' cannot be null.", result.Message);
        }

        [TestMethod]
        public void IsNotNullOrEmpty_WhenEnumerableIsEmpty_ThrowsWithExpectedMessage()
        {
            Exception result = AssertThrows<GuardException>(() => Guard.IsNotNullOrEmpty(new string[0], "argumentName"));
            Assert.AreEqual("Argument 'argumentName' cannot be empty.", result.Message);
        }

        [TestMethod]
        public void IsNotNullOrEmpty_WhenEnumerableIsNotNullOrEmpty_DoesNotThrow()
        {
            AssertDoesNotThrow(() => Guard.IsNotNullOrEmpty(new[] { "item1", "item2" }, "argumentName"));
        }

        [TestMethod]
        public void IsNotNullOrWhiteSpace_WhenIsNull_ThrowsWithExpectedException()
        {
            Exception result = AssertThrows<GuardException>(() => Guard.IsNotNullOrWhiteSpace(null, "argumentName"));
            Assert.AreEqual("Argument 'argumentName' cannot be null.", result.Message);
        }

        [TestMethod]
        public void IsNotNullOrWhiteSpace_WhenStringIsEmpty_ThrowsWithExpectedMessage()
        {
            Exception result = AssertThrows<GuardException>(() => Guard.IsNotNullOrWhiteSpace(string.Empty, "argumentName"));
            Assert.AreEqual("Argument 'argumentName' cannot be empty or whitespace only.", result.Message);
        }

        [TestMethod]
        public void IsNotNullOrWhiteSpace_WhenStringIsWhitespace_ThrowsWithExpectedMessage()
        {
            Exception result = AssertThrows<GuardException>(() => Guard.IsNotNullOrWhiteSpace(string.Empty, "argumentName"));
            Assert.AreEqual("Argument 'argumentName' cannot be empty or whitespace only.", result.Message);
        }

        [TestMethod]
        public void IsNotNullOrWhiteSpace_WhenStringIsNotNullOrWhitespace_DoesNotThrow()
        {
            AssertDoesNotThrow(() => Guard.IsNotNullOrWhiteSpace("Argument", "argumentName"));
        }

        [TestMethod]
        public void IsGreaterThan_WhenargumentIsGreaterThanExpected_DoesNotThrowException()
        {
            const int argument = 4;
            const int expected = 2;
            const string name = "argument";
            AssertDoesNotThrow(() => Guard.IsGreaterThan(argument, expected, name));
        }

        [TestMethod]
        public void IsGreaterThan_WhenargumentIsLessThanExpected_ThrowsException()
        {
            DateTime argument = new DateTime(2000, 1, 1);
            DateTime expected = new DateTime(2012, 1, 1);
            const string name = "argument";
            AssertThrows<GuardException>(() => Guard.IsGreaterThan(argument, expected, name));
        }

        [TestMethod]
        public void IsGreaterThan_WhenargumentIsEqualToExpected_ThrowsException()
        {
            const float argument = 2.0f;
            const float expected = 2.0f;
            const string name = "argument";
            AssertThrows<GuardException>(() => Guard.IsGreaterThan(argument, expected, name));
        }

        [TestMethod]
        public void IsGreaterThan_WhenExceptionIsThrownAndNameIsNotNull_ExceptionMessageContainsargumentName()
        {
            DateTime argument = new DateTime(2000, 1, 1);
            DateTime expected = new DateTime(2012, 1, 1);
            const string name = "argument";
            Exception resut = AssertThrows<GuardException>(() => Guard.IsGreaterThan(argument, expected, name));
            Assert.IsTrue(resut.Message.Contains(name));
        }

        [TestMethod]
        public void IsGreaterThan_WhenExceptionIsThrownAndNameIsNull_ExceptionMessageContainsUnknownargument()
        {
            DateTime argument = new DateTime(2000, 1, 1);
            DateTime expected = new DateTime(2012, 1, 1);
            const string name = null;
            Exception result = AssertThrows<GuardException>(() => Guard.IsGreaterThan(argument, expected, name));
            Assert.IsTrue(result.Message.Contains("[unknown]"));
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_WhenargumentIsGreaterThanExpected_DoesNotThrowException()
        {
            const int argument = 4;
            const int expected = 2;
            const string name = "argument";
            AssertDoesNotThrow(() => Guard.IsGreaterThanOrEqualTo(argument, expected, name));
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_WhenargumentIsLessThanExpected_ThrowsException()
        {
            DateTime argument = new DateTime(2000, 1, 1);
            DateTime expected = new DateTime(2012, 1, 1);
            const string name = "argument";
            AssertThrows<GuardException>(() => Guard.IsGreaterThanOrEqualTo(argument, expected, name));
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_WhenargumentIsEqualToExpected_DoesNotThrowException()
        {
            const float argument = 2.0f;
            const float expected = 2.0f;
            const string name = "argument";
            AssertDoesNotThrow(() => Guard.IsGreaterThanOrEqualTo(argument, expected, name));
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_WhenExceptionIsThrownAndNameIsNotNull_ExceptionMessageContainsargumentName()
        {
            DateTime argument = new DateTime(2000, 1, 1);
            DateTime expected = new DateTime(2012, 1, 1);
            const string name = "argument";
            Exception result = AssertThrows<GuardException>(() => Guard.IsGreaterThanOrEqualTo(argument, expected, name));
            Assert.IsTrue(result.Message.Contains(name));
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_WhenExceptionIsThrownAndNameIsNull_ExceptionMessageContainsUnknownargument()
        {
            DateTime argument = new DateTime(2000, 1, 1);
            DateTime expected = new DateTime(2012, 1, 1);
            const string name = null;
            Exception result = AssertThrows<GuardException>(() => Guard.IsGreaterThan(argument, expected, name));
            Assert.IsTrue(result.Message.Contains("[unknown]"));
        }

        [TestMethod]
        public void IsLessThan_WhenargumentIsLessThanExpected_DoesNotThrowException()
        {
            const int argument = 2;
            const int expected = 4;
            const string name = "argument";
            AssertDoesNotThrow(() => Guard.IsLessThan(argument, expected, name));
        }

        [TestMethod]
        public void IsLessThan_WhenargumentIsGreaterThanExpected_ThrowsException()
        {
            DateTime argument = new DateTime(2012, 1, 1);
            DateTime expected = new DateTime(2000, 1, 1);
            const string name = "argument";
            AssertThrows<GuardException>(() => Guard.IsLessThan(argument, expected, name));
        }

        [TestMethod]
        public void IsLessThan_WhenargumentIsEqualToExpected_ThrowsException()
        {
            const float argument = 2.0f;
            const float expected = 2.0f;
            const string name = "argument";
            AssertThrows<GuardException>(() => Guard.IsLessThan(argument, expected, name));
        }

        [TestMethod]
        public void IsLessThan_WhenExceptionIsThrownAndNameIsNotNull_ExceptionMessageContainsargumentName()
        {
            DateTime argument = new DateTime(2012, 1, 1);
            DateTime expected = new DateTime(2000, 1, 1);
            const string name = "argument";
            Exception result = AssertThrows<GuardException>(() => Guard.IsLessThan(argument, expected, name));
            Assert.IsTrue(result.Message.Contains(name));
        }

        [TestMethod]
        public void IsLessThan_WhenExceptionIsThrownAndNameIsNull_ExceptionMessageContainsUnknownargument()
        {
            DateTime argument = new DateTime(2012, 1, 1);
            DateTime expected = new DateTime(2000, 1, 1);
            const string name = null;
            Exception result = AssertThrows<GuardException>(() => Guard.IsLessThan(argument, expected, name));
            Assert.IsTrue(result.Message.Contains("[unknown]"));
        }

        [TestMethod]
        public void IsLessThanOrEqualTo_WhenargumentIsLessThanExpected_DoesNotThrowException()
        {
            const int argument = 2;
            const int expected = 4;
            const string name = "argument";
            AssertDoesNotThrow(() => Guard.IsLessThanOrEqualTo(argument, expected, name));
        }

        [TestMethod]
        public void IsLessThanOrEqualTo_WhenargumentIsGreaterThanExpected_ThrowsException()
        {
            DateTime argument = new DateTime(2012, 1, 1);
            DateTime expected = new DateTime(2000, 1, 1);
            const string name = "argument";
            AssertThrows<GuardException>(() => Guard.IsLessThanOrEqualTo(argument, expected, name));
        }

        [TestMethod]
        public void IsLessThanOrEqualTo_WhenargumentIsEqualToExpected_DoesNotThrowException()
        {
            const float argument = 2.0f;
            const float expected = 2.0f;
            const string name = "argument";
            AssertDoesNotThrow(() => Guard.IsLessThanOrEqualTo(argument, expected, name));
        }

        [TestMethod]
        public void IsLessThanOrEqualTo_WhenExceptionIsThrownAndNameIsNotNull_ExceptionMessageContainsargumentName()
        {
            DateTime argument = new DateTime(2012, 1, 1);
            DateTime expected = new DateTime(2000, 1, 1);
            const string name = "argument";
            Exception result = AssertThrows<GuardException>(() => Guard.IsLessThanOrEqualTo(argument, expected, name));
            Assert.IsTrue(result.Message.Contains(name));
        }

        [TestMethod]
        public void IsLessThanOrEqualTo_WhenExceptionIsThrownAndNameIsNull_ExceptionMessageContainsUnknownargument()
        {
            DateTime argument = new DateTime(2012, 1, 1);
            DateTime expected = new DateTime(2000, 1, 1);
            const string name = null;
            Exception result = AssertThrows<GuardException>(() => Guard.IsLessThan(argument, expected, name));
            Assert.IsTrue(result.Message.Contains("[unknown]"));
        }

        [TestMethod]
        public void IsEqualTo_WhenargumentAndExpectedAreEqual_DoesNotThrowException()
        {
            const int argument = 2;
            const int expected = 2;
            const string name = "argument";
            AssertDoesNotThrow(() => Guard.IsEqualTo(argument, expected, name));
        }

        [TestMethod]
        public void IsEqualTo_WhenargumentAndExpectedAreNotEqual_ThrowsException()
        {
            const decimal argument = 2.00M;
            const decimal expected = 3.00M;
            const string name = "argument";
            AssertThrows<GuardException>(() => Guard.IsEqualTo(argument, expected, name));
        }

        [TestMethod]
        public void IsEqualTo_WhenExceptionIsThrownAndargumentNameIsNotNull_ExceptionMessageContainsargumentName()
        {
            const decimal argument = 2.00M;
            const decimal expected = 3.00M;
            const string name = "argument";
            Exception result = AssertThrows<GuardException>(() => Guard.IsEqualTo(argument, expected, name));
            Assert.IsTrue(result.Message.Contains(name));
        }

        [TestMethod]
        public void IsEqualTo_WhenExceptionIsThrownAndargumentNameIsNull_ExceptionMessageContainsUnknownargument()
        {
            const decimal argument = 2.00M;
            const decimal expected = 3.00M;
            const string name = null;
            Exception result = AssertThrows<GuardException>(() => Guard.IsEqualTo(argument, expected, name));
            Assert.IsTrue(result.Message.Contains("[unknown]"));
        }

        [TestMethod]
        public void IsNotEqualTo_WhenargumentAndExpectedAreNotEqual_DoesNotThrowException()
        {
            const int argument = 2;
            const int expected = 3;
            const string name = "argument";
            AssertDoesNotThrow(() => Guard.IsNotEqualTo(argument, expected, name));
        }

        [TestMethod]
        public void IsNotEqualTo_WhenargumentAndExpectedAreEqual_ThrowsException()
        {
            const decimal argument = 2.00M;
            const decimal expected = 2.00M;
            const string name = "argument";
            AssertThrows<GuardException>(() => Guard.IsNotEqualTo(argument, expected, name));
        }

        [TestMethod]
        public void IsNotEqualTo_WhenExceptionIsThrownAndargumentNameIsNotNull_ExceptionMessageContainsargumentName()
        {
            const decimal argument = 2.00M;
            const decimal expected = 2.00M;
            const string name = "argument";
            Exception result = AssertThrows<GuardException>(() => Guard.IsNotEqualTo(argument, expected, name));
            Assert.IsTrue(result.Message.Contains(name));
        }

        [TestMethod]
        public void IsNotEqualTo_WhenExceptionIsThrownAndargumentNameIsNull_ExceptionMessageContainsUnknownargument()
        {
            const decimal argument = 2.00M;
            const decimal expected = 2.00M;
            const string name = null;
            Exception result = AssertThrows<GuardException>(() => Guard.IsNotEqualTo(argument, expected, name));
            Assert.IsTrue(result.Message.Contains("[unknown]"));
        }

        [TestMethod]
        public void IsInRange_WhenargumentIsInTheSpecifiedRange_DoesNotThrowException()
        {
            const int argument = 5;
            const int start = 3;
            const int end = 7;
            const string name = "argument";
            AssertDoesNotThrow(() => Guard.IsInRange(argument, start, end, name));
        }

        [TestMethod]
        public void IsInRange_WhenargumentEqualsStartOfRange_DoesNotThrowException()
        {
            const int argument = 3;
            const int start = 3;
            const int end = 7;
            const string name = "argument";
            AssertDoesNotThrow(() => Guard.IsInRange(argument, start, end, name));
        }

        [TestMethod]
        public void IsInRange_WhenargumentEqualsEndOfRange_DoesNotThrowException()
        {
            const int argument = 7;
            const int start = 3;
            const int end = 7;
            const string name = "argument";
            AssertDoesNotThrow(() => Guard.IsInRange(argument, start, end, name));
        }

        [TestMethod]
        public void IsInRange_WhenargumentIsNotInTheSpecifiedRange_ThrowsException()
        {
            const int argument = 5;
            const int start = 10;
            const int end = 7;
            const string name = "argument";
            AssertThrows<GuardException>(() => Guard.IsInRange(argument, start, end, name));
        }

        [TestMethod]
        public void IsInRange_WhenExceptionIsThrownAndargumentNameIsNotNull_ExceptionMessageContainsName()
        {
            const int argument = 5;
            const int start = 10;
            const int end = 7;
            const string name = "argument";
            Exception result = AssertThrows<GuardException>(() => Guard.IsInRange(argument, start, end, name));
            Assert.IsTrue(result.Message.Contains(name));
        }

        [TestMethod]
        public void IsInRange_WhenExceptionIsThrownAndargumentNameIsNull_ExceptionMessageContainsUnknownargument()
        {
            const int argument = 5;
            const int start = 10;
            const int end = 7;
            const string name = null;
            Exception result = AssertThrows<GuardException>(() => Guard.IsInRange(argument, start, end, name));
            Assert.IsTrue(result.Message.Contains("[unknown]"));
        }

        [TestMethod]
        public void IsInRangeExclusive_WhenargumentIsInTheSpecifiedRange_DoesNotThrowException()
        {
            const int argument = 5;
            const int start = 3;
            const int end = 7;
            const string name = "argument";
            AssertDoesNotThrow(() => Guard.IsInRangeExclusive(argument, start, end, name));
        }

        [TestMethod]
        public void IsInRangeExclusive_WhenargumentEqualsStartOfRange_ThrowsException()
        {
            const int argument = 3;
            const int start = 3;
            const int end = 7;
            const string name = "argument";
            AssertThrows<GuardException>(() => Guard.IsInRangeExclusive(argument, start, end, name));
        }

        [TestMethod]
        public void IsInRangeExclusive_WhenargumentEqualsEndOfRange_ThrowsException()
        {
            const int argument = 7;
            const int start = 3;
            const int end = 7;
            const string name = "argument";
            AssertThrows<GuardException>(() => Guard.IsInRangeExclusive(argument, start, end, name));
        }

        [TestMethod]
        public void IsInRangeExclusive_WhenargumentIsNotInTheSpecifiedRange_ThrowsException()
        {
            const int argument = 5;
            const int start = 10;
            const int end = 7;
            const string name = "argument";
            AssertThrows<GuardException>(() => Guard.IsInRangeExclusive(argument, start, end, name));
        }

        [TestMethod]
        public void IsInRangeExclusive_WhenExceptionIsThrownAndargumentNameIsNotNull_ExceptionMessageContainsName()
        {
            const int argument = 5;
            const int start = 10;
            const int end = 7;
            const string name = "argument";
            Exception result = AssertThrows<GuardException>(() => Guard.IsInRangeExclusive(argument, start, end, name));
            Assert.IsTrue(result.Message.Contains(name));
        }

        [TestMethod]
        public void IsInRangeExclusive_WhenExceptionIsThrownAndargumentNameIsNull_ExceptionMessageContainsUnknownargument()
        {
            const int argument = 5;
            const int start = 10;
            const int end = 7;
            const string name = null;
            Exception result = AssertThrows<GuardException>(() => Guard.IsInRangeExclusive(argument, start, end, name));
            Assert.IsTrue(result.Message.Contains("[unknown]"));
        }

        [TestMethod]
        public void IsNotInRange_WhenargumentIsInTheSpecifiedRange_ThrowsException()
        {
            const int argument = 5;
            const int start = 3;
            const int end = 7;
            const string name = "argument";
            AssertThrows<GuardException>(() => Guard.IsNotInRange(argument, start, end, name));
        }

        [TestMethod]
        public void IsNotInRange_WhenargumentEqualsStartOfRange_ThrowsException()
        {
            const int argument = 3;
            const int start = 3;
            const int end = 7;
            const string name = "argument";
            AssertThrows<GuardException>(() => Guard.IsNotInRange(argument, start, end, name));
        }

        [TestMethod]
        public void IsNotInRange_WhenargumentEqualsEndOfRange_ThrowsException()
        {
            const int argument = 7;
            const int start = 3;
            const int end = 7;
            const string name = "argument";
            AssertThrows<GuardException>(() => Guard.IsNotInRange(argument, start, end, name));
        }

        [TestMethod]
        public void IsNotInRange_WhenargumentIsNotInTheSpecifiedRange_DoesNotThrowException()
        {
            const int argument = 5;
            const int start = 10;
            const int end = 7;
            const string name = "argument";
            AssertDoesNotThrow(() => Guard.IsNotInRange(argument, start, end, name));
        }

        [TestMethod]
        public void IsNotInRange_WhenExceptionIsThrownAndargumentNameIsNotNull_ExceptionMessageContainsName()
        {
            const int argument = 8;
            const int start = 5;
            const int end = 10;
            const string name = "argument";
            Exception result = AssertThrows<GuardException>(() => Guard.IsNotInRange(argument, start, end, name));
            Assert.IsTrue(result.Message.Contains(name));
        }

        [TestMethod]
        public void IsNotInRange_WhenExceptionIsThrownAndargumentNameIsNull_ExceptionMessageContainsUnknownargument()
        {
            const int argument = 8;
            const int start = 5;
            const int end = 10;
            const string name = null;
            Exception result = AssertThrows<GuardException>(() => Guard.IsNotInRange(argument, start, end, name));
            Assert.IsTrue(result.Message.Contains("[unknown]"));
        }

        [TestMethod]
        public void IsNotInRangeExclusive_WhenargumentIsInTheSpecifiedRange_ThrowsException()
        {
            const int argument = 5;
            const int start = 3;
            const int end = 7;
            const string name = "argument";
            AssertThrows<GuardException>(() => Guard.IsNotInRangeExclusive(argument, start, end, name));
        }

        [TestMethod]
        public void IsNotInRangeExclusive_WhenargumentEqualsStartOfRange_DoesNotThrowException()
        {
            const int argument = 3;
            const int start = 3;
            const int end = 7;
            const string name = "argument";
            AssertDoesNotThrow(() => Guard.IsNotInRangeExclusive(argument, start, end, name));
        }

        [TestMethod]
        public void IsNotInRangeExclusive_WhenargumentEqualsEndOfRange_DoesNotThrowException()
        {
            const int argument = 7;
            const int start = 3;
            const int end = 7;
            const string name = "argument";
            AssertDoesNotThrow(() => Guard.IsNotInRangeExclusive(argument, start, end, name));
        }

        [TestMethod]
        public void IsNotInRangeExclusive_WhenargumentIsNotInTheSpecifiedRange_DoesNotThrowException()
        {
            const int argument = 5;
            const int start = 10;
            const int end = 7;
            const string name = "argument";
            AssertDoesNotThrow(() => Guard.IsNotInRangeExclusive(argument, start, end, name));
        }

        [TestMethod]
        public void IsNotInRangeExclusive_WhenExceptionIsThrownAndargumentNameIsNotNull_ExceptionMessageContainsName()
        {
            const int argument = 8;
            const int start = 5;
            const int end = 10;
            const string name = "argument";
            Exception result = AssertThrows<GuardException>(() => Guard.IsNotInRangeExclusive(argument, start, end, name));
            Assert.IsTrue(result.Message.Contains(name));
        }

        [TestMethod]
        public void IsNotInRangeExclusive_WhenExceptionIsThrownAndargumentNameIsNull_ExceptionMessageContainsUnknownargument()
        {
            const int argument = 8;
            const int start = 5;
            const int end = 10;
            const string name = null;
            Exception result = AssertThrows<GuardException>(() => Guard.IsNotInRangeExclusive(argument, start, end, name));
            Assert.IsTrue(result.Message.Contains("[unknown]"));
        }

        [TestMethod]
        public void IsRequiredThat_WhenConditionIsTrue_DoesNotThrowException()
        {
            const string message = "Custom Exception Message";
            AssertDoesNotThrow(() => Guard.IsRequiredThat(true, message));
        }

        [TestMethod]
        public void IsRequiredThat_WhenConditionIsFalse_ThrowsException()
        {
            const string message = "Custom Exception Message";
            AssertThrows<GuardException>(() => Guard.IsRequiredThat(false, message));
        }

        [TestMethod]
        public void IsRequiredThat_WhenExceptionThrowsAndMessageIsNotNull_ExceptionMessageIsMessage()
        {
            const string message = "Custom Exception Message";
            Exception result = AssertThrows<GuardException>(() => Guard.IsRequiredThat(false, message));
            Assert.AreEqual(result.Message, message);
        }

        [TestMethod]
        public void IsRequiredThat_WhenExceptionThrownAndMessageIsNull_ExceptionMessageIsRequirementFailed()
        {
            const string message = null;
            const string expectedMessage = "The required expectation was not met.";
            Exception exception = AssertThrows<GuardException>(() => Guard.IsRequiredThat(false, message));
            Assert.AreEqual(exception.Message, expectedMessage);
        }

        [TestMethod]
        public void IsTrue_WhenConditionIsFalse_ThrowsException()
        {
            AssertThrows<GuardException>(() => Guard.IsTrue(false, "Custom Message"));
        }

        [TestMethod]
        public void IsTrue_WhenCondition_IsTrue_DoesNotThrowException()
        {
            AssertDoesNotThrow(() => Guard.IsTrue(true, "Custom Message"));
        }

        [TestMethod]
        public void IsTrue_WhenExceptionIsThrownAndMessageIsNotNull_ExceptionMessageIsMessage()
        {
            const string message = "Custom Message";
            Exception result = AssertThrows<GuardException>(() => Guard.IsTrue(false, message));
            Assert.AreEqual(message, result.Message);
        }

        [TestMethod]
        public void IsTrue_WhenExceptionIsThrownAndMessageIsNotNull_ExceptionMessageIsMustBeTrue()
        {
            Exception result = AssertThrows<GuardException>(() => Guard.IsTrue(false, null));
            Assert.AreEqual("Condition must be true.", result.Message);
        }

        [TestMethod]
        public void IsFalse_WhenConditionIsTrue_ThrowsException()
        {
            AssertThrows<GuardException>(() => Guard.IsFalse(true, "Custom Message"));
        }

        [TestMethod]
        public void IsFalse_WhenCondition_IsFalse_DoesNotThrowException()
        {
            AssertDoesNotThrow(() => Guard.IsFalse(false, "Custom Message"));
        }

        [TestMethod]
        public void IsFalse_WhenExceptionIsThrownAndMessageIsNotNull_ExceptionMessageIsMessage()
        {
            const string message = "Custom Message";
            Exception result = AssertThrows<GuardException>(() => Guard.IsFalse(true, message));
            Assert.AreEqual(message, result.Message);
        }

        [TestMethod]
        public void IsFalse_WhenExceptionIsThrownAndMessageIsNotNull_ExceptionMessageIsMustBeTrue()
        {
            Exception result = AssertThrows<GuardException>(() => Guard.IsFalse(true, null));
            Assert.AreEqual("Condition must be true.", result.Message);
        }

        [TestMethod]        
        public void StringsMatch_MatchingStrings()
        {
            string string1 = "String_Matching_abc123";
            string string2 = "String_Matching_abc123";

            Assert.AreEqual(string1, string2);
            Guard.StringsMatch(string1, string2);            
        }

        [TestMethod]
        [ExpectedException(typeof(GuardException))]
        public void StringsMatch_NonMatchingStrings()
        {
            string string1 = "String_1";
            string string2 = "Non matching String_2";

            Assert.AreNotEqual(string1, string2);
            Guard.StringsMatch(string1, string2);
        }
   
        public TException AssertThrows<TException>(Action action) where TException: Exception
        {
            TException exception = null;
            try
            {
                action.Invoke();
            }
            catch (TException tex)
            {
                exception = tex;
            }
            Assert.IsNotNull(exception);
            return exception;
        }

        public void AssertDoesNotThrow(Action action)
        {
            Exception exception = null;
            try
            {
                action.Invoke();
            }
            catch(Exception ex)
            {
                exception = ex;
            }
            Assert.IsNull(exception, $"An unexpected exception occurred. '{exception?.Message}'");
        }
    }
}