<table width="95%" cellpadding="0" cellspacing="0" style="font-size: 11pt; font-family: Tahoma, Arial, Helvetica, sans-serif"> 
    <tr> 
        <td>
            <br /> <b><font color="#0B610B">{{ header }}</font></b> 
            <hr size="2" width="100%" align="center" />
        </td>
        </tr> 
        <tr> 
            <td> 
                <ul> 
                    <li>Type       &nbsp;：&nbsp;{{ type }}</li> 
                    <li>Message    &nbsp;：&nbsp;{{ message }}</li> 
                    <li>Alarm level&nbsp;：&nbsp;{{ loglevel }}</li> 
                    <li>TriggerTime&nbsp;：&nbsp;{{ createTime }}</li> 
            </ul> 
        </td> 
    </tr> 
    <tr> 
        <td>
            <b><font color="#0B610B">Stack trace</font></b> 
            <hr size="2" width="100%" align="center" />
        </td> 
    </tr> 
    <tr> 
        <td>
            <pre style="font-size: 11pt; font-family: Tahoma, Arial, Helvetica, sans-serif">{{ stacktrace }}</pre> 
            <br />
        </td> 
    </tr> 
    <tr>
        <td>
            <br /> 
            <b style="float: right"><font color="#0B610B">{{ footer }}</font></b> 
            <hr size="2" width="100%" align="center" />
        </td>
    </tr>
</table> 