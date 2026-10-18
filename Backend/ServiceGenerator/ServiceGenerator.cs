public class ParameterDTOViewCollection
{



    public List<ParameterInfoDTOView> ParameterInfoDTOViews = new List<ParameterInfoDTOView>();


    public void Add(ParameterInfoDTOView parameterInfoDTO)
    {
        ParameterInfoDTOViews.Add(parameterInfoDTO);



    }
    public List<ParameterInfoDTOView> GetList()
    {


        return ParameterInfoDTOViews;

    }



}
public class ParameterDTOCollection
{
    public List<ParameterInfoDTO> ParameterInfoDTO = new List<ParameterInfoDTO>();



    public void Add(ParameterInfoDTO parameterInfoDTO)
    {

        ParameterInfoDTO.Add(parameterInfoDTO);

    }
    public List<ParameterInfoDTO> GetList()
    {


        return ParameterInfoDTO;

    }



}