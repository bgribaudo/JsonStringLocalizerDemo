This repository contains a demo for https://github.com/aspnet/Mvc/issues/7887.


## Wasteful `Create()` Calls/Caching

1. In `src\JsonLocalizerMinimalistDemo\JsonLocalization\JsonLocalizationFactory.cs`, set debug breakpoints on both `Create()` methods. 
1. Start debugging.
1. Open browser window and visit site.
    - The first time a page is requested, the debug breakpoint for `Create(Type resourceSource)` is hit multiple times for each `resourceSource` (see table below). Calling this method multiple times per type for a single request leads to a (seemingly) wasteful creation of instances. It seems preferable to create one instance per type per request, then share those instances, as needed, across the request.

        |Value of `resourceSource`|Times Hit|
        |:----------------------------------|----:|
        |Microsoft.AspNetCore.Mvc.Controller|14|
        |Microsoft.AspNetCore.Mvc.ControllerBase|11|
        |JsonLocalizerMinimalistDemo.Models.ExampleModel|6|
    - On the other hand, the debug breakpoint on `Create(string baseName, string location)` is hit exactly one time during the first request.
1. Refresh the page.
    - Notice that the breakpoint on `Create(Type resourceSource)` is *not* hit. Cached localizers are used in place of calling this method again.
    - However, `Create(string baseName, string location)` is hit once. The localizer returned by this call will be used by the view for its localization 
      needs. The fact that the output of this create method isn't cached across requests means that it can create a localizer that is tailored to the 
      current request. In contrast, localizers created by `Create(Type resourceSource)` _cannot_ be tailored to the request, as they are shared across requests.
      
## No Restart Required Translation File Editing
1. Start the site.
1. Edit `src\JsonLocalizerMinimalistDemo\TranslationResources\JsonLocalizerMinimalistDemo.Views.Home.Index.en-US.json`. Notice that changes are reflected as soon as the edit is saved and the page refreshed. (This behavior aligns with how it is possible to edit `src\JsonLocalizerMinimalistDemo\Views\Home\Index.cshtml`, save, then refresh and see edits—without restarting).
1. Edit `src\JsonLocalizerMinimalistDemo\TranslationResources\JsonLocalizerMinimalistDemo.Models.ExampleModel.en-US.json`. In contrast to the previous, edits to this file are *not* reflected on the site until it is restarted. This is because the localizer for this file is cached vs. being created on a per request basis.

