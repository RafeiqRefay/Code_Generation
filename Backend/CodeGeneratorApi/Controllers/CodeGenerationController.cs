using Microsoft.AspNetCore.Mvc;


namespace CodeGeneratorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeGenerationController : ControllerBase
    {
  

        CodeGenerationPayload Mypaylod = new CodeGenerationPayload();
        HelperOptions options = new HelperOptions();

        SqlBuilder    sqlBuilder  = new SqlBuilder();
        DataBuilder dataBuilder = new DataBuilder();
        ServiceBuilder serviceBuilder = new ServiceBuilder();

        DTOViewBuilder dtoViewBuilder = new DTOViewBuilder();
        DTOBuilder dtoBuilder = new DTOBuilder();
        ControllerBuilder controllerBuilder= new ControllerBuilder();

        CommondPowerShell commondPowerShell = new CommondPowerShell();
        
        UpdateDTOBuilder updateDTOBuilder = new UpdateDTOBuilder();

        SearchBuilder searchBuilder = new SearchBuilder();
        EntityGeneric entityGeneric = new EntityGeneric();

        //  Create DTO in Front
        void GenerateDTO() {
            options.DTO = (dtoBuilder.BuildDTO());
          
        }

        // Create ViewDTO In front
        void GenerateDTOView() {
            options.DTOView = (dtoViewBuilder.BuildDTOView());
        }
        // Create UpdateDTO In front

        void GenerateUpdateDTO()
        {
            options.UpdateDTO = (updateDTOBuilder.BuildDTO());

        }
        // Create Search In front
        void GenerateSearch()
        {
            options.Search = (searchBuilder.BuildSearch());
           

        }
        // Create Entity In front
      
     

        // تخزين العملومات في ملف انشاء الفرونت 
        void StoreDataInEntityGeneric()
        {
            entityGeneric.parameterInfoDTO = Mypaylod.DtoParameters;
                entityGeneric.TableName = Mypaylod.TableName;
                entityGeneric.parameterInfoDTOView = Mypaylod.DtoViewParameters;
                entityGeneric.codeGenerationPayload = Mypaylod;
        }


        // تخزين العملومات في ملف انشاء الداتا بيس  

        void StoreDataSQL()
        {
            if (Mypaylod.ComplexQuere == true)
            {
                sqlBuilder.ComplexQuere = Mypaylod.GetQuereComplex;

            }

            sqlBuilder.NameTable = Mypaylod.TableName;
           
            sqlBuilder. parameterInfoView = Mypaylod.DtoViewParameters;
            sqlBuilder. parameterInfoDTO = Mypaylod.DtoParameters;
            sqlBuilder.codeGenerationPayload = Mypaylod;
           
        }
        // تخزين العملومات في ملف انشاء  Data Acssess 

        void StoreDataDataAccessLayer()
        {

            dataBuilder.TableName = Mypaylod.TableName;
            dataBuilder.parameterInfoDTO = Mypaylod.DtoParameters;
            dataBuilder.parameterInfoDTOView = Mypaylod.DtoViewParameters;
            dataBuilder.codeGenerationPayload = Mypaylod;
           

        }
        // تخزين العملومات في ملف انشاء Service

        void StoreDataService()
        {

            serviceBuilder.TableName = Mypaylod.TableName;
            serviceBuilder.parameterInfoDTO = Mypaylod.DtoParameters;
            serviceBuilder.parameterInfoDTOView = Mypaylod.DtoViewParameters;
            serviceBuilder.codeGenerationPayload= Mypaylod;



        }
        //تخزين العملومات في ملف انشاء ViewDTO 


        void StoreDataViewDTO()
        {

            dtoViewBuilder.TableName = Mypaylod.TableName;
            dtoViewBuilder.parameterInfoDTOView = Mypaylod.DtoViewParameters;
           
            

        }
        //تخزين العملومات في ملف انشاء DTO 

        void StoreDataDTO()
        {

            dtoBuilder.TableName = Mypaylod.TableName;
            dtoBuilder.parameterInfoDTO = Mypaylod.DtoParameters;
           
            
        }
        //تخزين العملومات في ملف انشاء Search

        void StoreSearch()
        {


            searchBuilder.parameterInfoViewDTO = Mypaylod.DtoViewParameters;
            searchBuilder.TableName = Mypaylod.TableName;


        }        //تخزين العملومات في ملف انشاء UpdateDTO

        void StoreDataUpdateDTO()
        {

            updateDTOBuilder.TableName = Mypaylod.TableName;
            updateDTOBuilder.parameterInfoDTO = Mypaylod.DtoParameters;


        }
        //تخزين العملومات في ملف انشاء Controller

        void StoreDataController()
        {

            controllerBuilder.TableName = Mypaylod.TableName;
            controllerBuilder.parameterInfoDTO = Mypaylod.DtoParameters;
            controllerBuilder.parameterInfoDTOView = Mypaylod.DtoViewParameters;
            controllerBuilder.codeGenerationPayload= Mypaylod;

        }

        //تخزين العملومات في ملف انشاء PowerShell

        void StoreDataPowerShell()
        {

            
            string ClassController = (controllerBuilder.GenerateControllerClass());
            string ClassService = (serviceBuilder.GenerateServiceClass());
            string ClassData = (dataBuilder.GenerateDataClass());
            string ClassDTO = (dtoBuilder.BuildDTO());
            string ClassViewDTO = (dtoViewBuilder.BuildDTOView());
            string ClassUpdateDTO = updateDTOBuilder.BuildDTO();
            string ProjectPath = Mypaylod.ProjectPath;
            string TableName = Mypaylod.TableName;
            string DataBaseName = Mypaylod.DataBaseName;
            string ClassSearch = searchBuilder.BuildSearch();
            string PathEntity = Mypaylod.PathEntity;
            string PathStore= Mypaylod.PathStore;
            string PathPage=Mypaylod.PathPage;


            commondPowerShell.ClassSearch = ClassSearch;
            commondPowerShell.DataBaseName = DataBaseName;
            commondPowerShell.ProjectPath = ProjectPath;
            commondPowerShell.InitializeProjectPaths();
            commondPowerShell.NameTable = TableName;
            commondPowerShell.ClassController = ClassController;
            commondPowerShell.ClassViewDTO = ClassViewDTO;
            commondPowerShell.ClassDTO = ClassDTO;
            commondPowerShell.ClassUpdateDTO = ClassUpdateDTO;
            commondPowerShell.ClassService = ClassService;
            commondPowerShell.ClassData = ClassData;
            commondPowerShell.PathPage = PathPage;
            commondPowerShell.PathEntity = PathEntity;
            commondPowerShell.PathStore = PathStore;


            commondPowerShell.FrontendEntity = entityGeneric.Code();
                commondPowerShell.SqlGetAll = sqlBuilder.GenerateGetAllSP();
                commondPowerShell.SqlAdd = sqlBuilder.GenerateAddSP();
                commondPowerShell.SqlDelete = sqlBuilder.GenerateDeleteSP();
                commondPowerShell.SqlGetOne = sqlBuilder.GenerateGetOneBySP();
                commondPowerShell.SqlUpdate = sqlBuilder.GenerateUpdateSP();
                 commondPowerShell.SqlSearch=sqlBuilder.GenerateSearchSP();
     

        }
        // تخزين الاكواد وارسالها للفرونت
              void StoreGAUDExtend()
        {
            options.ExecuteGenerateDTO = commondPowerShell.CreateDTO();
            options.ExecuteGenerateDTOView = commondPowerShell.CreateViewDTO();
           options.ExecuteGenerateUpdateDTO = commondPowerShell.CreateUpdateDTO();
            options.ExecuteGenerateSearch = commondPowerShell.CreateSearch();
            options.ExecuteGenerateService = commondPowerShell.CreateService();
            options.ExecuteGenerateData = commondPowerShell.CreateDataAccess();
            options.ExecuteGenerateController = commondPowerShell.CreateController();
            
            options.CleanCsprojScript = commondPowerShell.CleanCsprojScript();
            options.GenerateCreateSqlUpdate = commondPowerShell.GenerateCreateSqlUpdate();
            options.GenerateCreateSqlAdd = commondPowerShell.GenerateCreateSqlAdd();
            options.GenerateCreateSqlDelete = commondPowerShell.GenerateCreateSqlDelete();
            options.GenerateCreateSqlGetAll = commondPowerShell.GenerateCreateSqlGetAll();
            options.GenerateCreateSqlGetOne = commondPowerShell.GenerateCreateSqlGetOne();          
            options.GenerateCreateSqlSearch=commondPowerShell.GenerateCreateSqlSearch();
            options.GenerateEntity=commondPowerShell.CreateFrontendEntity();


        }
     
         void StoreGAUDInit()
        {


            StoreGAUDExtend();
            options.CreateFiLe = commondPowerShell.CreateFolder();
            options.CreateApiResult = commondPowerShell.CreateApiResult();
            options.CreateConnectionStringClass = commondPowerShell.CreateConnectionStringClass();
            options.CreateEnErrorType = commondPowerShell.CreateEnErrorType();
            options.CreateApiResponseHelper = commondPowerShell.CreateApiResponseHelper();

        }

    

        [HttpPost("generate")]
        public IActionResult Generate([FromBody] CodeGenerationPayload  payload)

        {

            if (payload == null || string.IsNullOrWhiteSpace(payload.TableName))
                return BadRequest("Invalid payload.");

            Mypaylod = payload;



            
            // === إنشاء الـ Builders ===
            StoreSearch();
            StoreDataSQL();
            StoreDataDataAccessLayer();
           StoreDataService();
            StoreDataViewDTO();
            StoreDataController();
            StoreDataDTO();
            StoreDataUpdateDTO();
            StoreDataInEntityGeneric();




            if (Mypaylod.GAUDInit != true ||Mypaylod.GAUDExtend!=true )
            {
                // === توليد DTOs ===
                GenerateDTO();
                GenerateDTOView();
                GenerateUpdateDTO();
                GenerateSearch();
                
               
            }
            
            // === توليد كل شيء إذا AllGAUD ===
            if (Mypaylod.AllGAUD == true)
            {





         
                StoreDataPowerShell();
      
                options.ExecuteGenerateService = commondPowerShell.CreateService();
                options.ExecuteGenerateData = commondPowerShell.CreateDataAccess();
                options.ExecuteGenerateController = commondPowerShell.CreateController();

                options.CleanCsprojScript = commondPowerShell.CleanCsprojScript();



                // === SQL ===
                if (payload.GetAll == true)
                {
                    options.ExecuteGenerateDTOView = commondPowerShell.CreateViewDTO();
                    options.GenerateCreateSqlGetAll = commondPowerShell.GenerateCreateSqlGetAll();



                }
                if (payload.GetOneBy == true)
                {
                    options.ExecuteGenerateDTOView = commondPowerShell.CreateViewDTO();
                    options.GenerateCreateSqlGetOne = commondPowerShell.GenerateCreateSqlGetOne();


                }
                if (payload.AddNew == true)
                {
                    options.ExecuteGenerateDTO = commondPowerShell.CreateDTO();
                    options.GenerateCreateSqlAdd = commondPowerShell.GenerateCreateSqlAdd();


                }
                if (payload.UpdateBy == true)
                {
                    options.ExecuteGenerateUpdateDTO = commondPowerShell.CreateUpdateDTO();
                    options.GenerateCreateSqlUpdate = commondPowerShell.GenerateCreateSqlUpdate();




                }
                if (payload.DeleteBy == true)
                {
                    options.ExecuteGenerateDTO = commondPowerShell.CreateDTO();
                    options.GenerateCreateSqlDelete = commondPowerShell.GenerateCreateSqlDelete();



                }
                if (payload.Search == true)
                {
                    options.GenerateCreateSqlSearch = commondPowerShell.GenerateCreateSqlSearch();
                    options.ExecuteGenerateSearch = commondPowerShell.CreateSearch();





                }
                options.GenerateEntity = commondPowerShell.CreateFrontendEntity();


            }
            if (Mypaylod.GAUDInit == true)
            {
                StoreDataPowerShell();

                StoreGAUDInit();


            }
            if (Mypaylod.GAUDExtend == true)
            {
                StoreDataPowerShell();

                StoreGAUDExtend();

            }



        


            return Ok(options);
        }
    }

    }







