using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUsingUnmanagedCPP
{
    class Program
    {
        #region Console Close Handler
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        private delegate bool EventHandler(CtrlType sig);
        static EventHandler closeConsoleHandler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool CloseConsoleHandler(CtrlType sig)
        {
            switch (sig)
            {
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                    try
                    {
                        UnmanagedCPP.STOP();
                        exitFlag = true;
                    } catch (Exception) { }
                    break;
                default:
                    return false;
            }
            return true;
        }
        #endregion

        #region Call UnmanagedCPP
        static bool exitFlag = false;
        static void CallBackProc(ref UnmanagedCPP.CALLBACK_FIRST_PARAM param, ref uint nCurrentTime)
        {
            Console.Out.WriteLine("CurrentTime(second)={0}, Struct.Param1={1}, Struct.Param2={2}", nCurrentTime, param.param1, param.param2);
        }

        static void Main(string[] args)
        {
            closeConsoleHandler += new EventHandler(CloseConsoleHandler);
            SetConsoleCtrlHandler(closeConsoleHandler, true);

            Console.Out.WriteLine("Please type \"quit\" to quit this program!");
            try
            {
                UnmanagedCPP.CallBack callback = CallBackProc;
                UnmanagedCPP.INIT(callback);
                string strInput = Console.In.ReadLine();
                while (exitFlag == false && string.Compare(strInput, "quit", true) != 0)
                {
                    strInput = Console.In.ReadLine();
                    System.Threading.Thread.Sleep(10);
                }
                UnmanagedCPP.STOP();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Error : {0}", ex.Message);
            }
        }
        #endregion
    }
}
