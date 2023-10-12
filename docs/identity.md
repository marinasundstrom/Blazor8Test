# Identity

This app has built in identity.

The users are stored in-memory, and cleared when the app goes to sleep (Scales to 0).

## Identity endpoints

Identity endpoints let users authenticate, and receive either a JSON Web Token, or an Auth cookie.

Although, identity endpoints are enabled, they are not used by the app itself.

But you can use identity endpoints to authenticate and authorize external API calls.

## Custom identity pages

This app implements its own pages for users to register and log in.

This is because the official support was not ready when this app was initially built.