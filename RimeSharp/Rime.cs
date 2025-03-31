using System.Runtime.InteropServices;
using RimeSharp.Helper;

namespace RimeSharp;

public delegate void RimeNotificationHandler(IntPtr context_object, IntPtr session_id, 
    [MarshalAs(UnmanagedType.LPUTF8Str)] string message_type, 
    [MarshalAs(UnmanagedType.LPUTF8Str)] string message_value);

public static partial class Rime
{
    public const string LibName = "rime";
    public const string Version = "1.13.1";

    private static RimeApi s_rimeApi;

    [LibraryImport(LibName, EntryPoint = "rime_get_api")]
    private static partial IntPtr RimeGetApi();

    public static RimeTraits Init(string dir, RimeNotificationHandler handler)
    {
        handler.Invoke(0, 0, "RimeSharp", $"RimeSharp version: {Version}");
        handler.Invoke(0, 0, "RimeSharp", $"Init start, load dll: {LibName}");

        var dataDir = Path.Combine(dir, "data");
        var logDir = Path.Combine(dir, "log");
        var buildDir = Path.Combine(dir, "build");

        Directory.CreateDirectory(dataDir);
        Directory.CreateDirectory(logDir);
        Directory.CreateDirectory(buildDir);

        using var handel = new SafeHandel<RimeTraits>(new()
        {
            AppName = "RimeSharp",
            MinLogLevel = 0,
            SharedDataDir = dataDir,
            UserDataDir = dataDir,
            LogDir = logDir,
            PrebuiltDataDir = buildDir,
            StagingDir = buildDir
        });
        var api = RimeGetApi();
        if (api == 0)
        {
            throw new Exception("Can not get rime api");
        }
        s_rimeApi = Marshal.PtrToStructure<RimeApi>(api);
        s_rimeApi.setup(handel.Ptr);
        s_rimeApi.set_notification_handler(handler, 0);
        s_rimeApi.initialize(handel.Ptr);
        if (s_rimeApi.start_maintenance(true))
        {
            s_rimeApi.join_maintenance_thread();
        }
        handler.Invoke(0, 0, "RimeSharp", $"Init end, load dll version: {Utf8Buffer.StringFromPtr(s_rimeApi.get_version())}");
        return handel.GetData();
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
        using var handel = new SafeHandel<RimeTraits>(traits);
        s_rimeApi.deployer_initialize(handel.Ptr);
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
        using var handel = new SafeHandel<RimeCommit>();
        var res = s_rimeApi.get_commit(session_id, handel.Ptr);
        commit = res ? handel.GetData() : null;
        if (res) s_rimeApi.free_commit(handel.Ptr);
        return res;
    }

    public static bool RimeGetContext(IntPtr session_id, out RimeContext? context)
    {
        using var handel = new SafeHandel<RimeContextHelper.RimeContextIn>();
        var res = s_rimeApi.get_context(session_id, handel.Ptr);
        context = res ? RimeContextHelper.MarshalFromIntPtr(handel.GetData()) : null;
        if (res) s_rimeApi.free_context(handel.Ptr);
        return res;
    }

    public static bool GetStatus(IntPtr session_id, out RimeStatus? status)
    {
        using var handel = new SafeHandel<RimeStatus>();
        var res = s_rimeApi.get_status(session_id, handel.Ptr);
        status = res ? handel.GetData() : null;
        if (res) s_rimeApi.free_status(handel.Ptr);
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
        using var handel = new SafePtrBuffer(1024);
        var res = s_rimeApi.get_property(session_id, buffer.DangerousGetHandle(), handel.Ptr, (ulong)handel.Size);
        value = res ? Utf8Buffer.StringFromPtr(handel.Ptr) : null;
        return res;
    }

    /// <summary>
    /// 获取输入方案列表
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static bool GetSchemaList(out RimeSchemaList? list)
    {
        using var handel = new SafeHandel<RimeSchemaListHelper.RimeSchemaListIn>();
        var res = s_rimeApi.get_schema_list(handel.Ptr);
        list = res ? RimeSchemaListHelper.MarshalFromIntPtr(handel.Ptr) : null;
        s_rimeApi.free_schema_list(handel.Ptr);
        return res;
    }

