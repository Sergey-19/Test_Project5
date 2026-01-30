using EQX.Core.Vision.Algorithms;
using Newtonsoft.Json;
using System.IO;

namespace EQX.Vision.Algorithms
{
    public class VisionToolRepository : IVisionToolRepository
    {
        private readonly string _optionFilePath;

        public VisionToolRepository(string optionFilePath)
        {
            _optionFilePath = optionFilePath;
        }

        public IEnumerable<IVisionTool> GetAll()
        {
            List<IVisionTool> allTool = new List<IVisionTool>();

            if (File.Exists(_optionFilePath) == false) return allTool;

            try
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };
                allTool = JsonConvert.DeserializeObject<List<IVisionTool>>(File.ReadAllText(_optionFilePath), settings)!;
            }
            catch { }

            return allTool;
        }
    }
}
