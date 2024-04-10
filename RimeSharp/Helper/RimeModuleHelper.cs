//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Runtime.InteropServices;

//namespace RimeSharp.Helper;

//[StructLayout(LayoutKind.Sequential)]
//internal static class RimeModuleHelper
//{
//    public struct RimeModuleIn
//    {
//        public int data_size;

//        [MarshalAs(UnmanagedType.LPUTF8Str)]
//        public string module_name;
//        public RimeModule.DType initialize;
//        public RimeModule.DType finalize;
//        //RimeCustomApi*
//        public IntPtr get_api;
//    }

//    public static RimeModule MarshalFromIntPtr(IntPtr ptr)
//    {
//        var obj = Marshal.PtrToStructure<RimeModuleIn>(ptr);

//        var context = new RimeModule
//        {
//            data_size = obj.data_size,
//            module_name = obj.module_name,
//            initialize = obj.initialize,
//            finalize = obj.finalize,
//            get_api = Marshal.PtrToStructure<RimeCustomApi>(obj.get_api)
//        };

//        return context;
//    }
//}
