using System.Numerics;

namespace Scd.ProjectX.Client.Utility
{
    /// <summary>
    /// A utility class for guarding parameters and values against invalid states.
    /// </summary>
    /// <remarks>It cleans up the code a little, but adds to the stacktrace.</remarks>
    internal static class Guard
    {
        /// <summary>
        /// Guards a value against null and a predicate. This should
        /// not be used unless there are no better guard options.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="predicate">The test that <paramref name="param"/> must pass.</param>
        /// <param name="param">The parameter value.</param>
        /// <param name="paramName">The parameter name.</param>
        /// <param name="message">The error message.</param>
        /// <returns>The <paramref name="param"/> value.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="param"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="param"/> fails the <paramref name="predicate"/>.</exception>
        public static T IsTrue<T>(Func<T, bool> predicate, T? param, string paramName, string message)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            // In this case, we may not case if a ref or nullable is null.
            if (!predicate(param))
            {
                throw new ArgumentException(message, paramName);
            }
#pragma warning restore CS8604 // Possible null reference argument.

            return param;
        }

        /// <summary>
        /// Guards a value against null and a predicate. This should
        /// not be used unless there are no better guard options.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="predicate">The test that <paramref name="param"/> must pass.</param>
        /// <param name="param">The parameter value.</param>
        /// <param name="paramName">The parameter name.</param>
        /// <param name="message">The error message.</param>
        /// <returns>The <paramref name="param"/> value.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="param"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="param"/> fails the <paramref name="predicate"/>.</exception>
        public static T Not<T>(Func<T, bool> predicate, T? param, string paramName, string message)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            // In this case, we may not case if a ref or nullable is null.
            if (predicate(param))
            {
                throw new ArgumentException(message, paramName);
            }
#pragma warning restore CS8604 // Possible null reference argument.

            return param;
        }

        /// <summary>
        /// Guards a value against null. This should not be used unless there are no better guard options.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="param">The parameter value.</param>
        /// <param name="paramName">The parameter name.</param>
        /// <returns>The <paramref name="param"/> value.</returns>
        /// <exception cref="ArgumentNullException">The provided parameter cannot be null.</exception>
        public static T NotNull<T>(T? param, string paramName)
        {
            if (param is null)
            {
                throw new ArgumentNullException(paramName, $"{paramName} cannot be null.");
            }
            return param;
        }

        /// <summary>
        /// Guards a collection against empty values.
        /// </summary>
        /// <param name="param">The parameter value.</param>
        /// <param name="paramName">The parameter name.</param>
        /// <returns>The <paramref name="param"/> value.</returns>
        /// <exception cref="ArgumentException">The provided collection cannot be empty.</exception>
        public static T NotNullOrEmpty<T, U>(T param, string paramName)
            where T : ICollection<U>
        {
            NotNull(param, paramName);

            if (!param.Any())
            {
                throw new ArgumentException("The provided collection cannot be empty.", paramName);
            }
            return param;
        }

        /// <summary>
        /// Guards a string against null or empty values.
        /// </summary>
        /// <param name="param">The parameter value.</param>
        /// <param name="paramName">The parameter name.</param>
        /// <returns>The <paramref name="param"/> value.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="param"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="param"/> fails the <paramref name="predicate"/>.</exception>
        public static string NotNullOrEmpty(string? param, string paramName) =>
            IsTrue(
                v => !string.IsNullOrEmpty(v),
                NotNull(param, paramName),
                paramName,
                $"{paramName} cannot be empty."
            );

        /// <summary>
        /// Guards a DateTime to ensure it is later than another DateTime.
        /// </summary>
        /// <param name="earlyDate"></param>
        /// <param name="lateDate"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static DateTime IsEarlierDate(DateTime earlyDate, DateTime lateDate, string paramName) =>
            IsTrue(
                v => earlyDate <= lateDate,
                earlyDate,
                paramName,
                $"{paramName} ({earlyDate}) must be prior to {lateDate}."
            );

        /// <summary>
        /// Guards a value against a specified value for its type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="param">The parameter value.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>The paramater.</returns>
        public static T Not<T>(T badValue, T param, string paramName) =>
            IsTrue(param => !param.Equals(badValue),
                param,
                paramName,
                $"{paramName} cannot be '{badValue}'."
            );

        /// <summary>
        /// Guards a value against a specified value for its type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="param">The parameter value.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>The paramater.</returns>
        public static T NotIn<T>(T[] badValues, T param, string paramName) =>
            IsTrue(param => badValues.All(bv => !bv?.Equals(param) ?? false),
                param,
                paramName,
                $"{paramName} cannot contain '{badValues}'."
            );

        /// <summary>
        /// Guards a value against the default value for its type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="param">The parameter value.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>The paramater.</returns>
        public static T NotDefault<T>(T param, string paramName) =>
        IsTrue(param => !EqualityComparer<T>.Default.Equals(param, default(T)),
                param,
                paramName,
                $"{paramName} cannot be the default value."
            );

        /// <summary>
        /// Guards a value against the negative values for its type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="param">The parameter value.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>The paramater.</returns>
        public static T NotNegative<T>(T param, string paramName)
            where T : INumber<T> =>
            Not(T.IsNegative,
                    param,
                    paramName,
                    $"{paramName} cannot be less than zero."
                );

        /// <summary>
        /// Guards a value is greater than a minimumValue.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="minValue">The value <paramref name="param"/> must be greaten than.</param>
        /// <param name="param">The parameter value.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>The paramater.</returns>
        public static T IsGreaterThan<T>(T minValue, T param, string paramName)
            where T : INumber<T> =>
            IsTrue(p => p > minValue,
                    param,
                    paramName,
                    $"{paramName} cannot be less than {minValue}."
                );
    }
}