using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JsonLocalizerMinimalistDemo.JsonLocalization
{
    class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        /*
         In non-demo code:
         - BaseLocation would likely be set by an option passed into the constructor.
         - Both Create() method would likely be more sophisticated in how they generate the
           base paths they pass to JsonStringLocalizer and s, probably would produce clearer paths.
         */

        private static readonly string _baseLocation = "TranslationResources";

        public IStringLocalizer Create(Type resourceSource) =>
            new JsonStringLocalizer(Path.Join(_baseLocation, resourceSource.FullName), CultureInfo.CurrentUICulture);

        public IStringLocalizer Create(string baseName, string location) =>
            new JsonStringLocalizer(Path.Join(_baseLocation, baseName), CultureInfo.CurrentUICulture);
    }
}
