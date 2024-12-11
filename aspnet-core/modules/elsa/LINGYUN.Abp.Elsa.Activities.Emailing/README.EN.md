# LINGYUN.Abp.Elsa.Activities.Emailing

Email sending activities integration module for Elsa workflow

## Features

* Provides **SendEmailing** activity for sending emails
  * Support sending to multiple recipients
  * Support template rendering for email content
  * Support JavaScript and Liquid syntax
  * Integration with ABP framework's `IEmailSender` interface
  * Integration with ABP framework's `ITemplateRenderer` interface for template rendering

## Configuration and Usage

```csharp
[DependsOn(
    typeof(AbpElsaActivitiesEmailingModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

## appsettings.json

```json
{
    "Elsa": {
        "Emailing": true    // Enable email sending activities
    }
}
```

## Activity Parameters

* **To**: List of recipient email addresses
* **Subject**: Email subject
* **Body**: Email body content
* **Culture**: Culture information
* **Template**: Template name
* **Model**: Model parameters used to format the template content

[中文文档](./README.md)
