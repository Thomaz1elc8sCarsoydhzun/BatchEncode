# BatchEncode 
**BatchEncode**  是一个用 C# 编写的命令行工具，用于批量转换文件编码，并支持通过文件扩展名进行过滤。它可以帮助开发者在处理不同编码的文件时高效完成转换任务。
## 功能特点 

- 批量将文件从一种编码转换为另一种编码。
 
- 支持自定义文件扩展名过滤，例如只处理 `.txt` 或 `.csv` 文件。

- 自动创建目标文件夹（如果不存在）。
 
- 基于现代命令行解析库 `System.CommandLine`，支持用户友好的命令行界面。


---


## 系统要求 
 
- **操作系统** ：Windows, macOS, Linux
 
- **.NET 版本** ：.NET 6 或更高版本


---


## 安装 

### 从源码构建 
 
1. 克隆仓库：


```bash
git clone https://github.com/yourusername/BatchEncode.git
cd BatchEncode
```
 
2. 构建项目：


```bash
dotnet build -c Release
```
 
3. 生成的可执行文件位于 `bin/Release/net6.0/` 目录下。


---


## 使用方法 

### 基本命令格式 


```bash
BatchEncode --source <source_directory> --target <target_directory> --source-encoding <source_encoding> --target-encoding <target_encoding> [--filter <file_extension>]
```

### 参数说明 
| 参数 | 必须 | 描述 | 
| --- | --- | --- | 
| --source | 是 | 源文件夹路径，包含需要转换的文件。 | 
| --target | 是 | 目标文件夹路径，转换后的文件将存放在此文件夹中。 | 
| --source-encoding | 是 | 源文件的编码格式，例如 UTF-8 或 GB2312。 | 
| --target-encoding | 是 | 目标文件的编码格式，例如 UTF-8 或 ASCII。 | 
| --filter | 否 | 文件扩展名过滤器，例如 *.txt（默认值为 *.*，表示处理所有文件）。 | 


---


### 示例 

#### 1. 转换所有文件 
将 `C:\SourceFolder` 中的所有文件从 `GB2312` 转换为 `UTF-8`，并保存到 `C:\TargetFolder`：

```bash
BatchEncode --source "C:\SourceFolder" --target "C:\TargetFolder" --source-encoding "GB2312" --target-encoding "UTF-8"
```

#### 2. 转换特定类型文件 
只转换 `.txt` 文件：

```bash
BatchEncode --source "C:\SourceFolder" --target "C:\TargetFolder" --source-encoding "UTF-8" --target-encoding "ASCII" --filter "*.txt"
```

#### 3. 查看帮助文档 

运行以下命令查看参数和使用说明：


```bash
BatchEncode --help
```


---


## 编码支持 
**BatchEncode**  支持 .NET 提供的所有编码类型，常用编码包括： 
- `UTF-8`
 
- `UTF-16`
 
- `ASCII`
 
- `GB2312`
 
- `ISO-8859-1`
完整支持列表可参考 [Microsoft 文档](https://learn.microsoft.com/en-us/dotnet/api/system.text.encoding.getencodings) 。
