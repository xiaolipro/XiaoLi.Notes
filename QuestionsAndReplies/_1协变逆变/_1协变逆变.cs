using Xunit.Abstractions;

namespace QuestionsAndReplies._1协变逆变;

public class _1协变逆变
{
    private static ITestOutputHelper _testOutputHelper;

    public _1协变逆变(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    /*
     * 协变：儿子-》父亲
     * 逆变：父亲-》儿子
     */

    [Fact]
    public void 协变()
    {
        var son = new Son();
        Father father = son;

        var sonForPeople = new People<Son>();
        //People<Father> fatherForPeople = sonForPeople;
    }


    [Fact]
    public void 逆变()
    {
        var father = new Father();
        //Son son = father;
    }


    class Father
    {
    }

    class Son : Father
    {
    }

    class People<T>
    {
        void Think()
        {
            _testOutputHelper.WriteLine(typeof(T).Name+"在思考");
        }
    }
}