# Extension tool set based on abp cli

Provides quick creation of template projects, JavaScript library commands and more.

## Getting started

```shell
dotnet tool install --global LINGYUN.Abp.Cli
```

## Usage

```shell
Usage:

    labp <command> <target> [options]

Command List:

    > help: Show command line help. Write ` labp help <command> `
    > create: Generate a new solution based on the customed ABP startup templates.
    > generate-proxy: Generates client service proxies and DTOs to consume HTTP APIs.
    > generate-view: Generate the view code from the http api proxy.

To get a detailed help for a command:

    labp help <command>
```

## Features

* Support for generating TypeScript client proxy code
  - Axios HTTP client
  - Vben Admin integration
  - UniApp integration
* Support for generating Flutter client proxy code
  - Dio HTTP client
  - REST service integration
* Support for generating view code
  - Vben Admin view templates
  - Flutter GetX view templates
* Custom ABP startup templates

## Feedback

Have a question and need feedback?

- [Github issues](https://github.com/colinin/abp-next-admin/issues)

[简体中文](./README.zh-Hans.md)