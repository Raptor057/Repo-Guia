using MediatR;

namespace Common.CleanArch
{
    public interface IRequest<out TResponse> : MediatR.IRequest<TResponse> where TResponse : IResponse
    {
    }

    public interface IResponse
    {
    }

    public interface IPresenter<in TResponse> where TResponse : IResponse
    {
        Task Handle(TResponse notification, CancellationToken cancellationToken);
    }

    public interface IInteractor<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
    {
    }

    public sealed class InteractorPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
    {
        private readonly IEnumerable<IPresenter<TResponse>> _presenters;

        public InteractorPipeline(IEnumerable<IPresenter<TResponse>> presenters)
        {
            _presenters = presenters;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var response = await next().ConfigureAwait(false);

            foreach (var presenter in _presenters)
            {
                await presenter.Handle(response, cancellationToken).ConfigureAwait(false);
            }

            return response;
        }
    }
}
