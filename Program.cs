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

        var rootCommand = new RootCommand("BatchEncode - A tool for batch file encoding conversion.") { sourceOption, targetOption, sourceEncodingOption, targetEncodingOption, filterOption };

        rootCommand.SetHandler(
            (source, target, sourceEncoding, targetEncoding, filter) =>
            {
                ConvertEncoding(source, target, sourceEncoding, targetEncoding, filter);
            },
            sourceOption, targetOption, sourceEncodingOption, targetEncodingOption, filterOption
        );

        return await rootCommand.InvokeAsync(args);
    }

    private static void ConvertEncoding(string sourceDirectory, string targetDirectory, string sourceEncoding, string targetEncoding, string filter)
    {
        // 确保目标文件夹存在
        Directory.CreateDirectory(targetDirectory);

        // 获取需要处理的文件列表
        var files = Directory.GetFiles(sourceDirectory, filter, SearchOption.AllDirectories);

        // 设置源和目标编码
        var sourceEnc = Encoding.GetEncoding(sourceEncoding);
        var targetEnc = Encoding.GetEncoding(targetEncoding);

        Console.WriteLine($"Starting encoding conversion for {files.Length} files...");

        // 使用 Parallel.ForEach 并行处理
        Parallel.ForEach(files, file =>
        {
            try
            {
                // 计算目标文件路径
                var relativePath = Path.GetRelativePath(sourceDirectory, file);
                var targetFile = Path.Combine(targetDirectory, relativePath);

                // 确保目标文件夹存在
                Directory.CreateDirectory(Path.GetDirectoryName(targetFile)!);

                // 读取源文件内容
                var content = File.ReadAllText(file, sourceEnc);

                // 写入目标文件
                File.WriteAllText(targetFile, content, targetEnc);

                Console.WriteLine($"Converted: {file} -> {targetFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file '{file}': {ex.Message}");
            }
        });

        Console.WriteLine("Encoding conversion completed.");
    }
}
