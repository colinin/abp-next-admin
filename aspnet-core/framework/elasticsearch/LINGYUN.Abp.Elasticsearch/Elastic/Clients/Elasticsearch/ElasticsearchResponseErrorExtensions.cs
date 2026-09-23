using Elastic.Transport.Products.Elasticsearch;
using System.Text;

namespace Elastic.Clients.Elasticsearch;

public static class ElasticsearchResponseErrorExtensions
{
    public static bool TryGetErrorMessage(this ElasticsearchResponse response, out string? errorMessage)
    {
        if (!response.IsSuccess())
        {
            var errorBuilder = new StringBuilder();
            if (response.TryGetOriginalException(out var ex) && ex != null)
            {
                errorBuilder.AppendLine(ex.Message);
            }
            if (response.TryGetElasticsearchServerError(out var error) && error != null)
            {
                errorBuilder.AppendLine(error.ToString());
            }

            if (errorBuilder.Length == 0)
            {
                errorBuilder.AppendLine(response.DebugInformation);
            }
            errorMessage = errorBuilder.ToString();
            return true;
        }
        errorMessage = null;
        return false;
    }
}
