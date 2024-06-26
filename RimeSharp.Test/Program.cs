﻿using System.Runtime.InteropServices;
using System.Text;

namespace RimeSharp.Test;

internal class Program
{
    static void PrintStatus(RimeStatus status)
    {
        Console.WriteLine($"schema: {status.schema_id} / {status.schema_name}");
        Console.Write("status: ");
        if (status.is_disabled)
            Console.Write("disabled ");
        if (status.is_composing)
            Console.Write("composing ");
        if (status.is_ascii_mode)
            Console.Write("ascii ");
        if (status.is_full_shape)
            Console.Write("full_shape ");
        if (status.is_simplified)
            Console.Write("simplified ");
        Console.WriteLine();
    }

    static void PrintComposition(RimeComposition composition)
    {
        string preedit = composition.preedit;
        if (preedit == null)
            return;
        int len = preedit.Length;
        int start = composition.sel_start;
        int end = composition.sel_end;
        int cursor = composition.cursor_pos;
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
        if (menu.num_candidates == 0)
            return;
        Console.WriteLine($"page: {menu.page_no + 1}{(menu.is_last_page ? '$' : ' ')} (of size {menu.page_size})");
        for (int i = 0; i < menu.num_candidates; ++i)
        {
            bool highlighted = i == menu.highlighted_candidate_index;
            Console.WriteLine($"{i + 1}. {(highlighted ? '[' : ' ')}{menu.candidates[i].text}{(highlighted ? ']' : ' ')}{(menu.candidates[i].comment != null ? menu.candidates[i].comment : "")}");
        }
    }

    static void PrintContext(RimeContext context)
    {
        if (context.composition.length > 0 || context.menu.num_candidates > 0)
        {
            PrintComposition(context.composition);
        }
        else
        {
            Console.WriteLine("(not composing)");
        }
        PrintMenu(context.menu);
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