    public static bool GetCurrentSchema(IntPtr session_id, out string? schema_id)
    {
        using var handel = new SafePtrBuffer(1024);
        var res = s_rimeApi.get_current_schema(session_id, handel.Ptr, (ulong)handel.Size);
        schema_id = res ? Utf8Buffer.StringFromPtr(handel.Ptr) : null;
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
        using var handel = new SafeHandel<RimeConfig>();
        var res = s_rimeApi.schema_open(buffer.DangerousGetHandle(), handel.Ptr);
        config = res ? handel.GetData() : null;
        return res;
    }

    public static bool ConfigOpen(string config_id, out RimeConfig? config)
    {
        using var buffer = new Utf8Buffer(config_id);
        using var handel = new SafeHandel<RimeConfig>();
        var res = s_rimeApi.config_open(buffer.DangerousGetHandle(), handel.Ptr);
        config = res ? handel.GetData() : null;
        return res;
    }

    public static bool ConfigClose(RimeConfig config)
    {
        using var handel = new SafeHandel<RimeConfig>(config);
        return s_rimeApi.config_close(handel.Ptr);
    }

    public static bool ConfigGetBool(RimeConfig config, string key, out bool? value)
    {
        using var buffer = new Utf8Buffer(key);
        using var handel = new SafeHandel<RimeConfig>(config);
        using var handel1 = new SafeHandel<bool>();
        var res = s_rimeApi.config_get_bool(handel.Ptr, buffer.DangerousGetHandle(), handel1.Ptr);
        value = res ? handel1.GetData() : null;
        return res;
    }

    public static bool ConfigGetInt(RimeConfig config, string key, out int? value)
    {
        using var buffer = new Utf8Buffer(key);
        using var handel = new SafeHandel<RimeConfig>(config);
        using var handel1 = new SafeHandel<int>();
        var res = s_rimeApi.config_get_int(handel.Ptr, buffer.DangerousGetHandle(), handel1.Ptr);
        value = res ? handel1.GetData() : null;
        return res;
    }

    public static bool ConfigGetDouble(RimeConfig config, string key, out double? value)
    {
        using var buffer = new Utf8Buffer(key);
        using var handel = new SafeHandel<RimeConfig>(config);
        using var handel1 = new SafeHandel<double>();
        var res = s_rimeApi.config_get_double(handel.Ptr, buffer.DangerousGetHandle(), handel1.Ptr);
        value = res ? handel1.GetData() : null;
        return res;
    }

    public static bool ConfigGetString(RimeConfig config, string key, out string? value)
    {
        using var buffer = new Utf8Buffer(key);
        using var handel = new SafeHandel<RimeConfig>(config);
        using var handel1 = new SafePtrBuffer(1024);
        var res = s_rimeApi.config_get_string(handel.Ptr, buffer.DangerousGetHandle(), handel1.Ptr, (ulong)handel1.Size);
        value = res ? Utf8Buffer.StringFromPtr(handel1.Ptr) : null;
        return res;
    }

    public static bool ConfigUpdateSignature(RimeConfig config, string signer)
    {
        using var buffer = new Utf8Buffer(signer);
        using var handel = new SafeHandel<RimeConfig>(config);
        return s_rimeApi.config_update_signature(handel.Ptr, buffer.DangerousGetHandle());
    }

    public static bool ConfigBeginMap(RimeConfig config, string key, out RimeConfigIterator? iterator)
    {
        using var buffer = new Utf8Buffer(key);
        using var handel = new SafeHandel<RimeConfig>(config);
        using var handel1 = new SafeHandel<RimeConfigIterator>();
        var res = s_rimeApi.config_begin_map(handel1.Ptr, handel.Ptr, buffer.DangerousGetHandle());
        iterator = res ? handel1.GetData() : null;
        return res;
    }

    public static bool ConfigNext(ref RimeConfigIterator iterator)
    {
        using var handel = new SafeHandel<RimeConfigIterator>(iterator);
        var res = s_rimeApi.config_next(handel.Ptr);
        if (res)
        {
            iterator = handel.GetData();
        }

        return res;
    }

