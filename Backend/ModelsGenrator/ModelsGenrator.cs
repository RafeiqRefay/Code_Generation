
    public class HelperOptions
    {
        public string SqlGetAll { get; set; } = null;
        public string SqlSearch { get; set; } = null;
        public string SqlGetOneBy { get; set; } = null;
        public string SqlAdd { get; set; } = null;
        public string SqlUpdate { get; set; } = null;
        public string SqlDelete { get; set; } = null;

        public string DTO { get; set; } = null;
        public string DTOView { get; set; } = null;
    public string Search { get; set; } = null;
    public string UpdateDTO { get; set; } = null;

        public string GetAllData { get; set; } = null;
        public string SearchData { get; set; } = null;
        public string GetOneByData { get; set; } = null;
        public string AddData { get; set; } = null;
        public string UpdateData { get; set; } = null;
        public string DeleteData { get; set; } = null;


        public string DeleteService { get; set; } = null;

        public string UpdateService { get; set; } = null;

        public string AddService { get; set; } = null;

        public string SearchService { get; set; } = null;

        public string GetAllService { get; set; } = null;

        public string GetOneService { get; set; } = null;


        public string GetAllController { get; set; } = null;
        public string SearchController { get; set; } = null;
        public string GetOneByController { get; set; } = null;
        public string AddController { get; set; } = null;
        public string UpdateController { get; set; } = null;
        public string DeleteController { get; set; } = null;


        public string GenerateControllerClass { get; set; } = null;

        public string GenerateServiceClass { get; set; } = null;

        public string GenerateDataClass { get; set; } = null;
   






    public string ExecuteGenerateDTOView { get; set; } = null;
    public string ExecuteGenerateDTO { get; set; } = null;

    public string ExecuteGenerateSearch { get; set; } = null;

    public string ExecuteGenerateUpdateDTO { get; set; } = null;


    public string ExecuteGenerateData { get; set; } = null;

    public string ExecuteGenerateService { get; set; } = null;

    public string ExecuteGenerateController { get; set; } = null;

    public string CreateFiLe { get; set; } = null;

    public string CreateApiResult { get; set; } = null;
    public string CreateEnErrorType { get; set; } = null;
    public string CreateApiResponseHelper { get; set; } = null;
    public string CreateConnectionStringClass { get; set; } = null;

    public string CleanCsprojScript { get; set; } = null;
    public string GenerateSqlExecutionScript { get; set; } = null;
    public string GenerateCreateSqlGetOne { get; set; } = null;
    public string GenerateCreateSqlGetAll { get; set; } = null;
    public string GenerateCreateSqlAdd { get; set; } = null;
    public string GenerateCreateSqlUpdate { get; set; } = null;
    public string GenerateCreateSqlDelete { get; set; } = null;

    public string GenerateEntity { get; set; } = null;


    public string GenerateCreateSqlSearch { get; set; } = null;





}

public class ParameterInfoDTOView
    {

        public enum GetUsageTypeView
        {
            None,
            GetAll,
            GetOne,
            Both
        }



    public string Name { get; set; } = null;
    public string DataType { get; set; } = null;
        public string DataTypeSql { get; set; } = null;

    public bool IsPrimaryKey { get; set; } = false;
      
    
 
        public bool UseInWhere { get; set; }= false;

        public GetUsageTypeView UsedInGet { get; set; }=GetUsageTypeView.None;

 

        public ParameterInfoDTOView(string Name, string DataType, string DataTypeSql, bool IsPrimaryKey,  bool UseInWhere, GetUsageTypeView UsedInGet)
        {
            this.Name = Name;
            this.DataType = DataType;
            this.DataTypeSql = DataTypeSql;
           
            this.IsPrimaryKey = IsPrimaryKey;
         
            this.UseInWhere = UseInWhere;
            this.UsedInGet = UsedInGet;
        
        }
        public ParameterInfoDTOView() { }


    }


public class ParameterInfoDTO
{

    public enum GetUsageType
    {
        None,
        Delete,
        Update,
        Both
    }



    public string Name { get; set; } = null;
    public string DataType { get; set; } = null;
    public string DataTypeSql { get; set; } = null;

    public string NameTable { get; set; } = null;
    public bool IsPrimaryKey { get; set; } = false;
    public bool IsForeignKey { get; set; } = false;


    public bool UseInWhere { get; set; }= false;

    public GetUsageType UsedInGet { get; set; }= GetUsageType.None;

    public bool UseColumnInUpdate { get; set; }= false;
    public string ReferencedColumn { get; set; } = null;


    public ParameterInfoDTO(string Name, string DataType, string DataTypeSql, string NameTable, bool IsPrimaryKey, bool IsForeignKey, bool UseInWhere, GetUsageType UsedInGet, bool UseColumnInUpdate,  string ReferencedColumn)
    {
        this.Name = Name;
        this.DataType = DataType;
        this.DataTypeSql = DataTypeSql;
        this.NameTable = NameTable;
        this.IsPrimaryKey = IsPrimaryKey;
        this.IsForeignKey = IsForeignKey;

        this.UseInWhere = UseInWhere;
        this.UsedInGet = UsedInGet;
        this.UseColumnInUpdate = UseColumnInUpdate;

        this.ReferencedColumn = ReferencedColumn;


    }
    public ParameterInfoDTO() { }

}





