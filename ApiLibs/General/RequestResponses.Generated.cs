using System.Net;

namespace ApiLibs
{
    public class ContinueResponse : RequestResponse
    {
        public ContinueResponse() : base(HttpStatusCode.Continue) { }
        public ContinueResponse(RequestResponse resp) : base(resp) { }
    }

    public class ContinueResponse<T> : ContinueResponse
    {
        public ContinueResponse() : base() { }
        public ContinueResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class SwitchingProtocolsResponse : RequestResponse
    {
        public SwitchingProtocolsResponse() : base(HttpStatusCode.Continue) { }
        public SwitchingProtocolsResponse(RequestResponse resp) : base(resp) { }
    }

    public class SwitchingProtocolsResponse<T> : SwitchingProtocolsResponse
    {
        public SwitchingProtocolsResponse() : base() { }
        public SwitchingProtocolsResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class OKResponse : RequestResponse
    {
        public OKResponse() : base(HttpStatusCode.Continue) { }
        public OKResponse(RequestResponse resp) : base(resp) { }
    }

    public class OKResponse<T> : OKResponse
    {
        public OKResponse() : base() { }
        public OKResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class CreatedResponse : RequestResponse
    {
        public CreatedResponse() : base(HttpStatusCode.Continue) { }
        public CreatedResponse(RequestResponse resp) : base(resp) { }
    }

    public class CreatedResponse<T> : CreatedResponse
    {
        public CreatedResponse() : base() { }
        public CreatedResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class AcceptedResponse : RequestResponse
    {
        public AcceptedResponse() : base(HttpStatusCode.Continue) { }
        public AcceptedResponse(RequestResponse resp) : base(resp) { }
    }

    public class AcceptedResponse<T> : AcceptedResponse
    {
        public AcceptedResponse() : base() { }
        public AcceptedResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class NonAuthoritativeInformationResponse : RequestResponse
    {
        public NonAuthoritativeInformationResponse() : base(HttpStatusCode.Continue) { }
        public NonAuthoritativeInformationResponse(RequestResponse resp) : base(resp) { }
    }

    public class NonAuthoritativeInformationResponse<T> : NonAuthoritativeInformationResponse
    {
        public NonAuthoritativeInformationResponse() : base() { }
        public NonAuthoritativeInformationResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class NoContentResponse : RequestResponse
    {
        public NoContentResponse() : base(HttpStatusCode.Continue) { }
        public NoContentResponse(RequestResponse resp) : base(resp) { }
    }

    public class NoContentResponse<T> : NoContentResponse
    {
        public NoContentResponse() : base() { }
        public NoContentResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class ResetContentResponse : RequestResponse
    {
        public ResetContentResponse() : base(HttpStatusCode.Continue) { }
        public ResetContentResponse(RequestResponse resp) : base(resp) { }
    }

    public class ResetContentResponse<T> : ResetContentResponse
    {
        public ResetContentResponse() : base() { }
        public ResetContentResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class PartialContentResponse : RequestResponse
    {
        public PartialContentResponse() : base(HttpStatusCode.Continue) { }
        public PartialContentResponse(RequestResponse resp) : base(resp) { }
    }

    public class PartialContentResponse<T> : PartialContentResponse
    {
        public PartialContentResponse() : base() { }
        public PartialContentResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class AmbiguousResponse : RequestResponse
    {
        public AmbiguousResponse() : base(HttpStatusCode.Continue) { }
        public AmbiguousResponse(RequestResponse resp) : base(resp) { }
    }

    public class AmbiguousResponse<T> : AmbiguousResponse
    {
        public AmbiguousResponse() : base() { }
        public AmbiguousResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class MultipleChoicesResponse : RequestResponse
    {
        public MultipleChoicesResponse() : base(HttpStatusCode.Continue) { }
        public MultipleChoicesResponse(RequestResponse resp) : base(resp) { }
    }

    public class MultipleChoicesResponse<T> : MultipleChoicesResponse
    {
        public MultipleChoicesResponse() : base() { }
        public MultipleChoicesResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class MovedResponse : RequestResponse
    {
        public MovedResponse() : base(HttpStatusCode.Continue) { }
        public MovedResponse(RequestResponse resp) : base(resp) { }
    }

    public class MovedResponse<T> : MovedResponse
    {
        public MovedResponse() : base() { }
        public MovedResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class MovedPermanentlyResponse : RequestResponse
    {
        public MovedPermanentlyResponse() : base(HttpStatusCode.Continue) { }
        public MovedPermanentlyResponse(RequestResponse resp) : base(resp) { }
    }

