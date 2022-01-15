<!DOCTYPE html> 
<html> 
    <head> 
        <meta charset="UTF-8"> 
        <title>{{ model.title }}</title> 
    </head> 
    <body leftmargin="8" marginwidth="0" topmargin="8" marginheight="4" offset="0"> 
        <table width="95%" cellpadding="0" cellspacing="0" style="font-size: 11pt; font-family: Tahoma, Arial, Helvetica, sans-serif"> 
            <tr> 
                <td>
                    <br />
                    <b><font color="#0B610B">{{ L "JobExecuteError" }}</font></b> 
                    <hr size="2" width="100%" align="center" />
                </td>
             </tr> 
             <tr> 
                 <td> 
                     <ul> 
                         {{ if model.tenantname }}
                            <li>{{ L "TenantName" }}&nbsp;:&nbsp;{{ model.tenantname }}</li> 
                         {{ end }}
                         <li>{{ L "JobGroup" }}&nbsp;:&nbsp;{{ model.group }}</li> 
                         <li>{{ L "JobName" }}&nbsp;:&nbsp;{{ model.name }}</li> 
                         <li>{{ L "JobId" }}&nbsp;:&nbsp;{{ model.id }}</li> 
                         <li>{{ L "JobType" }}&nbsp;:&nbsp;{{ model.type }}</li> 
                         <li>{{ L "TriggerTime" }}&nbsp;:&nbsp;{{ model.triggertime }}</li> 
                    </ul> 
                </td> 
            </tr>
            <tr> 
                <td>
                    <b><font color="#0B610B">{{ L "ErrorMessage" }}</font></b> 
                    <hr size="2" width="100%" align="center" />
                </td> 
            </tr> 
            <tr> 
                <td>
                    <pre style="font-size: 11pt; font-family: Tahoma, Arial, Helvetica, sans-serif">{{ model.message }}</pre> 
                    <br />
                </td> 
            </tr> 
            <tr>
                <td>
                    <br /> 
                    <b style="float: right"><font color="#0B610B">{{ model.footer }}</font></b> 
                    <hr size="2" width="100%" align="center" />
                </td>
            </tr>
        </table> 
    </body> 
</html>