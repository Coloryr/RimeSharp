namespace RimeSharp;

public unsafe struct RimeContext
{
    public int DataSize;

    // v0.9
    public RimeComposition Composition;
    public RimeMenu Menu;
    // v0.9.2
    public string CommitTextPreview;
    public string[] SelectLabels;
}