    public class MovedPermanentlyResponse<T> : MovedPermanentlyResponse
    {
        public MovedPermanentlyResponse() : base() { }
        public MovedPermanentlyResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class FoundResponse : RequestResponse
    {
        public FoundResponse() : base(HttpStatusCode.Continue) { }
        public FoundResponse(RequestResponse resp) : base(resp) { }
    }

    public class FoundResponse<T> : FoundResponse
    {
        public FoundResponse() : base() { }
        public FoundResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class RedirectResponse : RequestResponse
    {
        public RedirectResponse() : base(HttpStatusCode.Continue) { }
        public RedirectResponse(RequestResponse resp) : base(resp) { }
    }

    public class RedirectResponse<T> : RedirectResponse
    {
        public RedirectResponse() : base() { }
        public RedirectResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class RedirectMethodResponse : RequestResponse
    {
        public RedirectMethodResponse() : base(HttpStatusCode.Continue) { }
        public RedirectMethodResponse(RequestResponse resp) : base(resp) { }
    }

    public class RedirectMethodResponse<T> : RedirectMethodResponse
    {
        public RedirectMethodResponse() : base() { }
        public RedirectMethodResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class SeeOtherResponse : RequestResponse
    {
        public SeeOtherResponse() : base(HttpStatusCode.Continue) { }
        public SeeOtherResponse(RequestResponse resp) : base(resp) { }
    }

    public class SeeOtherResponse<T> : SeeOtherResponse
    {
        public SeeOtherResponse() : base() { }
        public SeeOtherResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class NotModifiedResponse : RequestResponse
    {
        public NotModifiedResponse() : base(HttpStatusCode.Continue) { }
        public NotModifiedResponse(RequestResponse resp) : base(resp) { }
    }

    public class NotModifiedResponse<T> : NotModifiedResponse
    {
        public NotModifiedResponse() : base() { }
        public NotModifiedResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class UseProxyResponse : RequestResponse
    {
        public UseProxyResponse() : base(HttpStatusCode.Continue) { }
        public UseProxyResponse(RequestResponse resp) : base(resp) { }
    }

    public class UseProxyResponse<T> : UseProxyResponse
    {
        public UseProxyResponse() : base() { }
        public UseProxyResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class UnusedResponse : RequestResponse
    {
        public UnusedResponse() : base(HttpStatusCode.Continue) { }
        public UnusedResponse(RequestResponse resp) : base(resp) { }
    }

    public class UnusedResponse<T> : UnusedResponse
    {
        public UnusedResponse() : base() { }
        public UnusedResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class RedirectKeepVerbResponse : RequestResponse
    {
        public RedirectKeepVerbResponse() : base(HttpStatusCode.Continue) { }
        public RedirectKeepVerbResponse(RequestResponse resp) : base(resp) { }
    }

    public class RedirectKeepVerbResponse<T> : RedirectKeepVerbResponse
    {
        public RedirectKeepVerbResponse() : base() { }
        public RedirectKeepVerbResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class TemporaryRedirectResponse : RequestResponse
    {
        public TemporaryRedirectResponse() : base(HttpStatusCode.Continue) { }
        public TemporaryRedirectResponse(RequestResponse resp) : base(resp) { }
    }

    public class TemporaryRedirectResponse<T> : TemporaryRedirectResponse
    {
        public TemporaryRedirectResponse() : base() { }
        public TemporaryRedirectResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class BadRequestResponse : RequestResponse
    {
        public BadRequestResponse() : base(HttpStatusCode.Continue) { }
        public BadRequestResponse(RequestResponse resp) : base(resp) { }
    }

    public class BadRequestResponse<T> : BadRequestResponse
    {
        public BadRequestResponse() : base() { }
        public BadRequestResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class UnauthorizedResponse : RequestResponse
    {
        public UnauthorizedResponse() : base(HttpStatusCode.Continue) { }
        public UnauthorizedResponse(RequestResponse resp) : base(resp) { }
    }

    public class UnauthorizedResponse<T> : UnauthorizedResponse
    {
        public UnauthorizedResponse() : base() { }
        public UnauthorizedResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class PaymentRequiredResponse : RequestResponse
    {
        public PaymentRequiredResponse() : base(HttpStatusCode.Continue) { }
        public PaymentRequiredResponse(RequestResponse resp) : base(resp) { }
    }

