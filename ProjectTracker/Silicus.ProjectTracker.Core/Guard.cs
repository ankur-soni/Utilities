using System;
using System.Linq;

namespace Silicus.ProjectTracker.Core
{
    /// <summary>
    /// This class contains the common validation Null patterns.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Checks an argument is empty or not for Guid type.
        /// </summary>
        /// <param name="argumentValue">A Guid value.</param>
        /// <param name="argumentName">The name of the argument.</param>
        public static void GuidNotEmpty([ValidatedNotNullAttribute] Guid argumentValue, string argumentName)
        {
            if (Guid.Empty == argumentValue)
            {
                throw new ArgumentException("An empty argument is not allowed.", argumentName);
            }
        }

        /// <summary>
        /// Checks an argument to ensure it isn't null
        /// </summary>
        /// <param name="argumentValue">The argument value to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        public static void ArgumentNotNull([ValidatedNotNullAttribute] object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// Checks a string argument to ensure it isn't null or empty
        /// </summary>
        /// <param name="argumentValue">The argument value to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <exception cref="ArgumentNullException">Throw if the argument is null</exception>
        /// <exception cref="ArgumentException">Throw if the argument is empty</exception>
        public static void ArgumentNotNullOrEmpty([ValidatedNotNullAttribute] string argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }

            if (string.IsNullOrEmpty(argumentValue))
            {
                throw new ArgumentException("Empty string", argumentName);
            }
        }

        /// <summary>
        /// Checks that the given argument is a positive value.
        /// </summary>
        public static void ArgumentMustBePositive(long argumentValue, string argumentName)
        {
            if (argumentValue <= 0)
            {
                throw new ArgumentException("Argument must be a positive value.", argumentName);
            }
        }

        /// <summary>
        /// Checks whether the given byte array is not a NULL byte.
        /// </summary>
        public static void ArgumentNotNullByte(byte[] argumentValue, string argumentName)
        {
            ArgumentNotNull(argumentValue, argumentName);

            var nullByte = new byte[0];

            if (argumentValue.SequenceEqual(nullByte))
            {
                throw new ArgumentException("The given array should not be a Null Byte", argumentName);
            }
        }

        /// <summary>
        /// This class is used for prevent FxCop from picking up CA1062 Warning when "ArgumentNotNull" method 
        /// is used to validate the nullability of an argument.
        /// http://social.msdn.microsoft.com/Forums/en-US/vstscode/thread/52d40a8e-0dad-41e9-826a-a6fac21b266c
        /// </summary>
        private sealed class ValidatedNotNullAttribute : Attribute
        {
        }
    }
}
