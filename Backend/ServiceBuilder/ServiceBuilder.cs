using System.Text;
using System.Xml.Serialization;

public class ServiceBuilder
{
    public string TableName { get; set; }

    public List<ParameterInfoDTO> parameterInfoDTO { get; set; } = new List<ParameterInfoDTO>();
    public List<ParameterInfoDTOView> parameterInfoDTOView { get; set; } = new List<ParameterInfoDTOView>();
    public CodeGenerationPayload codeGenerationPayload { get; set; }

    public string GenerateServiceClass()
    {
        var sb = new StringBuilder();

        sb.AppendLine(@"
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class " + TableName + @"Service
{");

        if (codeGenerationPayload.GetAll == true  || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
        {
            sb.AppendLine(BuildGetAllService());
        }

        if (codeGenerationPayload.AddNew == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
        {
            sb.AppendLine(BuildAddService());

        }
        if (codeGenerationPayload.UpdateBy == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
        {
            sb.AppendLine(BuildUpdateService());

        }
        if (codeGenerationPayload.DeleteBy == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
        {
            sb.AppendLine(BuildDeleteService());

        }
        if (codeGenerationPayload.Search == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)

        {
            sb.AppendLine(BuildSearchService());
        }
        if (codeGenerationPayload.GetOneBy == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
        {
            sb.AppendLine(BuildGetOneService());
        }





 



            sb.AppendLine("}");

        return sb.ToString();
    }

    // =============================
    // GET ALL
    // =============================
    public string BuildGetAllService()
    {
        return $@"
public static async Task<ApiResult<List<{TableName}ViewDTO>>> GetAll{TableName}Async(
    CancellationToken cancellationToken = default)
{{
    var result = await {TableName}Data.GetAll{TableName}Async(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {{
        return new ApiResult<List<{TableName}ViewDTO>>(
            null,
            ""No {TableName}s found."",
            ErrorType.NotFound
        );
    }}

    return new ApiResult<List<{TableName}ViewDTO>>(
        result.Data,
        ""{TableName}s retrieved successfully."",
        ErrorType.None
    );
}}
";
    }

    // =============================           
    // GET ALL BY (Name / NameAndAge / ...)
    // =============================
    public string BuildSearchService()
    {
        var parameters = parameterInfoDTOView
            .Where(p =>
                p.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.GetAll ||
                p.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.Both)
            .ToList();

        if (!parameters.Any())
            return string.Empty;

        string suffix = string.Join("And", parameters.Select(p  =>   p.Name));
        string methodParams = string.Join(", ",
            parameters.Select(p =>  $"{p.DataType}? {  p.Name}"));
        string dataParams = string.Join(", ", parameters.Select(p =>  p.Name));

        return $@"
public static async Task<ApiResult<List<{TableName}ViewDTO>>> Search{TableName}(
    {methodParams},
    CancellationToken cancellationToken = default)
{{
    var result = await {TableName}Data.Search{TableName}(
        {dataParams},
        cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {{
        return new ApiResult<List<{TableName}ViewDTO>>(
            null,
            ""No {TableName}s found."",
            ErrorType.NotFound
        );
    }}

    return new ApiResult<List<{TableName}ViewDTO>>(
        result.Data,
        ""{TableName}s retrieved successfully."",
        ErrorType.None
    );
}}
";
    }

    // =============================
    // GET ONE
    // =============================

    public string BuildGetOneService()
    {
        var key = parameterInfoDTOView.Where(p => p.UseInWhere &&((p.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.GetOne) ||  ( p.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.Both)));

        string suffix = string.Join("And", key.Select(p =>  p.Name));
        if (key == null)
            return string.Empty;

        string methodParams = string.Join(", ",
         key.Select(p =>  $"{p.DataType} {p.Name}"));
        string dataParams = string.Join(", ", key.Select(p => p.Name));

        return $@"
public static async Task<ApiResult<{TableName}ViewDTO>> Get{TableName}By{suffix}Async(
    {methodParams},
    CancellationToken cancellationToken = default)
{{
  
    return await {TableName}Data.Get{TableName}By{suffix}Async(
        {dataParams},
        cancellationToken);
}}
";
    }

    // =============================
    // ADD
    // =============================
    public string BuildAddService()
    {
        return $@"
public static async Task<ApiResult<{TableName}DTO>> Add{TableName}Async(
    {TableName}DTO dto,
    CancellationToken cancellationToken = default)
{{
    if (dto == null)
    {{
        return new ApiResult<{TableName}DTO>
        {{
            Data = null,
            Message = ""{TableName} cannot be null."",
            ErrorType = ErrorType.InvalidId
        }};
    }}

    return await {TableName}Data.Add{TableName}Async(dto, cancellationToken);
}}
";
    }

    // =============================
    // UPDATE
    // =============================
    public string BuildUpdateService()
    {
        var parameters = parameterInfoDTO.Where(p => (( p.UseInWhere&&(p.UsedInGet==ParameterInfoDTO.GetUsageType.Update ||p.UsedInGet==ParameterInfoDTO.GetUsageType.Both) )));
        string suffix = string.Join("And", parameters.Select(p =>   p.Name));
        string methodParams = string.Join(", ",
            parameters.Select(p =>  $"{p.DataType} {p.Name}"));
       


        return $@"
public static async Task<ApiResult<{TableName}DTO>> Update{TableName}By{suffix}Async(
    {TableName}UpdateDTO dto,
    CancellationToken cancellationToken = default)
{{
    if (dto == null )
    {{
        return new ApiResult<{TableName}DTO>
        {{
            Data = null,
            Message = ""Invalid {TableName} data."",
            ErrorType = ErrorType.InvalidId
        }};
    }}

    return await {TableName}Data.Update{TableName}By{suffix}Async(dto, cancellationToken);
}}
";
    }

    // =============================
    // DELETE
    // =============================
    public string BuildDeleteService()
    {
        var parameters = parameterInfoDTO.Where(p => ((p.UseInWhere && (p.UsedInGet == ParameterInfoDTO.GetUsageType.Delete || p.UsedInGet == ParameterInfoDTO.GetUsageType.Both))));
        string suffix = string.Join("And", parameters.Select(p => p.Name));
        string methodParams = string.Join(", ",
            parameters.Select(p => $"{p.DataType} {p.Name}"));
        string dataParams = string.Join(", ", parameters.Select(p=> p.Name));

        return $@"
public static async Task<ApiResult<{TableName}DTO>> Delete{TableName}By{suffix}Async(
    {methodParams},
    CancellationToken cancellationToken = default)
{{
   

    return await {TableName}Data.Delete{TableName}By{suffix}Async(
        {dataParams},
        cancellationToken);
}}
";
    }
}