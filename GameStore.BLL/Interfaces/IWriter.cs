namespace GameStore.BLL.Interfaces
{
    public interface IWriter
    {
        void WriteToFile(string filename, string text);
    }
}
