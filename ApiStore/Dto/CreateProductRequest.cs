namespace ApiStore.Dto;
public class CreateProductRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
}