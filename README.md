# ResolveThirdPartyReferenceLinks
Sandcastle (SHFB) component to resolve third-party reference links


## How To Use

Provide the `<configuration>` element in the component config, containing all the URL providers that your project needs e.g. This project provides two URL providers for two different external API docs. Both are `formattedProvider` that generate URLs based on a format

```xml
<ComponentConfig id="Resolve ThirdParty Reference Links" enabled="True">
    <component id="Resolve ThirdParty Reference Links">

        <configuration>
            <urlProviders>
                <!-- URL provider for Autodesk Revit API Documentation -->
                <formattedProvider title="Revit URL Provider">
                    <targetMatcher pattern="T:Autodesk\.Revit\..+" />
                    <urlFormatter format="https://api.apidocs.co/resolve/revit/{revitVersion}/?asset_id={target}" />
                    <parameters>
                        <parameter name="revitVersion" default="" />
                    </parameters>
                </formattedProvider>

                <!-- URL provider for RhinoCommon Documentation -->
                <formattedProvider title="RhinoCommon URL Provider">
                    <targetMatcher pattern="T:Rhino\.Geometry\..+" />
                    <targetFormatter>
                        <steps>
                            <replace pattern="T:" with="T_" />
                            <replace pattern="\." with="_" />
                        </steps>
                    </targetFormatter>
                    <urlFormatter format="https://developer.rhino3d.com/api/RhinoCommon/html/{target}.htm" />
                </formattedProvider>
            </urlProviders>
        </configuration>
        
        <revitVersion value="$(RevitVersion)" />

    </component>
</ComponentConfig>
```

### URL Formatters

**All URL formatters have:**

- `title` attribute
- `targetMatcher`: is a regular expression that matches with the given key provided by Sandcastle e.g `T:Autodesk.Revit.DB.Color`. If this matches the given key, then the provider will be used
- `parameters`: is a collection of `parameter` elements that are inputs provided to the formatter, from the component configurations in Sandcastle e.g. `revitVersion` in the example above. This allows URL provides to use parameters set during build

**FormatterUrlProvider (`formattedProvider`):**

FormatterUrlProvider generates URLs based on a given format:

- `targetFormatter`: is a set of steps to format the given type name e.g. `T:Rhino.Geometry.Curve` to `T_Rhino_Geometry_Curve` in example above
- `urlFormatter`: is the url format that includes `{}` tags for `target` and other parameters e.g. See `{revitVersion}` in the example above


**DictionaryUrlProvider:**
- NOT YET IMPLEMENTED

**ExternalUrlProvider:**
- NOT YET IMPLEMENTED


## Thanks

- [Eric Woodruff](https://github.com/EWSoftware/SHFB) for Sandcastle (SHFB)
- [Sand Castle](https://icons8.com/icon/Y8hpNo5KuUdv/sand-castle) icon by [Icons8](https://icons8.com)