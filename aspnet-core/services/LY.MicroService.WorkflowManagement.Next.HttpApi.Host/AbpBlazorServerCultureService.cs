﻿using Elsa.Studio.Localization.Services;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace LY.MicroService.WorkflowManagement.Next;

public class AbpBlazorServerCultureService(NavigationManager navigationManager) : ICultureService
{
    public Task ChangeCultureAsync(CultureInfo culture)
    {
        if (CultureInfo.CurrentUICulture.Name != culture.Name)
        {
            var cultureString = culture.Name;
            var uri = new Uri(navigationManager.Uri).GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);

            // Set culture of the current thread.
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            var cultureEscaped = Uri.EscapeDataString(cultureString);
            // var uriEscaped = Uri.EscapeDataString(uri);

            navigationManager.NavigateTo(
                $"Culture/Set?culture={cultureEscaped}&redirectUri=/",
                forceLoad: true);
        }

        return Task.CompletedTask;
    }
}
