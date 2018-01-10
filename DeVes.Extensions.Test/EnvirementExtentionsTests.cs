using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeVes.Extensions.Test
{
    public static class EnvirementExtentionsTests
    {
        [Fact]
        public static void GetMonthStart_20180115()
        {
            Assert.True(new DateTime(2018, 01, 15).GetMonthStart() == new DateTime(2018, 1, 1));
        }
        [Fact]
        public static void GetMonthStart_20180201()
        {
            Assert.True(new DateTime(2018, 2, 1).GetMonthStart() == new DateTime(2018, 2, 1));
        }
        [Fact]
        public static void GetMonthStart_20181231()
        {
            Assert.True(new DateTime(2018, 12, 31).GetMonthStart() == new DateTime(2018, 12, 1));
        }
        [Fact]
        public static void GetMonthStart_Null()
        {
            DateTime? _value = null;

            Assert.True(!_value.GetMonthStart().HasValue);
        }


        [Theory]
        [InlineData(4711, "4711")]
        [InlineData(753.1, "753,1")]
        [InlineData(true, "True")]
        [InlineData(false, "False")]
        public static void ConvertTo_String(object value, string expected)
        {
            Assert.True(value.ConvTo<string>() == expected);
        }

        [Theory]
        [InlineData("4711", 4711)]
        [InlineData("123", 123)]
        [InlineData("1", 1)]
        [InlineData("32767", 32767)]
        public static void ConvertTo_Short(object value, short expected)
        {
            Assert.True(value.ConvTo<short>() == expected);
        }

        [Theory]
        [InlineData("4711", 4711)]
        [InlineData("123", 123)]
        [InlineData("1", 1)]
        [InlineData("32767", 32767)]
        public static void ConvertTo_Int(object value, int expected)
        {
            Assert.True(value.ConvTo<int>() == expected);
        }

        [Theory]
        [InlineData("4711", 4711)]
        [InlineData("123", 123)]
        [InlineData("1", 1)]
        [InlineData("9223372036854775807", 9223372036854775807)]
        public static void ConvertTo_Long(object value, long expected)
        {
            Assert.True(value.ConvTo<long>() == expected);
        }
    }
}
