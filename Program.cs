using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Text;

class Program
{
    static async Task<int> Main(string[] args)
    {
        // 定义命令行参数
        var sourceOption = new Option<string>(
            name: "--source",
            description: "Source directory containing the files to convert."
        );

        var targetOption = new Option<string>(
            name: "--target",
            description: "Target directory to save the converted files."
        );

        var sourceEncodingOption = new Option<string>(
            name: "--source-encoding",
            description: "Encoding of the source files, e.g., UTF-8."
        );

        var targetEncodingOption = new Option<string>(
            name: "--target-encoding",
            description: "Encoding of the target files, e.g., ASCII."
        );

        var filterOption = new Option<string>(
            name: "--filter",
            description: "Filter for files, e.g., *.txt (default: *.*).",
            getDefaultValue: () => "*.*"
        );

        var logOption = new Option<bool>(
            name: "--log",
            description: "Enable logging to conversion.log.",
            getDefaultValue: () => false);
        
        var recursiveOption = new Option<bool>(
            name: "--recursive",
            description: "Enable recursive file processing (default: true).",
            getDefaultValue: () => true);


        var rootCommand = new RootCommand("BatchEncode - A tool for batch file encoding conversion.") { sourceOption, targetOption, sourceEncodingOption, targetEncodingOption, filterOption, logOption, recursiveOption };

        rootCommand.SetHandler(ConvertEncoding, sourceOption, targetOption, sourceEncodingOption, targetEncodingOption, filterOption, logOption, recursiveOption);

        return await rootCommand.InvokeAsync(args);
    }

    private static void ConvertEncoding(string sourceDirectory, string targetDirectory, string sourceEncoding, string targetEncoding, string filter, bool enableLogging, bool isRecursive)
    {
        // 确保目标目录存在
        Directory.CreateDirectory(targetDirectory);

        // 搜索文件的模式
        var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

        // 获取要处理的文件列表
        var files = Directory.GetFiles(sourceDirectory, filter, searchOption);
        var sourceEnc = Encoding.GetEncoding(sourceEncoding);
        var targetEnc = Encoding.GetEncoding(targetEncoding);

        StreamWriter? logWriter = null;
        if (enableLogging)
        {
            // 准备日志文件
            var logFile = Path.Combine(targetDirectory, "conversion.log");
            logWriter = new StreamWriter(logFile, append: false, encoding: Encoding.UTF8);

            logWriter.WriteLine($"Conversion started at {DateTime.Now}");
            logWriter.WriteLine($"Source Directory: {sourceDirectory}");
            logWriter.WriteLine($"Target Directory: {targetDirectory}");
            logWriter.WriteLine($"Source Encoding: {sourceEncoding}");
            logWriter.WriteLine($"Target Encoding: {targetEncoding}");
            logWriter.WriteLine($"Filter: {filter}");
            logWriter.WriteLine($"Recursive: {isRecursive}");
            logWriter.WriteLine();
        }

        Console.WriteLine($"Converting {files.Length} files...");

        // 并行处理文件
        Parallel.ForEach(files, file =>
        {
            try
            {
                var targetFile = Path.Combine(targetDirectory, Path.GetRelativePath(sourceDirectory, file));
                Directory.CreateDirectory(Path.GetDirectoryName(targetFile)!);

                // 转换文件内容
                File.WriteAllText(targetFile, File.ReadAllText(file, sourceEnc), targetEnc);
                Console.WriteLine($"Converted: {file}");
                if (enableLogging)
                {
                    lock (logWriter!)
                    {
                        logWriter.WriteLine($"SUCCESS: {file} -> {targetFile}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {file}: {ex.Message}");
                if (enableLogging)
                {
                    lock (logWriter!)
                    {
                        logWriter.WriteLine($"ERROR: {file} - {ex.Message}");
                    }
                }
            }
        });

        if (enableLogging)
        {
            logWriter!.WriteLine();
            logWriter.WriteLine($"Conversion completed at {DateTime.Now}");
            logWriter.Close();
        }

        Console.WriteLine("Conversion completed.");
        if (enableLogging)
        {
            Console.WriteLine("See conversion.log for details.");
        }
    }
}
