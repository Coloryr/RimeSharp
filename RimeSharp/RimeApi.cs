using System.Runtime.InteropServices;

namespace RimeSharp;

internal struct RimeApi
{
    public delegate void DTypeVoid();
    public delegate void DTypeVoid1(IntPtr ptr);
    public delegate void DTypeVoid2(IntPtr ptr, IntPtr ptr1);
    public delegate void DTypeVoid3(IntPtr ptr, ulong size);
    public delegate void DTypeVoid4(IntPtr ptr, IntPtr text, bool b1);
    public delegate void DTypeVoid5(RimeNotificationHandler handler, IntPtr context);

    public delegate bool DTypeBool();
    public delegate bool DTypeBool1(bool b);
    public delegate bool DTypeBool2(IntPtr ptr);
    public delegate bool DTypeBool3(IntPtr ptr, int int1, int int2);
    public delegate bool DTypeBool4(IntPtr ptr, bool b1);
    public delegate bool DTypeBool5(IntPtr ptr, ulong size);
    public delegate bool DTypeBool6(IntPtr ptr, IntPtr ptr1);
    public delegate bool DTypeBool7(IntPtr ptr, IntPtr ptr1, bool b1);
    public delegate bool DTypeBool8(IntPtr ptr, IntPtr ptr1, int int1);
    public delegate bool DTypeBool9(IntPtr ptr, IntPtr ptr1, double double1);
    public delegate bool DTypeBool10(IntPtr ptr, IntPtr ptr1, int int1);
    public delegate bool DTypeBool11(IntPtr ptr, IntPtr ptr1, ulong size);
    public delegate bool DTypeBool12(IntPtr ptr, IntPtr ptr1, IntPtr ptr2);
    public delegate bool DTypeBool13(IntPtr ptr, IntPtr ptr1, IntPtr ptr2, ulong size);

    public delegate IntPtr DTypeIntPtr();
    public delegate IntPtr DTypeIntPtr1(IntPtr ptr);
    public delegate IntPtr DTypeIntPtr2(IntPtr ptr, IntPtr text);
    public delegate IntPtr DTypeIntPtr3(IntPtr ptr, IntPtr text, bool b1);
    public delegate IntPtr DTypeIntPtr4(IntPtr ptr, IntPtr ptr1, IntPtr text);

    public delegate ulong DTypeSize(IntPtr ptr);
    public delegate ulong DTypeSize1(IntPtr ptr, IntPtr text);

    public delegate RimeStringSlice DTypeStringSlice(IntPtr ptr, IntPtr text, bool b1, bool b2);

    [MarshalAs(UnmanagedType.I4)]
    public int data_size;

    /// <summary>
    /// void (*setup)(RimeTraits* traits);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid1 setup;

    /// <summary>
    /// void (*set_notification_handler)(RimeNotificationHandler handler,
    /// void* context_object);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid5 set_notification_handler;

    /// <summary>
    ///  void (*initialize)(RimeTraits* traits);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid1 initialize;

    /// <summary>
    /// void (*finalize)(void);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid finalize;

    /// <summary>
    /// Bool (*start_maintenance)(Bool full_check);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool1 start_maintenance;

    /// <summary>
    /// Bool (*is_maintenance_mode)(void);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool is_maintenance_mode;

    /// <summary>
    /// void (*join_maintenance_thread)(void);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid join_maintenance_thread;

    /// <summary>
    /// void (*deployer_initialize)(RimeTraits* traits);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid1 deployer_initialize;

    /// <summary>
    /// Bool (*prebuild)(void);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool prebuild;

    /// <summary>
    /// Bool (*deploy)(void);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool deploy;

    /// <summary>
    /// Bool (*deploy_schema)(const char* schema_file);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool2 deploy_schema;

    /// <summary>
    /// Bool (*deploy_config_file)(const char* file_name, const char* version_key);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 deploy_config_file;

    /// <summary>
    /// Bool (*sync_user_data)(void);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool sync_user_data;

