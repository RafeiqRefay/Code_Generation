public class CodeGenerationPayload
{
      
  public  bool? AllGAUD { get; set; }
    public bool? GetAll { get; set; }

    public bool? GetOneBy { get; set; }
    public bool? AddNew { get; set; }
    public bool? UpdateBy { get; set; }
    public bool? DeleteBy { get; set; }
    public bool? GAUDInit {  get; set; }
    public bool? GAUDExtend { get; set; }

    public bool? Search { get; set; }
    public bool ? ComplexQuere { get; set; }

    public string? GetQuereComplex { get; set; } 




    public List<ParameterInfoDTO>? DtoParameters { get; set; }
    public List<ParameterInfoDTOView>? DtoViewParameters { get; set; }
    public string? TableName { get; set; }
    public string ? ProjectPath { get; set; }

    public string? DataBaseName { get; set; }

    public string ? PathStore { get; set; }
    public string? PathEntity { get; set; }
    public string? PathPage { get; set; }
}