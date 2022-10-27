using System.Net;
using Xunit.Abstractions;

namespace ThreadingBase.进阶;

public class _1APM
{
    private readonly ITestOutputHelper _testOutputHelper;

    public _1APM(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void APM()
    {
        var uri = new Uri("https://www.albahari.com/threading/part3.aspx");
        Func<Uri, int> f = CalcUriStringCount;
        var res = f.BeginInvoke(uri, null, null);
        // do something
        _testOutputHelper.WriteLine("我可以做别的事情");
        _testOutputHelper.WriteLine("共下载字符数：" + f.EndInvoke(res));
    }

    int CalcUriStringCount(Uri uri)
    {
        var client = new WebClient();
        var res = client.DownloadString(uri);
        return res.Length;
    }


    [Fact]
    void 回调()
    {
        var uri = new Uri("https://www.albahari.com/threading/part3.aspx");
        Func<Uri, int> func = CalcUriStringCount;
        var res = func.BeginInvoke(uri, new AsyncCallback(res =>
        {
            var target = res.AsyncState as Func<string, int>;
            _testOutputHelper.WriteLine("共下载字符数：" + target!.EndInvoke(res));
            _testOutputHelper.WriteLine("异步状态：" + res.AsyncState);
        }), func);
        // do something
        _testOutputHelper.WriteLine("我可以做别的事情");

        func.EndInvoke(res);
    }
}