    /// <summary>
    /// RimeSessionId (*create_session)(void);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeIntPtr create_session;

    /// <summary>
    /// Bool (*find_session)(RimeSessionId session_id);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool2 find_session;

    /// <summary>
    /// Bool (*destroy_session)(RimeSessionId session_id);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool2 destroy_session;

    /// <summary>
    /// void (*cleanup_stale_sessions)(void);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid cleanup_stale_sessions;

    /// <summary>
    /// void (*cleanup_all_sessions)(void);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid cleanup_all_sessions;

    /// <summary>
    /// Bool (*process_key)(RimeSessionId session_id, int keycode, int mask);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool3 process_key;

    /// <summary>
    /// Bool (*commit_composition)(RimeSessionId session_id);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool2 commit_composition;

    /// <summary>
    /// void (*clear_composition)(RimeSessionId session_id);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid1 clear_composition;

    /// <summary>
    /// Bool (*get_commit)(RimeSessionId session_id, RimeCommit* commit);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 get_commit;

    /// <summary>
    /// Bool (*free_commit)(RimeCommit* commit);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool2 free_commit;

    /// <summary>
    /// Bool (*get_context)(RimeSessionId session_id, RimeContext* context);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 get_context;

    /// <summary>
    /// Bool(*free_context)(RimeContext* ctx);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool2 free_context;

    /// <summary>
    /// Bool (*get_status)(RimeSessionId session_id, RimeStatus* status);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 get_status;

    /// <summary>
    /// Bool (*free_status)(RimeStatus* status);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool2 free_status;

    /// <summary>
    /// void (*set_option)(RimeSessionId session_id, const char* option, Bool value);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid4 set_option;

    /// <summary>
    /// Bool (*get_option)(RimeSessionId session_id, const char* option);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 get_option;

    /// <summary>
    /// void (*set_property)(RimeSessionId session_id,
    ///                        const char* prop,
    ///                        const char* value);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool12 set_property;

    /// <summary>
    /// Bool (*get_property)(RimeSessionId session_id,
    ///                        const char* prop,
    ///                        char* value,
    ///                        size_t buffer_size);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool13 get_property;

    /// <summary>
    /// Bool (*get_schema_list)(RimeSchemaList* schema_list);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool2 get_schema_list;

    /// <summary>
    /// void (*free_schema_list)(RimeSchemaList* schema_list);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid1 free_schema_list;

    /// <summary>
    /// Bool (*get_current_schema)(RimeSessionId session_id,
    ///                              char* schema_id,
    ///                              size_t buffer_size);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool11 get_current_schema;

    /// <summary>
    /// Bool (*select_schema)(RimeSessionId session_id, const char* schema_id);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 select_schema;

    /// <summary>
    /// Bool (*schema_open)(const char* schema_id, RimeConfig* config);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 schema_open;

    /// <summary>
    /// Bool (*config_open)(const char* config_id, RimeConfig* config);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 config_open;

    /// <summary>
    /// Bool (*config_close)(RimeConfig* config);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool2 config_close;

    /// <summary>
    /// Bool (*config_get_bool)(RimeConfig* config, const char* key, Bool* value);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool12 config_get_bool;

    /// <summary>
    /// Bool (*config_get_int)(RimeConfig* config, const char* key, int* value);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool12 config_get_int;

    /// <summary>
    /// Bool (*config_get_double)(RimeConfig* config, const char* key, double* value);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool12 config_get_double;

    /// <summary>
    /// Bool (*config_get_string)(RimeConfig* config,
    ///                             const char* key,
    ///                             char* value,
    ///                             size_t buffer_size);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool13 config_get_string;

    /// <summary>
    /// const char* (*config_get_cstring)(RimeConfig* config, const char* key);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeIntPtr2 config_get_cstring;

    /// <summary>
    /// Bool (*config_update_signature)(RimeConfig* config, const char* signer);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 config_update_signature;

    /// <summary>
    /// Bool (*config_begin_map)(RimeConfigIterator* iterator,
    ///                            RimeConfig* config,
    ///                            const char* key);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool12 config_begin_map;

