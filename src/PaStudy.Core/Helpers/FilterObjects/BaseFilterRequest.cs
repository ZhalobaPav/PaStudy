namespace PaStudy.Core.Helpers.FilterObjects;

public class BaseFilterRequest<T>
{
    public int Take { get; set; }
    public int Skip { get; set; }
    public T FilterOptions { get; set; }
}
