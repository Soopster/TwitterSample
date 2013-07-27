using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TwitterSample.Tests.Utils
{
    public static class ContractAssert
    {
        public static void Throws<TException>(Action act, string message) where TException : Exception
        {
            var ex = ExceptionAssert.Throws<TException>(() => act());
            Assert.AreEqual(message, ex.Message);
        }

        public static void ThrowsArgNull(Action act, string paramName)
        {
            var argNullEx = ExceptionAssert.Throws<ArgumentNullException>(() => act());
            Assert.AreEqual(paramName, argNullEx.ParamName);
        }

        public static void ThrowsArgNullOrEmpty(Action<string> act, string paramName)
        {
            var message = String.Format("'{0}' cannot be null or an empty string", paramName);
            ContractAssert.ThrowsArgException(() => act(null), paramName, message);
            ContractAssert.ThrowsArgException(() => act(String.Empty), paramName, message);
        }

        public static void ThrowsArgException(Action act, string paramName, string message)
        {
            var argEx = ExceptionAssert.Throws<ArgumentException>(() => act());
            Assert.AreEqual(paramName, argEx.ParamName);
            Assert.AreEqual(
                message + Environment.NewLine + String.Format("Parameter name: {0}", paramName),
                argEx.Message);
        }
    }
}