    /// <summary>
    /// Bool (*config_next)(RimeConfigIterator* iterator);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool2 config_next;

    /// <summary>
    /// void (*config_end)(RimeConfigIterator* iterator);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid1 config_end;

    /// <summary>
    /// Bool (*simulate_key_sequence)(RimeSessionId session_id,
    ///                                 const char* key_sequence);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 simulate_key_sequence;

    /// <summary>
    /// Bool (*register_module)(RimeModule* module);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool2 register_module;

    /// <summary>
    /// RimeModule* (*find_module)(const char* module_name);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeIntPtr1 find_module;

    /// <summary>
    /// Bool (*run_task)(const char* task_name);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool2 run_task;

    /// <summary>
    /// const char* (*get_shared_data_dir)(void);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [Obsolete("use get_shared_data_dir_s instead.")]
    public DTypeIntPtr get_shared_data_dir;

    /// <summary>
    /// const char* (*get_user_data_dir)(void);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [Obsolete("use get_user_data_dir_s instead.")]
    public DTypeIntPtr get_user_data_dir;

    /// <summary>
    /// const char* (*get_sync_dir)(void);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [Obsolete("use get_sync_dir_s instead.")]
    public DTypeIntPtr get_sync_dir;

    /// <summary>
    /// const char* (*get_user_id)(void);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeIntPtr get_user_id;

    /// <summary>
    /// void (*get_user_data_sync_dir)(char* dir, size_t buffer_size);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid3 get_user_data_sync_dir;

    /// <summary>
    /// Bool (*config_init)(RimeConfig* config);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool2 config_init;

    /// <summary>
    /// Bool (*config_load_string)(RimeConfig* config, const char* yaml);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 config_load_string;

    /// <summary>
    /// Bool (*config_set_bool)(RimeConfig* config, const char* key, Bool value);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool7 config_set_bool;

    /// <summary>
    /// Bool (*config_set_int)(RimeConfig* config, const char* key, int value);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool8 config_set_int;

    /// <summary>
    /// Bool (*config_set_double)(RimeConfig* config, const char* key, double value);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool9 config_set_double;

    /// <summary>
    /// Bool (*config_set_string)(RimeConfig* config,
    ///                             const char* key,
    ///                             const char* value);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool12 config_set_string;

    /// <summary>
    /// Bool (*config_get_item)(RimeConfig* config,
    ///                           const char* key,
    ///                           RimeConfig* value);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool12 config_get_item;

    /// <summary>
    /// Bool (*config_set_item)(RimeConfig* config,
    ///                           const char* key,
    ///                           RimeConfig* value);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool12 config_set_item;

    /// <summary>
    /// Bool (*config_clear)(RimeConfig* config, const char* key);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 config_clear;

    /// <summary>
    /// Bool (*config_create_list)(RimeConfig* config, const char* key);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 config_create_list;

    /// <summary>
    /// Bool (*config_create_map)(RimeConfig* config, const char* key);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 config_create_map;

    /// <summary>
    /// size_t (*config_list_size)(RimeConfig* config, const char* key);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeSize1 config_list_size;

    /// <summary>
    /// Bool (*config_begin_list)(RimeConfigIterator* iterator,
    ///                             RimeConfig* config,
    ///                             const char* key);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool12 config_begin_list;

    /// <summary>
    /// const char* (*get_input)(RimeSessionId session_id);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeIntPtr1 get_input;

    /// <summary>
    /// size_t (*get_caret_pos)(RimeSessionId session_id);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeSize get_caret_pos;

    /// <summary>
    /// Bool (*select_candidate)(RimeSessionId session_id, size_t index);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool5 select_candidate;

    /// <summary>
    /// const char* (*get_version)(void);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeIntPtr get_version;

    /// <summary>
    /// void (*set_caret_pos)(RimeSessionId session_id, size_t caret_pos);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid3 set_caret_pos;

