﻿using System;
using Nop.Plugin.Api.Extensions;
using NUnit.Framework;

namespace Nop.Plugin.Api.Tests.ExtensionsTests.StringExtensions
{
    [TestFixture]
    public class StringExtensionsTests_ToDateTimeNullable
    {
        [Test]
        [TestCase("invalid date")]
        [TestCase("20.30.10")]
        [TestCase("2016-30-30")]
        [TestCase("2016/78/12")]
        [TestCase("2016,12,34")]
        [TestCase("&^%$&(^%$_+")]
        [TestCase("2016,23,07")]
        [TestCase("2016.23.07")]
        [TestCase("0")]
        public void WhenInvalidDatePassed_ShouldReturnNull(string invalidDate)
        {
            //Arange

            //Act
            DateTime? cut = invalidDate.ToDateTimeNullable();

            //Assert
            Assert.IsNull(cut);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void WhenNullOrEmptyStringPassed_ShouldReturnNull(string nullOrEmpty)
        {
            //Arange

            //Act
            DateTime? cut = nullOrEmpty.ToDateTimeNullable();

            //Assert
            Assert.IsNull(cut);
        }

        [Test]
        [TestCase("2016/01/01")]
        [TestCase("2016.12.12")]
        [TestCase("2016,10,12")]
        [TestCase("2016,07,23")]
        [TestCase("2016,7,26")]
        [TestCase("2016-7-26")]
        [TestCase("2016-7-26 12:45")]
        [TestCase("2016/12/26 12:45")]
        public void WhenValidDatePassed_ShouldParseThatDate(string validDate)
        {
            //Arange
            DateTime validParsedDate = DateTime.Parse(validDate);

            //Act
            DateTime? cut = validDate.ToDateTimeNullable();

            //Assert
            Assert.AreEqual(validParsedDate, cut);
        }
    }
}