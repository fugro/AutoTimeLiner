# AutoTimeLiner Website

Front-end site for accessing the roadmap AWS Lambda. Site is hosted on AWS Amplify. Authentication provided by [Auth0](https://auth0.com).

## Development

Edit `defaults.json` to target your dev environment.

Run `npm run dev` to spin up the app. Open it in `localhost:3000`.

## Deployment

Edit `defaults.json` to target the appropriate environment.

Zip up the following files & folders and Drop the .zip file onto the AWS Amplify console to update the site.

* `css`
* `js`
* `node_modules`
* `config.json`
* `index.html`
* `package.json`
