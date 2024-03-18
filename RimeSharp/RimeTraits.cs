namespace RimeSharp;

public unsafe struct RimeTraits
{
    public int data_size;
    // v0.9
    public string shared_data_dir;
    public string user_data_dir;
    public string distribution_name;
    public string distribution_code_name;
    public string distribution_version;
    // v1.0
    /*!
     * Pass a C-string constant in the format "rime.x"
     * where 'x' is the name of your application.
     * Add prefix "rime." to ensure old log files are automatically cleaned.
     */
    public string app_name;

    //! A list of modules to load before initializing
    public byte** modules;
    // v1.6
    /*! Minimal level of logged messages.
     *  Value is passed to Glog library using FLAGS_minloglevel variable.
     *  0 = INFO (default), 1 = WARNING, 2 = ERROR, 3 = FATAL
     */
    public int min_log_level;
    /*! Directory of log files.
     *  Value is passed to Glog library using FLAGS_log_dir variable.
     *  NULL means temporary directory, and "" means only writing to stderr.
     */
    public string log_dir;
    /// <summary>
    /// prebuilt data directory. defaults to ${shared_data_dir}/build
    /// </summary>
    public string prebuilt_data_dir;
    /// <summary>
    /// staging directory. defaults to ${user_data_dir}/build
    /// </summary>
    public string staging_dir;
}
