using Posterr.Domain.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posterr.Domain.Core.Tests.Extensions
{
    public class StringExtensionTests
    {        
        [Theory]
        [InlineData("march 25,2021", "March 25,2021")]
        [InlineData("a", "A")]
        [InlineData("A", "A")]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData("aSdfghjhjh", "ASdfghjhjh")]
        [InlineData("asdfghjhjh", "Asdfghjhjh")]
        public void ToUpperFirstLetter_ReturnCorrectValues(string input, string expected)
        {
            // Act
            var result = input.ToUpperFirstLetter();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
