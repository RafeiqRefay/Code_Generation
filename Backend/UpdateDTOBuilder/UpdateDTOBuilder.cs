using System.Text;

public class UpdateDTOBuilder
{
    public List<ParameterInfoDTO> parameterInfoDTO { get; set; }
        = new List<ParameterInfoDTO>();

    public string TableName { get; set; }

    public string BuildDTO()
    {
        if (string.IsNullOrWhiteSpace(TableName))
            throw new InvalidOperationException("TableName is required");

        var sb = new StringBuilder();
        sb.AppendLine("using System.ComponentModel.DataAnnotations;\r\n");
        sb.AppendLine($"public class {TableName}UpdateDTO");
        sb.AppendLine("{");
        sb.AppendLine(" [Required]");

        // ===== Properties =====
        foreach (var p in parameterInfoDTO)
        {
            if (p.IsPrimaryKey)
            {
                sb.AppendLine($"    public {p.DataType}  {p.Name} {{ get; set; }}");

            }
            else
            {
                if (p.UseColumnInUpdate)
                {
                    sb.AppendLine($"    public {p.DataType}?  {p.Name} {{ get; set; }}");

                }
            }
        }

        sb.AppendLine();

        // ===== Default ctor =====
        sb.AppendLine($"    public {TableName}UpdateDTO() {{ }}");
        sb.AppendLine();

        // ===== Full ctor =====
        sb.Append($"    public {TableName}UpdateDTO(");
        sb.Append(string.Join(", ",
            parameterInfoDTO.Select(p =>
                $"{p.DataType} {ToCamelCase(p.Name)}")));
        sb.AppendLine(")");
        sb.AppendLine("    {");

        foreach (var p in parameterInfoDTO)
        {

            if (p.UseColumnInUpdate)
            {
                sb.AppendLine($"        {p.Name} = {ToCamelCase(p.Name)};");
            }
        }

        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }

    // ===== Helper =====
    private string ToCamelCase(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        return char.ToLowerInvariant(name[0]) + name.Substring(1);
    }
}
