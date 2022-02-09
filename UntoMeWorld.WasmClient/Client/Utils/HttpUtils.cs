namespace UntoMeWorld.WasmClient.Client.Utils;

public static class HttpUtils
{
    public static string BuildQuery(params object[] parameters)
    {
        if (parameters.Length % 2 != 0)
            throw new ArgumentException("Incorrect number of arguments, there must be a value for each query argument");

        var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
        
        for (var i = 0; i < parameters.Length; i += 2)
        {
            var queryParam = (string) parameters[i];
            var queryValue = parameters[i + 1]?.ToString()?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(queryValue))
                query.Add(queryParam, queryValue);
        }
        // Remove last &
        return "?" + query;
    }

    public static string BuildUrl(string endPoint, params object[] queryParameters)
        => endPoint + BuildQuery(queryParameters);
}