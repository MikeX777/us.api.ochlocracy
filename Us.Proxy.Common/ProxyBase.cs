using LanguageExt;
using Serilog;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Extensions;
using Us.Proxy.Common.Extensions;
using static LanguageExt.Prelude;
using static Newtonsoft.Json.JsonConvert;

namespace Us.Proxy.Common
{
    public class ProxyBase
    {
        private const string IdentifierHeader = "identifier";
        readonly ILogger log;
        readonly string baseUrl;
        readonly JsonDeserializer deserializer;
        readonly IHttpCallLogger httpCallLogger = new NullHttpCallLogger();
        protected readonly HttpClient httpClient;
        protected readonly ErrorSource errorSource = ErrorSource.Unspecified;

        public ProxyBase(HttpClient httpClient, string baseUrl, ErrorSource errorSource, IHttpCallLogger httpCallLogger, ILogger log)
        {
            this.httpClient = httpClient;
            this.baseUrl = baseUrl.IsEmpty()
                ? httpClient.BaseAddress != null
                    ? httpClient.BaseAddress.ToString()
                    : ""
                : baseUrl;

            this.errorSource = errorSource;
            this.httpCallLogger = httpCallLogger;
            this.log = log;
            deserializer = new JsonDeserializer(log, errorSource);
        }


        protected EitherAsync<Error, O> Get<O>(string route, CancellationToken cancellationToken = default) =>
            GetAsync<O>(route, cancellationToken).ToAsync();

        protected EitherAsync<Error, Option<O>> GetOption<O>(string route, CancellationToken cancellationToken = default) =>
            GetOptionAsync<O>(route, cancellationToken).ToAsync();

        protected EitherAsync<Error, O> Post<I, O>(string route, HttpContent json, CancellationToken cancellationToken = default) =>
            PostAsync<I, O>(route, json, cancellationToken).ToAsync();

        protected EitherAsync<Error, O> Put<I, O>(string route, HttpContent json, CancellationToken cancellationToken = default) =>
            PutAsync<I, O>(route, json, cancellationToken).ToAsync();

        protected EitherAsync<Error, O> Delete<O>(string route, CancellationToken cancellationToken = default) =>
            DeleteAsync<O>(route, cancellationToken).ToAsync();

        protected EitherAsync<Error, FileResponse> GetFileResponse(string route, CancellationToken cancellationToken = default) =>
            GetFileResponseAsync(route, cancellationToken).ToAsync();


        protected async Task<Either<Error, O>> GetAsync<O>(string route, CancellationToken cancellationToken = default) =>
            await ProcessAsync<O>(c => c.GetAsync(BuildUrl(baseUrl, route, HttpMethod.Get), cancellationToken), cancellationToken);

        protected async Task<Either<Error, O>> GetAsync<O>(string route,
            Func<HttpResponseMessage, string, ILogger, Either<Error, O>> toResultOrError,
            CancellationToken cancellationToken = default) =>
            await ProcessHttpCallAsync(c => c.GetAsync(BuildUrl(baseUrl, route, HttpMethod.Get)), (c, s) => toResultOrError(c, s, log), cancellationToken);

        protected async Task<Either<Error, Option<O>>> GetOptionAsync<O>(string route, CancellationToken cancellationToken = default) =>
            await ProcessOptionAsync<O>(c => c.GetAsync(BuildUrl(baseUrl, route, HttpMethod.Get), cancellationToken), cancellationToken);

        protected async Task<Either<Error, O>> PostAsync<I, O>(string route, HttpContent json, CancellationToken cancellationToken = default) =>
            await ProcessAsync<O>(c => c.PostAsync(BuildUrl(baseUrl, route, HttpMethod.Post), json, cancellationToken), cancellationToken);

        protected async Task<Either<Error, O>> PutAsync<I, O>(string route, HttpContent json, CancellationToken cancellationToken = default) =>
            await ProcessAsync<O>(c => c.PutAsync(BuildUrl(baseUrl, route, HttpMethod.Put), json, cancellationToken), cancellationToken);

