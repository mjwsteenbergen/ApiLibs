using System.Net;

namespace ApiLibs
{
    public class ContinueResponse<T> : RequestResponse<T>
    {
        public ContinueResponse() : base(null, HttpStatusCode.Continue) { }
        public ContinueResponse(RequestResponse resp) : base(resp, HttpStatusCode.Continue) { }
    }

    public class SwitchingProtocolsResponse<T> : RequestResponse<T>
    {
        public SwitchingProtocolsResponse() : base(null, HttpStatusCode.SwitchingProtocols) { }
        public SwitchingProtocolsResponse(RequestResponse resp) : base(resp, HttpStatusCode.SwitchingProtocols) { }
    }

    public class OKResponse<T> : RequestResponse<T>
    {
        public OKResponse() : base(null, HttpStatusCode.OK) { }
        public OKResponse(RequestResponse resp) : base(resp, HttpStatusCode.OK) { }
    }

    public class CreatedResponse<T> : RequestResponse<T>
    {
        public CreatedResponse() : base(null, HttpStatusCode.Created) { }
        public CreatedResponse(RequestResponse resp) : base(resp, HttpStatusCode.Created) { }
    }

    public class AcceptedResponse<T> : RequestResponse<T>
    {
        public AcceptedResponse() : base(null, HttpStatusCode.Accepted) { }
        public AcceptedResponse(RequestResponse resp) : base(resp, HttpStatusCode.Accepted) { }
    }

    public class NonAuthoritativeInformationResponse<T> : RequestResponse<T>
    {
        public NonAuthoritativeInformationResponse() : base(null, HttpStatusCode.NonAuthoritativeInformation) { }
        public NonAuthoritativeInformationResponse(RequestResponse resp) : base(resp, HttpStatusCode.NonAuthoritativeInformation) { }
    }

    public class NoContentResponse<T> : RequestResponse<T>
    {
        public NoContentResponse() : base(null, HttpStatusCode.NoContent) { }
        public NoContentResponse(RequestResponse resp) : base(resp, HttpStatusCode.NoContent) { }
    }

    public class ResetContentResponse<T> : RequestResponse<T>
    {
        public ResetContentResponse() : base(null, HttpStatusCode.ResetContent) { }
        public ResetContentResponse(RequestResponse resp) : base(resp, HttpStatusCode.ResetContent) { }
    }

    public class PartialContentResponse<T> : RequestResponse<T>
    {
        public PartialContentResponse() : base(null, HttpStatusCode.PartialContent) { }
        public PartialContentResponse(RequestResponse resp) : base(resp, HttpStatusCode.PartialContent) { }
    }

    public class AmbiguousResponse<T> : RequestResponse<T>
    {
        public AmbiguousResponse() : base(null, HttpStatusCode.Ambiguous) { }
        public AmbiguousResponse(RequestResponse resp) : base(resp, HttpStatusCode.Ambiguous) { }
    }

    public class MultipleChoicesResponse<T> : RequestResponse<T>
    {
        public MultipleChoicesResponse() : base(null, HttpStatusCode.MultipleChoices) { }
        public MultipleChoicesResponse(RequestResponse resp) : base(resp, HttpStatusCode.MultipleChoices) { }
    }

    public class MovedResponse<T> : RequestResponse<T>
    {
        public MovedResponse() : base(null, HttpStatusCode.Moved) { }
        public MovedResponse(RequestResponse resp) : base(resp, HttpStatusCode.Moved) { }
    }

    public class MovedPermanentlyResponse<T> : RequestResponse<T>
    {
        public MovedPermanentlyResponse() : base(null, HttpStatusCode.MovedPermanently) { }
        public MovedPermanentlyResponse(RequestResponse resp) : base(resp, HttpStatusCode.MovedPermanently) { }
    }

    public class FoundResponse<T> : RequestResponse<T>
    {
        public FoundResponse() : base(null, HttpStatusCode.Found) { }
        public FoundResponse(RequestResponse resp) : base(resp, HttpStatusCode.Found) { }
    }

    public class RedirectResponse<T> : RequestResponse<T>
    {
        public RedirectResponse() : base(null, HttpStatusCode.Redirect) { }
        public RedirectResponse(RequestResponse resp) : base(resp, HttpStatusCode.Redirect) { }
    }

    public class RedirectMethodResponse<T> : RequestResponse<T>
    {
        public RedirectMethodResponse() : base(null, HttpStatusCode.RedirectMethod) { }
        public RedirectMethodResponse(RequestResponse resp) : base(resp, HttpStatusCode.RedirectMethod) { }
    }

