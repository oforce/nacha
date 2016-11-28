using Xunit;
using Nacha.Constants;

namespace Nacha.Tests
{
    public class ConstantsHelperTests
    {
        [Fact]
        public void ConstantsHelper_InvalidItem()
        {
            var _result = ConstantsHelper.IsValid(typeof(StandardEntryClassCodeConstant), "BadValue");
            Assert.Equal(false, _result);
        }

        [Fact]
        public void ConstantsHelper_ValidItem()
        {
            var _result = ConstantsHelper.IsValid(typeof(StandardEntryClassCodeConstant), "CCD");
            Assert.Equal(true, _result);
        }
    }
}
