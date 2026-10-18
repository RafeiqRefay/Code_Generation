
    public class CommondPowerShell
    {
    public string ProjectPath { get; set; } = "";
    public string NameTable { get; set; } = "";
    public string ClassDTO { get; set; }= "";

    public string ClassUpdateDTO { get; set; } = "";
    public string ClassViewDTO { get; set; } = "";
    public string ClassSearch { get; set; } = "";
    public string ClassService { get; set; } = "";

    public string ClassData { get; set; } = "";

    public string ClassController { get; set; } = "";
    public string projectFolder { get; set; } = "";
    public string csprojFileName { get; set; } = "";
    public string DataBaseName { get; set; } = "";


    public string SqlGetAll { get; set; } = "";
    public string SqlGetOne { get; set; } = "";
    public string SqlAdd { get; set; } = "";
    public string SqlUpdate { get; set; } = "";
    public string SqlDelete { get; set; } = "";
    public string SqlSearch { get; set; } = "";
    public string FrontendEntity { get; set; } = "";
    public string PathEntity { get; set; } = "";
    public string PathPage { get; set; } = "";
    public string PathStore { get; set; } = "";

    public void InitializeProjectPaths()
    {
        // استخراج مجلد المشروع
        projectFolder = Path.GetDirectoryName(ProjectPath);
     

        // استخراج اسم الملف فقط
        csprojFileName = Path.GetFileName(ProjectPath);
    }

    public string CreateFolder()
    {
        return
            "# ==================================================\r\n" +
            "# ⚡ إعداد المسارات حسب مشروعك\r\n" +
            "# ==================================================\r\n" +
            $"$projectFolder = \"{projectFolder}\"\r\n" +
            $"$csprojFile = Join-Path $projectFolder \"{csprojFileName}\"\r\n" +
            "\r\n" +
            "# ==================================================\r\n" +
            "# 1️⃣ إنشاء المجلدات على القرص مع ملف placeholder\r\n" +
            "# ==================================================\r\n" +
            "$folders = @(\r\n" +
            "    \"Models\",\r\n" +
            "    \"Models\\DTO\",\r\n" +
             "    \"Models\\UpdateDTO\",\r\n" +

            "    \"Models\\ViewDTO\",\r\n" +
            "    \"Models\\Search\",\r\n"+

            "    \"Services\",\r\n" +
            "    \"DataAccessLayer\"\r\n" +
            ")\r\n" +
            "\r\n" +
            "foreach ($f in $folders) {\r\n" +
            "    $fullPath = Join-Path $projectFolder $f\r\n" +
            "\r\n" +
            "    # إنشاء المجلد إذا لم يكن موجود\r\n" +
            "    if (-not (Test-Path $fullPath)) {\r\n" +
            "        New-Item -ItemType Directory -Path $fullPath | Out-Null\r\n" +
            "        Write-Host \"✅ $f folder created on disk\"\r\n" +
            "    } else {\r\n" +
            "        Write-Host \"⚠️ $f folder already exists on disk\"\r\n" +
            "    }\r\n" +
            "\r\n" +
            "    # إنشاء ملف placeholder لضمان ظهور المجلد في Visual Studio\r\n" +
            "    $placeholder = Join-Path $fullPath \".keep\"\r\n" +
            "    if (-not (Test-Path $placeholder)) {\r\n" +
            "        New-Item -ItemType File -Path $placeholder | Out-Null\r\n" +
            "        Write-Host \"🎯 .keep placeholder created in $f\"\r\n" +
            "    }\r\n" +
            "}\r\n" +
            "\r\n" +
            "# ==================================================\r\n" +
            "# 2️⃣ إضافة المجلدات إلى .csproj\r\n" +
            "# ==================================================\r\n" +
            "[xml]$projXml = Get-Content $csprojFile\r\n" +
            "\r\n" +
            "# إنشاء ItemGroup جديد إذا لم يوجد\r\n" +
            "$itemGroup = $projXml.Project.ItemGroup | Where-Object { $_.Folder }\r\n" +
            "if (-not $itemGroup) {\r\n" +
            "    $itemGroup = $projXml.CreateElement(\"ItemGroup\")\r\n" +
            "    $projXml.Project.AppendChild($itemGroup) | Out-Null\r\n" +
            "}\r\n" +
            "\r\n" +
            "foreach ($f in $folders) {\r\n" +
            "    # تحقق أن المجلد غير موجود في XML مسبقًا\r\n" +
            "    if (-not ($projXml.Project.ItemGroup.Folder | Where-Object { $_.Include -eq \"$f\\\" })) {\r\n" +
            "        $folderNode = $projXml.CreateElement(\"Folder\")\r\n" +
            "        $folderNode.SetAttribute(\"Include\", \"$f\\\")\r\n" +
            "        $itemGroup.AppendChild($folderNode) | Out-Null\r\n" +
            "        Write-Host \"🎉 $f folder added to .csproj\"\r\n" +
            "    }\r\n" +
            "}\r\n" +
            "\r\n" +
            "# ==================================================\r\n" +
            "# 3️⃣ حفظ التعديلات\r\n" +
            "# ==================================================\r\n" +
            "$projXml.Save($csprojFile)\r\n" +
            "Write-Host \"✅ Folder structure ensured on disk and added to project!\"\r\n" +
            "Write-Host \"💡 كل المجلدات تظهر الآن مباشرة في Visual Studio بدون الحاجة لتشغيل السكربت مرتين.\"\r\n";
    }
    public string CreateFrontendEntity()
    {
        return $@"# ==========================
# Entity
# ==========================
$EntityPath = ""{PathEntity}""
$EntityFileName = ""{NameTable}Entity.js""

$Content = @'
{FrontendEntity}
'@

if (!(Test-Path $EntityPath)) {{
    New-Item -ItemType Directory -Path $EntityPath | Out-Null
}}

$EntityFullPath = Join-Path $EntityPath $EntityFileName
Set-Content -Path $EntityFullPath -Value $Content -Encoding UTF8

# ==========================
# Page (Empty File)
# ==========================
$PagePath = ""{PathPage}""
$PageFileName = ""{NameTable}Page.jsx""

if (!(Test-Path $PagePath)) {{
    New-Item -ItemType Directory -Path $PagePath | Out-Null
}}

$PageFullPath = Join-Path $PagePath $PageFileName
if (!(Test-Path $PageFullPath)) {{
    New-Item -ItemType File -Path $PageFullPath | Out-Null
}}

# ==========================
# Store (Empty File)
# ==========================
$StorePath = ""{PathStore}""
$StoreFileName = ""{NameTable}Store.js""

if (!(Test-Path $StorePath)) {{
    New-Item -ItemType Directory -Path $StorePath | Out-Null
}}

$StoreFullPath = Join-Path $StorePath $StoreFileName
if (!(Test-Path $StoreFullPath)) {{
    New-Item -ItemType File -Path $StoreFullPath | Out-Null
}}

Write-Host ""=====================================""
Write-Host ""Files created successfully!""
Write-Host $EntityFullPath
Write-Host $PageFullPath
Write-Host $StoreFullPath
Write-Host ""=====================================""
";
    }
    public string CreateDTO()
    {
        return $@"# =====================================
# مسار المشروع الرئيسي
# =====================================
$projectFolder = ""{projectFolder}""
$csprojFile = Join-Path $projectFolder ""{csprojFileName}""
$dtoFolder = Join-Path $projectFolder ""Models\DTO""

# إنشاء المجلد إذا لم يكن موجود
if (-Not (Test-Path $dtoFolder)) {{
    New-Item -ItemType Directory -Path $dtoFolder | Out-Null
}}

# إنشاء الكلاس {NameTable}DTO.cs
$dtoFile = Join-Path $dtoFolder ""{NameTable}DTO.cs""
if (-Not (Test-Path $dtoFile)) {{
    Set-Content -Path $dtoFile -Value @""
{ClassDTO}
""@
}}

# إضافة الكلاس إلى .csproj
[xml]$projXml = Get-Content $csprojFile

# الحصول على أول ItemGroup يحتوي على Compile أو إنشاء واحد جديد
$itemGroup = $projXml.Project.ItemGroup | Where-Object {{ $_.Compile }}
if (-not $itemGroup) {{
    $itemGroup = $projXml.CreateElement(""ItemGroup"")
    $projXml.Project.AppendChild($itemGroup) | Out-Null
}}

# التحقق من عدم وجود Compile بالفعل قبل الإضافة
if (-not ($projXml.Project.ItemGroup.Compile | Where-Object {{ $_.Include -eq ""Models\DTO\{NameTable}DTO.cs"" }})) {{
    $node = $projXml.CreateElement(""Compile"")
    $node.SetAttribute(""Include"", ""Models\DTO\{NameTable}DTO.cs"")
    $itemGroup.AppendChild($node) | Out-Null
}}

# حفظ التغييرات على ملف .csproj
$projXml.Save($csprojFile)";
    }

    public string CreateSearch()
    {
        return $@"# =====================================
# مسار المشروع الرئيسي
# =====================================
$projectFolder = ""{projectFolder}""
$csprojFile = Join-Path $projectFolder ""{csprojFileName}""
$dtoFolder = Join-Path $projectFolder ""Models\Search""

# إنشاء المجلد إذا لم يكن موجود
if (-Not (Test-Path $dtoFolder)) {{
    New-Item -ItemType Directory -Path $dtoFolder | Out-Null
}}

# إنشاء الكلاس {NameTable}DTO.cs
$dtoFile = Join-Path $dtoFolder ""{NameTable}Search.cs""
if (-Not (Test-Path $dtoFile)) {{
    Set-Content -Path $dtoFile -Value @""
{ClassSearch}
""@
}}

# إضافة الكلاس إلى .csproj
[xml]$projXml = Get-Content $csprojFile

# الحصول على أول ItemGroup يحتوي على Compile أو إنشاء واحد جديد
$itemGroup = $projXml.Project.ItemGroup | Where-Object {{ $_.Compile }}
if (-not $itemGroup) {{
    $itemGroup = $projXml.CreateElement(""ItemGroup"")
    $projXml.Project.AppendChild($itemGroup) | Out-Null
}}

# التحقق من عدم وجود Compile بالفعل قبل الإضافة
if (-not ($projXml.Project.ItemGroup.Compile | Where-Object {{ $_.Include -eq ""Models\Search\{NameTable}Search.cs"" }})) {{
    $node = $projXml.CreateElement(""Compile"")
    $node.SetAttribute(""Include"", ""Models\Search\{NameTable}Search.cs"")
    $itemGroup.AppendChild($node) | Out-Null
}}

# حفظ التغييرات على ملف .csproj
$projXml.Save($csprojFile)";
    }
    public string CreateViewDTO()
    {
        return $@"# =====================================
# مسار المشروع الرئيسي
# =====================================
$projectFolder = ""{projectFolder}""
$csprojFile = Join-Path $projectFolder ""{csprojFileName}""
$viewDtoFolder = Join-Path $projectFolder ""Models\ViewDTO""

# إنشاء المجلد إذا لم يكن موجود
if (-Not (Test-Path $viewDtoFolder)) {{
    New-Item -ItemType Directory -Path $viewDtoFolder | Out-Null
}}

# إنشاء الكلاس {NameTable}ViewDTO.cs
$viewDtoFile = Join-Path $viewDtoFolder ""{NameTable}ViewDTO.cs""
if (-Not (Test-Path $viewDtoFile)) {{
    Set-Content -Path $viewDtoFile -Value @""
{ClassViewDTO}
""@
}}

# إضافة الكلاس إلى .csproj
[xml]$projXml = Get-Content $csprojFile

$itemGroup = $projXml.Project.ItemGroup | Where-Object {{ $_.Compile }}
if (-not $itemGroup) {{
    $itemGroup = $projXml.CreateElement(""ItemGroup"")
    $projXml.Project.AppendChild($itemGroup) | Out-Null
}}

if (-not ($projXml.Project.ItemGroup.Compile | Where-Object {{ $_.Include -eq ""Models\ViewDTO\{NameTable}ViewDTO.cs"" }})) {{
    $node = $projXml.CreateElement(""Compile"")
    $node.SetAttribute(""Include"", ""Models\ViewDTO\{NameTable}ViewDTO.cs"")
    $itemGroup.AppendChild($node) | Out-Null
}}

$projXml.Save($csprojFile)";
    }
  
    public string CreateUpdateDTO()
    {
        return $@"# =====================================
# مسار المشروع الرئيسي
# =====================================
$projectFolder = ""{projectFolder}""
$csprojFile = Join-Path $projectFolder ""{csprojFileName}""
$UpdateDtoFolder = Join-Path $projectFolder ""Models\UpdateDTO""

# إنشاء المجلد إذا لم يكن موجود
if (-Not (Test-Path $UpdateDtoFolder)) {{
    New-Item -ItemType Directory -Path $UpdateDtoFolder | Out-Null
}}

# إنشاء الكلاس {NameTable}UpdateDTO.cs
$UpdateDtoFile = Join-Path $UpdateDtoFolder ""{NameTable}UpdateDTO.cs""
if (-Not (Test-Path $UpdateDtoFile)) {{
    Set-Content -Path $UpdateDtoFile -Value @""
{ClassUpdateDTO}
""@
}}

# إضافة الكلاس إلى .csproj
[xml]$projXml = Get-Content $csprojFile

$itemGroup = $projXml.Project.ItemGroup | Where-Object {{ $_.Compile }}
if (-not $itemGroup) {{
    $itemGroup = $projXml.CreateElement(""ItemGroup"")
    $projXml.Project.AppendChild($itemGroup) | Out-Null
}}

if (-not ($projXml.Project.ItemGroup.Compile | Where-Object {{ $_.Include -eq ""Models\UpdateDTO\{NameTable}UpdateDTO.cs"" }})) {{
    $node = $projXml.CreateElement(""Compile"")
    $node.SetAttribute(""Include"", ""Models\UpdateDTO\{NameTable}UpdateDTO.cs"")
    $itemGroup.AppendChild($node) | Out-Null
}}

$projXml.Save($csprojFile)";
    }


    public string CreateService()
    {
        return $@"# =====================================
# مسار المشروع الرئيسي
# =====================================
$projectFolder = ""{projectFolder}""
$csprojFile = Join-Path $projectFolder ""{csprojFileName}""
$servicesFolder = Join-Path $projectFolder ""Services""

if (-Not (Test-Path $servicesFolder)) {{
    New-Item -ItemType Directory -Path $servicesFolder | Out-Null
}}

$serviceFile = Join-Path $servicesFolder ""{NameTable}.cs""
if (-Not (Test-Path $serviceFile)) {{
    Set-Content -Path $serviceFile -Value @""
{ClassService}
""@
}}

[xml]$projXml = Get-Content $csprojFile
$itemGroup = $projXml.Project.ItemGroup | Where-Object {{ $_.Compile }}

if (-not $itemGroup) {{
    $itemGroup = $projXml.CreateElement(""ItemGroup"")
    $projXml.Project.AppendChild($itemGroup) | Out-Null
}}

if (-not ($projXml.Project.ItemGroup.Compile | Where-Object {{ $_.Include -eq ""Services\{NameTable}.cs"" }})) {{
    $node = $projXml.CreateElement(""Compile"")
    $node.SetAttribute(""Include"", ""Services\{NameTable}.cs"")
    $itemGroup.AppendChild($node) | Out-Null
}}

$projXml.Save($csprojFile)";
    }

    public string CreateDataAccess()
    {
        return $@"# =====================================
# مسار المشروع الرئيسي
# =====================================
$projectFolder = ""{projectFolder}""
$csprojFile = Join-Path $projectFolder ""{csprojFileName}""
$dalFolder = Join-Path $projectFolder ""DataAccessLayer""

if (-Not (Test-Path $dalFolder)) {{
    New-Item -ItemType Directory -Path $dalFolder | Out-Null
}}

$dalFile = Join-Path $dalFolder ""{NameTable}Data.cs""
if (-Not (Test-Path $dalFile)) {{
    Set-Content -Path $dalFile -Value @""
{ClassData}
""@
}}

[xml]$projXml = Get-Content $csprojFile
$itemGroup = $projXml.Project.ItemGroup | Where-Object {{ $_.Compile }}

if (-not $itemGroup) {{
    $itemGroup = $projXml.CreateElement(""ItemGroup"")
    $projXml.Project.AppendChild($itemGroup) | Out-Null
}}

if (-not ($projXml.Project.ItemGroup.Compile | Where-Object {{ $_.Include -eq ""DataAccessLayer\{NameTable}Data.cs"" }})) {{
    $node = $projXml.CreateElement(""Compile"")
    $node.SetAttribute(""Include"", ""DataAccessLayer\{NameTable}Data.cs"")
    $itemGroup.AppendChild($node) | Out-Null
}}

$projXml.Save($csprojFile)";
    }

    public string CreateController()
    {
        return $@"# =====================================
# مسار المشروع الرئيسي
# =====================================
$projectFolder = ""{projectFolder}""
$csprojFile = Join-Path $projectFolder ""{csprojFileName}""
$controllersFolder = Join-Path $projectFolder ""Controllers""

if (-Not (Test-Path $controllersFolder)) {{
    New-Item -ItemType Directory -Path $controllersFolder | Out-Null
}}

$controllerFile = Join-Path $controllersFolder ""{NameTable}Controller.cs""
if (-Not (Test-Path $controllerFile)) {{
    Set-Content -Path $controllerFile -Value @""
{ClassController}
""@
}}

[xml]$projXml = Get-Content $csprojFile
$itemGroup = $projXml.Project.ItemGroup | Where-Object {{ $_.Compile }}

if (-not $itemGroup) {{
    $itemGroup = $projXml.CreateElement(""ItemGroup"")
    $projXml.Project.AppendChild($itemGroup) | Out-Null
}}

if (-not ($projXml.Project.ItemGroup.Compile | Where-Object {{ $_.Include -eq ""Controllers\{NameTable}Controller.cs"" }})) {{
    $node = $projXml.CreateElement(""Compile"")
    $node.SetAttribute(""Include"", ""Controllers\{NameTable}Controller.cs"")
    $itemGroup.AppendChild($node) | Out-Null
}}

$projXml.Save($csprojFile)";
    }


    public  string CreateApiResult()
    {
        return $@"
# =====================================
# مسار المشروع الرئيسي
# =====================================
$projectFolder = ""{projectFolder}""
$csprojFile = Join-Path $projectFolder ""{csprojFileName}""

# إنشاء الكلاس ApiResult.cs مباشرة تحت المشروع
$apiResultFile = Join-Path $projectFolder ""ApiResult.cs""

if (-Not (Test-Path $apiResultFile)) {{
    Set-Content -Path $apiResultFile -Value @"" 

    public class ApiResult<T>
    {{
        public T? Data {{ get; set; }}
        public string? Message {{ get; set; }}

        public ErrorType ErrorType {{ get; set; }} = ErrorType.None;

        public ApiResult() {{ }}

        public ApiResult(T? data, string? message, ErrorType errorType)
        {{
            Data = data;
            Message = message;
            ErrorType = errorType;
        }}
    }}
""@
}}

# إضافة الكلاس إلى .csproj
[xml]$projXml = Get-Content $csprojFile
$itemGroup = $projXml.Project.ItemGroup | Where-Object {{ $_.Compile }}

if (-not $itemGroup) {{
    $itemGroup = $projXml.CreateElement(""ItemGroup"")
    $projXml.Project.AppendChild($itemGroup) | Out-Null
}}

if (-not ($projXml.Project.ItemGroup.Compile | Where-Object {{ $_.Include -eq ""ApiResult.cs"" }})) {{
    $node = $projXml.CreateElement(""Compile"")
    $node.SetAttribute(""Include"", ""ApiResult.cs"")
    $itemGroup.AppendChild($node) | Out-Null
}}

$projXml.Save($csprojFile)
";

    }






    public  string CreateEnErrorType()
    {
        return $@"
# =====================================
# مسار المشروع الرئيسي
# =====================================
$projectFolder = ""{projectFolder}""
$csprojFile = Join-Path $projectFolder ""{csprojFileName}""

# إنشاء الملف enErrorType.cs مباشرة تحت المشروع
$errorTypeFile = Join-Path $projectFolder ""enErrorType.cs""

if (-Not (Test-Path $errorTypeFile)) {{
    Set-Content -Path $errorTypeFile -Value @"" 

public enum ErrorType
{{
    None = 0,
    InvalidId = 1,
    NotFound = 2,
    AlreadyExists = 3,
    LogicalDependency = 4,
    DatabaseError = 5
}}

public static class ErrorTypeMapper
{{
    public static ErrorType GetErrorType(int errorCodeFromDb)
    {{
        return errorCodeFromDb switch
        {{
            0 => ErrorType.None,
            1 => ErrorType.InvalidId,
            2 => ErrorType.NotFound,
            3 => ErrorType.AlreadyExists,
            4 => ErrorType.LogicalDependency,
            _ => ErrorType.DatabaseError,
        }};
    }}
}}
""@
}}

# إضافة الملف إلى .csproj
[xml]$projXml = Get-Content $csprojFile
$itemGroup = $projXml.Project.ItemGroup | Where-Object {{ $_.Compile }}

if (-not $itemGroup) {{
    $itemGroup = $projXml.CreateElement(""ItemGroup"")
    $projXml.Project.AppendChild($itemGroup) | Out-Null
}}

if (-not ($projXml.Project.ItemGroup.Compile | Where-Object {{ $_.Include -eq ""enErrorType.cs"" }})) {{
    $node = $projXml.CreateElement(""Compile"")
    $node.SetAttribute(""Include"", ""enErrorType.cs"")
    $itemGroup.AppendChild($node) | Out-Null
}}

$projXml.Save($csprojFile)
";
    }
    public  string CreateApiResponseHelper()
    {
        return $@"
# =====================================
# مسار المشروع الرئيسي
# =====================================
$projectFolder = ""{projectFolder}""
$csprojFile = Join-Path $projectFolder ""{csprojFileName}""

# إنشاء الملف ApiResponseHelper.cs مباشرة تحت المشروع
$helperFile = Join-Path $projectFolder ""ApiResponseHelper.cs""

if (-Not (Test-Path $helperFile)) {{
    Set-Content -Path $helperFile -Value @"" 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public static class ApiResponseHelper
{{
    public static ActionResult GenerateApiResponse<T>(
        ControllerBase controller,
        ErrorType errorType,
        ApiResult<T>? result,
        int? newID = null,
        string? routeName = null,
        string? routeParamName = ""id"")
    {{
        T? safeData = result != null ? result.Data : default;

        var response = new
        {{
            data = safeData,
            message = result?.Message ?? GetDefaultMessage(errorType),
            errorCode = (int)errorType
        }};

         return errorType switch
        {{
            ErrorType.None =>
                (routeName != null && newID.HasValue)
                    ? controller.CreatedAtRoute(
                        routeName,
                        new RouteValueDictionary {{ {{ routeParamName!, newID.Value }} }},
                        response
                    )
                    : controller.Ok(response),

            ErrorType.InvalidId =>
                controller.BadRequest(response),

            ErrorType.NotFound =>
                controller.NotFound(response),

            ErrorType.AlreadyExists =>
                controller.Conflict(response),
            ErrorType.LogicalDependency =>
controller.Conflict(response),

            ErrorType.DatabaseError =>
                controller.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    response),

            _ =>
                controller.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new {{ data = default(T), message = ""Unexpected error occurred"", errorCode = 500 }})
        }};
    }}

    private static string GetDefaultMessage(ErrorType errorType) => errorType switch
    {{
        ErrorType.None => ""Operation successful"",
        ErrorType.InvalidId => ""Invalid ID provided"",
        ErrorType.NotFound => ""Resource not found"",
        ErrorType.AlreadyExists => ""Resource already exists"",
        ErrorType.DatabaseError => ""A database error occurred"",
        _ => ""Unexpected error occurred""
    }};
}}
""@
}}

