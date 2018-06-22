using JsonLocalizerMinimalistDemo.JsonLocalization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xunit;

namespace JsonLocalizerMinimalistDemoTests.JsonLocalization
{
    public class JsonStringLocalizerFactoryTests : IDisposable
    {
        private readonly JsonStringLocalizerFactory factory = new JsonStringLocalizerFactory();
        private readonly CultureInfo _startingUICulture = CultureInfo.CurrentUICulture;

        public JsonStringLocalizerFactoryTests() =>
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        public void Dispose() =>
            CultureInfo.CurrentUICulture = _startingUICulture;

        [Fact]
        public void Create_ByResourceName()
        {
            var result = factory.Create("baseName", "someLocation");

            var localizer = Assert.IsType<JsonStringLocalizer>(result);
            Assert.Equal(@"TranslationResources\baseName.en-US.json", localizer.Path);
        }

        [Fact]
        public void Create_ByType()
        {
            var result = factory.Create(typeof(SomeType));

            var localizer = Assert.IsType<JsonStringLocalizer>(result);
            Assert.Equal(@"TranslationResources\JsonLocalizerMinimalistDemoTests.JsonLocalization.JsonStringLocalizerFactoryTests+SomeType.en-US.json", localizer.Path);
        }

        private class SomeType { }
    }
}
