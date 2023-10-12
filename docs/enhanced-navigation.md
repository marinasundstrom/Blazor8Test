# Enhanced navigation

This page covers are feature which some other frameworks may refer to as _"soft navigation"_.

When navigating to a non-interactive server-rendered page in the web app, Blazor intercepts the request, and applies just the changes to the existing page. This way the page never fully reloads, and your app gets a smooth SPA-like feel.

As a consequence of the page never reloading, scripts don't run as usually expecting when navigating.

Below are ways to deal with this.

## Intercept with JavaScript event

If you want to run JavaScript after an enhanced navigation, you can do that by handling the ``enhancedload`` event.

```javascript 
Blazor.addEventListener('enhancedload', () => {
    console.log("Page loaded");
});
```

**Note:** The ``Blazor`` object is only available after the ``blazor.web.js`` script has been loaded.


## Disable enhanced navigation

You can disable enhanced navigation per link, making Blazor fully reload the page instead, by setting the ``data-enhance-nav`` attribute to ``false``.

```html
<a href="my-non-blazor-page" data-enhance-nav="false">My Non-Blazor Page</a>
```

## Preserve elements

Applying the ``data-permanent`` to an element will make it unaffected by the enhanced navigation - hence ``permanent``.

```html
<div data-permanent>
    This div gets updated dynamically by a script when the page loads!
</div>
```

**Note:** There is currently no way to just preserve certain attributes.