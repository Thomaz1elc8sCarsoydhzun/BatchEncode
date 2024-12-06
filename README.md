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
 
- **.NET 版本** ：.NET 8 或更高版本


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
 
3. 生成的可执行文件位于 `bin/Release/net8.0/` 目录下。


---


### 使用方法 
在命令行中运行 `BatchEncode` 程序，并使用以下参数：

```bash
BatchEncode --source <源目录> --target <目标目录> --source-encoding <源文件编码> --target-encoding <目标文件编码> [可选参数]
```

#### 必填参数 
 
- `--source`：指定包含待转换文件的源目录。
 
- `--target`：指定保存转换后文件的目标目录。
 
- `--source-encoding`：源文件的编码格式（例如：`UTF-8`）。
 
- `--target-encoding`：目标文件的编码格式（例如：`ASCII`）。

#### 可选参数 
 
- `--filter`：文件筛选条件（例如：`*.txt`）。默认为 `*.*`。
 
- `--log`：启用日志记录功能，日志文件保存为 `conversion.log`。默认值：`false`。
 
- `--recursive`：启用递归模式，处理子目录中的文件。默认值：`true`。
 
- `--backup`：启用备份功能，将源文件备份到目标目录下的 `backup` 文件夹中。默认值：`false`。


---


### 示例 

#### 启用备份和日志功能 
将 `input` 目录中的 `.txt` 文件转换到 `output` 目录，将编码从 `UTF-8` 转换为 `ASCII`，同时启用备份和日志记录功能：

```bash
BatchEncode --source ./input --target ./output --source-encoding UTF-8 --target-encoding ASCII --filter *.txt --log --backup
```

#### 非递归转换，禁用备份 
将 `input` 目录中的文件（不包括子目录）转换到 `output`，编码从 `UTF-8` 转换为 `ASCII`，禁用备份：

```bash
BatchEncode --source ./input --target ./output --source-encoding UTF-8 --target-encoding ASCII --recursive false
```

#### 默认筛选条件，禁用日志 
将 `input` 目录中的所有文件转换到 `output`，不设置文件筛选条件，也不启用日志功能：

```bash
BatchEncode --source ./input --target ./output --source-encoding UTF-8 --target-encoding ASCII
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
