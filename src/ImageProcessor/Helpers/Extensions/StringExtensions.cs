﻿// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="James South">
//     Copyright (c) James South.
//     Licensed under the Apache License, Version 2.0.
// </copyright>
// -----------------------------------------------------------------------

namespace ImageProcessor.Helpers.Extensions
{
    #region Using
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    #endregion

    /// <summary>
    /// Encapsulates a series of time saving extension methods to the <see cref="T:System.String"/> class.
    /// </summary>
    public static class StringExtensions
    {
        #region Cryptography
        /// <summary>
        /// Creates an MD5 fingerprint of the String.
        /// </summary>
        /// <param name="expression">The <see cref="T:System.String">String</see> instance that this method extends.</param>
        /// <returns>An MD5 fingerprint of the String.</returns>
        public static string ToMD5Fingerprint(this string expression)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(expression.ToCharArray());

            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] hash = md5.ComputeHash(bytes);

                // Concatenate the hash bytes into one long String.
                return hash.Aggregate(
                    new StringBuilder(32),
                    (sb, b) => sb.Append(b.ToString("X2", CultureInfo.InvariantCulture)))
                    .ToString().ToLowerInvariant();
            }
        }

        /// <summary>
        /// Creates an SHA1 fingerprint of the String.
        /// </summary>
        /// <param name="expression">The <see cref="T:System.String">String</see> instance that this method extends.</param>
        /// <returns>An SHA1 fingerprint of the String.</returns>
        public static string ToSHA1Fingerprint(this string expression)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(expression.ToCharArray());

            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] hash = sha1.ComputeHash(bytes);

                // Concatenate the hash bytes into one long String.
                return hash.Aggregate(
                    new StringBuilder(40),
                    (sb, b) => sb.Append(b.ToString("X2", CultureInfo.InvariantCulture)))
                    .ToString().ToLowerInvariant();
            }
        }

        /// <summary>
        /// Creates an SHA256 fingerprint of the String.
        /// </summary>
        /// <param name="expression">The <see cref="T:System.String">String</see> instance that this method extends.</param>
        /// <returns>An SHA256 fingerprint of the String.</returns>
        public static string ToSHA256Fingerprint(this string expression)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(expression.ToCharArray());

            using (SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider())
            {
                byte[] hash = sha256.ComputeHash(bytes);

                // Concatenate the hash bytes into one long String.
                return hash.Aggregate(
                    new StringBuilder(64),
                    (sb, b) => sb.Append(b.ToString("X2", CultureInfo.InvariantCulture)))
                    .ToString().ToLowerInvariant();
            }
        }

        /// <summary>
        /// Creates an SHA512 fingerprint of the String.
        /// </summary>
        /// <param name="expression">The <see cref="T:System.String">String</see> instance that this method extends.</param>
        /// <returns>An SHA256 fingerprint of the String.</returns>
        public static string ToSHA512Fingerprint(this string expression)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(expression.ToCharArray());

            using (SHA512CryptoServiceProvider sha512 = new SHA512CryptoServiceProvider())
            {
                byte[] hash = sha512.ComputeHash(bytes);

                // Concatenate the hash bytes into one long String.
                return hash.Aggregate(
                    new StringBuilder(70),
                    (sb, b) => sb.Append(b.ToString("X2", CultureInfo.InvariantCulture)))
                    .ToString().ToLowerInvariant();
            }
        }
        #endregion

        #region Numbers
        /// <summary>
        /// Creates an array of integers scraped from the String.
        /// </summary>
        /// <param name="expression">The <see cref="T:System.String">String</see> instance that this method extends.</param>
        /// <returns>An array of integers scraped from the String.</returns>
        public static int[] ToPositiveIntegerArray(this string expression)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(expression));

            Regex regex = new Regex(@"\d+", RegexOptions.Compiled);

            MatchCollection matchCollection = regex.Matches(expression);

            // Get the collections.
            int count = matchCollection.Count;
            int[] matches = new int[count];

            // Loop and parse the int values.
            for (int i = 0; i < count; i++)
            {
                matches[i] = int.Parse(matchCollection[i].Value);
            }

            return matches;
        }
        #endregion

        #region Files and Paths
        /// <summary>
        /// Checks the string to see whether the value is a valid virtual path name.
        /// </summary>
        /// <param name="expression">The <see cref="T:System.String">String</see> instance that this method extends.</param>
        /// <returns>True if the given string is a valid virtual path name</returns>
        public static bool IsValidVirtualPathName(this string expression)
        {
            // Check the start of the string.
            if (expression.StartsWith("~/"))
            {
                // Trim the first two characters and test the path.
                expression = expression.Substring(2);
                return expression.IsValidPathName();
            }

            return false;
        }

        /// <summary>
        /// Checks the string to see whether the value is a valid path name.
        /// </summary>
        /// <remarks>
        /// For an explanation 
        /// <see cref="http://stackoverflow.com/questions/62771/how-check-if-given-string-is-legal-allowed-file-name-under-windows"/>
        /// </remarks>
        /// <param name="expression">The <see cref="T:System.String">String</see> instance that this method extends.</param>
        /// <returns>True if the given string is a valid path name</returns>
        public static bool IsValidPathName(this string expression)
        {
            // Create a regex of invalid characters and test it.
            string invalidPathNameChars = new string(Path.GetInvalidFileNameChars());
            Regex regFixPathName = new Regex("[" + Regex.Escape(invalidPathNameChars) + "]");

            return !regFixPathName.IsMatch(expression);
        }
        #endregion
    }
}