    public static void ConfigEnd(RimeConfigIterator iterator)
    {
        using var handel = new SafeHandel<RimeConfigIterator>(iterator);
        s_rimeApi.config_end(handel.Ptr);
    }

    public static bool SimulateKeySequence(IntPtr session_id, string key_sequence)
    {
        using var buffer = new Utf8Buffer(key_sequence);
        return s_rimeApi.simulate_key_sequence(session_id, buffer.DangerousGetHandle());
    }

    public static bool RegisterModule(RimeModule module)
    {
        using var handel = new SafeHandel<RimeModule>(module);
        return s_rimeApi.register_module(handel.Ptr);
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
        using var handel = new SafePtrBuffer(1024);
        s_rimeApi.get_shared_data_dir_s(handel.Ptr, (ulong)handel.Size);
        return Utf8Buffer.StringFromPtr(handel.Ptr);
    }

    public static string? GetUserDataDir()
    {
        using var handel = new SafePtrBuffer(1024);
        s_rimeApi.get_user_data_dir_s(handel.Ptr, (ulong)handel.Size);
        return Utf8Buffer.StringFromPtr(handel.Ptr);
    }

    public static string? GetSyncDir()
    {
        using var handel = new SafePtrBuffer(1024);
        s_rimeApi.get_sync_dir_s(handel.Ptr, (ulong)handel.Size);
        return Utf8Buffer.StringFromPtr(handel.Ptr);
    }

    public static string? GetUserId()
    {
        return Utf8Buffer.StringFromPtr(s_rimeApi.get_user_id());
    }

    public static string? GetUserDataSyncDir()
    {
        using var handel = new SafePtrBuffer(1024);
        s_rimeApi.get_user_data_sync_dir(handel.Ptr, (ulong)handel.Size);
        return Utf8Buffer.StringFromPtr(handel.Ptr);
    }

    public static bool ConfigInit(out RimeConfig? config)
    {
        using var handel = new SafeHandel<RimeConfig>();
        var res = s_rimeApi.config_init(handel.Ptr);
        config = res ? handel.GetData() : null;
        return res;
    }

    public static bool ConfigLoadString(RimeConfig config, string yaml)
    {
        using var buffer = new Utf8Buffer(yaml);
        using var handel = new SafeHandel<RimeConfig>(config);
        return s_rimeApi.config_load_string(handel.Ptr, buffer.DangerousGetHandle());
    }

    public static bool ConfigSetBool(RimeConfig config, string key, bool value)
    {
        using var buffer = new Utf8Buffer(key);
        using var handel = new SafeHandel<RimeConfig>(config);
        return s_rimeApi.config_set_bool(handel.Ptr, buffer.DangerousGetHandle(), value);
    }

    public static bool ConfigSetInt(RimeConfig config, string key, int value)
    {
        using var buffer = new Utf8Buffer(key);
        using var handel = new SafeHandel<RimeConfig>(config);
        return s_rimeApi.config_set_int(handel.Ptr, buffer.DangerousGetHandle(), value);
    }

    public static bool ConfigSetDouble(RimeConfig config, string key, double value)
    {
        using var buffer = new Utf8Buffer(key);
        using var handel = new SafeHandel<RimeConfig>(config);
        return s_rimeApi.config_set_double(handel.Ptr, buffer.DangerousGetHandle(), value);
    }

    public static bool ConfigSetString(RimeConfig config, string key, string value)
    {
        using var buffer = new Utf8Buffer(key);
        using var buffer1 = new Utf8Buffer(value);
        using var handel = new SafeHandel<RimeConfig>(config);
        return s_rimeApi.config_set_string(handel.Ptr, buffer.DangerousGetHandle(), buffer1.DangerousGetHandle());
    }

    public static bool ConfigGetItem(RimeConfig config, string key, out RimeConfig? value)
    {
        using var buffer = new Utf8Buffer(key);
        using var handel = new SafeHandel<RimeConfig>(config);
        using var handel1 = new SafeHandel<RimeConfig>();
        var res = s_rimeApi.config_get_item(handel.Ptr, buffer.DangerousGetHandle(), handel1.Ptr);
        value = res ? handel1.GetData() : null;
        return res;
    }

