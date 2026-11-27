using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Final_Project.Exceptions;

namespace Final_Project.Helpers
{
    public static class JsonHelper
    {
        private static readonly string FolderPath = @"C:\Users\user\Desktop\JSON";

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Converters = { new JsonStringEnumConverter() }
        };

        public static async Task SaveAsync<T>(string fileName, T data)
        {
            try
            {
                Directory.CreateDirectory(FolderPath);

                string path = Path.Combine(FolderPath, fileName);
                string json = JsonSerializer.Serialize(data, Options);

                await File.WriteAllTextAsync(path, json);
            }
            catch (IOException ex)
            {
                throw new FileStorageException($"ფაილის შენახვა ვერ მოხერხდა: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new FileStorageException($"საქაღალდეზე წვდომა აკრძალულია: {ex.Message}");
            }
        }

        public static async Task<List<T>> LoadListAsync<T>(string fileName)
        {
            string path = Path.Combine(FolderPath, fileName);

            if (!File.Exists(path))
                return new List<T>();

            try
            {
                string json = await File.ReadAllTextAsync(path);

                if (string.IsNullOrWhiteSpace(json))
                    return new List<T>();

                List<T>? result = JsonSerializer.Deserialize<List<T>>(json, Options);

                return result ?? new List<T>();
            }
            catch (JsonException)
            {
                throw new FileStorageException($"ფაილი \"{fileName}\" დაზიანებულია!");
            }
            catch (IOException ex)
            {
                throw new FileStorageException($"ფაილის წაკითხვა ვერ მოხერხდა: {ex.Message}");
            }
        }
    }
}