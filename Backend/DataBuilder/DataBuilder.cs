using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public class DataBuilder
{
    public List<ParameterInfoDTOView> parameterInfoDTOView = new List<ParameterInfoDTOView>();
    public List<ParameterInfoDTO> parameterInfoDTO = new List<ParameterInfoDTO>();
    public string TableName { get; set; }
    public string ConnectionString { get; set; }
    public CodeGenerationPayload codeGenerationPayload { get; set; }


    public string GenerateDataClass()
    {
        var sb = new StringBuilder();

        sb.AppendLine($@"
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using ConnectionString;

public static class {TableName}Data
{{");

        if (codeGenerationPayload.GetAll == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
        {
            sb.AppendLine(BuildGetAllAsyncMethod());
        }

        if (codeGenerationPayload.AddNew == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
        {
            sb.AppendLine(GenerateAddMethod());

        }
        if (codeGenerationPayload.UpdateBy == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
        {
            sb.AppendLine(BuildUpdateMethod());

        }
        if ( codeGenerationPayload.DeleteBy == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
        {
            sb.AppendLine(BuildDeleteMethod());

        }
        if (codeGenerationPayload.Search == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)

        {
            sb.AppendLine(BuildSearchMethod());
        }
        if (codeGenerationPayload.GetOneBy == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
        {
            sb.AppendLine(GenerateGetMethod());
        }





            sb.AppendLine("}");

        return sb.ToString() ;
    }

    // Search      ---------------- \\ 
    public string BuildSearchMethod()
    {
        var sb = new StringBuilder();
        var whereColumns = parameterInfoDTOView.Where(p => p.UseInWhere && (p.UsedInGet==ParameterInfoDTOView.GetUsageTypeView.GetAll|| p.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.Both) ).Select(p =>  p.Name).ToList();
        var methodName = $"Search{TableName}";
        //whereColumns.Any()
        //? $"GetAll{TableName}By{string.Join("And", whereColumns)}Async"
        //: $"GetAll{TableName}Async";

        sb.AppendLine($"public static async Task<ApiResult<List<{TableName}ViewDTO>>> {methodName}(");
       foreach( var p in parameterInfoDTOView.Where(p => p.UseInWhere && (p.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.GetAll || p.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.Both)))
        {

            sb.AppendLine(p.DataType + "? " + p.Name + ",");
            
        }
      


        sb.AppendLine("    CancellationToken cancellationToken = default)");
        sb.AppendLine("{");
        sb.AppendLine($"    var result = new ApiResult<List<{TableName}ViewDTO>>();");
        sb.AppendLine($"    var list = new List<{TableName}ViewDTO>();");
        sb.AppendLine("    try");
        sb.AppendLine("    {");
        sb.AppendLine($"        await using var connection = new SqlConnection(connectionString._connectionString);");
        sb.AppendLine($"        await using var command = new SqlCommand(\"SP_Search{TableName}" +

                      "\", connection) { CommandType = CommandType.StoredProcedure };");
        foreach (var col in whereColumns)
        {
            var colInfo = parameterInfoDTOView.First(p => p.Name == col);
            var paramVar = colInfo.Name;

            sb.AppendLine(
                $"        command.Parameters.Add(\"@{paramVar}\", SqlDbType.{MapToSqlDbType(colInfo.DataType)}).Value = " +
                $"{paramVar} ;"
            );
        }

        sb.AppendLine("        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);");
        sb.AppendLine("        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);");
        sb.AppendLine("        while (reader.Read())");
        sb.AppendLine("        {");
        sb.AppendLine($"            list.Add(new {TableName}ViewDTO");
        sb.AppendLine("            {");

        for (int i = 0; i < parameterInfoDTOView.Count; i++)
        {
            var p = parameterInfoDTOView[i];
            var propName = p.Name;
            var comma = i < parameterInfoDTOView.Count - 1 ? "," : "";
            sb.AppendLine($"                {propName} = {MapReaderGetter(p.DataType, propName)}{comma}");
        }

        sb.AppendLine("            });");
        sb.AppendLine("        }");

        sb.AppendLine("        result.Data = list;");
        sb.AppendLine("    }");
        sb.AppendLine("    catch (Exception ex)");
        sb.AppendLine("    {");
        sb.AppendLine("        result.Data = null;");
        sb.AppendLine("        result.Message = \"Database error occurred while fetching data.\";");
        sb.AppendLine("        result.ErrorType = ErrorType.DatabaseError;");
        sb.AppendLine("    }");
        sb.AppendLine("    return result;");
        sb.AppendLine("}");
        return sb.ToString();

    }




    // ================= GET ALL =================


    public string BuildGetAllAsyncMethod()
    {
        var sb = new StringBuilder();
        var whereColumns = parameterInfoDTOView.Select(p => p.Name).ToList();//   Where(p => p.UseInWhere && (p.UsedInGet==ParameterInfoDTOView.GetUsageTypeView.GetAll|| p.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.Both) ).Select(p =>  p.Name).ToList();
        var methodName = $"GetAll{TableName}Async";
            //whereColumns.Any()
            //? $"GetAll{TableName}By{string.Join("And", whereColumns)}Async"
            //: $"GetAll{TableName}Async";

        sb.AppendLine($"public static async Task<ApiResult<List<{TableName}ViewDTO>>> {methodName}(");

        //for (int i = 0; i <parameterInfoDTOView.Count; i++)
        //    {
            
        //        var col = parameterInfoDTOView[i];
        //        if (!col.UseInWhere) continue;
        //        var paramName = col.Name;
        //        var comma = i < parameterInfoDTOView.Count - 1 ? "," : "";
        //        sb.AppendLine($"    {col.DataType} {paramName}{comma}");

            

                
        //    }
        

        sb.AppendLine("    CancellationToken cancellationToken = default)");
        sb.AppendLine("{");
        sb.AppendLine($"    var result = new ApiResult<List<{TableName}ViewDTO>>();");
        sb.AppendLine($"    var list = new List<{TableName}ViewDTO>();");
        sb.AppendLine("    try");
        sb.AppendLine("    {");
        sb.AppendLine($"        await using var connection = new SqlConnection(connectionString._connectionString);");
        sb.AppendLine($"        await using var command = new SqlCommand(\"SP_GetAll{TableName}" +
                       
                      "\", connection) { CommandType = CommandType.StoredProcedure };");
        //foreach (var col in whereColumns)
        //{
        //    var colInfo = parameterInfoDTOView.First(p => p.Name == col);
        //    var paramVar =  colInfo.Name;

        //    sb.AppendLine(
        //        $"        command.Parameters.Add(\"@{paramVar}\", SqlDbType.{MapToSqlDbType(colInfo.DataType)}).Value = " +
        //        $"{paramVar} ;"
        //    );
        //}

        sb.AppendLine("        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);");
        sb.AppendLine("        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);");
        sb.AppendLine("        while (reader.Read())");
        sb.AppendLine("        {");
        sb.AppendLine($"            list.Add(new {TableName}ViewDTO");
        sb.AppendLine("            {");

        for (int i = 0; i < parameterInfoDTOView.Count; i++)
        {
            var p = parameterInfoDTOView[i];
            var propName =  p.Name;
            var comma = i < parameterInfoDTOView.Count - 1 ? "," : "";
            sb.AppendLine($"                {propName} = {MapReaderGetter(p.DataType, propName)}{comma}");
        }

        sb.AppendLine("            });");
        sb.AppendLine("        }");

        sb.AppendLine("        result.Data = list;");
        sb.AppendLine("    }");
        sb.AppendLine("    catch (Exception ex)");
        sb.AppendLine("    {");
        sb.AppendLine("        result.Data = null;");
        sb.AppendLine("        result.Message = \"Database error occurred while fetching data.\";");
        sb.AppendLine("        result.ErrorType = ErrorType.DatabaseError;");
        sb.AppendLine("    }");
        sb.AppendLine("    return result;");
        sb.AppendLine("}");
        return sb.ToString();

    }

    // ================= GET =================
    public string GenerateGetMethod()
    {
        var whereColumns = parameterInfoDTOView.Where(p => p.UseInWhere&&( p.UsedInGet==ParameterInfoDTOView.GetUsageTypeView.GetOne|| p.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.Both) ).ToList();
        bool hasWhere = whereColumns.Any();
        string methodName = hasWhere
            ? $"Get{TableName}By{string.Join("And", whereColumns.Select(p =>  p.Name))}Async"
            : $"GetAll{TableName}Async";

        var sb = new StringBuilder();
        sb.AppendLine($"public static async Task<ApiResult<{TableName}ViewDTO>> {methodName}(");

        for (int i = 0; i < whereColumns.Count; i++)
        {
            var p = whereColumns[i];
            var paramName =  p.Name;
            var comma = i < whereColumns.Count - 1 ? "," : ",";
            sb.AppendLine($"    {p.DataType} {paramName}{comma}");
        }

        sb.AppendLine("    CancellationToken cancellationToken = default)");
        sb.AppendLine("{");
        sb.AppendLine($"    var result = new ApiResult<{TableName}ViewDTO>();");
        sb.AppendLine($"    var dto = new {TableName}ViewDTO();");
        sb.AppendLine("    try");
        sb.AppendLine("    {");
        sb.AppendLine("        await using var connection = new SqlConnection(connectionString._connectionString);");
        sb.AppendLine($"        await using var command = new SqlCommand(\"SP_Get{TableName}" +
                      (hasWhere ? "By" + string.Join("And", whereColumns.Select(p => p.Name)) : "") +
                      "\", connection) { CommandType = CommandType.StoredProcedure };");
        foreach (var p in whereColumns)
        {
            var paramName = p.Name;

            sb.AppendLine(
                $"        command.Parameters.Add(\"@{paramName}\", SqlDbType.{MapToSqlDbType(p.DataType)}).Value = {paramName};"
            );
        }

        sb.AppendLine("        var messageParameter = new SqlParameter(\"@Message\", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };");
        sb.AppendLine("        var errorTypeParameter = new SqlParameter(\"@ErrorType\", SqlDbType.Int) { Direction = ParameterDirection.Output };");
        sb.AppendLine("        command.Parameters.Add(messageParameter);");
        sb.AppendLine("        command.Parameters.Add(errorTypeParameter);");
        sb.AppendLine("        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);");
        sb.AppendLine("        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);");
        sb.AppendLine("        if (reader.Read())");
        sb.AppendLine("        {");

        for (int i = 0; i < parameterInfoDTOView.Count; i++)
        {
            var p = parameterInfoDTOView[i];
            var propName = p.Name;
            var comma = i < parameterInfoDTOView.Count - 1 ? ";" : ";";
            sb.AppendLine($"            dto.{propName} = {MapReaderGetter(p.DataType, propName)}{comma}");
        }

        sb.AppendLine("        }");
        sb.AppendLine("        reader.Close();");
        sb.AppendLine("        result.Data = dto;");
        sb.AppendLine("        result.Message = messageParameter.Value?.ToString();");
        sb.AppendLine("        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(errorTypeParameter.Value));");
        sb.AppendLine("    }");
        sb.AppendLine("    catch (Exception ex)");
        sb.AppendLine("    {");
        sb.AppendLine("        result.Data = null;");
        sb.AppendLine($"        result.Message = \"Database error occurred while fetching the {TableName.ToLower()}\";");
        sb.AppendLine("        result.ErrorType = ErrorType.DatabaseError;");
        sb.AppendLine("    }");
        sb.AppendLine("    return result;");
        sb.AppendLine("}");
        return sb.ToString();
    }

    // ================= ADD =================
    public string GenerateAddMethod()
    {
        var sb = new StringBuilder();
        string dtoName = $"{TableName}DTO";
        string methodName = $"Add{TableName}Async";

        sb.AppendLine($"public static async Task<ApiResult<{dtoName}>> {methodName}(");
        sb.AppendLine($"    {dtoName} {TableName.ToLower()},");
        sb.AppendLine("    CancellationToken cancellationToken = default)");
        sb.AppendLine("{");
        sb.AppendLine($"    var result = new ApiResult<{dtoName}>();");
        sb.AppendLine("    try");
        sb.AppendLine("    {");
        sb.AppendLine($"        await using var connection = new SqlConnection(connectionString._connectionString);");
        sb.AppendLine($"        await using var command = new SqlCommand(\"SP_Add{TableName}\", connection) {{ CommandType = CommandType.StoredProcedure }};");

        foreach (var p in parameterInfoDTO.Where(p => !p.IsPrimaryKey))
        {
            string sqlType = MapToSqlDbType(p.DataType);
            string sizePart = p.DataType == "string" ? ", 50" : "";
            string valueExpr = p.DataType == "string"
                ? $"(object?){TableName.ToLower()}.{p.Name} ?? DBNull.Value"
                : $"{TableName.ToLower()}.{p.Name}";
            sb.AppendLine($"        command.Parameters.Add(\"@{p.Name}\", SqlDbType.{sqlType}{sizePart}).Value = {valueExpr};");
        }

        sb.AppendLine("        var outputID = new SqlParameter(\"@NewID\", SqlDbType.Int) { Direction = ParameterDirection.Output };");
        sb.AppendLine("        var outputMsg = new SqlParameter(\"@Message\", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };");
        sb.AppendLine("        var outputErrorType = new SqlParameter(\"@ErrorType\", SqlDbType.Int) { Direction = ParameterDirection.Output };");
        sb.AppendLine("        command.Parameters.Add(outputID);");
        sb.AppendLine("        command.Parameters.Add(outputMsg);");
        sb.AppendLine("        command.Parameters.Add(outputErrorType);");
        sb.AppendLine("        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);");
        sb.AppendLine("        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);");

        var pk = parameterInfoDTO.First(p => p.IsPrimaryKey);
        sb.AppendLine($"        {TableName.ToLower()}.{pk.Name} = (int)outputID.Value;");
        sb.AppendLine($"        result.Data = {TableName.ToLower()};");
        sb.AppendLine("        result.Message = outputMsg.Value?.ToString();");
        sb.AppendLine("        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));");
        sb.AppendLine("    }");
        sb.AppendLine("    catch (Exception ex)");
        sb.AppendLine("    {");
        sb.AppendLine("        result.Data = null;");
        sb.AppendLine($"        result.Message = \"Database error occurred while adding the {TableName.ToLower()}.\";");
        sb.AppendLine("        result.ErrorType = ErrorType.DatabaseError;");
        sb.AppendLine("    }");
        sb.AppendLine("    return result;");
        sb.AppendLine("}");
        return sb.ToString();
    }

    // ================= UPDATE =================
    public string BuildUpdateMethod()
    {
        var sb = new StringBuilder();
        var dtoName = TableName + "DTO";

        var updateParams = parameterInfoDTO
            .Where(p => (p.UseInWhere && (p.UsedInGet == ParameterInfoDTO.GetUsageType.Update|| p.UsedInGet == ParameterInfoDTO.GetUsageType.Both)) || p.UseColumnInUpdate)
            .ToList();

        var whereParams = parameterInfoDTO
            .Where(p => p.UseInWhere &&( p.UsedInGet == ParameterInfoDTO.GetUsageType.Update|| p.UsedInGet == ParameterInfoDTO.GetUsageType.Both))
            .ToList();


        var bySuffix = whereParams.Any()
    ? $"By{string.Join("And", whereParams.Select(p => p.Name))}"
    : "";

        var methodName = $"Update{TableName}{bySuffix}Async";
        var spName = $"SP_Update{TableName}{bySuffix}";
      

        sb.AppendLine($"public static async Task<ApiResult<{dtoName}>> {methodName}(");
        sb.AppendLine($"    {TableName}UpdateDTO {TableName.ToLower()},");
        sb.AppendLine("    CancellationToken cancellationToken = default)");
        sb.AppendLine("{");

        sb.AppendLine($"{TableName}DTO {TableName.ToLower()}DTO = new  {TableName}DTO();");
        sb.AppendLine($"    var result = new ApiResult<{dtoName}>();");
        sb.AppendLine("    try");
        sb.AppendLine("    {");
        sb.AppendLine($"        await using var connection = new SqlConnection(connectionString._connectionString);");
        sb.AppendLine(
         $"        await using var command = new SqlCommand(\"{spName}\", connection) " +
         "{ CommandType = CommandType.StoredProcedure };");

        foreach (var p in updateParams)
        { 
            var sqlType = MapToSqlDbType(p.DataType);
            var size = p.DataType == "string" ? ", 500" : "";
            var valueExpr = $"(object?){TableName.ToLower()}.{p.Name} ?? DBNull.Value";
            sb.AppendLine($"        command.Parameters.Add(\"@{p.Name}\", SqlDbType.{sqlType}{size}).Value = {valueExpr};");
        }


        sb.AppendLine("        var outputMsg = new SqlParameter(\"@Message\", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };");
        sb.AppendLine("        var outputErrorType = new SqlParameter(\"@ErrorType\", SqlDbType.Int) { Direction = ParameterDirection.Output };");
        sb.AppendLine("        command.Parameters.Add(outputMsg);");
        sb.AppendLine("        command.Parameters.Add(outputErrorType);");

        sb.AppendLine("        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);");
        sb.AppendLine("        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);");
        sb.AppendLine("        if (reader.Read())");
        sb.AppendLine("        {");
        foreach (var p in parameterInfoDTO)
        {
            sb.AppendLine($"            {TableName.ToLower()}DTO.{p.Name} = {MapReaderGetter(p.DataType, p.Name)};");
        }
        sb.AppendLine("        }");
        sb.AppendLine("        reader.Close();");
        sb.AppendLine($"        result.Data = {TableName.ToLower()}DTO;");
        sb.AppendLine("        result.Message = outputMsg.Value?.ToString();");
        sb.AppendLine("        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));");
        sb.AppendLine("    }");
        sb.AppendLine("    catch (Exception ex)");
        sb.AppendLine("    {");
        sb.AppendLine("        result.Data = null;");
        sb.AppendLine($"        result.Message = \"Database error occurred while updating the {TableName.ToLower()}\";");
        sb.AppendLine("        result.ErrorType = ErrorType.DatabaseError;");
        sb.AppendLine("    }");
        sb.AppendLine("    return result;");
        sb.AppendLine("}");
        return sb.ToString();
    }

    // ================= DELETE =================
    public string BuildDeleteMethod()
    {
        var sb = new StringBuilder();
        var dtoName = TableName + "DTO";

        var deleteParams = parameterInfoDTO
            .Where(p => p.UseInWhere && (p.UsedInGet == ParameterInfoDTO.GetUsageType.Delete || p.UsedInGet == ParameterInfoDTO.GetUsageType.Both))
            .ToList();

        var methodName =
            deleteParams.Any()
                ? $"Delete{TableName}By{string.Join("And", deleteParams.Select(p => p.Name))}Async"
                : $"Delete{TableName}Async";

        var spName =
    deleteParams.Any()
        ? $"SP_Delete{TableName}By{string.Join("And", deleteParams.Select(p => p.Name))}"
        : $"SP_Delete{TableName}";

        sb.AppendLine($"public static async Task<ApiResult<{dtoName}>> {methodName}(");

        for (int i = 0; i < deleteParams.Count; i++)
        {
            var p = deleteParams[i];
            var paramName = char.ToLower(p.Name[0]) + p.Name.Substring(1);
            sb.AppendLine($"    {p.DataType} {paramName},");
        }

        sb.AppendLine("    CancellationToken cancellationToken = default)");
        sb.AppendLine("{");
        sb.AppendLine($"    var result = new ApiResult<{dtoName}>();");

        sb.AppendLine($"    var {TableName.ToLower()} = new {dtoName}");
        sb.AppendLine("    {");
        foreach (var p in deleteParams)
        {
            var paramName = p.Name.Substring(0, 1).ToLower() + p.Name.Substring(1);
            sb.AppendLine($"        {p.Name} = {paramName},");
        }
        sb.AppendLine("    };");

        sb.AppendLine("    try");
        sb.AppendLine("    {");
        sb.AppendLine($"        await using var connection = new SqlConnection(connectionString._connectionString);");
        sb.AppendLine(
          $"        await using var command = new SqlCommand(\"{spName}\", connection) " +
          "{ CommandType = CommandType.StoredProcedure };");

        foreach (var p in deleteParams)
        {
            var sqlType = MapToSqlDbType(p.DataType);
            var size = p.DataType == "string" ? ", 500" : "";
            var valueExpr = p.DataType == "string"
                ? $"(object?){TableName.ToLower()}.{p.Name} ?? DBNull.Value"
                : $"{TableName.ToLower()}.{p.Name}";
            sb.AppendLine($"        command.Parameters.Add(\"@{p.Name}\", SqlDbType.{sqlType}{size}).Value = {valueExpr};");
        }

        sb.AppendLine("        var outputMsg = new SqlParameter(\"@Message\", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };");
        sb.AppendLine("        var outputErrorType = new SqlParameter(\"@ErrorType\", SqlDbType.Int) { Direction = ParameterDirection.Output };");
        sb.AppendLine("        command.Parameters.Add(outputMsg);");
        sb.AppendLine("        command.Parameters.Add(outputErrorType);");

        sb.AppendLine("        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);");
        sb.AppendLine("        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);");
        sb.AppendLine("        result.Data = " + TableName.ToLower() + ";");
        sb.AppendLine("        result.Message = outputMsg.Value?.ToString();");
        sb.AppendLine("        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));");
        sb.AppendLine("    }");
        sb.AppendLine("    catch (Exception ex)");
        sb.AppendLine("    {");
        sb.AppendLine("        result.Data = null;");
        sb.AppendLine($"        result.Message = \"Database error occurred while deleting the {TableName.ToLower()}\";");
        sb.AppendLine("        result.ErrorType = ErrorType.DatabaseError;");
        sb.AppendLine("    }");
        sb.AppendLine("    return result;");
        sb.AppendLine("}");
        return sb.ToString();
    }

    // ================= HELPERS =================
    private string MapReaderGetter(string dataType, string columnName)
    {
        return dataType switch
        {
            "int" => $"reader.IsDBNull(reader.GetOrdinal(\"{columnName}\")) ? 0 : reader.GetInt32(reader.GetOrdinal(\"{columnName}\"))",
            "string" => $"reader.IsDBNull(reader.GetOrdinal(\"{columnName}\")) ? null : reader.GetString(reader.GetOrdinal(\"{columnName}\"))",
            "bool" => $"reader.IsDBNull(reader.GetOrdinal(\"{columnName}\")) ? false : reader.GetBoolean(reader.GetOrdinal(\"{columnName}\"))",
            "decimal" => $"reader.IsDBNull(reader.GetOrdinal(\"{columnName}\")) ? 0 : reader.GetDecimal(reader.GetOrdinal(\"{columnName}\"))",
            "DateTime" => $"reader.IsDBNull(reader.GetOrdinal(\"{columnName}\")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal(\"{columnName}\"))",
            _ => $"reader.IsDBNull(reader.GetOrdinal(\"{columnName}\")) ? null : reader.GetValue(reader.GetOrdinal(\"{columnName}\"))"
        };
    }

    private string MapToSqlDbType(string dataType)
    {
        return dataType switch
        {
            "int" => "Int",
            "string" => "NVarChar",
            "bool" => "Bit",
            "decimal" => "Decimal",
            "DateTime" => "DateTime",
            _ => "Variant"
        };
    }
}
