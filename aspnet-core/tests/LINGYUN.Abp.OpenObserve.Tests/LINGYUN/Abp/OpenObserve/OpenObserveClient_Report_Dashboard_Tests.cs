using LINGYUN.Abp.OpenObserve.Models.Report;
using Shouldly;
using System;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.OpenObserve;

public class OpenObserveClient_Report_Dashboard_Tests : AbpOpenObserveTestBase
{
    private static ReportFrequency CreateFrequency()
    {
        return new ReportFrequency { Type = "weeks", Interval = 1, Cron = string.Empty, AlignTime = false };
    }

    private static ReportDashboardReference[] CreateDashboards()
    {
        return
        [
            new ReportDashboardReference
            {
                Dashboard = "3hQ8",
                Folder = "default",
                Tabs = ["default"],
                TimeRange = new ReportTimeRange { Type = "relative", Period = "1w" },
            },
        ];
    }

    private static ReportDestination[] CreateDestinations()
    {
        return [new ReportDestination { Email = "team@abp.io" }];
    }

    [Fact]
    public async Task Should_Get_Reports_Without_Pagination_Wrapper()
    {
        HttpMessageHandler.EnqueueOk("[{\"report_id\":\"r1\",\"name\":\"weekly\",\"folder_id\":\"default\",\"enabled\":true}]");
        var client = CreateClient();

        var result = await client.GetReportsAsync(folder: "default", dashboardId: "d1", cache: true);

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Get);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/v2/default/reports?folder=default&dashboard_id=d1&cache=true");
        result.Length.ShouldBe(1);
        result[0].ReportId.ShouldBe("r1");
    }

    [Fact]
    public async Task Should_Create_Report()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"message\":\"Report saved\"}");
        var client = CreateClient();

        var startTime = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        var expectedStart = (startTime.Ticks - DateTime.UnixEpoch.Ticks) / 10;

        var result = await client.CreateReportAsync(
            "weekly-overview",
            CreateFrequency(),
            CreateDashboards(),
            CreateDestinations(),
            title: "Weekly Overview",
            description: "Weekly summary",
            message: "attached",
            startTime: startTime,
            timezone: "UTC",
            timezoneOffset: 0,
            folder: "f_1");

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Post);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/v2/default/reports?folder=f_1");

        var body = HttpMessageHandler.LastRequest.Body!;
        body.ShouldContain("\"name\":\"weekly-overview\"");
        body.ShouldContain("\"title\":\"Weekly Overview\"");
        body.ShouldContain("\"org_id\":\"default\"");
        body.ShouldContain($"\"start\":{expectedStart}");
        body.ShouldContain("\"frequency\":{\"type\":\"weeks\",\"interval\":1,\"cron\":\"\",\"align_time\":false}");
        body.ShouldContain("\"dashboards\":[{\"dashboard\":\"3hQ8\",\"folder\":\"default\",\"tabs\":[\"default\"],\"timerange\":{\"type\":\"relative\",\"period\":\"1w\",\"from\":0,\"to\":0}}]");
        body.ShouldContain("\"destinations\":[{\"email\":\"team@abp.io\"}]");
        result.Message.ShouldBe("Report saved");
    }

    [Fact]
    public async Task Should_Update_Report()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"message\":\"Report updated\"}");
        var client = CreateClient();

        await client.UpdateReportAsync(
            "r1",
            "weekly-overview",
            CreateFrequency(),
            CreateDashboards(),
            CreateDestinations(),
            folder: "f_archive");

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Put);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/v2/default/reports/r1?folder=f_archive");
    }

    [Fact]
    public async Task Should_Get_Report()
    {
        HttpMessageHandler.EnqueueOk("{\"report_id\":\"r1\",\"name\":\"weekly\"}");
        var client = CreateClient();

        var result = await client.GetReportAsync("r1", folder: "default");

        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/v2/default/reports/r1?folder=default");
        result.Name.ShouldBe("weekly");
    }

    [Fact]
    public async Task Should_Delete_Report()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"message\":\"Report deleted\"}");
        var client = CreateClient();

        var result = await client.DeleteReportAsync("r1", folder: "default");

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Delete);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/v2/default/reports/r1?folder=default");
        result.Message.ShouldBe("Report deleted");
    }

    [Fact]
    public async Task Should_Bulk_Delete_Reports()
    {
        HttpMessageHandler.EnqueueOk("{\"successful\":[\"r1\",\"r2\"],\"unsuccessful\":[],\"err\":null}");
        var client = CreateClient();

        var result = await client.BulkDeleteReportsAsync(["r1", "r2"]);

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Delete);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/v2/default/reports/bulk");
        HttpMessageHandler.LastRequest.Body!.ShouldBe("{\"ids\":[\"r1\",\"r2\"]}");
        result.Successful.Length.ShouldBe(2);
        result.Unsuccessful.Length.ShouldBe(0);
    }

    [Fact]
    public async Task Should_Move_Reports()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"message\":\"Report moved\"}");
        var client = CreateClient();

        var result = await client.MoveReportsAsync(["r1"], "f_archive");

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Patch);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/v2/default/reports/move");
        HttpMessageHandler.LastRequest.Body!.ShouldBe("{\"report_ids\":[\"r1\"],\"dst_folder_id\":\"f_archive\"}");
        result.Message.ShouldBe("Report moved");
    }

    [Fact]
    public async Task Should_Enable_Report()
    {
        HttpMessageHandler.EnqueueOk("{\"enabled\":true}");
        var client = CreateClient();

        var result = await client.EnableReportAsync("r1", value: true, folder: "default");

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Patch);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/v2/default/reports/r1/enable?folder=default&value=true");
        result.Enabled.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Trigger_Report()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"message\":\"Report triggered\"}");
        var client = CreateClient();

        var result = await client.TriggerReportAsync("r1", folder: "default");

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Put);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/v2/default/reports/r1/trigger?folder=default");
        result.Message.ShouldBe("Report triggered");
    }

    [Fact]
    public async Task Should_Add_Dashboard_Panel()
    {
        HttpMessageHandler.EnqueueOk("{\"panel\":{\"id\":\"p1\"},\"hash\":\"h2\",\"tabId\":\"default\"}");
        var client = CreateClient();

        var panel = JsonNode.Parse("{\"type\":\"bar\",\"title\":\"Requests\"}")!;

        var result = await client.AddDashboardPanelAsync("d1", "h1", panel, tabId: "default", folder: "default");

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Post);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/dashboards/d1/panels?hash=h1&folder=default");
        HttpMessageHandler.LastRequest.Body!.ShouldBe("{\"panel\":{\"type\":\"bar\",\"title\":\"Requests\"},\"tabId\":\"default\"}");
        result.Hash.ShouldBe("h2");
    }

    [Fact]
    public async Task Should_Update_Dashboard_Panel()
    {
        HttpMessageHandler.EnqueueOk("{\"panel\":{\"id\":\"p1\"},\"hash\":\"h3\"}");
        var client = CreateClient();

        var panel = JsonNode.Parse("{\"type\":\"line\"}")!;

        var result = await client.UpdateDashboardPanelAsync("d1", "p1", "h2", panel);

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Put);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/dashboards/d1/panels/p1?hash=h2");
        HttpMessageHandler.LastRequest.Body!.ShouldBe("{\"panel\":{\"type\":\"line\"}}");
        result.Hash.ShouldBe("h3");
    }

    [Fact]
    public async Task Should_Delete_Dashboard_Panel()
    {
        HttpMessageHandler.EnqueueOk("{\"hash\":\"h4\",\"panelId\":\"p1\"}");
        var client = CreateClient();

        var result = await client.DeleteDashboardPanelAsync("d1", "p1", "h3", tabId: "t1");

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Delete);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/dashboards/d1/panels/p1?hash=h3&tabId=t1");
        result.PanelId.ShouldBe("p1");
    }
}
