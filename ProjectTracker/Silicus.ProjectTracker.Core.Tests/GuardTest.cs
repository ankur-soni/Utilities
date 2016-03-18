using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Silicus.ProjectTracker.Core.Tests
{
    /// <summary>
    /// Contains the unit tests for common validation functions.
    /// </summary>
    [TestClass]
    public class GuardTest
    {
        [TestMethod(), TestCategory("Bamboo")]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentNotEmpty_WhenCalledWithEmptyGuid_ThrowsException()
        {
            Guard.GuidNotEmpty(Guid.Empty, "argument");
        }

        [TestMethod(), TestCategory("Bamboo")]
        public void ArgumentNotEmpty_WhenCalledWithNonEmptyGuid_DoestNotThrowsException()
        {
            // Arrange
            var argument = Guid.NewGuid();

            // Act
            Guard.GuidNotEmpty(argument, "argument");
        }

        [TestMethod(), TestCategory("Bamboo")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNotNull_WhenCalledWithNullArgument_ThrowsException()
        {
            Guard.ArgumentNotNull(null, "argument");
        }

        [TestMethod(), TestCategory("Bamboo")]
        public void ArgumentNotNull_WhenCalledWithNonNullArgument_DoesNotThrowException()
        {
            // Arrange
            var argument = new object();

            // Act
            Guard.ArgumentNotNull(argument, "argument");
        }

        [TestMethod(), TestCategory("Bamboo")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNotNullOrEmpty_WhenCalledWithNullArgument_ThrowsException()
        {
            Guard.ArgumentNotNullOrEmpty(null, "argument");
        }

        [TestMethod(), TestCategory("Bamboo")]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentNotNullOrEmpty_WhenCalledWithEmptyArgument_ThrowsException()
        {
            // Arrange
            var argument = string.Empty;

            // Act
            Guard.ArgumentNotNullOrEmpty(argument, "argument");
        }

        [TestMethod(), TestCategory("Bamboo")]
        public void ArgumentNotNullOrEmpty_WhenCalledWithNonNullNonEmptyArgument_DoesNotThrowException()
        {
            // Arrange
            var argument = "Test String";

            // Act
            Guard.ArgumentNotNullOrEmpty(argument, "argument");
        }

        [TestMethod(), TestCategory("Bamboo")]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentMustBePositive_WhenValueIsZero_ShouldThrowException()
        {
            // Arrange
            int argument = 0;

            // Act
            Guard.ArgumentMustBePositive(argument, "argument");
        }

        [TestMethod(), TestCategory("Bamboo")]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentMustBePositive_WhenValueLessThanZero_ShouldThrowException()
        {
            // Arrange
            int argument = -1;

            // Act
            Guard.ArgumentMustBePositive(argument, "argument");
        }

        [TestMethod(), TestCategory("Bamboo")]
        public void ArgumentMustBePositive_WhenValueIsMoreThanOne_ShouldNotThrowException()
        {
            // Arrange
            int argument = 45;

            // Act
            Guard.ArgumentMustBePositive(argument, "argument");

            // Assert
            Assert.IsTrue(true, "Completed the method without an exception.");
        }

        [TestMethod(), TestCategory("Bamboo")]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentNotNullByte_WhenArgumentIsNullByte_ShouldThrowException()
        {
            // Arrange
            byte[] nullByte = new byte[0];

            // Act
            Guard.ArgumentNotNullByte(nullByte, "nullByte");
        }

        [TestMethod(), TestCategory("Bamboo")]
        public void ArgumentNotNullByte_WhenArgumentIsAPopulatedArray_ShouldNotThrowException()
        {
            // Arrange
            var bytes = new byte[] { 1, 2, 3 };

            // Act
            Guard.ArgumentNotNullByte(bytes, "bytes");

            // Assert
            Assert.IsTrue(true, "Method completed without exception.");
        }
    }
}