        protected async Task<Either<Error, O>> DeleteAsync<O>(string route, CancellationToken cancellationToken = default) =>
            await ProcessAsync<O>(c => c.DeleteAsync(BuildUrl(baseUrl, route, HttpMethod.Delete), cancellationToken), cancellationToken);

        protected async Task<Either<Error, FileResponse>> GetFileResponseAsync(string route, CancellationToken cancellationToken = default) =>
            await ProcessFileResponseAsync(c => c.GetAsync(BuildUrl(baseUrl, route, HttpMethod.Get), cancellationToken), cancellationToken);


        protected async Task<Either<Error, R>> ProcessAsync<R>(Func<HttpClient, Task<HttpResponseMessage>> httpCall, CancellationToken cancellationToken = default) =>
            await ProcessHttpCallAsync(httpCall, (response, content) => ToResultOrError<R>(response, content), cancellationToken);

        protected async Task<Either<Error, Option<R>>> ProcessOptionAsync<R>(Func<HttpClient, Task<HttpResponseMessage>> httpCall, CancellationToken cancellationToken = default) =>
            await ProcessHttpCallAsync(httpCall, (response, content) => ToOptionResultOrError<R>(response, content), cancellationToken);

        protected async Task<Either<Error, FileResponse>> ProcessFileResponseAsync(Func<HttpClient, Task<HttpResponseMessage>> httpCall,
            CancellationToken cancellationToken = default) =>
            await ProcessHttpCallForFileAsync(httpCall, (response, _) => ToFileOrError(response), cancellationToken);


        private async Task<Either<Error, FileResponse>> ToFileOrError(HttpResponseMessage response) =>
            response.IsSuccessStatusCode
                ? response.Content != null
                    ? new FileResponse
                    {
                        FileName = response.Content.Headers?.ContentDisposition?.FileName ?? "",
                        ContentAsBytes = await response.Content.ReadAsByteArrayAsync()
                    }
                    : FileResponse.Empty()
                : ToError(response,
                    () => LogAsWarning(log, errorSource, response, $"Error return in {nameof(ToFileOrError)} for expected {typeof(FileResponse)}."));

        private Either<Error, R> ToResultOrError<R>(HttpResponseMessage response, string? content) =>
            response.IsSuccessStatusCode
                ? deserializer.As<R>(content)
                : ToError(response,
                    () => LogAsWarning(log, errorSource, response, $"Error return in {nameof(ToResultOrError)} for expected {typeof(R)}. Content: '{content}'"));

        private Either<Error, Option<R>> ToOptionResultOrError<R>(HttpResponseMessage response, string? content)
        {
            if (response.IsSuccessStatusCode)
                return deserializer.AsOption<R>(content);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return Option<R>.None;

            return ToError(response,
                () => LogAsWarning(log, errorSource, response, $"Error return in {nameof(ToOptionResultOrError)} for expected {typeof(R)}. Content: '{content}'"));
        }


        /// <summary>
        /// Processes an HTTP call specified by <paramref name="httpCall"/> lambda, logs it, and converts response message content to 
        /// either expected result <typeparamref name="R"/> or <see cref="Error"/>  depending on the outcome of the call.
        /// </summary>
        /// <typeparam name="R">Result type expected from from an HTTP call.</typeparam>
        /// <param name="httpCall">Lambda representing a actual HTTP call to be performed that takes <see cref="HttpClient"/> and returns <see cref="Task"/> or <see cref="HttpResponseMessage"/>.</param>
        /// <param name="respond">Result converter from content provided by <see cref="HttpResponseMessage"/> to expected type <typeparamref name="R"/> or <see cref="Error"/>.</param>
        /// <param name="cancellationToken">The cancelation token that cancels the operation.</param>
        /// <returns>Returns either expected result <typeparamref name="R"/> or <see cref="Error"/> depending on the outcome of the call.</returns>
        private async Task<Either<Error, R>> ProcessHttpCallAsync<R>(
            Func<HttpClient, Task<HttpResponseMessage>> httpCall,
            Func<HttpResponseMessage, string?, Either<Error, R>> respond,
            CancellationToken cancellationToken = default)
            => await TryHttp(async () =>
            {
                var start = DateTimeOffset.UtcNow;
                var stopWatch = Stopwatch.StartNew();

                using var response = await Send(httpCall);
                stopWatch.Stop();

                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                LogHttpCall(start, start.AddMilliseconds(stopWatch.ElapsedMilliseconds), stopWatch.ElapsedMilliseconds, response, content);
                return respond(response, content);
            });

