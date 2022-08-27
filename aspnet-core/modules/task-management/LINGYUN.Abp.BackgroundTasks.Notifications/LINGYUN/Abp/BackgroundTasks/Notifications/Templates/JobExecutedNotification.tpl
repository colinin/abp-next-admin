### <font color="{{ model.color }}" size=3>{{ model.title }}</font>
---
{{ if model.tenantname }}
*  **{{ L "TenantName"}}**: {{ model.tenantname }}
{{ end }}
*  **{{ L "JobGroup"}}**: {{ model.group }}
*  **{{ L "JobName"}}**: {{ model.name }}
*  **{{ L "JobId"}}**: {{ model.id }}
*  **{{ L "JobType"}}**: {{ model.type }}
*  **{{ L "TriggerTime"}}**: <font color="#1E90FF ">{{ model.triggertime }}</font>
---
{{ if model.error }}
{{ model.errormessage }}
{{ end }}