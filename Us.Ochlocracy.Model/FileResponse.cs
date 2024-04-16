namespace Us.Ochlocracy.Model
{
    public class FileResponse
    {
        public string FileName { get; set; } = string.Empty;
        public byte[] ContentAsBytes { get; set; } = []; 
        public static FileResponse Empty() => new();
    }
}