        /// <summary>
        /// Processes an HTTP call specified by <paramref name="httpCall"/> lambda, logs it, and converts response <see cref="FileResponse"/> 
        /// or <see cref="Error"/> depending on the outcome of the call.
        /// </summary>
        /// <param name="httpCall">Lambda representing a actual HTTP call to be performed that takes <see cref="HttpClient"/> and returns <see cref="Task"/> or <see cref="HttpResponseMessage"/>.</param>
        /// <param name="respond">Result converter from content provided by <see cref="HttpResponseMessage"/> to <see cref="FileResponse"/> or <see cref="Error"/>.</param>
        /// <param name="cancellationToken">The cancelation token that cancels the operation.</param>
        /// <returns>Returns either <see cref="FileResponse"/> or <see cref="Error"/> depending on the outcome of the call.</returns>
        private async Task<Either<Error, FileResponse>> ProcessHttpCallForFileAsync(
            Func<HttpClient, Task<HttpResponseMessage>> httpCall,
            Func<HttpResponseMessage, CancellationToken, Task<Either<Error, FileResponse>>> respond,
            CancellationToken cancellationToken = default)
            => await TryHttp(async () =>
            {
                var start = DateTimeOffset.UtcNow;
                var stopWatch = Stopwatch.StartNew();

                using var response = await Send(httpCall);
                stopWatch.Stop();

                LogHttpCall(start, start.AddMilliseconds(stopWatch.ElapsedMilliseconds), stopWatch.ElapsedMilliseconds, response);
                return await respond(response, cancellationToken);
            });

        protected virtual async Task<HttpResponseMessage> Send(Func<HttpClient, Task<HttpResponseMessage>> httpCall) =>
            await httpCall(httpClient);

        private HttpClient WriteIdentifierHeader(Guid? identifier) =>
            identifier != null
                ? httpClient.ReplaceHeader(IdentifierHeader, identifier.Value.ToString())
                : httpClient;

        private Task<Either<Error, R>> TryHttp<R>(Func<Task<Either<Error, R>>> operation) =>
            TryHttp(log, errorSource, operation);

