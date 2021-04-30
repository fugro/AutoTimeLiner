const fs = require("fs");
const express = require("express");
const { join } = require("path");
const app = express();
const jwt = require("express-jwt");
const jwksRsa = require("jwks-rsa");
var authConfig;

try {
  authConfig = require("./config.json");
} catch (error) {
  fs.copyFile("./defaults.json", "./config.json", (err) => {
  if (err) {
    console.log("Error Found:", err);
  }
});
  authConfig = require("./config.json");
}

// Serve static assets from the /public folder
//app.use(express.static(join(__dirname, "public")));
app.use(express.static(__dirname));

// create the JWT middleware
const checkJwt = jwt({
  secret: jwksRsa.expressJwtSecret({
    cache: true,
    rateLimit: true,
    jwksRequestsPerMinute: 5,
    jwksUri: `https://${authConfig.domain}/.well-known/jwks.json`
  }),

  audience: authConfig.audience,
  issuer: `https://${authConfig.domain}/`,
  algorithms: ["RS256"]
});

app.get("/api/external", checkJwt, (req, res) => {
  res.send({
    msg: "Your access token was successfully validated!"
  });
});

app.use(function(err, req, res, next) {
  if (err.name === "UnauthorizedError") {
    return res.status(401).send({ msg: "Invalid token" });
  }

  next(err, req, res);
});

// Endpoint to serve the configuration file
app.get("/config.json", (req, res) => {
  res.sendFile(join(__dirname, "config.json"));
});

// Serve the index page for all other requests
app.get("/*", (_, res) => {
  res.sendFile(join(__dirname, "index.html"));
});

// Listen on port 3000
app.listen(3000, () => console.log("Application running on port 3000"));