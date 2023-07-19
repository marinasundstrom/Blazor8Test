# Blazor app for .NET 8 Preview 6

Demonstrates the Server-side rendered pages, and the 2 render modes for embedded interactive components: Server and WebAssembly,

Check the ``Counter`` components.

Based on a sample by [Artak](https://github.com/mkArtakMSFT).

Data is stored temporary in-memory. There is no database.

## Contents

Demonstrating the following features that have been made possible by the new unified architecture:

* Server-side rendering (SSR) of page components - like "MVC Razor Pages" but with Razor components. Handy if you are building content-rich pages, that doesn't necessarily need full interactivity. Enables caching and Search-engine optimization (SEO).

* Streaming rendering (Show Data) - render the SSR page before the component has been initialized and then apply the rest once that has been loaded - in one HTTP request. No web sockets involved.

* Component Render modes: Selectively enabling interactivity on specific components (Counter) with the Server or WebAssembly render modes. There is an upcoming Auto-mode as well.

* Form model binding & validation in SSR pages.

* Authorization - Custom Login and Register pages with Server-side rendered Razor components (Instead of MVC Razor Pages).

Pre-rendering just works. If you have a server-side rendered page with interactive components then they will all be rendered together the first time.

## Highlights

* Largely server-side rendered app - pages rendered and served by server on a request-basis.

* Showing how to handle a pre-render scenario with components in WebAssembly.

* Authentication using cookies. Custom server-side rendered pages for logging in and registering user. 

* Make HTTP request from client Web Assembly to Web API with cookie authentication. Sharing cookie with app.

* Bootstrap - Handling themes: light mode and dark mode.

## Technical details

* There are two versions of the "Show data" page -``ShowData`` (server-side), and ``FetchData`` (client-side using Web API). Workaround with "FetchData" because you can't currently route to pages in WebAssembly.

* There is a ``RenderingContext`` object that can be injected into components to check whether your component is running on the server, or on the client (WebAssembly). There is also a property telling whether prerendering is in progress.

* Added a ``Shared`` project for stuff (Models etc) that is used by both Server and Client. There is an interface called ``IWeatherForecastService`` which has two implementations: one for server and another for the client.

* Using Cookie Authentication because that is best suitable for a server-side rendered app. Any client-side component in WebAssembly also uses cookie auth when doing HTTP request to the Web API.

* **Workaround**: I have created a custom ``ServerNavigationManager`` to deal with navigation from server-side rendered pages. It simply modifies the ``HttpContext.Request`` in order to make the browser redirect to the desired location.

## Publish

Published to Azure as a Container App with GitHub Actions.

https://blazor8app.graypebble-0c46426e.westus2.azurecontainerapps.io/
