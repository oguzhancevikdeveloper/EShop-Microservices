using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TReesponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TReesponse> where TRequest : ICommand<TReesponse>
{
    public async Task<TReesponse> Handle(TRequest request, RequestHandlerDelegate<TReesponse> next, CancellationToken cancellationToken)
    {
        // İstek nesnesi için bir doğrulama bağlamı oluştur.
        var context = new ValidationContext<TRequest>(request);

        // Tüm doğrulayıcıları (validators) kullanarak isteği asenkron olarak doğrula ve sonuçları topla.
        var validationResults =
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // Doğrulama sonuçları arasında hatası olanları bul ve bu hataları bir listeye dönüştür.
        var failures =
            validationResults
            .Where(r => r.Errors.Any())  // Hata içeren sonuçları filtrele.
            .SelectMany(r => r.Errors)   // Hataları birleştir.
            .ToList();                   // Hataları bir listeye dönüştür.

        // Eğer herhangi bir hata varsa, bir doğrulama istisnası (ValidationException) fırlat.
        if (failures.Any())
            throw new ValidationException(failures);

        // Eğer hata yoksa, bir sonraki işlemciye geç ve isteği işleme devam et.
        return await next();

    }
}
