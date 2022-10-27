using System.Net;
using Xunit.Abstractions;

namespace ThreadingBase.进阶;

public class _2EAP
{
    private readonly ITestOutputHelper _testOutputHelper;

    public _2EAP(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void EAP()
    {
        var client = new WebClient();
        client.DownloadStringCompleted += (sender, args) =>
        {
            if (args.Cancelled) _testOutputHelper.WriteLine("已取消");
            else if (args.Error != null) _testOutputHelper.WriteLine("发生异常：" + args.Error.Message);
            else
            {
                _testOutputHelper.WriteLine("共下载字符数：" + args.Result.Length);
                // 可以在这里更新UI。。
            }
        };
        _testOutputHelper.WriteLine("我在做别的事情");
        client.DownloadStringAsync(new Uri("https://www.albahari.com/threading/part3.aspx"));
        _testOutputHelper.WriteLine("我在做别的事情");
    }
}