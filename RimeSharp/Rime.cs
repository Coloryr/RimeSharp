using RimeSharp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RimeSharp;


public delegate void RimeNotificationHandler(IntPtr context_object, IntPtr session_id, [MarshalAs(UnmanagedType.LPUTF8Str)] string message_type, [MarshalAs(UnmanagedType.LPUTF8Str)] string message_value);

public static partial class Rime
{
    public const string LibName = "rime";

    public static RimeTraits Init(string dir, RimeNotificationHandler handler)
    {
        if (!dir.EndsWith('\\') && !dir.EndsWith('/'))
        {
            dir += "/";
        }
        var dir1 = Path.GetFullPath(dir + "data/");
        var dir2 = Path.GetFullPath(dir + "log/");
        var dir3 = Path.GetFullPath(dir + "build/");

        Directory.CreateDirectory(dir1);
        Directory.CreateDirectory(dir2);
        Directory.CreateDirectory(dir3);

        var traits = new RimeTraits();

        traits.data_size = Marshal.SizeOf(traits) - sizeof(int);
        traits.app_name = "RimeSharp";
        traits.min_log_level = 0;

        traits.shared_data_dir = traits.user_data_dir = dir1;
        traits.log_dir = dir2;
        traits.prebuilt_data_dir = traits.staging_dir = dir3;

        IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(traits));

        Marshal.StructureToPtr(traits, pnt, false);

        RimeSetup(pnt);

        RimeSetNotificationHandler(handler, 0);

        RimeInitialize(pnt);

        if (RimeStartMaintenance(true))
        {
            RimeJoinMaintenanceThread();
        }

