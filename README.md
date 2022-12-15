# Napos - Your POS tool

## How to run, build

You can run the application in two modes

1. Run as web (development):
    - Run web API `dotnet run` in  `\Napos.Web`
    - Run the UI `quasar dev` in `\Napos.UI`

2. Run as desktop (production):
    - Build the UI first `/Napos.UI/build_pwa.cmd`
    - Run the desktop app `dotnet run` in `\Napos`

You may need to rebuild the API for the client (`\services\domain.ts`). Please run `npm run codegen` in `\Napos.UI` (see `codegen.js`).
