using EventSeller.Services.Interfaces.Exporters;
using System.Collections;
using System.Reflection;
using System.Text;

namespace EventSeller.Services.Helpers
{
    public class CsvFileExporter : ICsvFileExport
    {
        private const string Delimiter = ";";

        public async Task<Stream> ExportFileAsync<TClass>(TClass statistics)
        {
            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true))
            {
                if (typeof(IEnumerable).IsAssignableFrom(typeof(TClass)) && typeof(TClass) != typeof(string))
                {
                    var headers = GetHeaders(typeof(TClass).GetGenericArguments().FirstOrDefault() ?? typeof(TClass));
                    await writer.WriteLineAsync(string.Join(Delimiter, headers));

                    foreach (var item in (IEnumerable)statistics)
                    {
                        var values = FlattenObjectToStrings(item);
                        await writer.WriteLineAsync(string.Join(Delimiter, values));
                    }
                }
                else
                {
                    var headers = GetHeaders(typeof(TClass));
                    await writer.WriteLineAsync(string.Join(Delimiter, headers));

                    var values = FlattenObjectToStrings(statistics);
                    await writer.WriteLineAsync(string.Join(Delimiter, values));
                }
            }

            stream.Position = 0;
            return stream;
        }

        private List<string> GetHeaders(Type type)
        {
            var headers = new List<string>();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var propName = property.Name;
                var propertyType = property.PropertyType;

                if (typeof(IEnumerable).IsAssignableFrom(propertyType) && propertyType != typeof(string))
                {
                    var elementType = propertyType.GetGenericArguments().FirstOrDefault() ?? propertyType.GetElementType();
                    if (elementType != null)
                    {
                        headers.AddRange(GetHeaders(elementType));
                    }
                }
                else if (propertyType.IsClass && propertyType != typeof(string))
                {
                    headers.AddRange(GetHeaders(propertyType));
                }
                else
                {
                    headers.Add(propName);
                }
            }
            return headers;
        }

        private List<string> FlattenObjectToStrings(object obj)
        {
            var values = new List<string>();
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                if (value != null)
                {
                    if (typeof(IEnumerable).IsAssignableFrom(value.GetType()) && value.GetType() != typeof(string))
                    {
                        foreach (var item in (IEnumerable)value)
                        {
                            values.AddRange(FlattenObjectToStrings(item));
                        }
                    }
                    else if (value.GetType().IsClass && value.GetType() != typeof(string))
                    {
                        values.AddRange(FlattenObjectToStrings(value));
                    }
                    else
                    {
                        values.Add(value.ToString());
                    }
                }
                else
                {
                    values.Add(string.Empty);
                }
            }
            return values;
        }
    }
}