# إضافة الملف إلى .csproj
[xml]$projXml = Get-Content $csprojFile
$itemGroup = $projXml.Project.ItemGroup | Where-Object {{ $_.Compile }}

if (-not $itemGroup) {{
    $itemGroup = $projXml.CreateElement(""ItemGroup"")
    $projXml.Project.AppendChild($itemGroup) | Out-Null
}}

if (-not ($projXml.Project.ItemGroup.Compile | Where-Object {{ $_.Include -eq ""ApiResponseHelper.cs"" }})) {{
    $node = $projXml.CreateElement(""Compile"")
    $node.SetAttribute(""Include"", ""ApiResponseHelper.cs"")
    $itemGroup.AppendChild($node) | Out-Null
}}

$projXml.Save($csprojFile)
";
    }

    public  string CreateConnectionStringClass()
    {

        return $@"
# =====================================
# مسار المشروع الرئيسي
# =====================================
$projectFolder = ""{projectFolder}""
$csprojFile = Join-Path $projectFolder ""{csprojFileName}""

# إنشاء الملف ConnectionString.cs مباشرة تحت المشروع
$filePath = Join-Path $projectFolder ""ConnectionString.cs""

if (-Not (Test-Path $filePath)) {{
    Set-Content -Path $filePath -Value @"" 
namespace ConnectionString
{{
    public class connectionString
    {{
       public static string _connectionString = 
       ""Server=(localdb)\\MSSQLLocalDB;Database={DataBaseName};Trusted_Connection=True;"";
    }}
}}
""@
}}

# إضافة الملف إلى .csproj
[xml]$projXml = Get-Content $csprojFile
$itemGroup = $projXml.Project.ItemGroup | Where-Object {{ $_.Compile }}

if (-not $itemGroup) {{
    $itemGroup = $projXml.CreateElement(""ItemGroup"")
    $projXml.Project.AppendChild($itemGroup) | Out-Null
}}

if (-not ($projXml.Project.ItemGroup.Compile | Where-Object {{ $_.Include -eq ""ConnectionString.cs"" }})) {{
    $node = $projXml.CreateElement(""Compile"")
    $node.SetAttribute(""Include"", ""ConnectionString.cs"")
    $itemGroup.AppendChild($node) | Out-Null
}}

$projXml.Save($csprojFile)
";
    }

    public string CleanCsprojScript()
    {
        return $@"# ============================================
# تنظيف csproj من Compile المكررة بأمان
# ============================================

$projectFolder = ""{projectFolder}""
$csprojFile = Join-Path $projectFolder ""{csprojFileName}""

if (-not (Test-Path $csprojFile)) {{
    Write-Host ""❌ csproj file not found!""
    exit
}}

[xml]$projXml = Get-Content $csprojFile

# حذف جميع Compile nodes
$compileNodes = $projXml.SelectNodes(""//Compile"")
foreach ($node in $compileNodes) {{
    $node.ParentNode.RemoveChild($node) | Out-Null
}}

# حذف EnableDefaultCompileItems إذا وجد
$enableNode = $projXml.SelectSingleNode(""//EnableDefaultCompileItems"")
if ($enableNode -ne $null) {{
    $enableNode.ParentNode.RemoveChild($enableNode) | Out-Null
}}

# حفظ التغييرات
$projXml.Save($csprojFile)

Write-Host ""✅ All duplicate Compile items removed.""
Write-Host ""✅ EnableDefaultCompileItems removed (if existed).""
Write-Host ""🎯 Project is now back to standard .NET behavior.""
";
    }

    public string GenerateCreateSqlGetAll()
    {
        return $@"# ============================================
# Create Stored Procedure SP_GetAllRole
# ============================================

$serverName   = ""(localdb)\MSSQLLocalDB""
$databaseName = ""{DataBaseName}""

$connectionString = ""Server=$serverName;Database=$databaseName;Integrated Security=True;""

try {{
    $connection = New-Object System.Data.SqlClient.SqlConnection $connectionString
    $connection.Open()

    # 2️⃣ إنشاء الإجراء (لازم يكون أول سطر)
    $createQuery = ""
{SqlGetAll}
""
    $command = $connection.CreateCommand()
    $command.CommandText = $createQuery
    $command.ExecuteNonQuery() | Out-Null

    Write-Host ""✅ Stored Procedure created successfully.""
}}
catch {{
    Write-Host ""❌ Error: $_""
}}
finally {{
    if ($connection.State -eq 'Open') {{
        $connection.Close()
    }}
}}";
    }

    public string GenerateCreateSqlGetOne()
    {
        return $@"# ============================================
# Create Stored Procedure SP_GetAllRole
# ============================================

$serverName   = ""(localdb)\MSSQLLocalDB""
$databaseName = ""{DataBaseName}""

$connectionString = ""Server=$serverName;Database=$databaseName;Integrated Security=True;""

try {{
    $connection = New-Object System.Data.SqlClient.SqlConnection $connectionString
    $connection.Open()

    # 2️⃣ إنشاء الإجراء (لازم يكون أول سطر)
    $createQuery = ""
{SqlGetOne}
""
    $command = $connection.CreateCommand()
    $command.CommandText = $createQuery
    $command.ExecuteNonQuery() | Out-Null

    Write-Host ""✅ Stored Procedure created successfully.""
}}
catch {{
    Write-Host ""❌ Error: $_""
}}
finally {{
    if ($connection.State -eq 'Open') {{
        $connection.Close()
    }}
}}";
    }

    public string GenerateCreateSqlAdd()
    {
        return $@"# ============================================
# Create Stored Procedure SP_GetAllRole
# ============================================

$serverName   = ""(localdb)\MSSQLLocalDB""
$databaseName = ""{DataBaseName}""

$connectionString = ""Server=$serverName;Database=$databaseName;Integrated Security=True;""

try {{
    $connection = New-Object System.Data.SqlClient.SqlConnection $connectionString
    $connection.Open()

    # 2️⃣ إنشاء الإجراء (لازم يكون أول سطر)
    $createQuery = ""
{SqlAdd}
""
    $command = $connection.CreateCommand()
    $command.CommandText = $createQuery
    $command.ExecuteNonQuery() | Out-Null

    Write-Host ""✅ Stored Procedure created successfully.""
}}
catch {{
    Write-Host ""❌ Error: $_""
}}
finally {{
    if ($connection.State -eq 'Open') {{
        $connection.Close()
    }}
}}";
    }

    public string GenerateCreateSqlUpdate()
    {
        return $@"# ============================================
# Create Stored Procedure SP_GetAllRole
# ============================================

$serverName   = ""(localdb)\MSSQLLocalDB""
$databaseName = ""{DataBaseName}""

$connectionString = ""Server=$serverName;Database=$databaseName;Integrated Security=True;""

try {{
    $connection = New-Object System.Data.SqlClient.SqlConnection $connectionString
    $connection.Open()

    # 2️⃣ إنشاء الإجراء (لازم يكون أول سطر)
    $createQuery = ""
{SqlUpdate}
""
    $command = $connection.CreateCommand()
    $command.CommandText = $createQuery
    $command.ExecuteNonQuery() | Out-Null

    Write-Host ""✅ Stored Procedure created successfully.""
}}
catch {{
    Write-Host ""❌ Error: $_""
}}
finally {{
    if ($connection.State -eq 'Open') {{
        $connection.Close()
    }}
}}";
    }

    public string GenerateCreateSqlDelete()
    {
        return $@"# ============================================
# Create Stored Procedure SP_GetAllRole
# ============================================

$serverName   = ""(localdb)\MSSQLLocalDB""
$databaseName = ""{DataBaseName}""

$connectionString = ""Server=$serverName;Database=$databaseName;Integrated Security=True;""

try {{
    $connection = New-Object System.Data.SqlClient.SqlConnection $connectionString
    $connection.Open()

    # 2️⃣ إنشاء الإجراء (لازم يكون أول سطر)
    $createQuery = ""
{SqlDelete}
""
    $command = $connection.CreateCommand()
    $command.CommandText = $createQuery
    $command.ExecuteNonQuery() | Out-Null

    Write-Host ""✅ Stored Procedure created successfully.""
}}
catch {{
    Write-Host ""❌ Error: $_""
}}
finally {{
    if ($connection.State -eq 'Open') {{
        $connection.Close()
    }}
}}";
    }
    public string GenerateCreateSqlSearch()
    {
        return $@"# ============================================
# Create Stored Procedure SP_GetAllRole
# ============================================

$serverName   = ""(localdb)\MSSQLLocalDB""
$databaseName = ""{DataBaseName}""

$connectionString = ""Server=$serverName;Database=$databaseName;Integrated Security=True;""

try {{
    $connection = New-Object System.Data.SqlClient.SqlConnection $connectionString
    $connection.Open()

    # 2️⃣ إنشاء الإجراء (لازم يكون أول سطر)
    $createQuery = ""
{SqlSearch}
""
    $command = $connection.CreateCommand()
    $command.CommandText = $createQuery
    $command.ExecuteNonQuery() | Out-Null

    Write-Host ""✅ Stored Procedure created successfully.""
}}
catch {{
    Write-Host ""❌ Error: $_""
}}
finally {{
    if ($connection.State -eq 'Open') {{
        $connection.Close()
    }}
}}";
    }


}

