using System;
using System.Threading;

class Program
{   
    // 委托类型
    public delegate void TickEventHandler(object sender, EventArgs e);
    public delegate void AlarmEventHandler(object sender, EventArgs e);
    // 闹钟类
    class AlarmClock
    {
        private DateTime alarmTime;
        public event TickEventHandler Tick;
        public event AlarmEventHandler Alarm;
        //响铃时间
        public AlarmClock(DateTime alarmTime)
        {
            this.alarmTime = alarmTime;
        }

        public void Start()
        {
            while (true)
            {
                DateTime now = DateTime.Now;
                OnTick(now);

                if (now >= alarmTime)
                {
                    OnAlarm();
                    break;
                }
                Thread.Sleep(1000);//每秒检查
            }
        }

        protected virtual void OnTick(DateTime currentTime)
        {
            Tick?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnAlarm()
        {
            Alarm?.Invoke(this, EventArgs.Empty);
        }
    }

    static void Main(string[] args)
    {
        // 设置响铃时间为五秒钟后
        DateTime alarmTime = DateTime.Now.AddSeconds(5);
        AlarmClock clock = new AlarmClock(alarmTime);

        // 订阅事件
        clock.Tick += (sender, e) =>
        {
            Console.SetCursorPosition(0, Console.CursorTop); // 将光标移到行首
            Console.Write($"当前时间: {DateTime.Now:HH:mm:ss}    "); // 覆盖当前行，末尾加空格清除残留字符
        };
        clock.Alarm += (sender, e) => Console.WriteLine("\n响铃！时间到！"); // 换行显示响铃

        Console.WriteLine($"闹钟设置为: {alarmTime:HH:mm:ss}");
        clock.Start();
    }
}
