using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.Notifications.WeChat.MiniProgram
{
    /// <summary>
    /// 微信小程序订阅消息渠道数据测试
    /// </summary>
    /// <remarks>
    /// 统一写法: 先 new 渠道数据 → 通过 <c>IJsonSerializer</c> 序列化写入通知数据 →
    /// 再让渠道实际发布, 断言捕获到的模板消息。
    /// </remarks>
    public class WeChatMiniProgramChannelData_Tests : AbpNotificationsWeChatMiniProgramTestsBase
    {
        private const string Provider = WeChatMiniProgramNotificationPublishProvider.ProviderName;

        private FakeSubscribeMessager Messager
            => (FakeSubscribeMessager)GetRequiredService<LINGYUN.Abp.WeChat.MiniProgram.Messages.ISubscribeMessager>();

        // ---------- 1. 解析渠道数据 ----------

        [Fact]
        public void Should_Resolve_New_Prefixed_Keys()
        {
            var data = WriteChannelData(new WeChatMiniProgramSubscribeChannelData
            {
                TemplateId = "TPL_FROM_PREFIXED_KEY",
                RedirectPage = "pages/order/detail?id=123",
                WeAppState = "developer",
                WeAppLanguage = "zh_CN"
            });

            var payload = ChannelDataResolver.Resolve<WeChatMiniProgramSubscribeChannelData>(data);

            payload.ShouldNotBeNull();
            payload!.TemplateId.ShouldBe("TPL_FROM_PREFIXED_KEY");
            payload.RedirectPage.ShouldBe("pages/order/detail?id=123");
            payload.WeAppState.ShouldBe("developer");
            payload.WeAppLanguage.ShouldBe("zh_CN");
        }

        [Fact]
        public void Should_Declare_Payload_When_Only_TemplateId_Is_Written()
        {
            // 跳转参数是"可选读取项", 不得参与存在性判定
            var data = WriteChannelData(new WeChatMiniProgramSubscribeChannelData
            {
                TemplateId = "TPL_ONLY"
            });

            ChannelDataResolver.IsDeclared(data, typeof(WeChatMiniProgramSubscribeChannelData))
                .ShouldBeTrue();
            ChannelDataResolver.Resolve<WeChatMiniProgramSubscribeChannelData>(data)!
                .RedirectPage.ShouldBeNull();
        }

        [Fact]
        public void Should_Match_Prefixed_Key_Case_Insensitively()
        {
            var data = new NotificationData();
            data.TrySetData(WeChatMiniProgramNotificationDataKeys.TemplateId, "TPL_CASE");

            ChannelDataResolver.IsDeclared(data, typeof(WeChatMiniProgramSubscribeChannelData))
                .ShouldBeTrue();
            ChannelDataResolver.Resolve<WeChatMiniProgramSubscribeChannelData>(data)!
                .TemplateId.ShouldBe("TPL_CASE");

            // 换一种大小写写入也应命中
            var upper = new NotificationData();
            upper.TrySetData("WX-MP:TEMPLATE_ID", "TPL_UPPER");

            ChannelDataResolver.Resolve<WeChatMiniProgramSubscribeChannelData>(upper)!
                .TemplateId.ShouldBe("TPL_UPPER");
        }

        // ---------- 2. 渠道实际发布(端到端) ----------

        [Fact]
        public async Task Should_Send_With_Resolved_Channel_Data()
        {
            Messager.Clear();

            var data = WriteChannelData(new WeChatMiniProgramSubscribeChannelData
            {
                TemplateId = "TPL_SEND",
                RedirectPage = "pages/a/b",
                WeAppState = "developer",
                WeAppLanguage = "en_US"
            });

            await PublishAsync(data, NewUser());

            Messager.SentTemplates.Count.ShouldBe(1);

            var sent = Messager.SentTemplates[0];
            sent.TemplateId.ShouldBe("TPL_SEND");
            sent.Page.ShouldBe("pages/a/b");
            sent.State.ShouldBe("developer");
            sent.Lang.ShouldBe("en_US");
        }

        [Fact]
        public async Task Should_Fall_Back_To_Default_TemplateId_When_Nothing_Written()
        {
            Messager.Clear();

            await PublishAsync(new NotificationData(), NewUser());

            Messager.SentTemplates.Count.ShouldBe(1);
            Messager.SentTemplates[0].TemplateId.ShouldBe("DEFAULT_TEMPLATE_ID");
        }

        [Fact]
        public async Task Should_Read_Legacy_Bare_Keys_As_Fallback()
        {
            Messager.Clear();

            // 历史遗留的无前缀键, 已安装的生产者可能仍在写入
            // (测试历史兼容路径, 因此显式压制弃用告警)
            var data = new NotificationData();
#pragma warning disable CS0618 // Type or member is obsolete: this test verifies the legacy compatibility fallback
            data.TrySetData(WeChatMiniProgramNotificationDataKeys.Legacy.TemplateId, "TPL_LEGACY");
            data.TrySetData(WeChatMiniProgramNotificationDataKeys.Legacy.RedirectPage, "pages/legacy");
#pragma warning restore CS0618

            await PublishAsync(data, NewUser());

            Messager.SentTemplates.Count.ShouldBe(1);
            Messager.SentTemplates[0].TemplateId.ShouldBe("TPL_LEGACY");
            Messager.SentTemplates[0].Page.ShouldBe("pages/legacy");
        }

        [Fact]
        public async Task Should_Prefer_Prefixed_Key_Over_Legacy_Key()
        {
            Messager.Clear();

            var data = WriteChannelData(new WeChatMiniProgramSubscribeChannelData
            {
                TemplateId = "TPL_NEW"
            });
            // 同时写入遗留键, 带前缀的必须优先
#pragma warning disable CS0618 // Type or member is obsolete: this test verifies the prefixed key wins over the legacy key
            data.TrySetData(WeChatMiniProgramNotificationDataKeys.Legacy.TemplateId, "TPL_LEGACY");
#pragma warning restore CS0618

            await PublishAsync(data, NewUser());

            Messager.SentTemplates[0].TemplateId.ShouldBe("TPL_NEW");
        }

        [Fact]
        public async Task Should_Not_Send_When_TemplateId_Missing_And_No_Default()
        {
            Messager.Clear();

            // 覆盖默认模板为空
            var options = GetRequiredService<Microsoft.Extensions.Options.IOptions<AbpNotificationsWeChatMiniProgramOptions>>().Value;
            var original = options.DefaultTemplateId;
            options.DefaultTemplateId = string.Empty;

            try
            {
                var context = await PublishAsync(new NotificationData(), NewUser());

                Messager.SentTemplates.Count.ShouldBe(0);
                context.Reason.ShouldNotBeNullOrWhiteSpace();
            }
            finally
            {
                options.DefaultTemplateId = original;
            }
        }

        // ---------- 3. 模板数据取自属性包 ----------

        [Fact]
        public async Task Should_Pass_Extra_Properties_As_Template_Data()
        {
            Messager.Clear();

            var data = WriteChannelData(new WeChatMiniProgramSubscribeChannelData
            {
                TemplateId = "TPL_DATA"
            });
            data.TrySetData("character_string1", "SO20251001001");
            data.TrySetData("thing2", "顺丰速运");

            await PublishAsync(data, NewUser());

            var templateData = Messager.SentTemplates[0].Data;
            templateData.ShouldNotBeNull();
            templateData!.ShouldContainKey("character_string1");
            templateData["character_string1"].ShouldBe("SO20251001001");
            templateData.ShouldContainKey("thing2");
        }

        // ---------- 辅助 ----------

        private static UserIdentifier NewUser() => new(Guid.NewGuid(), "tester");

        private async Task<NotificationPublishContext> PublishAsync(
            NotificationData data,
            UserIdentifier user)
        {
            var provider = GetRequiredService<WeChatMiniProgramNotificationPublishProvider>();

            var notification = new NotificationInfo
            {
                Name = "Test",
                ContentType = NotificationContentType.Text,
                Data = data
            };
            notification.SetId(1);

            var context = new NotificationPublishContext(notification, new[] { user });

            await provider.PublishAsync(context);

            return context;
        }
    }
}
