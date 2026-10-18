using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using System.Threading;
using System.Threading.Tasks;
using static ParameterInfoDTO;


    public class ControllerBuilder
    {

        public string TableName { get; set; } // Room, User, Product...

        // قوائم البيانات
        public List<ParameterInfoDTO> parameterInfoDTO { get; set; } = new List<ParameterInfoDTO>();
        public List<ParameterInfoDTOView> parameterInfoDTOView { get; set; } = new List<ParameterInfoDTOView>();
        public CodeGenerationPayload codeGenerationPayload { get; set; }




    public string GenerateControllerClass()
    {
        var sb = new StringBuilder();

        sb.AppendLine(@"
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;


[Route(""api/" + TableName.ToLower() + @"s"")]
[ApiController]
public class " + TableName + @"Controller : ControllerBase
{");



        if (codeGenerationPayload.GetAll == true || codeGenerationPayload.GAUDExtend == true|| codeGenerationPayload.GAUDInit==true)
        {
            sb.AppendLine(GenerateGetAll());
        }

        if (codeGenerationPayload.AddNew == true|| codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
        {
            sb.AppendLine(GenerateAdd());

        }
        if (codeGenerationPayload.UpdateBy == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
        {
            sb.AppendLine(GenerateUpdate());

        }
        if (codeGenerationPayload.DeleteBy == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
        {
            sb.AppendLine(GenerateDelete());

        }
        if (codeGenerationPayload.Search == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)

        {
            sb.AppendLine(GenerateSearch());
        }
        if (codeGenerationPayload.GetOneBy == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
        {
            sb.AppendLine(GenerateGetOneBy());
        }




        sb.AppendLine("}");

            return sb.ToString();
        }



        // ================================
        // Helpers
        // ================================
    
        private string BuildMethodParameters(List<ParameterInfoDTOView> parameters)
        {
            return string.Join(", ",
                parameters.Select(p =>    $"{p.DataType} {(p.Name).ToLower()}"));
        }

    // ================================
    // GET ALL BY (any number of columns)
    // ================================




    public string GenerateSearch()
    {
        // اختيار الأعمدة المستخدمة في GetAll أو Both والتي UseInWhere = true
        var parameters = parameterInfoDTOView
            .Where(p => (p.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.GetAll ||
                         p.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.Both)
                        && p.UseInWhere)
            .ToList();
      
        if (!parameters.Any())
            return string.Empty;

        // Method Name: GetRoomByNameAndStatus...
        var methodName = $"Search{TableName}";
         

        // Route: by-name/{name}/status/{status}...
        var route = string.Join("/",
            parameters.Select(p => $"{(p.Name.ToLower())}/{{{(p.Name.ToLower())}}}"));

        // Method parameters: string name, int status...
        var methodParams = BuildMethodParameters(parameters);

        // Service parameters: name, status...
        var serviceParams = string.Join(", ", parameters.Select(p => ("request."+p.Name)));

        string httpTypes =
    @"    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]";

        return httpTypes + $@" 
[HttpGet(""Search{TableName}"")]
public async Task<ActionResult> {methodName}(
   [FromQuery]   Search{TableName}Request   request ,
    CancellationToken cancellationToken)
{{
    var result = await {TableName}Service.Search{TableName}(
        {serviceParams},
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}}";
    }


    public string GenerateGetAll()
        {
            // اسم الطريقة: GetAllRooms, GetAllUsers ...
            var methodName = $"GetAll{TableName}";

            // مسار الـ Route: simply "/"
            var route = "all"; // يمكنك وضع "all" إذا أحببت، مثلاً "all-rooms"
        string HttpType = "  [ProducesResponseType(StatusCodes.Status200OK)]\r\n    [ProducesResponseType(StatusCodes.Status404NotFound)]\r\n    [ProducesResponseType(StatusCodes.Status500InternalServerError)]";


            return HttpType+$@"
[HttpGet(""{route}"", Name = ""{methodName}"")]
public async Task<ActionResult> {methodName}(
    CancellationToken cancellationToken)
{{
    var result = await {TableName}Service.{methodName}Async(cancellationToken);
    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}}";
        }
        // ================================
        // GET ONE BY (primary key or unique combination)
        // ================================
        public string GenerateGetOneBy()
        {
            // اختيار كل الأعمدة التي تستخدم للـ GetOne أو Both، والتي UseInWhere = true
            var keyParameters = parameterInfoDTOView
                .Where(p => (p.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.GetOne ||
                             p.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.Both)
                             && p.UseInWhere)
                .ToList();

            if (!keyParameters.Any())
                return string.Empty;

        // اسم الطريقة: GetRoomByNameAndAgeAndClean...
        var methodName = $"Get{TableName}By" + string.Join("And", keyParameters.Select(p => p.Name));

        // Route: name/{name}/age/{age}/clean/{clean}...
        var route = string.Join("/",
           keyParameters.Select(p =>
           {
               var propName =  p.Name;
               return $"{{{propName.ToLower()}}}"; // هنا حولناها لأقواس {}
           })
           

);

        // Method parameters: string name, int age, bool clean...
        var methodParams = BuildMethodParameters(keyParameters);

            // Service parameters: name, age, clean
            var serviceParams = string.Join(", ", keyParameters.Select(p =>  (p.Name).ToLower()));
        string HttpType = " [ProducesResponseType(StatusCodes.Status200OK)]\r\n    [ProducesResponseType(StatusCodes.Status400BadRequest)]\r\n    [ProducesResponseType(StatusCodes.Status404NotFound)]\r\n    [ProducesResponseType(StatusCodes.Status500InternalServerError)]";

            return HttpType+$@"
[HttpGet(""{route}"", Name = ""{methodName}"")]
public async Task<ActionResult> {methodName}(
    {methodParams},
    CancellationToken cancellationToken)
{{
    var result = await {TableName}Service.{methodName}Async(
        {serviceParams},
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}}";
        }


    // ================================
    // ADD
    // ================================
    public string GenerateAdd()
    {
        string PM = "";
        var KeyParameter = parameterInfoDTOView.Where(p => p.UseInWhere && (p.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.GetOne || p.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.Both));
        var route = string.Join("/", KeyParameter.Select(p => (p.Name).ToLower()));
        var methodGetOne= string.Join("And", KeyParameter.Select(p => (p.Name)));

        for (int i=0; i<parameterInfoDTO.Count; i++)
        {
            if ( parameterInfoDTO[i].IsPrimaryKey == true)
            {
                PM = parameterInfoDTO[i].Name;
            }
        }
       
       
        
        string httpTypes = @"       [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]";

        var methodName = $"Add{TableName}Async";

        return httpTypes + $@"
[HttpPost(Name = ""Add{TableName}"")]
public async Task<ActionResult> Add{TableName}(
    [FromBody] {TableName}DTO dto,
    CancellationToken cancellationToken)
{{
    var result = await {TableName}Service.{methodName}(dto, cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(
        this,
        result.ErrorType,
        result,
        newID: result.Data?.{PM},
        routeName: nameof(Get{TableName}By{methodGetOne}),routeParamName:""{route}"" );
}}";
    }


    // ================================
    // UPDATE
    // ================================
    public string GenerateUpdate()
    {
        // الأعمدة المستخدمة لتحديد السجل (WHERE) ديناميكي
        var whereParameters = parameterInfoDTO
            .Where(p => (p.UsedInGet == GetUsageType.Update || p.UsedInGet == GetUsageType.Both)
                        && p.UseInWhere)
            .ToList();

        // اسم الميثود: UpdateRoomByIdAndNameAndAge
        var methodName = whereParameters.Any()
      ? $"Update{TableName}By" + string.Join("And", whereParameters.Select(p => p.Name)) 
      : $"Update{TableName}";


        // Route segments: id/{id}/name/{name}/age/{age}
        var route = whereParameters.Any()
        ? string.Join("/", whereParameters.Select(p => $"{{{p.Name.ToLower()}}}"))
        : "{id}";
        string SS = "";

        for (int i = 0; i < whereParameters.Count(); i++)
        {
            SS += $"dto.{whereParameters[i].Name}={whereParameters[i].Name.ToLower()};";

           
        }

        // Method parameters: int id, string name, bool isActive...
        var methodParams = string.Join(", ",
            whereParameters.Select(p => $"{p.DataType} {p.Name.ToLower()}"));

        if (methodParams.Length > 0)
            methodParams = methodParams + ", "; // إضافة فاصلة قبل dto

        // Service parameters: id, name, isActive...
        var serviceParams = string.Join(", ", whereParameters.Select(p => (p.Name.ToLower())));


        string httpTypes =
    @"    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]";

        return httpTypes + $@"
[HttpPut(""{route}"", Name = ""{methodName}"")]
public async Task<ActionResult> {methodName}(
    {methodParams}[FromBody] {TableName}UpdateDTO dto,
    CancellationToken cancellationToken)  
{{  
      {SS}

    var result = await {TableName}Service.{methodName}Async(
      dto,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}}";
    }


    // ================================
    // DELETE
    // ================================
    public string GenerateDelete()
    {
        // الأعمدة المستخدمة لتحديد السجل (WHERE)
        var whereParameters = parameterInfoDTO
            .Where(p => (p.UsedInGet == GetUsageType.Delete || p.UsedInGet == GetUsageType.Both)
                        && p.UseInWhere)
            .ToList();

        // اسم الميثود
        var methodName = whereParameters.Any()
        ? $"Delete{TableName}By" + string.Join("And", whereParameters.Select(p => p.Name)) 
        : $"Delete{TableName}";

        // Route segments: id/{id}/name/{name}/age/{age}
        var route = whereParameters.Any()
          ? string.Join("/", whereParameters.Select(p => $"{{{p.Name.ToLower()}}}"))
          : "{id}";


        // Method parameters: int id, string name, bool isActive...
        var methodParams = string.Join(", ",
            whereParameters.Select(p => $"{p.DataType} {(p.Name.ToLower())}"));

        if (methodParams.Length > 0)
            methodParams = methodParams + ", "; // إضافة فاصلة قبل CancellationToken

        string httpTypes =
    @"    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]";

        return httpTypes + $@"
[HttpDelete(""{route}"", Name = ""{methodName}"")]
public async Task<ActionResult> {methodName}(
    {methodParams}CancellationToken cancellationToken)
{{

    var result = await {TableName}Service.{methodName}Async(
        {string.Join(", ", whereParameters.Select(p => (p.Name.ToLower())))}
        {(whereParameters.Any() ? ", " : "")}cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}}";

    }
}