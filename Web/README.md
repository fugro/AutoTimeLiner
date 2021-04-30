# AutoTimeLiner Website

Front-end site for accessing the roadmap AWS Lambda. Site is hosted on AWS Amplify. Authentication provided by Auth0.

## Development

Edit `auth_config.json` to target your dev environment.

Run `npm run dev` to spin up the app. Open it in `localhost:3000`.

## Deployment

Edit `auth_config.json` to target the appropriate environment.

Zip up the following files & folders Drop the .zip file onto the AWS Amplify console to update the site.

* `css`
* `js`
* `node_modules`
* `auth_config.json`
* `index.html`
* `package.json`
