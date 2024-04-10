using RimeSharp.Helper;
using System.Runtime.InteropServices;

namespace RimeSharp;


public delegate void RimeNotificationHandler(IntPtr context_object, IntPtr session_id, [MarshalAs(UnmanagedType.LPUTF8Str)] string message_type, [MarshalAs(UnmanagedType.LPUTF8Str)] string message_value);

public static partial class Rime
{
    public const string LibName = "rime";
    public const string Version = "1.11.0";

    private static RimeApi s_rimeApi;

    [LibraryImport(LibName, EntryPoint = "rime_get_api")]
    private static partial IntPtr RimeGetApi();

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

        var pnt = Marshal.AllocHGlobal(Marshal.SizeOf(traits));

        Marshal.StructureToPtr(traits, pnt, false);

        var api = RimeGetApi();
        s_rimeApi = Marshal.PtrToStructure<RimeApi>(api);
        s_rimeApi.setup(pnt);
        s_rimeApi.set_notification_handler(handler, 0);
        s_rimeApi.initialize(pnt);
        if (s_rimeApi.start_maintenance(true))
        {
            s_rimeApi.join_maintenance_thread();
        }

        traits = Marshal.PtrToStructure<RimeTraits>(pnt);
        Marshal.FreeHGlobal(pnt);
        return traits;
    }

    public static void Close()
    {
        s_rimeApi.finalize();
    }

    public static bool IsMaintenanceMode()
    {
        return s_rimeApi.is_maintenance_mode();
    }

    public static void DeployerInitialize(RimeTraits traits)
    {
        var pnt = Marshal.AllocHGlobal(Marshal.SizeOf(traits));
        Marshal.StructureToPtr(traits, pnt, false);
        s_rimeApi.deployer_initialize(pnt);
        Marshal.FreeHGlobal(pnt);
    }

    public static bool Prebuild()
    {
        return s_rimeApi.prebuild();
    }

    public static bool Deploy()
    {
        return s_rimeApi.deploy();
    }

    public static bool DeploySchema(string schema_file)
    {
        using var buffer = new Utf8Buffer(schema_file);
        return s_rimeApi.deploy_schema(buffer.DangerousGetHandle());
    }

    public static bool DeployConfigFile(string file_name, string version_key)
    {
        using var buffer = new Utf8Buffer(file_name);
        using var buffer1 = new Utf8Buffer(version_key);
        return s_rimeApi.deploy_config_file(buffer.DangerousGetHandle(), buffer1.DangerousGetHandle());
    }

    public static bool SyncUserData()
    {
        return s_rimeApi.sync_user_data();
    }

    public static IntPtr CreateSession()
    {
        return s_rimeApi.create_session();
    }

    public static bool FindSession(IntPtr session_id)
    {
        return s_rimeApi.find_session(session_id);
    }

    public static bool DestroySession(IntPtr session_id)
    {
        return s_rimeApi.destroy_session(session_id);
    }

    public static void CleanupStaleSessions()
    {
        s_rimeApi.cleanup_stale_sessions();
    }
    public static void CleanupAllSessions()
    {
        s_rimeApi.cleanup_all_sessions();
    }

    public static bool ProcessKey(IntPtr session_id, int keycode, int mask) 
    {
        return s_rimeApi.process_key(session_id, keycode, mask);
    }
    public static bool CommitComposition(IntPtr session_id)
    {
        return s_rimeApi.commit_composition(session_id);
    }
    public static void ClearComposition(IntPtr session_id)
    {
        s_rimeApi.clear_composition(session_id);
    }

    public static bool GetCommit(IntPtr session_id, out RimeCommit? commit)
    {
        var context1 = new RimeCommit();
        context1.data_size = Marshal.SizeOf(context1) - sizeof(int);
        var pnt = Marshal.AllocHGlobal(Marshal.SizeOf(context1));
        Marshal.StructureToPtr(context1, pnt, false);
        var res = s_rimeApi.get_commit(session_id, pnt);
        commit = res ? Marshal.PtrToStructure<RimeCommit>(pnt) : null;
        if (res) s_rimeApi.free_commit(pnt);
        Marshal.FreeHGlobal(pnt);
        return res;
    }

    public static bool RimeGetContext(IntPtr session_id, out RimeContext? context)
    {
        var context1 = new RimeContextHelper.RimeContextIn();
        context1.data_size = Marshal.SizeOf(context1) - sizeof(int);
        var pnt = Marshal.AllocHGlobal(Marshal.SizeOf(context1));
        Marshal.StructureToPtr(context1, pnt, false);
        var res = s_rimeApi.get_context(session_id, pnt);
        context = res ? RimeContextHelper.MarshalFromIntPtr(pnt) : null;
        if(res) s_rimeApi.free_context(pnt);
        Marshal.FreeHGlobal(pnt);
        return res;
    }

    public static bool GetStatus(IntPtr session_id, out RimeStatus? status)
    {
        var context1 = new RimeStatus();
        context1.data_size = Marshal.SizeOf(context1) - sizeof(int);
        var pnt = Marshal.AllocHGlobal(Marshal.SizeOf(context1));
        Marshal.StructureToPtr(context1, pnt, false);
        var res = s_rimeApi.get_status(session_id, pnt);
        status = res ? context1 : null;
        if (res) s_rimeApi.free_status(pnt);
        Marshal.FreeHGlobal(pnt);
        return res;
    }

    public static void SetOption(IntPtr session_id, string option, bool value)
    {
        using var buffer = new Utf8Buffer(option);
        s_rimeApi.set_option(session_id, buffer.DangerousGetHandle(), value);
    }

    public static bool GetOption(IntPtr session_id, string option)
    {
        using var buffer = new Utf8Buffer(option);
        return s_rimeApi.get_option(session_id, buffer.DangerousGetHandle());
    }

    public static void SetProperty(IntPtr session_id, string prop, string value)
    {
        using var buffer = new Utf8Buffer(prop);
        using var buffer1 = new Utf8Buffer(value);
        s_rimeApi.set_property(session_id, buffer.DangerousGetHandle(), buffer1.DangerousGetHandle());
    }

    public static bool GetProperty(IntPtr session_id, string prop, out string? value)
    {
        using var buffer = new Utf8Buffer(prop);
        var ptr = Marshal.AllocHGlobal(1024);
        var res = s_rimeApi.get_property(session_id, buffer.DangerousGetHandle(), ptr, 1024);
        value = res ? Utf8Buffer.StringFromPtr(ptr) : null;
        Marshal.FreeHGlobal(ptr);
        return res;
    }

    /// <summary>
    /// 获取输入方案列表
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static bool GetSchemaList(out RimeSchemaList? list)
    {
        var pnt = Marshal.AllocHGlobal(Marshal.SizeOf<RimeSchemaListHelper.RimeSchemaListIn>());
        var res = s_rimeApi.get_schema_list(pnt);
        list = res ? RimeSchemaListHelper.MarshalFromIntPtr(pnt) : null;
        s_rimeApi.free_schema_list(pnt);
        Marshal.FreeHGlobal(pnt);
        return res;
    }

    public static bool GetCurrentSchema(IntPtr session_id, out string? schema_id)
    {
        var ptr = Marshal.AllocHGlobal(1024);
        var res = s_rimeApi.get_current_schema(session_id, ptr, 1024);
        schema_id = res ? Utf8Buffer.StringFromPtr(ptr) : null;
        Marshal.FreeHGlobal(ptr);
        return res;
    }

    public static bool SelectSchema(IntPtr session_id, string schema_id)
    {
        using var buffer = new Utf8Buffer(schema_id);
        return s_rimeApi.select_schema(session_id, buffer.DangerousGetHandle());
    }

    public static bool SchemaOpen(string schema_id, out RimeConfig? config)
    {
        using var buffer = new Utf8Buffer(schema_id);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        var res = s_rimeApi.schema_open(buffer.DangerousGetHandle(), ptr);
        config = res ? Marshal.PtrToStructure<RimeConfig>(ptr) : null;
        Marshal.FreeHGlobal(ptr);
        return res;
    }

    public static bool ConfigOpen(string config_id, out RimeConfig? config)
    {
        using var buffer = new Utf8Buffer(config_id);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        var res = s_rimeApi.config_open(buffer.DangerousGetHandle(), ptr);
        config = res ? Marshal.PtrToStructure<RimeConfig>(ptr) : null;
        Marshal.FreeHGlobal(ptr);
        return res;
    }

    public static bool ConfigClose(RimeConfig config)
    {
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        Marshal.StructureToPtr(config, ptr, false);
        try
        {
            return s_rimeApi.config_close(ptr);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static bool ConfigGetBool(RimeConfig config, string key, out bool? value)
    {
        using var buffer = new Utf8Buffer(key);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        var ptr1 = Marshal.AllocHGlobal(sizeof(bool));
        Marshal.StructureToPtr(config, ptr, false);
        var res = s_rimeApi.config_get_bool(ptr, buffer.DangerousGetHandle(), ptr1);
        value = res ? Marshal.ReadByte(ptr1) != 0 : null;
        Marshal.FreeHGlobal(ptr);
        Marshal.FreeHGlobal(ptr1);
        return res;
    }

    public static bool ConfigGetInt(RimeConfig config, string key, out int? value)
    {
        using var buffer = new Utf8Buffer(key);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        var ptr1 = Marshal.AllocHGlobal(sizeof(int));
        Marshal.StructureToPtr(config, ptr, false);
        var res = s_rimeApi.config_get_int(ptr, buffer.DangerousGetHandle(), ptr1);
        value = res ? Marshal.ReadInt32(ptr1) : null;
        Marshal.FreeHGlobal(ptr);
        Marshal.FreeHGlobal(ptr1);
        return res;
    }

    public static bool ConfigGetDouble(RimeConfig config, string key, out double? value)
    {
        using var buffer = new Utf8Buffer(key);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        var ptr1 = Marshal.AllocHGlobal(sizeof(double));
        Marshal.StructureToPtr(config, ptr, false);
        var res = s_rimeApi.config_get_double(ptr, buffer.DangerousGetHandle(), ptr1);
        value = res ? Marshal.ReadInt32(ptr1) : null;
        Marshal.FreeHGlobal(ptr);
        Marshal.FreeHGlobal(ptr1);
        return res;
    }

    public static bool ConfigGetString(RimeConfig config, string key, out string? value)
    {
        using var buffer = new Utf8Buffer(key);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        var ptr1 = Marshal.AllocHGlobal(1024);
        Marshal.StructureToPtr(config, ptr, false);
        var res = s_rimeApi.config_get_string(ptr, buffer.DangerousGetHandle(), ptr1, 1024);
        value = res ? Utf8Buffer.StringFromPtr(ptr1) : null;
        Marshal.FreeHGlobal(ptr);
        Marshal.FreeHGlobal(ptr1);
        return res;
    }

    public static bool ConfigUpdateSignature(RimeConfig config, string signer)
    {
        using var buffer = new Utf8Buffer(signer);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        Marshal.StructureToPtr(config, ptr, false);
        try
        {
            return s_rimeApi.config_update_signature(ptr, buffer.DangerousGetHandle());
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static bool ConfigBeginMap(RimeConfig config, string key, out RimeConfigIterator? iterator)
    {
        using var buffer = new Utf8Buffer(key);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfigIterator>());
        var ptr1 = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        Marshal.StructureToPtr(config, ptr1, false);
        var res = s_rimeApi.config_begin_map(ptr, ptr1, buffer.DangerousGetHandle());
        iterator = res ? Marshal.PtrToStructure<RimeConfigIterator>(ptr) : null;
        Marshal.FreeHGlobal(ptr);
        Marshal.FreeHGlobal(ptr1);

        return res;
    }

    public static bool ConfigNext(ref RimeConfigIterator iterator)
    {
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfigIterator>());
        Marshal.StructureToPtr(iterator, ptr, false);
        var res = s_rimeApi.config_next(ptr);
        if (res)
        {
            iterator = Marshal.PtrToStructure<RimeConfigIterator>(ptr);
        }

        Marshal.FreeHGlobal(ptr);

        return res;
    }

    public static void ConfigEnd(RimeConfigIterator iterator)
    {
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfigIterator>());
        Marshal.StructureToPtr(iterator, ptr, false);
        try
        {
            s_rimeApi.config_end(ptr);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static bool SimulateKeySequence(IntPtr session_id, string key_sequence)
    {
        using var buffer = new Utf8Buffer(key_sequence);
        return s_rimeApi.simulate_key_sequence(session_id, buffer.DangerousGetHandle());
    }

    public static bool RegisterModule(RimeModule module)
    {
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeModule>());
        Marshal.StructureToPtr(module, ptr, false);
        try
        {
            return s_rimeApi.register_module(ptr);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static RimeModule? FindModule(string module_name)
    {
        using var buffer = new Utf8Buffer(module_name);
        var ptr = s_rimeApi.find_module(buffer.DangerousGetHandle());
        if (ptr == IntPtr.Zero)
        {
            return null;
        }

        return Marshal.PtrToStructure<RimeModule>(ptr);
    }

    public static bool RunTask(string task_name)
    {
        using var buffer = new Utf8Buffer(task_name);
        return s_rimeApi.run_task(buffer.DangerousGetHandle());
    }

    public static string? GetSharedDataDir()
    {
        var ptr = Marshal.AllocHGlobal(1024);

        try
        {
            s_rimeApi.get_shared_data_dir_s(ptr, 1024);
            return Utf8Buffer.StringFromPtr(ptr);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static string? GetUserDataDir()
    {
        var ptr = Marshal.AllocHGlobal(1024);

        try
        {
            s_rimeApi.get_user_data_dir_s(ptr, 1024);
            return Utf8Buffer.StringFromPtr(ptr);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static string? GetSyncDir()
    {
        var ptr = Marshal.AllocHGlobal(1024);

        try
        {
            s_rimeApi.get_sync_dir_s(ptr, 1024);
            return Utf8Buffer.StringFromPtr(ptr);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static string? GetUserId()
    {
        return Utf8Buffer.StringFromPtr(s_rimeApi.get_user_id());
    }

    public static string? GetUserDataSyncDir()
    {
        var ptr = Marshal.AllocHGlobal(1024);

        try
        {
            s_rimeApi.get_user_data_sync_dir(ptr, 1024);
            return Utf8Buffer.StringFromPtr(ptr);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static bool ConfigInit(out RimeConfig? config)
    {
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        var res = s_rimeApi.config_init(ptr);
        config = res ? Marshal.PtrToStructure<RimeConfig>(ptr) : null;
        return res;
    }

    public static bool ConfigLoadString(RimeConfig config, string yaml)
    {
        using var buffer = new Utf8Buffer(yaml);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        Marshal.StructureToPtr(config, ptr, false);
        try
        {
            return s_rimeApi.config_load_string(ptr, buffer.DangerousGetHandle());
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static bool ConfigSetBool(RimeConfig config, string key, bool value)
    {
        using var buffer = new Utf8Buffer(key);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        Marshal.StructureToPtr(config, ptr, false);
        try
        {
            return s_rimeApi.config_set_bool(ptr, buffer.DangerousGetHandle(), value);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static bool ConfigSetInt(RimeConfig config, string key, int value)
    {
        using var buffer = new Utf8Buffer(key);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        Marshal.StructureToPtr(config, ptr, false);
        try
        {
            return s_rimeApi.config_set_int(ptr, buffer.DangerousGetHandle(), value);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static bool ConfigSetDouble(RimeConfig config, string key, double    value)
    {
        using var buffer = new Utf8Buffer(key);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        Marshal.StructureToPtr(config, ptr, false);
        try
        {
            return s_rimeApi.config_set_double(ptr, buffer.DangerousGetHandle(), value);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static bool ConfigSetString(RimeConfig config, string key, string value)
    {
        using var buffer = new Utf8Buffer(key);
        using var buffer1 = new Utf8Buffer(value);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        Marshal.StructureToPtr(config, ptr, false);
        try
        {
            return s_rimeApi.config_set_string(ptr, buffer.DangerousGetHandle(), buffer1.DangerousGetHandle());
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static bool ConfigGetItem(RimeConfig config, string key, out RimeConfig? value)
    {
        using var buffer = new Utf8Buffer(key);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        Marshal.StructureToPtr(config, ptr, false);
        var ptr1 = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        var res = s_rimeApi.config_get_item(ptr, buffer.DangerousGetHandle(), ptr1);
        value = res ? Marshal.PtrToStructure<RimeConfig>(ptr1) : null;
        Marshal.FreeHGlobal(ptr);
        Marshal.FreeHGlobal(ptr1);

        return res;
    }

    public static bool ConfigSetItem(RimeConfig config, string key, RimeConfig value)
    {
        using var buffer = new Utf8Buffer(key);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        Marshal.StructureToPtr(config, ptr, false);
        var ptr1 = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        Marshal.StructureToPtr(value, ptr1, false);
        try
        {
            return s_rimeApi.config_set_item(ptr, buffer.DangerousGetHandle(), ptr1);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
            Marshal.FreeHGlobal(ptr1);
        }
    }

    public static bool ConfigClear(RimeConfig config, string key)
    {
        using var buffer = new Utf8Buffer(key);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        Marshal.StructureToPtr(config, ptr, false);

        try
        {
            return s_rimeApi.config_clear(ptr, buffer.DangerousGetHandle());
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static bool ConfigCreateList(RimeConfig config, string key)
    {
        using var buffer = new Utf8Buffer(key);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        Marshal.StructureToPtr(config, ptr, false);

        try
        {
            return s_rimeApi.config_create_list(ptr, buffer.DangerousGetHandle());
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static bool ConfigCreateMap(RimeConfig config, string key)
    {
        using var buffer = new Utf8Buffer(key);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        Marshal.StructureToPtr(config, ptr, false);

        try
        {
            return s_rimeApi.config_create_map(ptr, buffer.DangerousGetHandle());
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static ulong ConfigListSize(RimeConfig config, string key)
    {
        using var buffer = new Utf8Buffer(key);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        Marshal.StructureToPtr(config, ptr, false);

        try
        {
            return s_rimeApi.config_list_size(ptr, buffer.DangerousGetHandle());
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static bool ConfigBeginList(RimeConfig config, string key, out RimeConfigIterator? iterator)
    {
        using var buffer = new Utf8Buffer(key);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        Marshal.StructureToPtr(config, ptr, false);
        var ptr1 = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfigIterator>());
        var res = s_rimeApi.config_begin_list(ptr, buffer.DangerousGetHandle(), ptr1);
        iterator = res ? Marshal.PtrToStructure<RimeConfigIterator>(ptr1) : null;
        Marshal.FreeHGlobal(ptr);
        Marshal.FreeHGlobal(ptr1);
        return res;
    }

    public static string? GetInput(IntPtr session_id)
    {
        return Utf8Buffer.StringFromPtr(s_rimeApi.get_input(session_id));
    }

    public static ulong GetCaretPos(IntPtr session_id)
    {
        return s_rimeApi.get_caret_pos(session_id);
    }

    public static bool SelectCandidate(IntPtr session_id, ulong index)
    {
        return s_rimeApi.select_candidate(session_id, index);
    }

    public static string? GetVersion()
    {
        return Utf8Buffer.StringFromPtr(s_rimeApi.get_version());
    }

    public static void SetCaretPos(IntPtr session_id, ulong caret_pos)
    {
        s_rimeApi.set_caret_pos(session_id, caret_pos);
    }

    public static void SelectCandidateOnCurrentPage(IntPtr session_id, ulong caret_pos)
    {
        s_rimeApi.set_caret_pos(session_id, caret_pos);
    }

    public static bool CandidateListBegin(IntPtr session_id,
                               out RimeCandidateListIterator? iterator)
    {
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeCandidateListIterator>());
        var res = s_rimeApi.candidate_list_begin(session_id, ptr);
        iterator = res ? Marshal.PtrToStructure<RimeCandidateListIterator>(ptr) : null;

        Marshal.FreeHGlobal(ptr);

        return res;
    }
    public static bool CandidateListNext(ref RimeCandidateListIterator iterator)
    {
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeCandidateListIterator>());
        Marshal.StructureToPtr(iterator, ptr, false);
        var res = s_rimeApi.candidate_list_next(ptr);
        if (res)
        {
            iterator = Marshal.PtrToStructure<RimeCandidateListIterator>(ptr);
        }

        Marshal.FreeHGlobal(ptr);

        return res;
    }
    public static void CandidateListEnd(RimeCandidateListIterator iterator)
    {
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeCandidateListIterator>());
        Marshal.StructureToPtr(iterator, ptr, false);
        try
        {
            s_rimeApi.candidate_list_end(ptr);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static bool UserConfigOpen(string config_id, out RimeConfig? config)
    {
        using var buffer = new Utf8Buffer(config_id);
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeConfig>());
        var res = s_rimeApi.user_config_open(buffer.DangerousGetHandle(), ptr);
        config = res ? Marshal.PtrToStructure<RimeConfig>(ptr) : null;
        Marshal.FreeHGlobal(ptr);
        return res;
    }

    public static bool CandidateListFromIndex(IntPtr session_id, int index, out RimeCandidateListIterator? iterator)
    {
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<RimeCandidateListIterator>());
        var res = s_rimeApi.candidate_list_from_index(session_id, ptr, index);
        iterator = res ? Marshal.PtrToStructure<RimeCandidateListIterator>(ptr) : null;

        return res;
    }

    public static string? GetPrebuiltDataDir()
    {
        var ptr = Marshal.AllocHGlobal(1024);

        try
        {
            s_rimeApi.get_prebuilt_data_dir_s(ptr, 1024);
            return Utf8Buffer.StringFromPtr(ptr);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static string? GetStagingDir()
    {
        var ptr = Marshal.AllocHGlobal(1024);

        try
        {
            s_rimeApi.get_staging_dir_s(ptr, 1024);
            return Utf8Buffer.StringFromPtr(ptr);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static void GetStateLabel(IntPtr session_id, string option_name, bool state)
    {
        using var buffer = new Utf8Buffer(option_name);
        s_rimeApi.get_state_label(session_id, buffer.DangerousGetHandle(), state);
    }

    public static bool DeleteCandidate(IntPtr session_id, ulong index)
    {
        return s_rimeApi.delete_candidate(session_id, index);
    }

    public static bool DeleteCandidateOnCurrentPage(IntPtr session_id, ulong index)
    {
        return s_rimeApi.delete_candidate_on_current_page(session_id, index);
    }

    public static RimeStringSlice GetStateLabelAbbreviated(IntPtr session_id, string option_name, bool state, bool abbreviated)
    {
        using var buffer = new Utf8Buffer(option_name);
        return s_rimeApi.get_state_label_abbreviated(session_id, buffer.DangerousGetHandle(), state, abbreviated);
    }

    public static bool SetInput(IntPtr session_id, string input)
    {
        using var buffer = new Utf8Buffer(input);
        return s_rimeApi.set_input(session_id, buffer.DangerousGetHandle());
    }

    public static bool HighlightCandidate(IntPtr session_id, ulong index)
    {
        return s_rimeApi.highlight_candidate(session_id, index);
    }

    public static bool HighlightCandidateOnCurrentPage(IntPtr session_id, ulong index)
    {
        return s_rimeApi.highlight_candidate_on_current_page(session_id, index);
    }

    public static bool ChangePage(IntPtr session_id, bool backward)
    {
        return s_rimeApi.change_page(session_id, backward);
    }
}
