namespace RimeSharp.Test;

internal class Program
{
    static void PrintStatus(RimeStatus status)
    {
        Console.WriteLine($"schema: {status.SchemaId} / {status.SchemaName}");
        Console.Write("status: ");
        if (status.IsDisabled)
            Console.Write("disabled ");
        if (status.IsComposing)
            Console.Write("composing ");
        if (status.IsAsciiMode)
            Console.Write("ascii ");
        if (status.IsFullShape)
            Console.Write("full_shape ");
        if (status.IsSimplified)
            Console.Write("simplified ");
        Console.WriteLine();
    }

    static void PrintComposition(RimeComposition composition)
    {
        string preedit = composition.Preedit;
        if (preedit == null)
            return;
        int len = preedit.Length;
        int start = composition.SelectStart;
        int end = composition.SelectEnd;
        int cursor = composition.CursorPos;
        for (int i = 0; i <= len; ++i)
        {
            if (start < end)
            {
                if (i == start)
                {
                    Console.Write('[');
                }
                else if (i == end)
                {
                    Console.Write(']');
                }
            }
            if (i == cursor)
                Console.Write('|');
            if (i < len)
                Console.Write(preedit[i]);
        }
        Console.WriteLine();
    }

    static void PrintMenu(RimeMenu menu)
    {
        if (menu.NumCandidates == 0)
            return;
        Console.WriteLine($"page: {menu.PageNo + 1}{(menu.IsLastPage ? '$' : ' ')} (of size {menu.PageSize})");
        for (int i = 0; i < menu.NumCandidates; ++i)
        {
            bool highlighted = i == menu.HighlightedCandidateIndex;
            Console.WriteLine($"{i + 1}. {(highlighted ? '[' : ' ')}{menu.Candidates[i].Text}{(highlighted ? ']' : ' ')}{(menu.Candidates[i].Comment != null ? menu.Candidates[i].Comment : "")}");
        }
    }

    static void PrintContext(RimeContext context)
    {
        if (context.Composition.Length > 0 || context.Menu.NumCandidates > 0)
        {
            PrintComposition(context.Composition);
        }
        else
        {
            Console.WriteLine("(not composing)");
        }
        PrintMenu(context.Menu);
    }

    static unsafe void Main(string[] args)
    {
        Rime.Init(AppContext.BaseDirectory, Handel);

        Rime.GetSchemaList(out var list);

        var session = Rime.CreateSession();
        //Rime.RimeSelectSchema(session, list.list[0].name);

        Console.WriteLine("Start input");
        while (true)
        {
            var key = Console.ReadLine();
            var res = Rime.SimulateKeySequence(session, key);
            if (!res)
            {
                Console.WriteLine("Error key input");
            }
            else
            {
                if (Rime.GetCommit(session, out var commit))
                {
                    Console.WriteLine("commit: " + commit!.Value.text);
                }

                if (Rime.GetStatus(session, out var status))
                {
                    PrintStatus((RimeStatus)status!);
                }

                if (Rime.RimeGetContext(session, out var context))
                {
                    PrintContext((RimeContext)context!);
                }

                Rime.HighlightCandidate(session, 1);
            }
        }
    }

    public static void Handel(IntPtr context_object, IntPtr session_id, string message_type, string message_value)
    {
        Console.WriteLine($"message: [{session_id}] [{message_type}] {message_value}");
    }
}
