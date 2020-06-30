English | [简体中文](./README.md)

## Overview

This is a [vue-typescript-admin-template](http://armour.github.io/vue-typescript-admin-template) -based Abp framework background management interface

## Documentation

[Docs](https://armour.github.io/vue-typescript-admin-docs)

## Screenshots

![Login](./vueJs/images/userLogin.png)

![Tenants](./vueJs/images/tenant.png)

![Roles](./vueJs/images/userRoles.png)

![Permissions](./vueJs/images/userPermissions.png)

![IM](./vueJs/images/im-notifications.png)

![Clients](./vueJs/images/client.png)

![Aggregate route](./vueJs/images/aggregate.png)

## Related Projects

[Armour/vue-typescript-admin-mock-server](https://github.com/armour/vue-typescript-admin-mock-server) (mock server for this project)

[Armour/vue-typescript-admin-docs](https://github.com/armour/vue-typescript-admin-docs) (documentation source for this project)

Javascript version:

[PanJiaChen/vue-admin-template](https://github.com/PanJiaChen/vue-admin-template) (a vue2.0 minimal admin template) 

[PanJiaChen/vue-element-admin](https://github.com/PanJiaChen/vue-element-admin) (full features supported vue admin) 

[PanJiaChen/electron-vue-admin](https://github.com/PanJiaChen/electron-vue-admin) (a vue electron admin project)

## Features

```txt
- Login / Logout

- Permission Authentication
  - Page permission
  - Directive permission
  - Permission configuration page
  - Two-step login

- Multi-environment build
  - Dev / Stage / Prod

- Global Features
  - I18n
  - Dynamic themes
  - Dynamic sidebar (supports multi-level routing)
  - Dynamic breadcrumb
  - Tags-view (supports right-click operation)
  - Clipboard
  - Svg icons
  - Search
  - Screenfull
  - Settings
  - Mock data / Mock server
  - PWA support

- Components
  - Editors
    - Rich Text Editor
    - Markdown Editor
    - JSON Editor
  - Avatar Upload
  - Back To Top
  - CountTo
  - Dropzone
  - Draggable Dialog
  - Draggable Kanban
  - Draggable List
  - Draggable Select
  - ECharts
  - Mixin
  - SplitPane
  - Sticky

- Table
  - Dynamic Table
  - Draggable Table
  - Inline Edit Table
  - Complex Table

- Excel
  - Export Excel
  - Upload Excel
  - Excel Visualization

- Zip
  - Export zip

- PDF
  - Download pdf

- Dashboard
- Guide Page
- Advanced Example Page
- Error Log
- Error Page
  - 401
  - 404
```

## Preparation

You need to install [node](http://nodejs.org/) and [git](https://git-scm.com/) locally. The project is based on [typescript](https://www.typescriptlang.org/), [vue](https://vuejs.org/index.html), [vuex](https://vuex.vuejs.org/), [vue-router](https://router.vuejs.org/), [vue-cli](https://github.com/vuejs/vue-cli) , [axios](https://github.com/axios/axios) and [element-ui](https://github.com/ElemeFE/element), Understanding and learning these knowledge in advance will greatly help you on using this project.

## Project Structure

```bash
├── mock/                      # mock server & mock data
├── public                     # public static assets (directly copied)
│   │── favicon.ico            # favicon
│   │── manifest.json          # PWA config file
│   └── index.html             # index.html template
├── src                        # main source code
│   ├── api                    # api service
│   ├── assets                 # module assets like fonts, images (processed by webpack)
│   ├── components             # global components
│   ├── directives             # global directives
│   ├── filters                # global filter
│   ├── icons                  # svg icons
│   ├── lang                   # i18n language
│   ├── layout                 # global layout
│   ├── pwa                    # PWA service worker related files
│   ├── router                 # router
│   ├── store                  # store
│   ├── styles                 # global css
│   ├── utils                  # global utils
│   ├── views                  # views
│   ├── App.vue                # main app component
│   ├── main.ts                # app entry file
│   ├── permission.ts          # permission authentication
│   ├── settings.ts            # setting file
│   └── shims.d.ts             # type definition shims
├── tests/                     # tests
├── .circleci/                 # automated CI configuration
├── .browserslistrc            # browserslist config file (to support Autoprefixer)
├── .editorconfig              # editor code format consistency config
├── .env.xxx                   # env variable configuration
├── .eslintrc.js               # eslint config
├── babel.config.js            # babel config
├── cypress.json               # e2e test config
├── jest.config.js             # jest unit test config
├── package.json               # package.json
├── postcss.config.js          # postcss config
├── tsconfig.json              # typescript config
└── vue.config.js              # vue-cli config
```

## Project setup

With [yarn](https://yarnpkg.com/lang/en/) or [npm](https://www.npmjs.com/get-npm)


#### Install dependencies

```bash
yarn install

```

### Custom vue project config

[.env.Github.production](./.env.Github.production)  变更为  .env.production
</br>
[vue.config.github.js ](./vue.config.github.js )   变更为  vue.config.js

Modify the server address that the development environment will use for the proxy. Provide the following three addresses: IdentityService, IdentityServer, and ApiService

```bash

    proxy: {
      [process.env.VUE_APP_BASE_IDENTITY_SERVER]: {
        target: 'your identityService address',
        changeOrigin: true,
        pathRewrite: {
          ['^' + process.env.VUE_APP_BASE_IDENTITY_SERVER]: ''
        }
      },
      [process.env.VUE_APP_BASE_IDENTITY_SERVICE]: {
        target: 'your identityServer address',
        changeOrigin: true,
        pathRewrite: {
          ['^' + process.env.VUE_APP_BASE_IDENTITY_SERVICE]: ''
        }
      },
      [process.env.VUE_APP_BASE_API]: {
        target: 'you api gateway address',
        changeOrigin: true,
        pathRewrite: {
          ['^' + process.env.VUE_APP_BASE_API]: ''
        }
      }
    }

```

Modify the actual address of the production environment, as above

```bash

# Base api
# Remeber to change this to your production server address
# Here I used my mock server for this project
# VUE_APP_BASE_API = 'https://vue-typescript-admin-mock-server.armour.now.sh/mock-api/v1/'

VUE_APP_BASE_API = 'your api gateway address'

VUE_APP_BASE_IDENTITY_SERVICE = 'your identityService address'

VUE_APP_BASE_IDENTITY_SERVER = 'your identityServer address'

# client id
VUE_APP_CLIENT_ID = 'vue-admin-element'
# client secret
VUE_APP_CLIENT_SECRET = '1q2w3e*'

```

### Compiles background services

```shell
cd aspnet-core/services

start-all-service.bat

```

#### Compiles and hot-reloads for development

```shell

cd vueJs

start-vue-admin.bat

```

#### Compiles and minifies for production

```bash
yarn run build:prod
```

#### Lints and fixes files

```bash
yarn run lint
```

#### Run your unit tests

```bash
yarn run test:unit
```

#### Run your end-to-end tests

```bash
yarn run test:e2e
```

#### Generate all svg components

```bash
yarn run svg
```

#### Customize Vue configuration

See [Configuration Reference](https://cli.vuejs.org/config/).

## Browsers support

Modern browsers and Internet Explorer 10+.

| [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/edge/edge_48x48.png" alt="IE / Edge" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>IE / Edge | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/firefox/firefox_48x48.png" alt="Firefox" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Firefox | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/chrome/chrome_48x48.png" alt="Chrome" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Chrome | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/safari/safari_48x48.png" alt="Safari" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Safari |
| --------- | --------- | --------- | --------- |
| IE10, IE11, Edge| last 2 versions| last 2 versions| last 2 versions

## Contributing

See [CONTRIBUTING.md](https://github.com/Armour/vue-typescript-admin-template/blob/master/.github/CONTRIBUTING.md)

## License

[MIT License](https://github.com/Armour/vue-typescript-admin-template/blob/master/LICENSE)