    /// <summary>
    /// Bool (*select_candidate_on_current_page)(RimeSessionId session_id,
    ///                                            size_t index);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid3 select_candidate_on_current_page;

    /// <summary>
    /// Bool (*candidate_list_begin)(RimeSessionId session_id,
    ///                                RimeCandidateListIterator* iterator);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 candidate_list_begin;

    /// <summary>
    /// Bool (*candidate_list_next)(RimeCandidateListIterator* iterator);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool2 candidate_list_next;

    /// <summary>
    /// void (*candidate_list_end)(RimeCandidateListIterator* iterator);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid1 candidate_list_end;

    /// <summary>
    /// Bool (*user_config_open)(const char* config_id, RimeConfig* config);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 user_config_open;

    /// <summary>
    /// Bool (*candidate_list_from_index)(RimeSessionId session_id,
    ///                                     RimeCandidateListIterator* iterator,
    ///                                     int index);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool10 candidate_list_from_index;

    /// <summary>
    /// const char* (*get_prebuilt_data_dir)(void);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [Obsolete("use get_prebuilt_data_dir_s instead.")]
    public DTypeIntPtr get_prebuilt_data_dir;

    /// <summary>
    /// const char* (*get_staging_dir)(void);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [Obsolete("use get_staging_dir_s instead.")]
    public DTypeIntPtr get_staging_dir;

    /// <summary>
    /// void (*commit_proto)(RimeSessionId session_id,
    ///                        RIME_PROTO_BUILDER* commit_builder);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [Obsolete("for capnproto API, use \"proto\" module from librime-proto plugin.")]
    public DTypeVoid2 commit_proto;

    /// <summary>
    /// void (*context_proto)(RimeSessionId session_id,
    ///                         RIME_PROTO_BUILDER* context_builder);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid2 context_proto;

    /// <summary>
    /// void (*status_proto)(RimeSessionId session_id,
    ///                        RIME_PROTO_BUILDER* status_builder);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid2 status_proto;

    /// <summary>
    /// const char* (*get_state_label)(RimeSessionId session_id,
    ///                                  const char* option_name,
    ///                                  Bool state);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeIntPtr3 get_state_label;

    /// <summary>
    /// Bool (*delete_candidate)(RimeSessionId session_id, size_t index);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool5 delete_candidate;

    /// <summary>
    /// Bool (*delete_candidate_on_current_page)(RimeSessionId session_id,
    ///                                            size_t index);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool5 delete_candidate_on_current_page;

    /// <summary>
    /// RimeStringSlice (*get_state_label_abbreviated)(RimeSessionId session_id,
    ///                                                  const char* option_name,
    ///                                                  Bool state,
    ///                                                  Bool abbreviated);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeStringSlice get_state_label_abbreviated;

    /// <summary>
    /// Bool (*set_input)(RimeSessionId session_id, const char* input);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool6 set_input;

    /// <summary>
    /// void (*get_shared_data_dir_s)(char* dir, size_t buffer_size);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid3 get_shared_data_dir_s;

    /// <summary>
    /// void (*get_user_data_dir_s)(char* dir, size_t buffer_size);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid3 get_user_data_dir_s;

    /// <summary>
    /// void (*get_prebuilt_data_dir_s)(char* dir, size_t buffer_size);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid3 get_prebuilt_data_dir_s;

    /// <summary>
    /// void (*get_staging_dir_s)(char* dir, size_t buffer_size);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid3 get_staging_dir_s;

    /// <summary>
    /// void (*get_sync_dir_s)(char* dir, size_t buffer_size);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeVoid3 get_sync_dir_s;

    /// <summary>
    /// Bool (*highlight_candidate)(RimeSessionId session_id, size_t index);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool5 highlight_candidate;

    /// <summary>
    /// Bool (*highlight_candidate_on_current_page)(RimeSessionId session_id,
    ///                                               size_t index);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool5 highlight_candidate_on_current_page;

    /// <summary>
    /// Bool (*change_page)(RimeSessionId session_id, Bool backward);
    /// </summary>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DTypeBool4 change_page;
}
