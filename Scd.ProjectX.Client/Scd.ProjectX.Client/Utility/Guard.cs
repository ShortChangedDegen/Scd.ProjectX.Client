namespace Scd.ProjectX.Client.Utility
{
    public static class Guard
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
                throw new ArgumentNullException(paramName, "The provided parameter cannot be null.");
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
        public static T NotNullOrEmpty<T>(T param, string paramName) where T : ICollection<T>
        {
            if (NotNull(param, paramName).Count == 0)
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
    }
}