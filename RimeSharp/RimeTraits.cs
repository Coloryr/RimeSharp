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
    public string SharedDataDir;
    public string UserDataDir;
    public string DistributionName;
    public string DistributionCodeName;
    public string DistributionVersion;

    // v1.0
    /// <summary>
    /// Pass a C-string constant in the format "rime.x"
    /// where 'x' is the name of your application.
    /// Add prefix "rime." to ensure old log files are automatically cleaned.
    /// </summary>
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
    public string LogDir;
    /// <summary>
    /// prebuilt data directory. defaults to ${shared_data_dir}/build
    /// </summary>
    public string PrebuiltDataDir;
    /// <summary>
    /// staging directory. defaults to ${user_data_dir}/build
    /// </summary>
    public string StagingDir;

    public RimeTraits()
    {
        DataSize = Marshal.SizeOf<RimeTraits>() - sizeof(int);
    }
}