    public class SeeOtherResponse<T> : RequestResponse<T>
    {
        public SeeOtherResponse() : base(null, HttpStatusCode.SeeOther) { }
        public SeeOtherResponse(RequestResponse resp) : base(resp, HttpStatusCode.SeeOther) { }
    }

    public class NotModifiedResponse<T> : RequestResponse<T>
    {
        public NotModifiedResponse() : base(null, HttpStatusCode.NotModified) { }
        public NotModifiedResponse(RequestResponse resp) : base(resp, HttpStatusCode.NotModified) { }
    }

    public class UseProxyResponse<T> : RequestResponse<T>
    {
        public UseProxyResponse() : base(null, HttpStatusCode.UseProxy) { }
        public UseProxyResponse(RequestResponse resp) : base(resp, HttpStatusCode.UseProxy) { }
    }

    public class UnusedResponse<T> : RequestResponse<T>
    {
        public UnusedResponse() : base(null, HttpStatusCode.Unused) { }
        public UnusedResponse(RequestResponse resp) : base(resp, HttpStatusCode.Unused) { }
    }

    public class RedirectKeepVerbResponse<T> : RequestResponse<T>
    {
        public RedirectKeepVerbResponse() : base(null, HttpStatusCode.RedirectKeepVerb) { }
        public RedirectKeepVerbResponse(RequestResponse resp) : base(resp, HttpStatusCode.RedirectKeepVerb) { }
    }

    public class TemporaryRedirectResponse<T> : RequestResponse<T>
    {
        public TemporaryRedirectResponse() : base(null, HttpStatusCode.TemporaryRedirect) { }
        public TemporaryRedirectResponse(RequestResponse resp) : base(resp, HttpStatusCode.TemporaryRedirect) { }
    }

    public class BadRequestResponse<T> : RequestResponse<T>
    {
        public BadRequestResponse() : base(null, HttpStatusCode.BadRequest) { }
        public BadRequestResponse(RequestResponse resp) : base(resp, HttpStatusCode.BadRequest) { }
    }

    public class UnauthorizedResponse<T> : RequestResponse<T>
    {
        public UnauthorizedResponse() : base(null, HttpStatusCode.Unauthorized) { }
        public UnauthorizedResponse(RequestResponse resp) : base(resp, HttpStatusCode.Unauthorized) { }
    }

    public class PaymentRequiredResponse<T> : RequestResponse<T>
    {
        public PaymentRequiredResponse() : base(null, HttpStatusCode.PaymentRequired) { }
        public PaymentRequiredResponse(RequestResponse resp) : base(resp, HttpStatusCode.PaymentRequired) { }
    }

    public class ForbiddenResponse<T> : RequestResponse<T>
    {
        public ForbiddenResponse() : base(null, HttpStatusCode.Forbidden) { }
        public ForbiddenResponse(RequestResponse resp) : base(resp, HttpStatusCode.Forbidden) { }
    }

    public class NotFoundResponse<T> : RequestResponse<T>
    {
        public NotFoundResponse() : base(null, HttpStatusCode.NotFound) { }
        public NotFoundResponse(RequestResponse resp) : base(resp, HttpStatusCode.NotFound) { }
    }

    public class MethodNotAllowedResponse<T> : RequestResponse<T>
    {
        public MethodNotAllowedResponse() : base(null, HttpStatusCode.MethodNotAllowed) { }
        public MethodNotAllowedResponse(RequestResponse resp) : base(resp, HttpStatusCode.MethodNotAllowed) { }
    }

    public class NotAcceptableResponse<T> : RequestResponse<T>
    {
        public NotAcceptableResponse() : base(null, HttpStatusCode.NotAcceptable) { }
        public NotAcceptableResponse(RequestResponse resp) : base(resp, HttpStatusCode.NotAcceptable) { }
    }

    public class ProxyAuthenticationRequiredResponse<T> : RequestResponse<T>
    {
        public ProxyAuthenticationRequiredResponse() : base(null, HttpStatusCode.ProxyAuthenticationRequired) { }
        public ProxyAuthenticationRequiredResponse(RequestResponse resp) : base(resp, HttpStatusCode.ProxyAuthenticationRequired) { }
    }

    public class RequestTimeoutResponse<T> : RequestResponse<T>
    {
        public RequestTimeoutResponse() : base(null, HttpStatusCode.RequestTimeout) { }
        public RequestTimeoutResponse(RequestResponse resp) : base(resp, HttpStatusCode.RequestTimeout) { }
    }

    public class ConflictResponse<T> : RequestResponse<T>
    {
        public ConflictResponse() : base(null, HttpStatusCode.Conflict) { }
        public ConflictResponse(RequestResponse resp) : base(resp, HttpStatusCode.Conflict) { }
    }

