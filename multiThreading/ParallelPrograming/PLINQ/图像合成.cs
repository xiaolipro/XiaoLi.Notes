using System;
using System.Linq;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.PLINQ;

public class 图像合成
{
    private readonly ITestOutputHelper _testOutputHelper;

    public 图像合成(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void 合成()
    {
        Camera[] cameras = Enumerable.Range(0, 4) // 创建 4 个摄像头对象
            .Select(i => new Camera(i))
            .ToArray();

        while (true)
        {
            string[] data = cameras
                .AsParallel()
                .AsOrdered()  // 这里这有四个元素，追踪的成本几乎可以忽略不计算
                .WithDegreeOfParallelism(4)
                .Select(c => c.GetNextFrame()).ToArray();

            _testOutputHelper.WriteLine(string.Join(", ", data)); // 显示数据...
        }
    }
}

class Camera
{
    public readonly int CameraID;

    public Camera(int cameraID)
    {
        CameraID = cameraID;
    }

    // 获取来自摄像头的图像: 返回一个字符串来代替图像
    public string GetNextFrame()
    {
        Thread.Sleep(123); // 模拟获取图像的时间
        return "Frame from camera " + CameraID;
    }
}