using System;
using System.Diagnostics;
using System.IO;

namespace HaloWwise
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Make Debug Write and Console Write the same thing with Trace
            TraceListener[] listeners = new TraceListener[] { new TextWriterTraceListener(Console.Out) };
            Debug.Listeners.AddRange(listeners);

            if (args.Length < 2)
            {
                Trace.WriteLine("Un-packs *.wem and *.bnk files from a Halo 4/5 Wwise *.pck file.");
                Trace.WriteLine("Usage: ");
                Trace.WriteLine("\tHaloWwise.exe <input.pck> <extraction path>");
                Trace.WriteLine("\tHaloWwise.exe <input directory (recursive)> <extraction path>");

                Trace.WriteLine("\n\nNote: If you place ww2ogg.exe and revorb.exe (with the codebook) in the same Path as this Exe, it will attempt to convert *.wem files to *.ogg during extraction (H5 only)\n");

                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                return;
            }

            string input = args[0];
            string output = args[1];

            // If output isn't Directory, or not there, bail
            if (File.Exists(output))
            {
                if (!File.GetAttributes(output).HasFlag(FileAttributes.Directory))
                {
                    Trace.WriteLine("Error: Output is not a Directory");
                    return;
                }
            }
            else
            {
                if (!Directory.Exists(output))
                {
                    Directory.CreateDirectory(output);
                }
            }
            //if (!File.GetAttributes(output).HasFlag(FileAttributes.Directory))
            //    Trace.WriteLine("Error: Output is not a Directory");
            //else

            // Chage what we do, depending on if input is a file or a folder
            if (File.Exists(input))
            {
                // Extract just the one Pack
                try
                {
                    PackManager packManager = new PackManager(input, output);
                    packManager.ExtractPack();
                }
                catch
                {
                    Trace.WriteLine($"Error: Unable to process {input}");
                    return;
                }
            }
            else
            {
                if (Directory.Exists(input))
                {
                    // Find all Packs, extract them to output
                    string[] packs = Directory.GetFiles(input, "*.pck", SearchOption.AllDirectories);
                    foreach (string pack in packs)
                    {
                        try
                        {
                            PackManager packManager = new PackManager(pack, output);
                            packManager.ExtractPack();
                        }
                        catch
                        {
                            Trace.WriteLine($"Error: Unable to process {pack}");
                        }
                    }
                }
                else
                {
                    Trace.WriteLine($"Error: {input} not found.");
                }
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
