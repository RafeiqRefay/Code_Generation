using System.Text;

public class SearchBuilder
{
    public List<ParameterInfoDTOView> parameterInfoViewDTO { get; set; }
        = new List<ParameterInfoDTOView>();

    public string TableName { get; set; }

    public string BuildSearch()
    {
      

        var sb = new StringBuilder();
        sb.AppendLine($"public class Search{TableName}Request");
        sb.AppendLine("{");

        // ===== Properties =====
        foreach (var p in parameterInfoViewDTO.Where(p => (p.UsedInGet==ParameterInfoDTOView.GetUsageTypeView.GetAll || p.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.Both)))
        {
           

            sb.AppendLine($"    public {p.DataType}? {p.Name} {{ get; set; }}");
        }

        sb.AppendLine();

   
        sb.AppendLine("}");

        return sb.ToString();
    }


 
}