        traits = Marshal.PtrToStructure<RimeTraits>(pnt);
        Marshal.FreeHGlobal(pnt);
        return traits;
    }

    /// <summary>
    /// Call this function before accessing any other API.
    /// </summary>
    /// <param name="traits"></param>
    [DllImport(LibName)]
    private static extern void RimeSetup(IntPtr traits);

    /// <summary>
    /// Pass a C-string constant in the format "rime.x"
    /// where 'x' is the name of your application.
    /// Add prefix "rime." to ensure old log files are automatically cleaned.
    /// </summary>
    /// <param name="app_name"></param>
    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    [Obsolete("Use RimeSetup() instead")]
    public static partial void RimeSetupLogging(string app_name);

    /// <summary>
    /// - on loading schema:
    ///   + message_type="schema", message_value="luna_pinyin/Luna Pinyin"
    /// - on changing mode:
    ///   + message_type="option", message_value="ascii_mode"
    ///   + message_type="option", message_value="!ascii_mode"
    /// - on deployment:
    ///   + session_id = 0, message_type="deploy", message_value="start"
    ///   + session_id = 0, message_type="deploy", message_value="success"
    ///   + session_id = 0, message_type="deploy", message_value="failure"
    /// 
    ///   handler will be called with context_object as the first parameter
    /// every time an event occurs in librime, until RimeFinalize() is called.
    /// when handler is NULL, notification is disabled.
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="context_object"></param>
    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    public static partial void RimeSetNotificationHandler(RimeNotificationHandler handler, IntPtr context_object);

    [LibraryImport(LibName)]
    private static partial void RimeInitialize(IntPtr traits);

    [LibraryImport(LibName)]
    public static partial void RimeFinalize();

    [LibraryImport(LibName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RimeStartMaintenance([MarshalAs(UnmanagedType.Bool)] bool full_check);

    [Obsolete("Use RimeStartMaintenance(full_check = False) instead.")]
    [LibraryImport(LibName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RimeStartMaintenanceOnWorkspaceChange();
    [LibraryImport(LibName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RimeIsMaintenancing();
    [LibraryImport(LibName)]
    public static partial void RimeJoinMaintenanceThread();

    [DllImport(LibName)]
    public static extern void RimeDeployerInitialize(IntPtr traits);
    [LibraryImport(LibName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RimePrebuildAllSchemas();
    [LibraryImport(LibName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RimeDeployWorkspace();
    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RimeDeploySchema(string schema_file);
    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RimeDeployConfigFile(string file_name, string version_key);

    [LibraryImport(LibName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RimeSyncUserData();

    [LibraryImport(LibName)]
    public static partial IntPtr RimeCreateSession();
    [LibraryImport(LibName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RimeFindSession(IntPtr session_id);
    [LibraryImport(LibName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RimeDestroySession(IntPtr session_id);
    [LibraryImport(LibName)]
    public static partial void RimeCleanupStaleSessions();
    [LibraryImport(LibName)]
    public static partial void RimeCleanupAllSessions();

    [LibraryImport(LibName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RimeProcessKey(IntPtr session_id, int keycode, int mask);
    /// <summary>
    /// return True if there is unread commit text
    /// </summary>
    /// <param name="session_id"></param>
    /// <returns></returns>
    [LibraryImport(LibName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RimeCommitComposition(IntPtr session_id);
    [LibraryImport(LibName)]
    public static partial void RimeClearComposition(IntPtr session_id);

    [DllImport(LibName)]
    public static extern unsafe bool RimeGetCommit(IntPtr session_id, ref RimeCommit ptr);

    [LibraryImport(LibName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static unsafe partial bool RimeFreeCommit(IntPtr ptr);

    [DllImport(LibName)]
    private static extern unsafe bool RimeGetContext(IntPtr session_id, IntPtr ptr);

    public static bool RimeGetContext(IntPtr session_id, out RimeContext context)
    {
        var context1 = new RimeContext();
        context1.data_size = Marshal.SizeOf(context1) - sizeof(int);
        IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(context1));
        Marshal.StructureToPtr(context1, pnt, false);
        var res = RimeGetContext(session_id, ptr: pnt);
        if (res)
        {
            context = RimeContextHelper.MarshalFromIntPtr(pnt);
            RimeFreeContext(pnt);
        }
        else
        {
            context = new();
        }

        Marshal.FreeHGlobal(pnt);
        return res;
    }

    [DllImport(LibName)]
    private static extern unsafe bool RimeFreeContext(IntPtr context);
    [DllImport(LibName)]
    public static extern unsafe bool RimeGetStatus(IntPtr session_id, ref RimeStatus status);
    [DllImport(LibName)]
    public static extern unsafe bool RimeFreeStatus(ref RimeStatus status);

    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RimeSimulateKeySequence(IntPtr session_id, string key_sequence);

    [DllImport(LibName)]
    public static extern bool RimeSetInput(IntPtr session_id, string input);

    [DllImport(LibName)]
    public static extern void RimeSetOption(IntPtr session_id,
                            string option,
                            bool value);

    [DllImport(LibName)]
    public static extern bool RimeGetOption(IntPtr session_id, string option);
    [DllImport(LibName)]
    public static extern void RimeSetProperty(IntPtr session_id,
                              string prop,
                              string value);
    [DllImport(LibName)]
    public static extern bool RimeGetProperty(IntPtr session_id,
                              string prop,
                              ref string value,
                              ulong buffer_size);
    [LibraryImport(LibName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static unsafe partial bool RimeGetSchemaList(out RimeSchemaListHelper.RimeSchemaListIn schema_list);

    /// <summary>
    /// 获取输入方案列表
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static bool RimeGetSchemaList(out RimeSchemaList list)
    {
        var res = RimeGetSchemaList(schema_list: out var ptr);

        if (ptr.size > 0 && ptr.list != IntPtr.Zero)
        {
            var array = new RimeSchemaListItem[ptr.size];

            var current = ptr.list;
            var itemSize = Marshal.SizeOf(typeof(RimeSchemaListItem));
            for (var i = 0; i < ptr.size; i++)
            {
                array[i] = Marshal.PtrToStructure<RimeSchemaListItem>(current);
                current = IntPtr.Add(current, itemSize);
            }

            list = new RimeSchemaList { size = ptr.size, list = array };
        }
        else
        {
            list = new RimeSchemaList { size = ptr.size, list = [] };
        }

        return res;
    }

    [DllImport(LibName)]
    public static extern unsafe void RimeFreeSchemaList(ref RimeSchemaList schema_list);
    [DllImport(LibName)]
    public static extern bool RimeGetCurrentSchema(IntPtr session_id,
                                       ref string schema_id,
                                       ulong buffer_size);

    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RimeSelectSchema(IntPtr session_id, string schema_id);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    private static extern bool RimeCandidateListBegin(IntPtr session_id, out IntPtr ptr);

    public static bool RimeCandidateListBegin(IntPtr session_id, out RimeCandidateListIterator iterator)
    {
        var res = RimeCandidateListBegin(session_id, ptr: out var ptr);
        if (res)
        {
            iterator = Marshal.PtrToStructure<RimeCandidateListIterator>(ptr);
        }
        else
        {
            iterator = new();
        }
        return res;
    }

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool RimeCandidateListNext(IntPtr iterator);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void RimeCandidateListEnd(IntPtr iterator);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool RimeCandidateListFromIndex(IntPtr session_id, ref IntPtr iterator, int index);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool RimeSelectCandidate(IntPtr session_id, ulong index);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool RimeSelectCandidateOnCurrentPage(IntPtr session_id, ulong index);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool RimeDeleteCandidate(IntPtr session_id, ulong index);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool RimeDeleteCandidateOnCurrentPage(IntPtr session_id, ulong index);
}
