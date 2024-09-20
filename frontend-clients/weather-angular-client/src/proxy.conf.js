var jsonServerTarget = "http://localhost:3000";
var defaultTarget = process.env["services__apiservice__http__0"];
module.exports = {
  "/api": {
    target: process.env["FAKE_BACKEND"] ? jsonServerTarget : defaultTarget,
    pathRewrite: {
      "^/api": "",
    },
  },
};