    public static bool ConfigSetItem(RimeConfig config, string key, RimeConfig value)
    {
        using var buffer = new Utf8Buffer(key);
        using var handel = new SafeHandel<RimeConfig>(config);
        using var handel1 = new SafeHandel<RimeConfig>(value);
        return s_rimeApi.config_set_item(handel.Ptr, buffer.DangerousGetHandle(), handel1.Ptr);
    }

    public static bool ConfigClear(RimeConfig config, string key)
    {
        using var buffer = new Utf8Buffer(key);
        using var handel = new SafeHandel<RimeConfig>(config);
        return s_rimeApi.config_clear(handel.Ptr, buffer.DangerousGetHandle());
    }

    public static bool ConfigCreateList(RimeConfig config, string key)
    {
        using var buffer = new Utf8Buffer(key);
        using var handel = new SafeHandel<RimeConfig>(config);
        return s_rimeApi.config_create_list(handel.Ptr, buffer.DangerousGetHandle());
    }

    public static bool ConfigCreateMap(RimeConfig config, string key)
    {
        using var buffer = new Utf8Buffer(key);
        using var handel = new SafeHandel<RimeConfig>(config);
        return s_rimeApi.config_create_map(handel.Ptr, buffer.DangerousGetHandle());
    }

    public static ulong ConfigListSize(RimeConfig config, string key)
    {
        using var buffer = new Utf8Buffer(key);
        using var handel = new SafeHandel<RimeConfig>(config);
        return s_rimeApi.config_list_size(handel.Ptr, buffer.DangerousGetHandle());
    }

    public static bool ConfigBeginList(RimeConfig config, string key, out RimeConfigIterator? iterator)
    {
        using var buffer = new Utf8Buffer(key);
        using var handel = new SafeHandel<RimeConfig>(config);
        using var handel1 = new SafeHandel<RimeConfigIterator>();
        var res = s_rimeApi.config_begin_list(handel.Ptr, buffer.DangerousGetHandle(), handel1.Ptr);
        iterator = res ? handel1.GetData() : null;
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
        using var handel = new SafeHandel<RimeCandidateListIterator>();
        var res = s_rimeApi.candidate_list_begin(session_id, handel.Ptr);
        iterator = res ? handel.GetData() : null;
        return res;
    }
    public static bool CandidateListNext(ref RimeCandidateListIterator iterator)
    {
        using var handel = new SafeHandel<RimeCandidateListIterator>(iterator);
        var res = s_rimeApi.candidate_list_next(handel.Ptr);
        if (res)
        {
            iterator = handel.GetData();
        }
        return res;
    }
    public static void CandidateListEnd(RimeCandidateListIterator iterator)
    {
        using var handel = new SafeHandel<RimeCandidateListIterator>(iterator);
        s_rimeApi.candidate_list_end(handel.Ptr);
    }

    public static bool UserConfigOpen(string config_id, out RimeConfig? config)
    {
        using var buffer = new Utf8Buffer(config_id);
        using var handel = new SafeHandel<RimeConfig>();
        var res = s_rimeApi.user_config_open(buffer.DangerousGetHandle(), handel.Ptr);
        config = res ? handel.GetData() : null;
        return res;
    }

    public static bool CandidateListFromIndex(IntPtr session_id, int index, out RimeCandidateListIterator? iterator)
    {
        using var handel = new SafeHandel<RimeCandidateListIterator>();
        var res = s_rimeApi.candidate_list_from_index(session_id, handel.Ptr, index);
        iterator = res ? handel.GetData() : null;

        return res;
    }

    public static string? GetPrebuiltDataDir()
    {
        using var handel = new SafePtrBuffer(1024);
        s_rimeApi.get_prebuilt_data_dir_s(handel.Ptr, (ulong)handel.Size);
        return Utf8Buffer.StringFromPtr(handel.Ptr);
    }

    public static string? GetStagingDir()
    {
        using var handel = new SafePtrBuffer(1024);
        s_rimeApi.get_staging_dir_s(handel.Ptr, (ulong)handel.Size);
        return Utf8Buffer.StringFromPtr(handel.Ptr);
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