        /// <summary>
        /// Wraps <paramref name="operation"/> lambda representing an HTTP call in a try/catch statement, returns <see cref="Either{L, R}"/> expected result <typeparamref name="R"/> 
        /// or in case of exception logs it, assigns failure <see cref="HttpStatusCode"/> and converts exception to return type of <see cref="Error"/>.
        /// </summary>
        /// <typeparam name="R">Type returned by <paramref name="operation"/></typeparam>
        /// <param name="log">Logger instance.</param>
        /// <param name="operation">Lambda producing <see cref="Either{L, R}"/> result of <typeparamref name="R"/> or error condition of <see cref="Error"/>.</param>
        /// <returns></returns>
        public static async Task<Either<Error, R>> TryHttp<R>(ILogger log, ErrorSource errorSource, Func<Task<Either<Error, R>>> operation, Guid? identifier = null)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex)
            {
                var statusCode = ex is HttpRequestException || ex is TaskCanceledException
                    ? HttpStatusCode.ServiceUnavailable
                    : HttpStatusCode.InternalServerError;

                var message = $"Error performing HTTP call in {errorSource}";
                return ToExceptionalError(statusCode, ex, message, null, //TODO: TryHttp may not throw exception during conversion to error. Cause this condition and check.
                    () => LogAsWarningOrError(log, errorSource, statusCode, null, ex, message));
            }
        }

        /// <summary>
        /// Indicates whether <paramref name="statusCode"/> is in 200 range.
        /// </summary>
        /// <param name="statusCode">Status code to check.</param>
        public static bool IsSuccessStatusCode(HttpStatusCode statusCode) =>
            ((int)statusCode >= 200) && ((int)statusCode <= 299);

        /// <summary>
        /// Indicates whether <paramref name="statusCode"/> is in 400 range.
        /// </summary>
        /// <param name="statusCode">Status code to check.</param>
        public static bool IsBadRequest(HttpStatusCode statusCode) =>
            ((int)statusCode >= 400 && (int)statusCode <= 499);

        /// <summary>
        /// Indicates whether <paramref name="statusCode"/> is in 500 range.
        /// </summary>
        /// <param name="statusCode">Status code to check.</param>
        public static bool IsServerError(HttpStatusCode statusCode) =>
            ((int)statusCode >= 500 && (int)statusCode <= 599);

        internal static Error ToError(ILogger log, ErrorSource errorSource, HttpResponseMessage response, string content)
        {
            var method = response.RequestMessage.Method;
            var url = response.RequestMessage.RequestUri.ToString();
            return ToError(log, errorSource, response.StatusCode, content, method, url);
        }

        /// <summary>
        /// Will log an error and create and return an instance of <see cref="ErrorResponse"/>.
        /// </summary>
        /// <param name="log">Logger instance.</param>
        /// <param name="errorSource"><see cref="ErrorSource"/> enum value identifying proxy in case of error.</param>
        /// <param name="statusCode"><sse cref="HttpStatusCode"/> enum value associated with error response.</param>
        /// <param name="message">Error description or details.</param>
        /// <param name="method"><see cref="HttpMethod"/> that resulted in error response.</param>
        /// <param name="url">Url of the request that resulted in error.</param>
        public static Error ToError(ILogger log, ErrorSource errorSource, HttpStatusCode statusCode, string message, HttpMethod method = null, string url = null)
        {
            if (IsBadRequest(statusCode))
            {
                log?.Warning("Proxy call BAD_REQUEST. ErrorSource:{ErrorSource}, StatusCode:{StatusCode} {HttpMethod} {url}", errorSource, statusCode,
                    method, url);
            }

            if (IsServerError(statusCode))
            {
                log?.Error("Proxy call SERVER_ERROR. ErrorSource:{ErrorSource}, StatusCode:{StatusCode} {HttpMethod} {url}", errorSource, statusCode,
                    method, url);
            }

            return Error.Create(errorSource, statusCode, message);
        }

        /// <summary>
        /// Performs conversion of external error representations received from downstream APIs into our internal <see cref="Error"/>.
        /// </summary>
        /// <param name="response"><see cref="HttpResponseMessage"/> representing the error returned by the downstream API.</param>
        /// <param name="log">Logging performing lambda.</param>
        /// <returns></returns>
        private Error ToError(HttpResponseMessage response, Func<Unit>? log = null)
        {
            log?.Invoke();
            return MapError(response.StatusCode, response);
        }

        /// <summary>
        /// Overridable template method for providing individual proxy implementations with ability to convert their error representations to our internal <see cref="Error"/>.
        /// </summary>
        /// <param name="errorSource"><see cref="ErrorSource"/> enum value identifying source of the error or warning.</param>
        /// <param name="statusCode"><see cref="HttpStatusCode"/> enum value associated with the error or warning.
        /// <param name="response"><see cref="HttpResponseMessage"/> associated with the error or warning. Its content will contain an error representation used by the downstream API.</param>
        /// <returns></returns>
        protected virtual Error MapError(HttpStatusCode statusCode, HttpResponseMessage response) =>
            Error.Create(errorSource, response.StatusCode, string.Empty, ErrorType.Integrations, exception: null);

        /// <summary>
        /// Converts internal exceptions and error conditions to <see cref="Error"/>
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="ex"></param>
        /// <param name="errorMessage"></param>
        /// <param name="errorCode"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        private static Error ToExceptionalError(HttpStatusCode statusCode, Exception? ex = null, string errorMessage = "", string? errorCode = null, Func<Unit>? log = null)
        {
            log?.Invoke();
            return Error.Create(ErrorSource.Unspecified, statusCode, errorMessage, ErrorType.Integrations, ex); // TODO MC: Add error code
        }

        /// <summary>
        /// Logs a warning.
        /// </summary>
        /// <param name="log">Logger instance.</param>
        /// <param name="errorSource"><see cref="ErrorSource"/> enum value identifying source of the error or warning.</param>
        /// <param name="response"><see cref="HttpResponseMessage"/> associated with the error or warning. If provided it will be used to extract HTTP call metadata.</param>
        /// <param name="message">Human-readable error or warning description or details.</param>
        /// <returns>Nothing</returns>
        protected static Unit LogAsWarning(ILogger log, ErrorSource errorSource, HttpResponseMessage response, string? message)
        {
            var (url, method) = GetRequestMetadata(response);

            log.Warning("Warning: ErrorSource:{ErrorSource}, StatusCode:{StatusCode} {HttpMethod} {url}. {Message}", errorSource, response.StatusCode, method, url, message);
            return Unit.Default;
        }

        /// <summary>
        /// Logs a warning or error based on <paramref name="statusCode"/>.
        /// </summary>
        /// <param name="log">Logger instance.</param>
        /// <param name="errorSource"><see cref="ErrorSource"/> enum value identifying source of the error or warning.</param>
        /// <param name="statusCode"><sse cref="HttpStatusCode"/> enum value associated with the error or warning. Determines whether condition will be logged as error or warning.</param>
        /// <param name="response"><see cref="HttpResponseMessage"/> associated with the error or warning. If provided it will be used to extract HTTP call metadata.</param>
        /// <param name="ex"><see cref="Exception"/> associated with the error or warning.</param>
        /// <param name="errorMessage">Human-readable error or warning description or details.</param>
        /// <param name="errorCode">Machine-readable error or warning code.</param>
        /// <returns>Nothing</returns>
        private static Unit LogAsWarningOrError(ILogger log, ErrorSource errorSource, HttpStatusCode statusCode, HttpResponseMessage? response = null, Exception? ex = null, string? errorMessage = null, string? errorCode = null)
        {
            var (url, method) = GetRequestMetadata(response);

            if (statusCode.IsBadRequest())
                log.Warning("Proxy call BAD_REQUEST. ErrorSource:{ErrorSource}, StatusCode:{StatusCode} {HttpMethod} {url}", errorSource, statusCode, method, url);

            if (statusCode.IsServerError())
                log.Error("Proxy call SERVER_ERROR. ErrorSource:{ErrorSource}, StatusCode:{StatusCode} {HttpMethod} {url}. Exception: {Exception}. Message: {Message}. ErrorCode: {ErrorCode}", errorSource, statusCode, method, url, ex, errorMessage, errorCode);

            return Unit.Default;
        }

        /// <summary>
        /// Logs a warning or error based on <paramref name="statusCode"/>.
        /// </summary>
        /// <param name="log">Logger instance.</param>
        /// <param name="errorSource"><see cref="ErrorSource"/> enum value identifying source of the error or warning.</param>
        /// <param name="statusCode"><sse cref="HttpStatusCode"/> enum value associated with the error or warning. Determines whether condition will be logged as error or warning.</param>
        /// <param name="errorMessage">Human-readable error or warning description or details.</param>
        /// <param name="method">Type of Http method called</param>
        /// <param name="url">Url of the Http called</param>
        /// <returns></returns>
        public static Unit LogAsWarningOrError(ILogger log, ErrorSource errorSource, HttpStatusCode statusCode, string errorMessage, HttpMethod? method = null, string? url = null)
        {
            if (statusCode.IsBadRequest())
                log.Warning("Proxy call BAD_REQUEST. ErrorSource:{ErrorSource}, StatusCode:{StatusCode} {HttpMethod} {url}", errorSource, statusCode, method, url);

            if (statusCode.IsServerError())
                log.Error("Proxy call SERVER_ERROR. ErrorSource:{ErrorSource}, StatusCode:{StatusCode} {HttpMethod} {url}. Message: {Message}.", errorSource, statusCode,  method, url, errorMessage);

            return Unit.Default;
        }

        private static (string? Url, string? Method) GetRequestMetadata(HttpResponseMessage? response) =>
            response != null
                ? (response.RequestMessage?.RequestUri?.ToString(), response.RequestMessage?.Method.ToString())
                : (null, null);


        private string BuildUrl(string baseAddress, string route, HttpMethod method)
        {
            var url = baseAddress.Last() == '/'
                ? $"{baseAddress}{route}"
                : $"{baseAddress}/{route}";

            log.Information("Proxy call {HttpMethod} {url}", method.ToString(), url);
            return url;
        }

        protected static string BuildRoute(string route, Dictionary<string, string>? parameters = null)
        {
            static string strip(string value) => value.TrimEnd('&').TrimEnd('?');

            if (parameters == null)
                return strip(route);

            var sb = new StringBuilder(route);
            parameters.Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value)).ToList().ForEach(kv =>
                sb.Append(Uri.EscapeDataString(kv.Key) + "=").Append(Uri.EscapeDataString(kv.Value)).Append('&'));

            return strip(sb.ToString());
        }

        protected static string ConvertToString(DateTimeOffset? value) =>
            value.HasValue ? ConvertToString(value.Value) : "";

        protected static string ConvertToString(DateTimeOffset value) =>
            value.ToString("s", CultureInfo.InvariantCulture);

        protected static string ConvertToString(object? value, CultureInfo? ci = null) =>
            value == null
                ? ""
                : value is Enum
                    ? EnumToString(value, Culture(ci))
                    : value is bool
                        ? Convert.ToString((bool)value, Culture(ci)).ToLowerInvariant()
                        : value is byte[]? Convert.ToBase64String((byte[])value)
                            : value.GetType().IsArray
                                ? string.Join(",", ((Array)value).OfType<object>().Select(o => ConvertToString(o, Culture(ci))))
                                : Convert.ToString(value, Culture(ci)) ?? "";

        private static CultureInfo Culture(CultureInfo? ci = null) =>
            ci ?? CultureInfo.InvariantCulture;

        private static string EnumToString(object value, CultureInfo cultureInfo)
        {
            if (Enum.GetName(value.GetType(), value) is not string name)
                return Convert.ToString(value, cultureInfo) ?? "";

            if (value.GetType().GetTypeInfo().GetDeclaredField(name) is FieldInfo field
                && field.GetCustomAttribute(typeof(EnumMemberAttribute)) is EnumMemberAttribute attribute)
                return attribute.Value ?? name;

            return Convert.ToString(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()), cultureInfo)) ?? "";
        }


        public static StringContent ToContent<I>(I data) =>
            new(SerializeObject(data), Encoding.UTF8, "application/json");

        public static StringContent EmptyContent() =>
            new("", Encoding.UTF8, "application/json");

        public static string ToJson(object value) =>
            SerializeObject(value, Newtonsoft.Json.Formatting.Indented);


        protected static async Task<Either<Error, ICollection<O>>> PaginateAllAsync<I, O>(
            Func<int, int, Task<Either<Error, I>>> getSome,
            int pageSize,
            Func<I, ICollection<O>> listAccessor,
            Func<I, int> totalAccessor)
        {
            var results = new List<O>();
            var pageNumber = 1;
            var more = false;
            Error? error = null;

            do
            {
                var response = await getSome(pageNumber, pageSize);
                response.Match(
                    right =>
                    {
                        results.AddRange(listAccessor(right));
                        more = totalAccessor(right) > pageSize * pageNumber;
                        pageNumber++;
                    },
                    left => error = left);

                if (error != null)
                    return error;

            } while (more);

            return results;
        }

        protected static async Task<Either<Error, ICollection<O>>> GetManyCollectionsAsync<O, OId>(
            Func<OId, Task<Either<Error, ICollection<O>>>> getOne,
            ICollection<OId> ids)
        {
            var results = new List<O>();
            Error? error = null;

            foreach (var id in ids)
            {
                var response = await getOne(id);
                response.Match(
                    right => results.AddRange(right),
                    left => error = left);

                if (error != null)
                    return error;
            }

            return results;
        }

        protected static async Task<Either<Error, ICollection<O>>> GetManyOptionsByIdAsync<O, OId>(
            Func<OId, Task<Either<Error, Option<O>>>> getOne,
            List<OId> ids)
        {
            var results = new List<O>();
            Error? error = null;

            foreach (var id in ids)
            {
                var response = await getOne(id);
                response.Match(
                    right => right.Match(
                        some => results.Add(some),
                        () => { }),
                    left => { error = left; });

                if (error != null)
                    return error;
            }

            return results;
        }

        private async void LogHttpCall(DateTimeOffset start, DateTimeOffset end, long duration, HttpResponseMessage response, string content = "")
        {
            httpCallLogger.Log(new HttpCall
            {
                Sender = httpCallLogger.Sender ?? "",
                Recipient = httpCallLogger.Recipient ?? "",
                Uri = response.RequestMessage != null && response.RequestMessage.RequestUri != null
                    ? response.RequestMessage.RequestUri.ToString()
                    : "",
                RequestStart = start,
                RequestEnd = end,
                Duration = duration,
                Method = response.RequestMessage != null
                    ? response.RequestMessage.Method.ToString()
                    : "",
                RequestHeaders = response.RequestMessage != null
                    ? ToJson(response.RequestMessage.Headers.RemoveHeader("API-Key"))
                    : "",
                ResponseBody = content,
                ResponseHeaders = ToJson(response.Headers),
                StatusCode = (int)response.StatusCode,
                CorrelationId = httpCallLogger.CorrelationIdHeader.HasValue()
                    ? response.ReadRequestHeader(httpCallLogger.CorrelationIdHeader ?? "")
                    : "",
                RequestBody = await response.ReadRequestContent(),
            });
        }


        private class JsonDeserializer
        {
            readonly ILogger log;
            readonly ErrorSource errorSource;

            internal JsonDeserializer(ILogger log, ErrorSource errorSource) =>
                (this.log, this.errorSource) = (log, errorSource);

            internal Either<Error, R> As<R>(string? content) =>
                content != null
                    ? Try<R, R>(content,
                        r => r != null
                            ? r
                            : ExceptionalError($"Failed to deserialize value '{content}' as {typeof(R)}."))
                    : NullValueError<R>();

            internal Either<Error, Option<R>> AsOption<R>(string? content) =>
                content != null
                    ? Try<R, Option<R>>(content,
                        r => r != null
                            ? Some(r)
                            : Option<R>.None)
                    : NullValueError<Option<R>>();

            private Either<Error, B> Try<A, B>(string content, Func<A?, Either<Error, B>> project)
            {
                try
                {
                    var o = DeserializeObject<A>(content);
                    return project(o);
                }
                catch (Exception ex)
                {
                    return ExceptionalError($"Failed to deserialize value '{content}' as {typeof(A)}. {ex.Message}", ex);
                }
            }


            private Either<Error, R> NullValueError<R>() =>
                ExceptionalError($"Cannot deserialize null value as {typeof(R)}.");

            private Error ExceptionalError(string message = "", Exception? ex = null) =>
                ToExceptionalError(HttpStatusCode.InternalServerError, ex, message, null,
                    () => LogAsWarningOrError(log, errorSource, HttpStatusCode.InternalServerError, null, ex, message));
        }
    }
}
