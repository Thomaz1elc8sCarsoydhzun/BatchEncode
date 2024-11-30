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

    private static void ConvertEncoding(string source, string target, string sourceEncoding, string targetEncoding, string filter)
    {
        try
        {
            // 获取编码
            Encoding sourceEnc = Encoding.GetEncoding(sourceEncoding);
            Encoding targetEnc = Encoding.GetEncoding(targetEncoding);

            // 确保目标目录存在
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            // 遍历源文件夹中的匹配文件
            foreach (string filePath in Directory.GetFiles(source, filter))
            {
                string fileName = Path.GetFileName(filePath);
                string targetFilePath = Path.Combine(target, fileName);

                // 读取文件内容
                string content = File.ReadAllText(filePath, sourceEnc);

                // 写入新文件
                File.WriteAllText(targetFilePath, content, targetEnc);

                Console.WriteLine($"Converted: {filePath} -> {targetFilePath}");
            }

            Console.WriteLine("Encoding conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