    public class GoneResponse<T> : RequestResponse<T>
    {
        public GoneResponse() : base(null, HttpStatusCode.Gone) { }
        public GoneResponse(RequestResponse resp) : base(resp, HttpStatusCode.Gone) { }
    }

    public class LengthRequiredResponse<T> : RequestResponse<T>
    {
        public LengthRequiredResponse() : base(null, HttpStatusCode.LengthRequired) { }
        public LengthRequiredResponse(RequestResponse resp) : base(resp, HttpStatusCode.LengthRequired) { }
    }

    public class PreconditionFailedResponse<T> : RequestResponse<T>
    {
        public PreconditionFailedResponse() : base(null, HttpStatusCode.PreconditionFailed) { }
        public PreconditionFailedResponse(RequestResponse resp) : base(resp, HttpStatusCode.PreconditionFailed) { }
    }

    public class RequestEntityTooLargeResponse<T> : RequestResponse<T>
    {
        public RequestEntityTooLargeResponse() : base(null, HttpStatusCode.RequestEntityTooLarge) { }
        public RequestEntityTooLargeResponse(RequestResponse resp) : base(resp, HttpStatusCode.RequestEntityTooLarge) { }
    }

    public class RequestUriTooLongResponse<T> : RequestResponse<T>
    {
        public RequestUriTooLongResponse() : base(null, HttpStatusCode.RequestUriTooLong) { }
        public RequestUriTooLongResponse(RequestResponse resp) : base(resp, HttpStatusCode.RequestUriTooLong) { }
    }

    public class UnsupportedMediaTypeResponse<T> : RequestResponse<T>
    {
        public UnsupportedMediaTypeResponse() : base(null, HttpStatusCode.UnsupportedMediaType) { }
        public UnsupportedMediaTypeResponse(RequestResponse resp) : base(resp, HttpStatusCode.UnsupportedMediaType) { }
    }

    public class RequestedRangeNotSatisfiableResponse<T> : RequestResponse<T>
    {
        public RequestedRangeNotSatisfiableResponse() : base(null, HttpStatusCode.RequestedRangeNotSatisfiable) { }
        public RequestedRangeNotSatisfiableResponse(RequestResponse resp) : base(resp, HttpStatusCode.RequestedRangeNotSatisfiable) { }
    }

    public class ExpectationFailedResponse<T> : RequestResponse<T>
    {
        public ExpectationFailedResponse() : base(null, HttpStatusCode.ExpectationFailed) { }
        public ExpectationFailedResponse(RequestResponse resp) : base(resp, HttpStatusCode.ExpectationFailed) { }
    }

    public class UpgradeRequiredResponse<T> : RequestResponse<T>
    {
        public UpgradeRequiredResponse() : base(null, HttpStatusCode.UpgradeRequired) { }
        public UpgradeRequiredResponse(RequestResponse resp) : base(resp, HttpStatusCode.UpgradeRequired) { }
    }

    public class InternalServerErrorResponse<T> : RequestResponse<T>
    {
        public InternalServerErrorResponse() : base(null, HttpStatusCode.InternalServerError) { }
        public InternalServerErrorResponse(RequestResponse resp) : base(resp, HttpStatusCode.InternalServerError) { }
    }

    public class NotImplementedResponse<T> : RequestResponse<T>
    {
        public NotImplementedResponse() : base(null, HttpStatusCode.NotImplemented) { }
        public NotImplementedResponse(RequestResponse resp) : base(resp, HttpStatusCode.NotImplemented) { }
    }

    public class BadGatewayResponse<T> : RequestResponse<T>
    {
        public BadGatewayResponse() : base(null, HttpStatusCode.BadGateway) { }
        public BadGatewayResponse(RequestResponse resp) : base(resp, HttpStatusCode.BadGateway) { }
    }

    public class ServiceUnavailableResponse<T> : RequestResponse<T>
    {
        public ServiceUnavailableResponse() : base(null, HttpStatusCode.ServiceUnavailable) { }
        public ServiceUnavailableResponse(RequestResponse resp) : base(resp, HttpStatusCode.ServiceUnavailable) { }
    }

    public class GatewayTimeoutResponse<T> : RequestResponse<T>
    {
        public GatewayTimeoutResponse() : base(null, HttpStatusCode.GatewayTimeout) { }
        public GatewayTimeoutResponse(RequestResponse resp) : base(resp, HttpStatusCode.GatewayTimeout) { }
    }

    public class HttpVersionNotSupportedResponse<T> : RequestResponse<T>
    {
        public HttpVersionNotSupportedResponse() : base(null, HttpStatusCode.HttpVersionNotSupported) { }
        public HttpVersionNotSupportedResponse(RequestResponse resp) : base(resp, HttpStatusCode.HttpVersionNotSupported) { }
    }


}