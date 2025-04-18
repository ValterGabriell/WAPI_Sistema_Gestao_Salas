namespace WAPI_GS.Dto
{
    public class DtoGetCombo
    {
        public string Id { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;

        public DtoGetCombo(string id, string label)
        {
            Id = id;
            Label = label;
        }
    }
}
