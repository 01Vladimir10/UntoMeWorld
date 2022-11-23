namespace UntoMeWorld.Application.Model;

public class LabelReportRowModel<T>
{
    public T Item { get; set; } = default!;
    public int Index { get; set; }
}