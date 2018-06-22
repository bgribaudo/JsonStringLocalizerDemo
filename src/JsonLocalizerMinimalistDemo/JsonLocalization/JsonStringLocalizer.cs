using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace JsonLocalizerMinimalistDemo.JsonLocalization
{
    class JsonStringLocalizer : IStringLocalizer
    {
        private readonly IReadOnlyDictionary<string, string> translations;
        private readonly string _basePath;

        public JsonStringLocalizer(string basePath, CultureInfo culture)
        {
            _basePath = basePath;
            Culture = culture;

            try
            {
                var json = File.ReadAllText(Path);
                translations = JsonConvert.DeserializeObject<IReadOnlyDictionary<string, string>>(json);
            }
            catch (IOException)
            {
                /* If the localizer can't find a value for a key, it is to return that key as the value. 
                   This behavior should hold true both:
                   - when the specified JSON localization file has been found but does not contain the 
                     requested key 
                   - and when the translation file was not found
                   
                   In the case of the latter (what we're handling here), we simply provide an empty 
                   dictionary since a not found file means that no key -> value translations exist. */
                translations = new Dictionary<string, string>();
            }
        }

        public CultureInfo Culture { get; }

        public string Path => 
            $"{_basePath}.{Culture}.json";

        public LocalizedString this[string name] =>
            this[name, new object[] { }];

        public LocalizedString this[string name, params object[] arguments] =>
            translations.TryGetValue(name, out var value) ? 
                new LocalizedString(name, String.Format(value, arguments), false) : 
                new LocalizedString(name, name, true);

        // NOTE - for simplicity's sake, includeParentCultures is ignored by this demo code
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) =>
            translations.Select(t => new LocalizedString(t.Key, t.Value, false));

        public IStringLocalizer WithCulture(CultureInfo culture) =>
            new JsonStringLocalizer(_basePath, culture);
    }
}
