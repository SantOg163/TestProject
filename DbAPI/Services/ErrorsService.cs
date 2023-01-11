namespace DbAPI.Services
{
    public class ErrorsService
    {
        public static string _errorPath = Directory.GetCurrentDirectory().Replace("\\bin\\Debug", "") + "\\errors.txt";
        public void AddError(string error)
        {
            using (StreamWriter writer = new StreamWriter(_errorPath))
            {
                writer.WriteLine(error);
            }
        }
        public List<string> GetAllErrors()
        {
            using (StreamReader reader = new StreamReader(_errorPath))
            {
                return reader.ReadToEnd().Split('\n').ToList();
            }
        }
    }
}
