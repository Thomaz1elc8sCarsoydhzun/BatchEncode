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
        var rootCommand = new RootCommand("BatchEncode: A tool to batch convert file encodings");

        var sourceDirOption = new Option<string>(
            name: "--source",
            description: "Path to the source directory",
            isDefault: false
        )
        { IsRequired = true };

        var targetDirOption = new Option<string>(
            name: "--target",
            description: "Path to the target directory",
            isDefault: false
        )
        { IsRequired = true };

        var sourceEncodingOption = new Option<string>(
            name: "--source-encoding",
            description: "Source file encoding (e.g., GB2312, UTF-8)",
            isDefault: false
        )
        { IsRequired = true };

        var targetEncodingOption = new Option<string>(
            name: "--target-encoding",
            description: "Target file encoding (e.g., UTF-8, ASCII)",
            isDefault: false
        )
        { IsRequired = true };

        var filterOption = new Option<string>(
            name: "--filter",
            description: "File extension filter (e.g., .txt, .csv). Leave empty for all files.",
            isDefault: () => "*.*"
        );

        // 添加参数到命令中
        rootCommand.AddOption(sourceDirOption);
        rootCommand.AddOption(targetDirOption);
        rootCommand.AddOption(sourceEncodingOption);
        rootCommand.AddOption(targetEncodingOption);
        rootCommand.AddOption(filterOption);

        rootCommand.Description = "BatchEncode: Convert file encodings in bulk with optional file filtering.";

        // 定义命令执行逻辑
        rootCommand.Handler = CommandHandler.Create<string, string, string, string, string>(ConvertEncoding);

        // 运行命令
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
