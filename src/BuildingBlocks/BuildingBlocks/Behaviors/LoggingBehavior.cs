using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) :
    IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IRequest<TResponse> where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        // İstek işleme sürecinin başladığını, istek türü, yanıt türü ve istek verileri ile birlikte logla.
        logger.LogInformation("[BAŞLANGIÇ] İstek işleniyor: Request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        // İsteğin işlenme süresini ölçmek için bir kronometre başlat.
        var timer = new Stopwatch();
        timer.Start();

        // Pipeline'daki bir sonraki işlemi çalıştır ve yanıtı bekle.
        var response = await next();

        // İstek işlendikten sonra kronometreyi durdur.
        timer.Stop();
        var timeTaken = timer.Elapsed;

        // Eğer istek işleme süresi 3 saniyeden uzun sürdüyse, bir uyarı mesajı logla.
        if (timeTaken.Seconds > 3)
            logger.LogWarning("[PERFORMANS] {Request} isteği {TimeTaken} saniye sürdü.",
                typeof(TRequest).Name, timeTaken.Seconds);

        // İstek işleme sürecinin bittiğini, istek ve yanıt türleri ile birlikte logla.
        logger.LogInformation("[SON] {Request} başarıyla işlendi. Yanıt: {Response}", typeof(TRequest).Name, typeof(TResponse).Name);

        // Yanıtı pipeline'a geri döndür.
        return response;


    }
}
