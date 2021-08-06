# Custom Authorizer Lambda

For all the gory details, see https://auth0.com/docs/integrations/aws-api-gateway-custom-authorizers#create-an-auth0-api.

This provides the codebase for an AWS Lambda that validates a token against Auth0. The Lambda is meant to be used as a custom authorizer for an API Gateway.

Edit the `.env` file so that it points to your specific Auth0 settings. Go to your Auth0 API and generate a token. Put that token in the `event.json` file (after "Bearer ") and edit the `methodArn` to match the API Gateway method being secured.

Run `npm install`. Then run `npm test`. You should see a JSON-policy-looking-result with the text "Effect": "Allow" inside.

Now you can zip up the files required for the lambda deployment: `.env`, `package.json`, and any `.js` files.

Upload this .zip package to a Node.js AWS Lambda function. Test using the values in `event.json`.

# Logging In

Followed https://auth0.com/docs/quickstart/spa/vanillajs/01-login to enable SSO (Single Sign-On). After a successful login, the app knows the access token to contact the roadmap-generation API.