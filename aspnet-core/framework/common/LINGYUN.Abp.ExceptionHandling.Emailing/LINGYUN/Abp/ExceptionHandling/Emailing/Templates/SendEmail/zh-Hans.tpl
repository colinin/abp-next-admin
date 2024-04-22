<!DOCTYPE html> 
<html lang="zh-Hans"> 
    <head> 
        <meta charset="UTF-8"> 
        <title>{{ model.title }}</title> 
    </head> 
    <body leftmargin="8" marginwidth="0" topmargin="8" marginheight="4" offset="0"> 
        <table width="95%" cellpadding="0" cellspacing="0" style="font-size: 11pt; font-family: Tahoma, Arial, Helvetica, sans-serif"> 
            <tr> 
                <td>
                    <br /> <b><font color="#0B610B">{{ model.header }}</font></b> 
                    <hr size="2" width="100%" align="center" />
                </td>
             </tr> 
             <tr> 
                 <td> 
                     <ul> 
                         <li>异常类型&nbsp;：&nbsp;{{ model.type }}</li> 
                         <li>异常信息&nbsp;：&nbsp;{{ model.message }}</li> 
                         <li>告警级别&nbsp;：&nbsp;{{ model.loglevel }}</li> 
                         <li>触发时间&nbsp;：&nbsp;{{ model.triggertime }}</li> 
                    </ul> 
                </td> 
            </tr>
            {{ if model.sendstacktrace }}
            <tr> 
                <td>
                    <b><font color="#0B610B">异常堆栈</font></b> 
                    <hr size="2" width="100%" align="center" />
                </td> 
            </tr> 
            <tr> 
                <td>
                    <pre style="font-size: 11pt; font-family: Tahoma, Arial, Helvetica, sans-serif">{{ model.stacktrace }}</pre> 
                    <br />
                </td> 
            </tr> 
            {{ end }}
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