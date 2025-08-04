namespace RimeSharp;

using System.Runtime.InteropServices;

/// <summary>
/// Rime traits structure
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct RimeTraits
{
    public readonly int DataSize;

    // v0.9
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string SharedDataDir;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string UserDataDir;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string DistributionName;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string DistributionCodeName;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string DistributionVersion;

    // v1.0
    /// <summary>
    /// Pass a C-string constant in the format "rime.x"
    /// where 'x' is the name of your application.
    /// Add prefix "rime." to ensure old log files are automatically cleaned.
    /// </summary>
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string AppName;

    /// <summary>
    /// A list of modules to load before initializing
    /// </summary>
    public byte** Modules;

    // v1.6
    /// <summary>
    /// Minimal level of logged messages.
    /// Value is passed to Glog library using FLAGS_minloglevel variable.
    /// 0 = INFO (default), 1 = WARNING, 2 = ERROR, 3 = FATAL
    /// </summary>
    public int MinLogLevel;
    /// <summary>
    /// Directory of log files.
    /// Value is passed to Glog library using FLAGS_log_dir variable.
    /// NULL means temporary directory, and "" means only writing to stderr.
    /// </summary>
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string LogDir;
    /// <summary>
    /// prebuilt data directory. defaults to ${shared_data_dir}/build
    /// </summary>
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string PrebuiltDataDir;
    /// <summary>
    /// staging directory. defaults to ${user_data_dir}/build
    /// </summary>
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string StagingDir;

    public RimeTraits()
    {
        DataSize = Marshal.SizeOf<RimeTraits>() - sizeof(int);
    }
}
