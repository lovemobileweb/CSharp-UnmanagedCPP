using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUsingUnmanagedCPP
{
    internal static class UnmanagedCPP
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void CallBack(ref CALLBACK_FIRST_PARAM param, ref uint nCurrentTime);

        [DllImport("UnmanagedCPP.dll")]
        public static extern void INIT(CallBack pfnCallBack);

        [DllImport("UnmanagedCPP.dll")]
        public static extern void STOP();

        [StructLayout(LayoutKind.Sequential)]
        public struct CALLBACK_FIRST_PARAM
        {
            public int param1;
            public int param2;
        } 
    }
}
