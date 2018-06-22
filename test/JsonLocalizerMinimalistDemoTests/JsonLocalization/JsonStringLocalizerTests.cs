using JsonLocalizerMinimalistDemo.JsonLocalization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xunit;

namespace JsonLocalizerMinimalistDemoTests.JsonLocalization
{
    public class JsonStringLocalizerTests 
    {
        private readonly static CultureInfo _culture = new CultureInfo("en-US");
        private readonly JsonStringLocalizer _localizer = new JsonStringLocalizer(@"Fixtures\JsonStringLocalization", _culture);

        [Fact]
        public void Culture()
        {
            Assert.Equal(_culture, _localizer.Culture);
        }

        [Fact]
        public void Path()
        {
            Assert.Equal(@"Fixtures\JsonStringLocalization.en-US.json", _localizer.Path);
        }

        [Fact]
        public void FetchExistingKey()
        {
            var result = _localizer["Key1"];

            var expected = new LocalizedString("Key1", "Value1", resourceNotFound: false);
            Assert.Equal(expected, result, LocalizedStringComparer.Instance);
        }

        [Fact]
        public void FetchExistingKey_WithParams()
        {
            var result = _localizer["Key2", 10, 20];

            var expected = new LocalizedString("Key2", "Value2 10 20", resourceNotFound: false);
            Assert.Equal(expected, result, LocalizedStringComparer.Instance);
        }

        [Fact]
        public void FetchNonExistentKey_KeyShouldBeReturnedAsValue()
        {
            var result = _localizer["Does Not Exist"];

            var expected = new LocalizedString("Does Not Exist", "Does Not Exist", resourceNotFound: true);
            Assert.Equal(expected, result, LocalizedStringComparer.Instance);
        }

        [Fact]
        public void GetAllStrings()
        {
            var result = _localizer.GetAllStrings();

            var expected = new[]
            {
                new LocalizedString("Key1", "Value1", false),
                new LocalizedString("Key2", "Value2 {0} {1}", false)
            };
            Assert.Equal(expected, result, LocalizedStringComparer.Instance);
        }

        [Fact]
        public void NonExistentTranslationFile_KeysReturnedAsValues()
        {
            var localizer = new JsonStringLocalizer("does not exist.json", _culture);

            var result = localizer["Some Key"];

            var expected = new LocalizedString("Some Key", "Some Key", resourceNotFound: true);
            Assert.Equal(expected, result, LocalizedStringComparer.Instance);
        }

        [Fact]
        public void WithCulture()
        {
            var result = _localizer.WithCulture(new CultureInfo("es-US"));

            var localizer = Assert.IsType<JsonStringLocalizer>(result);
            Assert.Equal(@"Fixtures\JsonStringLocalization.es-US.json", localizer.Path);
        }

        private class LocalizedStringComparer : IEqualityComparer<LocalizedString>
        {
            public static LocalizedStringComparer Instance =>
                new LocalizedStringComparer();

            public bool Equals(LocalizedString x, LocalizedString y) =>
                x.Name == y.Name
                && x.Value == y.Value
                && x.ResourceNotFound == y.ResourceNotFound;

            public int GetHashCode(LocalizedString obj) =>
                obj.GetHashCode();
        }
    }
}
