# Blazor app for .NET 8 Preview 6

Demonstrates the Server-side rendered pages, and the 2 render modes for embedded interactive components: Server and WebAssembly,

Check the ``Counter`` components.

Based on a sample by [Artak](https://github.com/mkArtakMSFT).

## Details

Demonstrating the following features that have been made possible by the new unified architecture:

* Server-side rendering (SSR) of page components - like "MVC Razor Pages" but with Razor components. Handy if you are building content-rich pages, that doesn't necessarily need full interactivity. Enables caching and Search-engine optimization (SEO).

* Streaming rendering (Show Data) - render the SSR page before the component has been initialized and then apply the rest once that has been loaded - in one HTTP request. No web sockets involved.

* Component Render modes: Selectively enabling interactivity on specific components (Counter) with the Server or WebAssembly render modes. There is an upcoming Auto-mode as well.

* Form model binding & validation in SSR pages.

Pre-rendering just works. If you have a server-side rendered page with interactive components then they will all be rendered together the first time.

## Publish

Published to Azure as a Container App with GitHub Actions.

https://blazor8app.graypebble-0c46426e.westus2.azurecontainerapps.io/