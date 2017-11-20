using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isRock.Framework.AOP;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //BMIProcessor BMI =  new BMIProcessor();
            //上面這行改為底下這樣
            BMIProcessor BMI = PolicyInjection.Create<BMIProcessor>();
            //其餘程式碼完全不變
            BMI.Height = 170;
            BMI.Weight = 70;

            //計算BMI
            var ret = BMI.Calculate();

            Console.WriteLine($"\nBMI : {ret}");
            Console.ReadKey();
        }
    }

    public class Logging : PolicyInjectionAttributeBase
    {
        //指定Log File Name
        public string LogFileName { get; set; }
        //override AfterInvoke方法
        public override void AfterInvoke()
        {
            var msg = $"\r\n Method '{this.MethodBase.Name}' has been called - {DateTime.Now.ToString()} ";
            SaveLog(msg);
        }
        //寫入Log
        private void SaveLog(string msg)
        {
            if (System.IO.File.Exists(LogFileName))
            {
                System.IO.File.AppendAllText(LogFileName, msg);
            }
            else
            {
                System.IO.File.WriteAllText(LogFileName, msg);
            }
        }
    }

public class BMIProcessor : PolicyInjectionComponentBase
{
    public int Weight { get; set; }
    public int Height { get; set; }
    public Decimal BMI
    {
        get
        {
            return Calculate();
        }
    }

    [Logging(LogFileName ="log.txt")]
    //計算BMI
    public Decimal Calculate()
    {
        Decimal result = 0;
        Decimal height = (Decimal)Height / 100;
        result = Weight / (height * height);

        return result;
    }
}
}
