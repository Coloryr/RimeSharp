namespace RimeSharp;

public unsafe struct RimeContext
{
    public int data_size;
    // v0.9
    public RimeComposition composition;
    public RimeMenu menu;
    // v0.9.2
    public string commit_text_preview;
    public string[] select_labels;
}