    public class PaymentRequiredResponse<T> : PaymentRequiredResponse
    {
        public PaymentRequiredResponse() : base() { }
        public PaymentRequiredResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class ForbiddenResponse : RequestResponse
    {
        public ForbiddenResponse() : base(HttpStatusCode.Continue) { }
        public ForbiddenResponse(RequestResponse resp) : base(resp) { }
    }

    public class ForbiddenResponse<T> : ForbiddenResponse
    {
        public ForbiddenResponse() : base() { }
        public ForbiddenResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class NotFoundResponse : RequestResponse
    {
        public NotFoundResponse() : base(HttpStatusCode.Continue) { }
        public NotFoundResponse(RequestResponse resp) : base(resp) { }
    }

    public class NotFoundResponse<T> : NotFoundResponse
    {
        public NotFoundResponse() : base() { }
        public NotFoundResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class MethodNotAllowedResponse : RequestResponse
    {
        public MethodNotAllowedResponse() : base(HttpStatusCode.Continue) { }
        public MethodNotAllowedResponse(RequestResponse resp) : base(resp) { }
    }

    public class MethodNotAllowedResponse<T> : MethodNotAllowedResponse
    {
        public MethodNotAllowedResponse() : base() { }
        public MethodNotAllowedResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class NotAcceptableResponse : RequestResponse
    {
        public NotAcceptableResponse() : base(HttpStatusCode.Continue) { }
        public NotAcceptableResponse(RequestResponse resp) : base(resp) { }
    }

    public class NotAcceptableResponse<T> : NotAcceptableResponse
    {
        public NotAcceptableResponse() : base() { }
        public NotAcceptableResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class ProxyAuthenticationRequiredResponse : RequestResponse
    {
        public ProxyAuthenticationRequiredResponse() : base(HttpStatusCode.Continue) { }
        public ProxyAuthenticationRequiredResponse(RequestResponse resp) : base(resp) { }
    }

    public class ProxyAuthenticationRequiredResponse<T> : ProxyAuthenticationRequiredResponse
    {
        public ProxyAuthenticationRequiredResponse() : base() { }
        public ProxyAuthenticationRequiredResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class RequestTimeoutResponse : RequestResponse
    {
        public RequestTimeoutResponse() : base(HttpStatusCode.Continue) { }
        public RequestTimeoutResponse(RequestResponse resp) : base(resp) { }
    }

    public class RequestTimeoutResponse<T> : RequestTimeoutResponse
    {
        public RequestTimeoutResponse() : base() { }
        public RequestTimeoutResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class ConflictResponse : RequestResponse
    {
        public ConflictResponse() : base(HttpStatusCode.Continue) { }
        public ConflictResponse(RequestResponse resp) : base(resp) { }
    }

    public class ConflictResponse<T> : ConflictResponse
    {
        public ConflictResponse() : base() { }
        public ConflictResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class GoneResponse : RequestResponse
    {
        public GoneResponse() : base(HttpStatusCode.Continue) { }
        public GoneResponse(RequestResponse resp) : base(resp) { }
    }

    public class GoneResponse<T> : GoneResponse
    {
        public GoneResponse() : base() { }
        public GoneResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class LengthRequiredResponse : RequestResponse
    {
        public LengthRequiredResponse() : base(HttpStatusCode.Continue) { }
        public LengthRequiredResponse(RequestResponse resp) : base(resp) { }
    }

    public class LengthRequiredResponse<T> : LengthRequiredResponse
    {
        public LengthRequiredResponse() : base() { }
        public LengthRequiredResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class PreconditionFailedResponse : RequestResponse
    {
        public PreconditionFailedResponse() : base(HttpStatusCode.Continue) { }
        public PreconditionFailedResponse(RequestResponse resp) : base(resp) { }
    }

    public class PreconditionFailedResponse<T> : PreconditionFailedResponse
    {
        public PreconditionFailedResponse() : base() { }
        public PreconditionFailedResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class RequestEntityTooLargeResponse : RequestResponse
    {
        public RequestEntityTooLargeResponse() : base(HttpStatusCode.Continue) { }
        public RequestEntityTooLargeResponse(RequestResponse resp) : base(resp) { }
    }

    public class RequestEntityTooLargeResponse<T> : RequestEntityTooLargeResponse
    {
        public RequestEntityTooLargeResponse() : base() { }
        public RequestEntityTooLargeResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class RequestUriTooLongResponse : RequestResponse
    {
        public RequestUriTooLongResponse() : base(HttpStatusCode.Continue) { }
        public RequestUriTooLongResponse(RequestResponse resp) : base(resp) { }
    }

