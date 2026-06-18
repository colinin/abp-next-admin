<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>{{L "InactiveUserDeactivationNotifier"}}</title>
</head>
<body style="font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; background-color: #f5f5f5; margin: 0; padding: 20px;">
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td align="center">
                <table width="500" cellpadding="0" cellspacing="0" border="0" style="background-color: #ffffff; border-radius: 4px;">
                    <tr>
                        <td style="padding: 30px 30px 20px 30px;">
                            <h2 style="margin: 0 0 8px 0; font-size: 20px; color: #333;">{{L "InactiveUserDeactivationHeader"}}</h2>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 0 30px 20px 30px;">
                            <p style="margin: 0 0 16px 0; font-size: 14px; color: #555; line-height: 1.5;"><strong>{{model.email}}</strong></p>
                            <p style="margin: 0 0 16px 0; font-size: 14px; color: #555; line-height: 1.5;">
                                {{L "InactiveUserDeactivationNotice"}}<strong>{{model.inactivityDays}}</strong>{{L "InactiveUserDeactivationWarning"}}
                                <strong>{{model.reactivateDate | date.to_string '%F'}}</strong>{{L "InactiveUserDeactivationAction"}}
                             </p>
                            <p style="margin: 20px 0 16px 0; font-size: 14px; color: #555; line-height: 1.5;">{{L "NoReactionClosure"}}</p>
                            <p style="margin: 20px 0 0 0; font-size: 12px; color: #999;">{{L "AutomatedNotification"}}</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>