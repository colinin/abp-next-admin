using LINGYUN.Abp.PrintDesigner.Templates;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Demo.Templates;

public class BookLabelTemplateSeedContributor : IDataSeedContributor, ITransientDependency
{
    public const string TemplateName = "书籍标签";

    private readonly ITemplateDataSeeder _templateDataSeeder;

    public BookLabelTemplateSeedContributor(ITemplateDataSeeder templateDataSeeder)
    {
        _templateDataSeeder = templateDataSeeder;
    }

    public async virtual Task SeedAsync(DataSeedContext context)
    {
        await _templateDataSeeder.SeedAsync(TemplateName, DesignerDataJson, ExtendedDataJson);
    }

    private const string DesignerDataJson = """
{
  "name": "书籍标签",
  "canvasSize": {
    "width": 320,
    "height": 200
  },
  "unit": "px",
  "zoom": 1,
  "showGrid": true,
  "watermark": {
    "enabled": false,
    "text": "",
    "angle": -30,
    "color": "#000000",
    "opacity": 0.1,
    "size": 24,
    "density": 160
  },
  "variables": [
    { "id": "name", "label": "书名" },
    { "id": "authorName", "label": "作者" },
    { "id": "type", "label": "类型" },
    { "id": "publishDate", "label": "出版日期" },
    { "id": "price", "label": "价格" },
    { "id": "id", "label": "图书编号" },
    { "id": "printTime", "label": "打印时间" }
  ],
  "pages": [
    {
      "id": "page-1",
      "elements": [
        {
          "id": "bl-frame",
          "type": "rect",
          "x": 4,
          "y": 4,
          "width": 312,
          "height": 192,
          "style": {
            "backgroundColor": "transparent",
            "borderWidth": 1,
            "borderColor": "#CBD5E1",
            "borderStyle": "solid"
          }
        },
        {
          "id": "bl-title",
          "type": "text",
          "x": 12,
          "y": 10,
          "width": 120,
          "height": 20,
          "content": "书籍标签",
          "style": {
            "fontSize": 14,
            "color": "#0F172A",
            "fontWeight": "700",
            "verticalAlign": "middle"
          }
        },
        {
          "id": "bl-library",
          "type": "text",
          "x": 136,
          "y": 12,
          "width": 168,
          "height": 14,
          "content": "LINGYUN 图书馆",
          "style": {
            "fontSize": 9,
            "color": "#64748B",
            "verticalAlign": "middle",
            "textAlign": "right"
          }
        },
        {
          "id": "bl-line-1",
          "type": "line",
          "x": 12,
          "y": 36,
          "width": 296,
          "height": 2,
          "style": {
            "borderWidth": 1,
            "borderColor": "#CBD5E1",
            "borderStyle": "dashed"
          }
        },
        {
          "id": "bl-book-label",
          "type": "text",
          "x": 12,
          "y": 44,
          "width": 50,
          "height": 14,
          "content": "书名",
          "style": {
            "fontSize": 8,
            "color": "#64748B",
            "verticalAlign": "middle"
          }
        },
        {
          "id": "bl-book-name",
          "type": "text",
          "x": 64,
          "y": 42,
          "width": 244,
          "height": 20,
          "content": "深入理解计算机系统",
          "variable": "@name",
          "style": {
            "fontSize": 13,
            "color": "#0F172A",
            "fontWeight": "700",
            "verticalAlign": "middle"
          }
        },
        {
          "id": "bl-author-label",
          "type": "text",
          "x": 12,
          "y": 70,
          "width": 50,
          "height": 14,
          "content": "作者",
          "style": {
            "fontSize": 8,
            "color": "#64748B",
            "verticalAlign": "middle"
          }
        },
        {
          "id": "bl-author",
          "type": "text",
          "x": 64,
          "y": 68,
          "width": 120,
          "height": 16,
          "content": "Randal E. Bryant",
          "variable": "@authorName",
          "style": {
            "fontSize": 10,
            "color": "#0F172A",
            "verticalAlign": "middle"
          }
        },
        {
          "id": "bl-type-label",
          "type": "text",
          "x": 190,
          "y": 70,
          "width": 40,
          "height": 14,
          "content": "类型",
          "style": {
            "fontSize": 8,
            "color": "#64748B",
            "verticalAlign": "middle"
          }
        },
        {
          "id": "bl-type",
          "type": "text",
          "x": 232,
          "y": 68,
          "width": 76,
          "height": 16,
          "content": "Science",
          "variable": "@type",
          "style": {
            "fontSize": 9,
            "color": "#0F172A",
            "verticalAlign": "middle"
          }
        },
        {
          "id": "bl-date-label",
          "type": "text",
          "x": 12,
          "y": 92,
          "width": 68,
          "height": 14,
          "content": "出版日期",
          "style": {
            "fontSize": 8,
            "color": "#64748B",
            "verticalAlign": "middle"
          }
        },
        {
          "id": "bl-publish-date",
          "type": "text",
          "x": 84,
          "y": 90,
          "width": 140,
          "height": 16,
          "content": "2016-11-01",
          "variable": "@publishDate",
          "style": {
            "fontSize": 9,
            "color": "#0F172A",
            "verticalAlign": "middle"
          }
        },
        {
          "id": "bl-price-label",
          "type": "text",
          "x": 232,
          "y": 92,
          "width": 34,
          "height": 14,
          "content": "价格",
          "style": {
            "fontSize": 8,
            "color": "#64748B",
            "verticalAlign": "middle"
          }
        },
        {
          "id": "bl-price",
          "type": "text",
          "x": 268,
          "y": 90,
          "width": 40,
          "height": 16,
          "content": "139.00",
          "variable": "@price",
          "style": {
            "fontSize": 9,
            "color": "#0F172A",
            "verticalAlign": "middle",
            "textAlign": "right"
          }
        },
        {
          "id": "bl-id-label",
          "type": "text",
          "x": 12,
          "y": 116,
          "width": 68,
          "height": 14,
          "content": "图书编号",
          "style": {
            "fontSize": 8,
            "color": "#64748B",
            "verticalAlign": "middle"
          }
        },
        {
          "id": "bl-id",
          "type": "text",
          "x": 84,
          "y": 114,
          "width": 150,
          "height": 16,
          "content": "3a24dba8-7038-210e-a20c-cb9f8c4800e5",
          "variable": "@id",
          "style": {
            "fontSize": 7,
            "color": "#64748B",
            "verticalAlign": "middle"
          }
        },
        {
          "id": "bl-qr",
          "type": "qrcode",
          "x": 244,
          "y": 110,
          "width": 64,
          "height": 64,
          "content": "3a24dba8-7038-210e-a20c-cb9f8c4800e5",
          "variable": "@id",
          "style": {}
        },
        {
          "id": "bl-line-2",
          "type": "line",
          "x": 12,
          "y": 140,
          "width": 296,
          "height": 2,
          "style": {
            "borderWidth": 1,
            "borderColor": "#CBD5E1",
            "borderStyle": "dashed"
          }
        },
        {
          "id": "bl-barcode-label",
          "type": "text",
          "x": 12,
          "y": 148,
          "width": 60,
          "height": 12,
          "content": "图书条码",
          "style": {
            "fontSize": 8,
            "color": "#64748B",
            "verticalAlign": "middle"
          }
        },
        {
          "id": "bl-barcode",
          "type": "barcode",
          "x": 12,
          "y": 160,
          "width": 220,
          "height": 30,
          "content": "3a24dba8-7038-210e-a20c-cb9f8c4800e5",
          "variable": "@id",
          "style": {
            "barcodeFormat": "CODE128",
            "showText": false,
            "barcodeWidth": 1,
            "barcodeHeight": 28,
            "fontSize": 8,
            "textAlign": "center"
          }
        },
        {
          "id": "bl-print-time",
          "type": "text",
          "x": 244,
          "y": 178,
          "width": 64,
          "height": 12,
          "content": "2026-01-01 12:00",
          "variable": "@printTime",
          "style": {
            "fontSize": 6,
            "color": "#94A3B8",
            "verticalAlign": "middle",
            "textAlign": "right"
          }
        }
      ]
    }
  ],
  "continuousPaper": false,
  "guides": [],
  "allowDragOutsideCanvas": false,
  "headerHeight": 100,
  "footerHeight": 100,
  "showHeaderLine": false,
  "showFooterLine": false,
  "enableHeaderFooterLineRendering": false,
  "headerLineStyle": "dashed",
  "footerLineStyle": "dashed",
  "headerLineColor": "#f87171",
  "footerLineColor": "#f87171",
  "headerLineWidth": 1,
  "footerLineWidth": 1,
  "headerLineSpanMode": "percent",
  "footerLineSpanMode": "percent",
  "headerLineSpan": 100,
  "footerLineSpan": 100,
  "showMinimap": false,
  "showHistoryPanel": false,
  "canvasBackground": "#ffffff",
  "canvasBackgroundImage": null,
  "pageSpacingX": 0,
  "pageSpacingY": 0,
  "testData": {
    "name": "深入理解计算机系统",
    "authorName": "Randal E. Bryant",
    "type": "Science",
    "publishDate": "2016-11-01",
    "price": "139.00",
    "id": "3a24dba8-7038-210e-a20c-cb9f8c4800e5",
    "printTime": "2026-01-01 12:00"
  }
}
""";

    private const string ExtendedDataJson = """
{
  "availableVariables": [
    { "id": "name", "label": "书名" },
    { "id": "authorName", "label": "作者" },
    { "id": "type", "label": "类型" },
    { "id": "publishDate", "label": "出版日期" },
    { "id": "price", "label": "价格" },
    { "id": "id", "label": "图书编号" },
    { "id": "printTime", "label": "打印时间" }
  ]
}
""";
}