    public class RequestUriTooLongResponse<T> : RequestUriTooLongResponse
    {
        public RequestUriTooLongResponse() : base() { }
        public RequestUriTooLongResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class UnsupportedMediaTypeResponse : RequestResponse
    {
        public UnsupportedMediaTypeResponse() : base(HttpStatusCode.Continue) { }
        public UnsupportedMediaTypeResponse(RequestResponse resp) : base(resp) { }
    }

    public class UnsupportedMediaTypeResponse<T> : UnsupportedMediaTypeResponse
    {
        public UnsupportedMediaTypeResponse() : base() { }
        public UnsupportedMediaTypeResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class RequestedRangeNotSatisfiableResponse : RequestResponse
    {
        public RequestedRangeNotSatisfiableResponse() : base(HttpStatusCode.Continue) { }
        public RequestedRangeNotSatisfiableResponse(RequestResponse resp) : base(resp) { }
    }

    public class RequestedRangeNotSatisfiableResponse<T> : RequestedRangeNotSatisfiableResponse
    {
        public RequestedRangeNotSatisfiableResponse() : base() { }
        public RequestedRangeNotSatisfiableResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class ExpectationFailedResponse : RequestResponse
    {
        public ExpectationFailedResponse() : base(HttpStatusCode.Continue) { }
        public ExpectationFailedResponse(RequestResponse resp) : base(resp) { }
    }

    public class ExpectationFailedResponse<T> : ExpectationFailedResponse
    {
        public ExpectationFailedResponse() : base() { }
        public ExpectationFailedResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class UpgradeRequiredResponse : RequestResponse
    {
        public UpgradeRequiredResponse() : base(HttpStatusCode.Continue) { }
        public UpgradeRequiredResponse(RequestResponse resp) : base(resp) { }
    }

    public class UpgradeRequiredResponse<T> : UpgradeRequiredResponse
    {
        public UpgradeRequiredResponse() : base() { }
        public UpgradeRequiredResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class InternalServerErrorResponse : RequestResponse
    {
        public InternalServerErrorResponse() : base(HttpStatusCode.Continue) { }
        public InternalServerErrorResponse(RequestResponse resp) : base(resp) { }
    }

    public class InternalServerErrorResponse<T> : InternalServerErrorResponse
    {
        public InternalServerErrorResponse() : base() { }
        public InternalServerErrorResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class NotImplementedResponse : RequestResponse
    {
        public NotImplementedResponse() : base(HttpStatusCode.Continue) { }
        public NotImplementedResponse(RequestResponse resp) : base(resp) { }
    }

    public class NotImplementedResponse<T> : NotImplementedResponse
    {
        public NotImplementedResponse() : base() { }
        public NotImplementedResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class BadGatewayResponse : RequestResponse
    {
        public BadGatewayResponse() : base(HttpStatusCode.Continue) { }
        public BadGatewayResponse(RequestResponse resp) : base(resp) { }
    }

    public class BadGatewayResponse<T> : BadGatewayResponse
    {
        public BadGatewayResponse() : base() { }
        public BadGatewayResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class ServiceUnavailableResponse : RequestResponse
    {
        public ServiceUnavailableResponse() : base(HttpStatusCode.Continue) { }
        public ServiceUnavailableResponse(RequestResponse resp) : base(resp) { }
    }

    public class ServiceUnavailableResponse<T> : ServiceUnavailableResponse
    {
        public ServiceUnavailableResponse() : base() { }
        public ServiceUnavailableResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class GatewayTimeoutResponse : RequestResponse
    {
        public GatewayTimeoutResponse() : base(HttpStatusCode.Continue) { }
        public GatewayTimeoutResponse(RequestResponse resp) : base(resp) { }
    }

    public class GatewayTimeoutResponse<T> : GatewayTimeoutResponse
    {
        public GatewayTimeoutResponse() : base() { }
        public GatewayTimeoutResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
    public class HttpVersionNotSupportedResponse : RequestResponse
    {
        public HttpVersionNotSupportedResponse() : base(HttpStatusCode.Continue) { }
        public HttpVersionNotSupportedResponse(RequestResponse resp) : base(resp) { }
    }

    public class HttpVersionNotSupportedResponse<T> : HttpVersionNotSupportedResponse
    {
        public HttpVersionNotSupportedResponse() : base() { }
        public HttpVersionNotSupportedResponse(RequestResponse resp) : base(resp) { }
        public new T Content() => base.Convert<T>();
    }
}