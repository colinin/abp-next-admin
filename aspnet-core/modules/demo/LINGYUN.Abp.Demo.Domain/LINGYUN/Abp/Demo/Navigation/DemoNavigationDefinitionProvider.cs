using LINGYUN.Abp.UI.Navigation;
using Volo.Abp.Data;

namespace LINGYUN.Abp.Demo.Navigation;

public class DemoNavigationDefinitionProvider : NavigationDefinitionProvider
{
    public override void Define(INavigationDefinitionContext context)
    {
        var demo = new ApplicationMenu(
            name: "Vben5Demo",
            displayName: "演示功能",
            url: "/demo",
            component: "",
            description: "演示功能",
            order: 999,
            icon: "carbon:demo")
            .SetProperty("title", "abp.demo.title");

        demo.AddItem(
            new ApplicationMenu(
                name: "Vben5DemoBooks",
                displayName: "书籍列表",
                url: "/demo/books",
                component: "/demos/books/index",
                icon: "carbon:book",
                description: "书籍列表")
            .SetProperty("title", "abp.demo.books")
        );

        context.Add(new NavigationDefinition(demo));
    }
}
