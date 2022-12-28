using System.Collections;
using Xunit.Abstractions;

namespace QuestionsAndReplaies;

public class _1协变逆变replay
{
    private static ITestOutputHelper _testOutputHelper;

    public _1协变逆变replay(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    //https://learn.microsoft.com/zh-cn/search/?scope=.NET&terms=%E5%8D%8F%E5%8F%98%E9%80%86%E5%8F%98

    /* Variant 变种
     * Covariance 协变：儿子-》父亲
     * Contravariance 逆变：父亲-》儿子
     * 限制：只有接口类型和委托类型才能具有 Variant 类型参数。 接口或委托类型可以同时具有协变和逆变类型参数。
     */
    
    [Fact]
    public void 协变()
    {
        var son = new Son();
        Father father = son;
        
        var sonForPeople = new People<Son>();
        sonForPeople.Think();
        IPeople<Father> fatherForPeople = sonForPeople;
        fatherForPeople.Think();
    }
    
    // [Fact]
    // public void 逆变()
    // {
    //     object[] array = new String[10];  
    //     var father = new Father();
    //
    //     var fatherForPeople = new People<Father>();
    //     fatherForPeople.Think(father);
    //     IPeople<Son> sonForPeople = fatherForPeople;
    //     sonForPeople.Think(new Son());
    // }

    class Father
    {
    }

    class Son : Father
    {
    }

    class People<T> : IPeople<T>
    {
        public void Think()
        {
            _testOutputHelper.WriteLine(typeof(T).Name + "在思考");
        }
    }

    interface IPeople<out T>
    {
        void Think();
    }
}