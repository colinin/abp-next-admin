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
                    <li>异常类型&nbsp;：&nbsp;{{ model.title }}</li> 
                    <li>异常信息&nbsp;：&nbsp;{{ model.message }}</li> 
                    <li>告警级别&nbsp;：&nbsp;{{ model.loglevel }}</li> 
                    <!-- 全局变量 creationTime -->
                    <li>触发时间&nbsp;：&nbsp;{{ creationTime }}</li> 
            </ul> 
        </td> 
    </tr>
    <tr> 
        <td>
            <b><font color="#0B610B">异常堆栈</font></b> 
            <hr size="2" width="100%" align="center" />
        </td> 
    </tr> 
    <tr> 
        <td>
            <pre style="font-size: 11pt; font-family: Tahoma, Arial, Helvetica, sans-serif">{{ model.stackTrace }}</pre> 
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